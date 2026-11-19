using Microsoft.Extensions.DependencyInjection;

namespace BellasBoutique.Application.Facturacion;

public static class RegistroDeServicios
{
    public static IServiceCollection AddFacturacion(this IServiceCollection servicios)
    {
        servicios.AddScoped<IVentaService, VentaService>();
        servicios.AddScoped<IReporteService, ReporteService>();
        return servicios;
    }
}
