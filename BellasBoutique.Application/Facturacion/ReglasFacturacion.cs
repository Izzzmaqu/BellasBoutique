namespace BellasBoutique.Application.Facturacion;

public static class ReglasFacturacion
{
    public const decimal TasaIva = 0.13m;

    public static decimal CalcularImporteLinea(int cantidad, decimal precioUnitario) => cantidad * precioUnitario;

    // El IVA se calcula una sola vez sobre el subtotal de la venta y se redondea al colón,
    // así lo que se guarda es exactamente lo que se muestra en la factura.
    public static TotalesVenta CalcularTotales(decimal subtotal)
    {
        var iva = Math.Round(subtotal * TasaIva, 0, MidpointRounding.AwayFromZero);
        return new TotalesVenta(subtotal, iva, subtotal + iva);
    }

    public static string FormatearNumeroFactura(int ventaId) => ventaId.ToString("D6");
}
