using Microsoft.Extensions.DependencyInjection;

namespace BellasBoutique.Application.Catalogo;

public static class RegistroDeServicios
{
    public static IServiceCollection AddCatalogo(this IServiceCollection servicios)
    {
        servicios.AddScoped<IProductoService, ProductoService>();
        return servicios;
    }
}
