using BellasBoutique.Application.Catalogo.ViewModels;

namespace BellasBoutique.Application.Catalogo;

/// <summary>Casos de uso de la gestión de productos del administrador.</summary>
public interface IProductoService
{
    /// <summary>Productos activos ordenados por nombre. Los dados de baja no aparecen.</summary>
    Task<IReadOnlyList<ProductoListadoViewModel>> ListarActivosAsync(CancellationToken cancelacion);

    /// <summary>Datos del producto para editarlo, o null si no existe o está dado de baja.</summary>
    Task<ProductoFormularioViewModel?> ObtenerPorIdAsync(int id, CancellationToken cancelacion);

    /// <summary>Opciones de categoría y proveedor para el formulario.</summary>
    Task<ListasDesplegablesViewModel> ObtenerListasDesplegablesAsync(CancellationToken cancelacion);

    /// <summary>Crea un producto activo. El modelo debe llegar ya validado.</summary>
    Task<ResultadoOperacion> CrearAsync(ProductoFormularioViewModel modelo, CancellationToken cancelacion);

    /// <summary>Actualiza un producto activo. El modelo debe llegar ya validado.</summary>
    Task<ResultadoOperacion> EditarAsync(int id, ProductoFormularioViewModel modelo, CancellationToken cancelacion);

    /// <summary>Baja lógica: marca el producto como inactivo sin borrarlo.</summary>
    Task<ResultadoOperacion> DarDeBajaAsync(int id, CancellationToken cancelacion);
}
