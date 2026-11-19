namespace BellasBoutique.Application.Catalogo.ViewModels;

/// <summary>Una fila de la tabla de productos.</summary>
public record ProductoListadoViewModel(
    int Id,
    string Nombre,
    string Categoria,
    string Proveedor,
    decimal Precio,
    int Stock,
    bool StockBajo);
