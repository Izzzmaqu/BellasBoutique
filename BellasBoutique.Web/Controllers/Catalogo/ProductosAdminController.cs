using BellasBoutique.Application;
using BellasBoutique.Application.Catalogo;
using BellasBoutique.Application.Catalogo.ViewModels;
using BellasBoutique.Web.Infraestructura;
using Microsoft.AspNetCore.Mvc;

namespace BellasBoutique.Web.Controllers.Catalogo;

public class ProductosAdminController(IProductoService servicioProductos) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancelacion)
    {
        var productos = await servicioProductos.ListarActivosAsync(cancelacion);
        return View(productos);
    }

    [HttpGet]
    public async Task<IActionResult> Crear(CancellationToken cancelacion)
    {
        return await MostrarFormularioAsync(new ProductoFormularioViewModel(), cancelacion);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(ProductoFormularioViewModel modelo, CancellationToken cancelacion)
    {
        if (!ModelState.IsValid)
        {
            return await MostrarFormularioAsync(modelo, cancelacion);
        }

        var resultado = await servicioProductos.CrearAsync(modelo, cancelacion);
        if (!resultado.Exito)
        {
            return await MostrarFormularioAsync(modelo, cancelacion, resultado.Mensaje);
        }

        return VolverAlListado(resultado);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id, CancellationToken cancelacion)
    {
        var modelo = await servicioProductos.ObtenerPorIdAsync(id, cancelacion);
        if (modelo is null)
        {
            return NotFound();
        }

        return await MostrarFormularioAsync(modelo, cancelacion);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, ProductoFormularioViewModel modelo, CancellationToken cancelacion)
    {
        if (!ModelState.IsValid)
        {
            return await MostrarFormularioAsync(modelo, cancelacion);
        }

        var resultado = await servicioProductos.EditarAsync(id, modelo, cancelacion);
        if (!resultado.Exito)
        {
            return await MostrarFormularioAsync(modelo, cancelacion, resultado.Mensaje);
        }

        return VolverAlListado(resultado);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DarDeBaja(int id, CancellationToken cancelacion)
    {
        var resultado = await servicioProductos.DarDeBajaAsync(id, cancelacion);
        return VolverAlListado(resultado);
    }

    private async Task<IActionResult> MostrarFormularioAsync(
        ProductoFormularioViewModel modelo,
        CancellationToken cancelacion,
        string? mensajeDeError = null)
    {
        if (mensajeDeError is not null)
        {
            ModelState.AddModelError(string.Empty, mensajeDeError);
        }

        modelo.Listas = await servicioProductos.ObtenerListasDesplegablesAsync(cancelacion);
        return View(modelo);
    }

    private RedirectToActionResult VolverAlListado(ResultadoOperacion resultado)
    {
        var clave = resultado.Exito ? ClavesTempData.Exito : ClavesTempData.Error;
        TempData[clave] = resultado.Mensaje;
        return RedirectToAction(nameof(Index));
    }
}
