using BellasBoutique.Data.Catalogo;
using Microsoft.EntityFrameworkCore;

namespace BellasBoutique.Data;

/// <summary>
/// Contexto único de la base de datos. Cada módulo agrega aquí sus DbSet y
/// pone sus reglas en clases IEntityTypeConfiguration dentro de su carpeta.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> opciones) : DbContext(opciones)
{
    public DbSet<Categoria> Categorias => Set<Categoria>();

    public DbSet<Proveedor> Proveedores => Set<Proveedor>();

    public DbSet<Producto> Productos => Set<Producto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
