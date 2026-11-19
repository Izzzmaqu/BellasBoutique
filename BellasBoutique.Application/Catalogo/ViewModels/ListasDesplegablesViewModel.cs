namespace BellasBoutique.Application.Catalogo.ViewModels;

/// <summary>Opciones para los select de categoría y proveedor del formulario de producto.</summary>
public record ListasDesplegablesViewModel(
    IReadOnlyList<OpcionViewModel> Categorias,
    IReadOnlyList<OpcionViewModel> Proveedores)
{
    public static ListasDesplegablesViewModel Vacias { get; } = new([], []);
}
