using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BellasBoutique.Data.Facturacion.Configuraciones;

public class VentaConfiguracion : IEntityTypeConfiguration<Venta>
{
    public void Configure(EntityTypeBuilder<Venta> builder)
    {
        builder.ToTable("Ventas", tabla =>
        {
            // EF guarda el decimal como texto en SQLite; sin el CAST la comparación sería entre textos, no entre números.
            tabla.HasCheckConstraint("CK_Ventas_Subtotal_NoNegativo", "CAST(\"Subtotal\" AS REAL) >= 0");
            tabla.HasCheckConstraint("CK_Ventas_Iva_NoNegativo", "CAST(\"Iva\" AS REAL) >= 0");
            tabla.HasCheckConstraint("CK_Ventas_Total_NoNegativo", "CAST(\"Total\" AS REAL) >= 0");
        });

        builder.Property(venta => venta.Subtotal)
            .HasPrecision(18, 2);

        builder.Property(venta => venta.Iva)
            .HasPrecision(18, 2);

        builder.Property(venta => venta.Total)
            .HasPrecision(18, 2);

        // Como texto y no como número: la tabla se lee sola y reordenar el enum no cambia el significado de los datos guardados.
        builder.Property(venta => venta.EstadoPago)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasIndex(venta => venta.Fecha);

        builder.HasMany(venta => venta.Detalles)
            .WithOne(detalle => detalle.Venta)
            .HasForeignKey(detalle => detalle.VentaId)
            .OnDelete(DeleteBehavior.Cascade);

        // Datos de desarrollo para probar historial y reportes; los montos siguen la regla de IVA de ReglasFacturacion.
        builder.HasData(
            CrearVenta(1, new DateTime(2026, 9, 5, 10, 15, 0), 1, 52900m, 6877m, EstadoPago.Exitoso),
            CrearVenta(2, new DateTime(2026, 9, 18, 16, 40, 0), 2, 41000m, 5330m, EstadoPago.Fallido),
            CrearVenta(3, new DateTime(2026, 9, 27, 11, 5, 0), 3, 61900m, 8047m, EstadoPago.Exitoso),
            CrearVenta(4, new DateTime(2026, 10, 3, 9, 30, 0), 1, 27500m, 3575m, EstadoPago.Pendiente),
            CrearVenta(5, new DateTime(2026, 10, 14, 14, 20, 0), 4, 47700m, 6201m, EstadoPago.Exitoso),
            CrearVenta(6, new DateTime(2026, 10, 14, 18, 45, 0), 2, 59500m, 7735m, EstadoPago.Exitoso),
            CrearVenta(7, new DateTime(2026, 10, 29, 12, 10, 0), 5, 59800m, 7774m, EstadoPago.Fallido),
            CrearVenta(8, new DateTime(2026, 11, 11, 10, 0, 0), 3, 41000m, 5330m, EstadoPago.Fallido),
            CrearVenta(9, new DateTime(2026, 11, 12, 15, 30, 0), 4, 37000m, 4810m, EstadoPago.Pendiente),
            CrearVenta(10, new DateTime(2026, 11, 12, 17, 50, 0), 5, 32000m, 4160m, EstadoPago.Exitoso),
            CrearVenta(11, new DateTime(2026, 11, 13, 9, 20, 0), 1, 103400m, 13442m, EstadoPago.Exitoso),
            CrearVenta(12, new DateTime(2026, 11, 13, 19, 5, 0), 2, 31800m, 4134m, EstadoPago.Pendiente));
    }

    private static Venta CrearVenta(
        int id,
        DateTime fecha,
        int clienteId,
        decimal subtotal,
        decimal iva,
        EstadoPago estadoPago) =>
        new()
        {
            Id = id,
            Fecha = fecha,
            ClienteId = clienteId,
            Subtotal = subtotal,
            Iva = iva,
            Total = subtotal + iva,
            EstadoPago = estadoPago
        };
}
