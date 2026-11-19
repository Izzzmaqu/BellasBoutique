using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;

namespace BellasBoutique.Web.Infraestructura;

/// <summary>
/// Hace que un controlador de la carpeta Controllers/&lt;Modulo&gt;/ busque sus vistas en
/// Views/&lt;Modulo&gt;/&lt;Controlador&gt;/. Los controladores fuera de un módulo usan las rutas normales.
/// </summary>
public class ModuloViewLocationExpander : IViewLocationExpander
{
    private const string ClaveModulo = "modulo";
    private const string EspacioDeNombresDeControladores = "BellasBoutique.Web.Controllers.";

    // ASP.NET guarda en caché las rutas de vistas usando estos valores como parte de la clave,
    // por eso el módulo se registra aquí y no solo dentro de ExpandViewLocations.
    public void PopulateValues(ViewLocationExpanderContext contexto)
    {
        if (contexto.ActionContext.ActionDescriptor is not ControllerActionDescriptor accion)
        {
            return;
        }

        var espacioDeNombres = accion.ControllerTypeInfo.Namespace ?? string.Empty;
        if (espacioDeNombres.StartsWith(EspacioDeNombresDeControladores, StringComparison.Ordinal))
        {
            contexto.Values[ClaveModulo] = espacioDeNombres[EspacioDeNombresDeControladores.Length..];
        }
    }

    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext contexto, IEnumerable<string> ubicaciones)
    {
        if (!contexto.Values.TryGetValue(ClaveModulo, out var modulo) || modulo is null)
        {
            return ubicaciones;
        }

        // {1} es el nombre del controlador y {0} el de la vista.
        return ubicaciones.Prepend($"/Views/{modulo}/{{1}}/{{0}}.cshtml");
    }
}
