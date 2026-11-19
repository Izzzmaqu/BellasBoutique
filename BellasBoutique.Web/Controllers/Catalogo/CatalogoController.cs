using BellasBoutique.Application.Catalogo;
using Microsoft.AspNetCore.Mvc;

namespace BellasBoutique.Web.Controllers.Catalogo;

public class CatalogoController(IProductoService servicioProductos) : Controller
{
    public async Task<IActionResult> Index(int? categoriaId, CancellationToken cancelacion)
    {
        var catalogo = await servicioProductos.ListarCatalogoAsync(categoriaId, cancelacion);
        return View(catalogo);
    }
}
