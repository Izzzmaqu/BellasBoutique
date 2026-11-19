using BellasBoutique.Application.Catalogo.ViewModels;
using BellasBoutique.Data;
using BellasBoutique.Data.Catalogo;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BellasBoutique.Application.Catalogo;

// Cada operación de escritura termina en un único SaveChangesAsync. EF ejecuta ese guardado
// dentro de una transacción, así que la operación es atómica sin abrir una transacción explícita.
// Facturación sí la necesitará: al vender se lee el stock, se valida y se escriben Venta,
// DetalleVenta y el nuevo stock, y la lectura y las escrituras deben quedar en el mismo bloque.
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
        var categorias = await contexto.Categorias
            .AsNoTracking()
            .OrderBy(categoria => categoria.Nombre)
            .ProyectarAOpciones()
            .ToListAsync(cancelacion);

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

    /// <summary>Producto que todavía se puede modificar, o null si no existe o está dado de baja.</summary>
    private Task<Producto?> BuscarActivoAsync(int id, CancellationToken cancelacion) =>
        contexto.Productos.FirstOrDefaultAsync(producto => producto.Id == id && producto.Activo, cancelacion);

    /// <summary>Devuelve el mensaje de la primera regla de negocio que no se cumple, o null si se cumplen todas.</summary>
    private async Task<string?> BuscarErrorDeReglaAsync(
        ProductoFormularioViewModel modelo,
        int? idProductoEditado,
        CancellationToken cancelacion)
    {
        var nombre = modelo.Nombre.Trim();

        // La columna Nombre usa la colación NOCASE, así que en SQL esta comparación ignora mayúsculas.
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
            // Otra petición guardó el mismo nombre entre la comprobación y este guardado;
            // el índice único de la base de datos lo detuvo.
            return ResultadoOperacion.Fallido(MensajeNombreRepetido);
        }
    }

    private static bool EsNombreRepetido(DbUpdateException error) =>
        error.InnerException is SqliteException { SqliteExtendedErrorCode: SQLitePCL.raw.SQLITE_CONSTRAINT_UNIQUE };
}
