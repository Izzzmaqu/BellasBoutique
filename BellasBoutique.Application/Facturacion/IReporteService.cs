using BellasBoutique.Application.Facturacion.ViewModels;

namespace BellasBoutique.Application.Facturacion;

public interface IReporteService
{
    Task<ReporteViewModel> ObtenerReporteAsync(PeriodoReporte periodo, DateOnly fecha, CancellationToken cancelacion);
}
