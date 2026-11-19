using System.Globalization;
using BellasBoutique.Application.Facturacion;
using Microsoft.AspNetCore.Mvc;

namespace BellasBoutique.Web.Controllers.Facturacion;

public class ReportesController(IReporteService servicioReportes) : Controller
{
    private const string FormatoMes = "yyyy-MM";

    public async Task<IActionResult> Index(
        PeriodoReporte periodo,
        DateOnly? fecha,
        string? mes,
        CancellationToken cancelacion)
    {
        var fechaElegida = ObtenerFechaElegida(periodo, fecha, mes);
        var reporte = await servicioReportes.ObtenerReporteAsync(periodo, fechaElegida, cancelacion);
        return View(reporte);
    }

    // El selector de mes del navegador envía "aaaa-mm", que no se enlaza solo a DateOnly.
    private static DateOnly ObtenerFechaElegida(PeriodoReporte periodo, DateOnly? fecha, string? mes)
    {
        var esMesValido = DateOnly.TryParseExact(
            mes, FormatoMes, CultureInfo.InvariantCulture, DateTimeStyles.None, out var primerDiaDelMes);

        if (periodo == PeriodoReporte.Mensual && esMesValido)
        {
            return primerDiaDelMes;
        }

        return fecha ?? DateOnly.FromDateTime(DateTime.Today);
    }
}
