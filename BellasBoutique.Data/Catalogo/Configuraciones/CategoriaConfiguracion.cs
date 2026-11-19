using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BellasBoutique.Data.Catalogo.Configuraciones;

public class CategoriaConfiguracion : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categorias");

        builder.Property(categoria => categoria.Nombre)
            .IsRequired()
            .HasMaxLength(100)
            .UseCollation("NOCASE");

        builder.HasIndex(categoria => categoria.Nombre).IsUnique();

        builder.HasData(
            new Categoria { Id = 1, Nombre = "Ropa" },
            new Categoria { Id = 2, Nombre = "Calzado" },
            new Categoria { Id = 3, Nombre = "Accesorios" });
    }
}
