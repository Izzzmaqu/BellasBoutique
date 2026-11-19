namespace BellasBoutique.Application.Facturacion;

public static class RangoDeFechas
{
    // El último día del calendario no tiene día siguiente: en ese caso no hay límite superior y no se lanza excepción.
    public static DateTime InicioDelDiaSiguiente(DateOnly dia) =>
        dia == DateOnly.MaxValue
            ? DateTime.MaxValue
            : dia.AddDays(1).ToDateTime(TimeOnly.MinValue);
}
