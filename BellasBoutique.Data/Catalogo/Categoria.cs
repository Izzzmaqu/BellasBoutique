namespace BellasBoutique.Data.Catalogo;

/// <summary>Agrupa productos por tipo, por ejemplo Ropa o Calzado.</summary>
public class Categoria
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;
}
