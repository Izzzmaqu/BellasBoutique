using BellasBoutique.Data.Catalogo;
using Microsoft.EntityFrameworkCore;

namespace BellasBoutique.Data;

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
