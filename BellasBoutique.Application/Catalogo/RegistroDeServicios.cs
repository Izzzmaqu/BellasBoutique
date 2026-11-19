using Microsoft.Extensions.DependencyInjection;

namespace BellasBoutique.Application.Catalogo;

public static class RegistroDeServicios
{
    /// <summary>Registra los servicios del módulo Catálogo.</summary>
    public static IServiceCollection AddCatalogo(this IServiceCollection servicios)
    {
        servicios.AddScoped<IProductoService, ProductoService>();
        return servicios;
    }
}
