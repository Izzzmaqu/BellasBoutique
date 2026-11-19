using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BellasBoutique.Data.Facturacion.Configuraciones;

public class DetalleVentaConfiguracion : IEntityTypeConfiguration<DetalleVenta>
{
    public void Configure(EntityTypeBuilder<DetalleVenta> builder)
    {
        builder.ToTable("DetallesVenta", tabla =>
        {
            tabla.HasCheckConstraint("CK_DetallesVenta_Cantidad_Positiva", "\"Cantidad\" > 0");
            // EF guarda el decimal como texto en SQLite; sin el CAST la comparación sería entre textos, no entre números.
            tabla.HasCheckConstraint("CK_DetallesVenta_PrecioUnitario_NoNegativo", "CAST(\"PrecioUnitario\" AS REAL) >= 0");
        });

        builder.Property(detalle => detalle.PrecioUnitario)
            .HasPrecision(18, 2);

        // Restrict: un producto con ventas no se puede borrar, solo dar de baja, para no perder el historial.
        builder.HasOne(detalle => detalle.Producto)
            .WithMany()
            .HasForeignKey(detalle => detalle.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new DetalleVenta { Id = 1, VentaId = 1, ProductoId = 1, Cantidad = 2, PrecioUnitario = 18500m },
            new DetalleVenta { Id = 2, VentaId = 1, ProductoId = 6, Cantidad = 1, PrecioUnitario = 15900m },
            new DetalleVenta { Id = 3, VentaId = 2, ProductoId = 4, Cantidad = 1, PrecioUnitario = 41000m },
            new DetalleVenta { Id = 4, VentaId = 3, ProductoId = 2, Cantidad = 1, PrecioUnitario = 32000m },
            new DetalleVenta { Id = 5, VentaId = 3, ProductoId = 5, Cantidad = 1, PrecioUnitario = 29900m },
            new DetalleVenta { Id = 6, VentaId = 4, ProductoId = 3, Cantidad = 1, PrecioUnitario = 27500m },
            new DetalleVenta { Id = 7, VentaId = 5, ProductoId = 6, Cantidad = 3, PrecioUnitario = 15900m },
            new DetalleVenta { Id = 8, VentaId = 6, ProductoId = 4, Cantidad = 1, PrecioUnitario = 41000m },
            new DetalleVenta { Id = 9, VentaId = 6, ProductoId = 1, Cantidad = 1, PrecioUnitario = 18500m },
            new DetalleVenta { Id = 10, VentaId = 7, ProductoId = 5, Cantidad = 2, PrecioUnitario = 29900m },
            new DetalleVenta { Id = 11, VentaId = 8, ProductoId = 4, Cantidad = 1, PrecioUnitario = 41000m },
            new DetalleVenta { Id = 12, VentaId = 9, ProductoId = 1, Cantidad = 2, PrecioUnitario = 18500m },
            new DetalleVenta { Id = 13, VentaId = 10, ProductoId = 2, Cantidad = 1, PrecioUnitario = 32000m },
            new DetalleVenta { Id = 14, VentaId = 11, ProductoId = 1, Cantidad = 1, PrecioUnitario = 18500m },
            new DetalleVenta { Id = 15, VentaId = 11, ProductoId = 3, Cantidad = 2, PrecioUnitario = 27500m },
            new DetalleVenta { Id = 16, VentaId = 11, ProductoId = 5, Cantidad = 1, PrecioUnitario = 29900m },
            new DetalleVenta { Id = 17, VentaId = 12, ProductoId = 6, Cantidad = 2, PrecioUnitario = 15900m });
    }
}
