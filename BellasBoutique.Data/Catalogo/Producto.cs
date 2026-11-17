namespace BellasBoutique.Data.Catalogo;

/// <summary>Artículo que se vende en la tienda.</summary>
public class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public int CategoriaId { get; set; }

    public Categoria Categoria { get; set; } = null!;

    public int ProveedorId { get; set; }

    public Proveedor Proveedor { get; set; } = null!;

    // Baja lógica: un producto dado de baja no se borra porque las ventas pasadas lo siguen referenciando.
    public bool Activo { get; set; } = true;
}
