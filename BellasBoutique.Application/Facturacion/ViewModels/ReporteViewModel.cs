namespace BellasBoutique.Application.Facturacion.ViewModels;

public record ReporteViewModel(
    PeriodoReporte Periodo,
    DateOnly Fecha,
    int CantidadProductosVendidos,
    decimal Ingresos,
    IReadOnlyList<ProductoVendidoViewModel> ProductosVendidos,
    IReadOnlyList<ProductoStockBajoViewModel> ProductosConStockBajo);
