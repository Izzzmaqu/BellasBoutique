using BellasBoutique.Application.Facturacion.ViewModels;

namespace BellasBoutique.Application.Facturacion;

public interface IVentaService
{
    Task<ResultadoRegistroVenta> RegistrarVentaAsync(SolicitudVenta solicitud, CancellationToken cancelacion);

    Task<VentasViewModel> ListarVentasAsync(FiltroVentas filtro, CancellationToken cancelacion);

    Task<FacturaViewModel?> ObtenerFacturaAsync(int ventaId, CancellationToken cancelacion);
}
