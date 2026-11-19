using BellasBoutique.Data.Facturacion;

namespace BellasBoutique.Application.Facturacion.ViewModels;

public record FacturaViewModel(
    int VentaId,
    DateTime Fecha,
    int ClienteId,
    EstadoPago EstadoPago,
    IReadOnlyList<LineaFacturaViewModel> Lineas,
    decimal Subtotal,
    decimal Iva,
    decimal Total)
{
    public string NumeroFactura => ReglasFacturacion.FormatearNumeroFactura(VentaId);
}
