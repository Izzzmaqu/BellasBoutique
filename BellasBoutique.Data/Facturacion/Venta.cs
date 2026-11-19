namespace BellasBoutique.Data.Facturacion;

public class Venta
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    // Sin FK todavía: la entidad Cliente la define el módulo de clientes de otro integrante.
    public int ClienteId { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Iva { get; set; }

    public decimal Total { get; set; }

    public EstadoPago EstadoPago { get; set; }

    public List<DetalleVenta> Detalles { get; set; } = [];
}
