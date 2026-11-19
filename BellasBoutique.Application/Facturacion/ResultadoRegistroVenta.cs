namespace BellasBoutique.Application.Facturacion;

// Igual que ResultadoOperacion, pero con el Id de la venta para que el carrito pueda abrir la factura.
public record ResultadoRegistroVenta(bool Exito, string Mensaje, int? VentaId)
{
    public static ResultadoRegistroVenta Correcto(string mensaje, int ventaId) => new(true, mensaje, ventaId);

    public static ResultadoRegistroVenta Fallido(string mensaje) => new(false, mensaje, null);
}
