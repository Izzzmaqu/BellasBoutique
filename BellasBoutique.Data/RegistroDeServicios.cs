using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BellasBoutique.Data;

public static class RegistroDeServicios
{
    /// <summary>Registra AppDbContext con SQLite usando la cadena de conexión indicada.</summary>
    public static IServiceCollection AddDatos(this IServiceCollection servicios, string cadenaConexion)
    {
        servicios.AddDbContext<AppDbContext>(opciones => opciones.UseSqlite(cadenaConexion));
        return servicios;
    }
}
