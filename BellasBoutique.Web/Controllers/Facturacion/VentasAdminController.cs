using BellasBoutique.Application.Facturacion;
using BellasBoutique.Application.Facturacion.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BellasBoutique.Web.Controllers.Facturacion;

public class VentasAdminController(IVentaService servicioVentas) : Controller
{
    public async Task<IActionResult> Index(FiltroVentas filtro, CancellationToken cancelacion)
    {
        var modelo = await servicioVentas.ListarVentasAsync(filtro, cancelacion);
        return View(modelo);
    }

    public async Task<IActionResult> Factura(int id, CancellationToken cancelacion)
    {
        var factura = await servicioVentas.ObtenerFacturaAsync(id, cancelacion);
        if (factura is null)
        {
            return NotFound();
        }

        return View(factura);
    }
}
