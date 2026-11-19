using BellasBoutique.Data.Catalogo;

namespace BellasBoutique.Data.Facturacion;

public class DetalleVenta
{
    public int Id { get; set; }

    public int VentaId { get; set; }

    public Venta Venta { get; set; } = null!;

    public int ProductoId { get; set; }

    public Producto Producto { get; set; } = null!;

    public int Cantidad { get; set; }

    // Foto del precio al momento de la venta: si el producto cambia de precio, la factura no cambia.
    public decimal PrecioUnitario { get; set; }
}
