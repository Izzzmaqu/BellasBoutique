using System.ComponentModel.DataAnnotations;

namespace BellasBoutique.Application.Catalogo.ViewModels;

public class ProductoFormularioViewModel
{
    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Display(Name = "Descripción")]
    [StringLength(500, ErrorMessage = "La descripción no puede tener más de 500 caracteres.")]
    public string? Descripcion { get; set; }

    [Display(Name = "Precio")]
    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
    public int? Precio { get; set; }

    [Display(Name = "Stock")]
    [Required(ErrorMessage = "El stock es obligatorio.")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    public int? Stock { get; set; }

    [Display(Name = "Categoría")]
    [Required(ErrorMessage = "Seleccione una categoría.")]
    public int? CategoriaId { get; set; }

    [Display(Name = "Proveedor")]
    [Required(ErrorMessage = "Seleccione un proveedor.")]
    public int? ProveedorId { get; set; }

    public ListasDesplegablesViewModel Listas { get; set; } = ListasDesplegablesViewModel.Vacias;
}
