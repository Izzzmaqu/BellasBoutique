using System.ComponentModel.DataAnnotations;
using BellasBoutique.Data.Facturacion;

namespace BellasBoutique.Application.Facturacion.ViewModels;

public class FiltroVentas : IValidatableObject
{
    [Display(Name = "Estado")]
    public EstadoPago? Estado { get; set; }

    [Display(Name = "Desde")]
    public DateOnly? Desde { get; set; }

    [Display(Name = "Hasta")]
    public DateOnly? Hasta { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext contextoValidacion)
    {
        if (Desde > Hasta)
        {
            yield return new ValidationResult(
                "La fecha \"Desde\" no puede ser posterior a la fecha \"Hasta\".",
                [nameof(Desde)]);
        }
    }
}
