using BellasBoutique.Data.Facturacion;

namespace BellasBoutique.Application.Facturacion.ViewModels;

public record VentaListadoViewModel(
    int VentaId,
    DateTime Fecha,
    string Producto,
    int Cantidad,
    decimal PrecioUnitario,
    EstadoPago EstadoPago)
{
    public string NumeroFactura => ReglasFacturacion.FormatearNumeroFactura(VentaId);

    public decimal Importe => ReglasFacturacion.CalcularImporteLinea(Cantidad, PrecioUnitario);
}
