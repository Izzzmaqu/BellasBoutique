using BellasBoutique.Application.Catalogo.ViewModels;
using BellasBoutique.Data.Catalogo;

namespace BellasBoutique.Application.Catalogo;

public static class ProductoMapeos
{
    public static IQueryable<ProductoListadoViewModel> ProyectarAListado(this IQueryable<Producto> productos) =>
        productos.Select(producto => new ProductoListadoViewModel(
            producto.Id,
            producto.Nombre,
            producto.Categoria.Nombre,
            producto.Proveedor.Nombre,
            producto.Precio,
            producto.Stock,
            producto.Stock <= ReglasCatalogo.UmbralStockBajo));

    public static IQueryable<ProductoFormularioViewModel> ProyectarAFormulario(this IQueryable<Producto> productos) =>
        productos.Select(producto => new ProductoFormularioViewModel
        {
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            Precio = (int)producto.Precio,
            Stock = producto.Stock,
            CategoriaId = producto.CategoriaId,
            ProveedorId = producto.ProveedorId
        });

    public static IQueryable<OpcionViewModel> ProyectarAOpciones(this IQueryable<Categoria> categorias) =>
        categorias.Select(categoria => new OpcionViewModel(categoria.Id, categoria.Nombre));

    public static IQueryable<OpcionViewModel> ProyectarAOpciones(this IQueryable<Proveedor> proveedores) =>
        proveedores.Select(proveedor => new OpcionViewModel(proveedor.Id, proveedor.Nombre));

    public static void CopiarA(this ProductoFormularioViewModel modelo, Producto producto)
    {
        producto.Nombre = modelo.Nombre.Trim();
        producto.Descripcion = string.IsNullOrWhiteSpace(modelo.Descripcion) ? null : modelo.Descripcion.Trim();
        producto.Precio = modelo.Precio!.Value;
        producto.Stock = modelo.Stock!.Value;
        producto.CategoriaId = modelo.CategoriaId!.Value;
        producto.ProveedorId = modelo.ProveedorId!.Value;
    }
}
