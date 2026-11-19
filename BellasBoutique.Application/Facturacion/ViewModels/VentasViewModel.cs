namespace BellasBoutique.Application.Facturacion.ViewModels;

public record VentasViewModel(IReadOnlyList<VentaListadoViewModel> Filas, FiltroVentas Filtro);
