using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BellasBoutique.Data.Catalogo.Configuraciones;

/// <summary>
/// Reglas de la tabla Productos: campos obligatorios, nombre único, precio y stock no negativos,
/// relaciones con Categoria y Proveedor, y datos iniciales.
/// </summary>
public class ProductoConfiguracion : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("Productos", tabla =>
        {
            // EF guarda los decimal de SQLite como texto; sin el CAST la comparación sería alfabética
            // ('0.00' > '0' sería verdadero) y solo acertaría por casualidad.
            tabla.HasCheckConstraint("CK_Productos_Precio_NoNegativo", "CAST(\"Precio\" AS REAL) >= 0");
            tabla.HasCheckConstraint("CK_Productos_Stock_NoNegativo", "\"Stock\" >= 0");
        });

        builder.Property(producto => producto.Nombre)
            .IsRequired()
            .HasMaxLength(100)
            .UseCollation("NOCASE");

        builder.HasIndex(producto => producto.Nombre).IsUnique();

        builder.Property(producto => producto.Descripcion)
            .HasMaxLength(500);

        builder.Property(producto => producto.Precio)
            .HasPrecision(18, 2);

        builder.HasOne(producto => producto.Categoria)
            .WithMany()
            .HasForeignKey(producto => producto.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(producto => producto.Proveedor)
            .WithMany()
            .HasForeignKey(producto => producto.ProveedorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Producto { Id = 1, Nombre = "Blusa de lino", Precio = 18500m, Stock = 14, CategoriaId = 1, ProveedorId = 1 },
            new Producto { Id = 2, Nombre = "Vestido midi", Precio = 32000m, Stock = 8, CategoriaId = 1, ProveedorId = 1 },
            new Producto { Id = 3, Nombre = "Sandalias de cuero", Precio = 27500m, Stock = 9, CategoriaId = 2, ProveedorId = 2 },
            new Producto { Id = 4, Nombre = "Botines negros", Precio = 41000m, Stock = 6, CategoriaId = 2, ProveedorId = 2 },
            new Producto { Id = 5, Nombre = "Cartera de mano", Precio = 29900m, Stock = 12, CategoriaId = 3, ProveedorId = 3 },
            new Producto { Id = 6, Nombre = "Collar de perlas", Precio = 15900m, Stock = 20, CategoriaId = 3, ProveedorId = 3 });
    }
}
