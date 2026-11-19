namespace BellasBoutique.Application.Facturacion.ViewModels;

public record LineaFacturaViewModel(string Producto, int Cantidad, decimal PrecioUnitario)
{
    public decimal Importe => ReglasFacturacion.CalcularImporteLinea(Cantidad, PrecioUnitario);
}
