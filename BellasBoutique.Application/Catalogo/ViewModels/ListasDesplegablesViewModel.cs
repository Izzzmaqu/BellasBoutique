namespace BellasBoutique.Application.Catalogo.ViewModels;

public record ListasDesplegablesViewModel(
    IReadOnlyList<OpcionViewModel> Categorias,
    IReadOnlyList<OpcionViewModel> Proveedores)
{
    public static ListasDesplegablesViewModel Vacias { get; } = new([], []);
}
