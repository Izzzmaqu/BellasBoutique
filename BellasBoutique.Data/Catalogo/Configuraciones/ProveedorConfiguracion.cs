using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BellasBoutique.Data.Catalogo.Configuraciones;

/// <summary>Reglas de la tabla Proveedores: nombre obligatorio y único, y datos iniciales.</summary>
public class ProveedorConfiguracion : IEntityTypeConfiguration<Proveedor>
{
    public void Configure(EntityTypeBuilder<Proveedor> builder)
    {
        builder.ToTable("Proveedores");

        builder.Property(proveedor => proveedor.Nombre)
            .IsRequired()
            .HasMaxLength(100)
            .UseCollation("NOCASE");

        builder.HasIndex(proveedor => proveedor.Nombre).IsUnique();

        builder.HasData(
            new Proveedor { Id = 1, Nombre = "Proveedor A" },
            new Proveedor { Id = 2, Nombre = "Proveedor B" },
            new Proveedor { Id = 3, Nombre = "Proveedor C" });
    }
}
