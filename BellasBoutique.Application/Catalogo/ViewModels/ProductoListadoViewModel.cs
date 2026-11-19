namespace BellasBoutique.Application.Catalogo.ViewModels;

public record ProductoListadoViewModel(
    int Id,
    string Nombre,
    string Categoria,
    string Proveedor,
    decimal Precio,
    int Stock,
    bool StockBajo);
