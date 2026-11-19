using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;

namespace BellasBoutique.Web.Infraestructura;

public class ModuloViewLocationExpander : IViewLocationExpander
{
    private const string ClaveModulo = "modulo";
    private const string EspacioDeNombresDeControladores = "BellasBoutique.Web.Controllers.";

    public void PopulateValues(ViewLocationExpanderContext contexto)
    {
        if (contexto.ActionContext.ActionDescriptor is not ControllerActionDescriptor accion)
        {
            return;
        }

        var espacioDeNombres = accion.ControllerTypeInfo.Namespace ?? string.Empty;
        if (espacioDeNombres.StartsWith(EspacioDeNombresDeControladores, StringComparison.Ordinal))
        {
            var restoDelEspacioDeNombres = espacioDeNombres[EspacioDeNombresDeControladores.Length..];
            contexto.Values[ClaveModulo] = restoDelEspacioDeNombres.Split('.')[0];
        }
    }

    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext contexto, IEnumerable<string> ubicaciones)
    {
        if (!contexto.Values.TryGetValue(ClaveModulo, out var modulo) || modulo is null)
        {
            return ubicaciones;
        }

        return ubicaciones.Prepend($"/Views/{modulo}/{{1}}/{{0}}.cshtml");
    }
}
