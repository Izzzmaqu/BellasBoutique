namespace BellasBoutique.Application.Catalogo.ViewModels;

public record CatalogoViewModel(
    IReadOnlyList<ProductoCatalogoViewModel> Productos,
    IReadOnlyList<OpcionViewModel> Categorias,
    int? CategoriaSeleccionadaId);
