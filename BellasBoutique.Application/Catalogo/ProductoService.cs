using BellasBoutique.Application.Catalogo.ViewModels;
using BellasBoutique.Data;
using BellasBoutique.Data.Catalogo;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BellasBoutique.Application.Catalogo;

// Cada escritura hace un único SaveChangesAsync, que EF ejecuta en una transacción implícita y por eso es atómica.
// Facturación sí usará una transacción explícita: lee el stock y escribe Venta, DetalleVenta y stock en un solo bloque.
public class ProductoService(AppDbContext contexto) : IProductoService
{
    private const string MensajeNombreRepetido = "Ya existe un producto con ese nombre (puede estar dado de baja).";
    private const string MensajeProductoNoDisponible = "El producto no existe o está dado de baja.";

    public async Task<IReadOnlyList<ProductoListadoViewModel>> ListarActivosAsync(CancellationToken cancelacion)
    {
        return await contexto.Productos
            .AsNoTracking()
            .Where(producto => producto.Activo)
            .OrderBy(producto => producto.Nombre)
            .ProyectarAListado()
            .ToListAsync(cancelacion);
    }

    public async Task<CatalogoViewModel> ListarCatalogoAsync(int? categoriaId, CancellationToken cancelacion)
    {
        var disponibles = contexto.Productos
            .AsNoTracking()
            .Where(producto => producto.Activo && producto.Stock > 0);

        if (categoriaId is not null)
        {
            disponibles = disponibles.Where(producto => producto.CategoriaId == categoriaId);
        }

        var productos = await disponibles
            .OrderBy(producto => producto.Nombre)
            .ProyectarACatalogo()
            .ToListAsync(cancelacion);

        var categorias = await ListarCategoriasAsync(cancelacion);

        return new CatalogoViewModel(productos, categorias, categoriaId);
    }

    public async Task<ProductoFormularioViewModel?> ObtenerPorIdAsync(int id, CancellationToken cancelacion)
    {
        return await contexto.Productos
            .AsNoTracking()
            .Where(producto => producto.Id == id && producto.Activo)
            .ProyectarAFormulario()
            .FirstOrDefaultAsync(cancelacion);
    }

    public async Task<ListasDesplegablesViewModel> ObtenerListasDesplegablesAsync(CancellationToken cancelacion)
    {
        var categorias = await ListarCategoriasAsync(cancelacion);

        var proveedores = await contexto.Proveedores
            .AsNoTracking()
            .OrderBy(proveedor => proveedor.Nombre)
            .ProyectarAOpciones()
            .ToListAsync(cancelacion);

        return new ListasDesplegablesViewModel(categorias, proveedores);
    }

    public async Task<ResultadoOperacion> CrearAsync(ProductoFormularioViewModel modelo, CancellationToken cancelacion)
    {
        var errorDeRegla = await BuscarErrorDeReglaAsync(modelo, idProductoEditado: null, cancelacion);
        if (errorDeRegla is not null)
        {
            return ResultadoOperacion.Fallido(errorDeRegla);
        }

        var producto = new Producto();
        modelo.CopiarA(producto);
        contexto.Productos.Add(producto);

        return await GuardarAsync($"Se creó el producto \"{producto.Nombre}\".", cancelacion);
    }

    public async Task<ResultadoOperacion> EditarAsync(int id, ProductoFormularioViewModel modelo, CancellationToken cancelacion)
    {
        var producto = await BuscarActivoAsync(id, cancelacion);
        if (producto is null)
        {
            return ResultadoOperacion.Fallido(MensajeProductoNoDisponible);
        }

        var errorDeRegla = await BuscarErrorDeReglaAsync(modelo, idProductoEditado: id, cancelacion);
        if (errorDeRegla is not null)
        {
            return ResultadoOperacion.Fallido(errorDeRegla);
        }

        modelo.CopiarA(producto);

        return await GuardarAsync($"Se actualizó el producto \"{producto.Nombre}\".", cancelacion);
    }

    public async Task<ResultadoOperacion> DarDeBajaAsync(int id, CancellationToken cancelacion)
    {
        var producto = await BuscarActivoAsync(id, cancelacion);
        if (producto is null)
        {
            return ResultadoOperacion.Fallido(MensajeProductoNoDisponible);
        }

        producto.Activo = false;

        return await GuardarAsync($"Se dio de baja el producto \"{producto.Nombre}\".", cancelacion);
    }

    private async Task<IReadOnlyList<OpcionViewModel>> ListarCategoriasAsync(CancellationToken cancelacion)
    {
        return await contexto.Categorias
            .AsNoTracking()
            .OrderBy(categoria => categoria.Nombre)
            .ProyectarAOpciones()
            .ToListAsync(cancelacion);
    }

    private Task<Producto?> BuscarActivoAsync(int id, CancellationToken cancelacion) =>
        contexto.Productos.FirstOrDefaultAsync(producto => producto.Id == id && producto.Activo, cancelacion);

    private async Task<string?> BuscarErrorDeReglaAsync(
        ProductoFormularioViewModel modelo,
        int? idProductoEditado,
        CancellationToken cancelacion)
    {
        var nombre = modelo.Nombre.Trim();

        var nombreRepetido = await contexto.Productos.AnyAsync(
            producto => producto.Nombre == nombre && producto.Id != idProductoEditado,
            cancelacion);
        if (nombreRepetido)
        {
            return MensajeNombreRepetido;
        }

        var categoriaExiste = await contexto.Categorias.AnyAsync(
            categoria => categoria.Id == modelo.CategoriaId,
            cancelacion);
        if (!categoriaExiste)
        {
            return "La categoría seleccionada no existe.";
        }

        var proveedorExiste = await contexto.Proveedores.AnyAsync(
            proveedor => proveedor.Id == modelo.ProveedorId,
            cancelacion);
        if (!proveedorExiste)
        {
            return "El proveedor seleccionado no existe.";
        }

        return null;
    }

    private async Task<ResultadoOperacion> GuardarAsync(string mensajeDeExito, CancellationToken cancelacion)
    {
        try
        {
            await contexto.SaveChangesAsync(cancelacion);
            return ResultadoOperacion.Correcto(mensajeDeExito);
        }
        catch (DbUpdateException error) when (EsNombreRepetido(error))
        {
            return ResultadoOperacion.Fallido(MensajeNombreRepetido);
        }
    }

    private static bool EsNombreRepetido(DbUpdateException error) =>
        error.InnerException is SqliteException { SqliteExtendedErrorCode: SQLitePCL.raw.SQLITE_CONSTRAINT_UNIQUE };
}
