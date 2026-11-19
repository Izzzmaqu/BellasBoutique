using BellasBoutique.Data.Catalogo;
using BellasBoutique.Data.Facturacion;
using Microsoft.EntityFrameworkCore;

namespace BellasBoutique.Data;

public class AppDbContext(DbContextOptions<AppDbContext> opciones) : DbContext(opciones)
{
    public DbSet<Categoria> Categorias => Set<Categoria>();

    public DbSet<Proveedor> Proveedores => Set<Proveedor>();

    public DbSet<Producto> Productos => Set<Producto>();

    public DbSet<Venta> Ventas => Set<Venta>();

    public DbSet<DetalleVenta> DetallesVenta => Set<DetalleVenta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
