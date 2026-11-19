using BellasBoutique.Application.Catalogo.ViewModels;

namespace BellasBoutique.Application.Catalogo;

public interface IProductoService
{
    Task<IReadOnlyList<ProductoListadoViewModel>> ListarActivosAsync(CancellationToken cancelacion);

    Task<CatalogoViewModel> ListarCatalogoAsync(int? categoriaId, CancellationToken cancelacion);

    Task<ProductoFormularioViewModel?> ObtenerPorIdAsync(int id, CancellationToken cancelacion);

    Task<ListasDesplegablesViewModel> ObtenerListasDesplegablesAsync(CancellationToken cancelacion);

    Task<ResultadoOperacion> CrearAsync(ProductoFormularioViewModel modelo, CancellationToken cancelacion);

    Task<ResultadoOperacion> EditarAsync(int id, ProductoFormularioViewModel modelo, CancellationToken cancelacion);

    Task<ResultadoOperacion> DarDeBajaAsync(int id, CancellationToken cancelacion);
}
