namespace BellasBoutique.Application.Catalogo.ViewModels;

public record ProductoCatalogoViewModel(
    int Id,
    string Nombre,
    string Categoria,
    string? Descripcion,
    decimal Precio,
    int Stock);
