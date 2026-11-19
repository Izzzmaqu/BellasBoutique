using BellasBoutique.Application.Catalogo;
using BellasBoutique.Application.Facturacion.ViewModels;
using BellasBoutique.Data;
using BellasBoutique.Data.Facturacion;
using Microsoft.EntityFrameworkCore;

namespace BellasBoutique.Application.Facturacion;

// Las sumas se hacen en memoria sobre las ventas del período: SQLite guarda el decimal como texto
// y sumarlo en la base de datos no es confiable. El volumen de un día o un mes es pequeño.
public class ReporteService(AppDbContext contexto) : IReporteService
{
    public async Task<ReporteViewModel> ObtenerReporteAsync(
        PeriodoReporte periodo,
        DateOnly fecha,
        CancellationToken cancelacion)
    {
        var (inicio, finExclusivo) = CalcularRango(periodo, fecha);

        var ventasExitosas = contexto.Ventas
            .AsNoTracking()
            .Where(venta => venta.EstadoPago == EstadoPago.Exitoso
                && venta.Fecha >= inicio
                && venta.Fecha < finExclusivo);

        var subtotales = await ventasExitosas
            .Select(venta => venta.Subtotal)
            .ToListAsync(cancelacion);

        var lineasVendidas = await ventasExitosas
            .SelectMany(venta => venta.Detalles)
            .Select(detalle => new LineaVendida(detalle.Producto.Nombre, detalle.Cantidad, detalle.PrecioUnitario))
            .ToListAsync(cancelacion);

        var productosVendidos = AgruparPorProducto(lineasVendidas);
        var productosConStockBajo = await ListarProductosConStockBajoAsync(cancelacion);

        return new ReporteViewModel(
            periodo,
            fecha,
            productosVendidos.Sum(producto => producto.Cantidad),
            subtotales.Sum(),
            productosVendidos,
            productosConStockBajo);
    }

    private static (DateTime Inicio, DateTime FinExclusivo) CalcularRango(PeriodoReporte periodo, DateOnly fecha)
    {
        var primerDia = periodo == PeriodoReporte.Mensual
            ? new DateOnly(fecha.Year, fecha.Month, 1)
            : fecha;

        var diaSiguienteAlPeriodo = periodo == PeriodoReporte.Mensual
            ? primerDia.AddMonths(1)
            : primerDia.AddDays(1);

        return (primerDia.ToDateTime(TimeOnly.MinValue), diaSiguienteAlPeriodo.ToDateTime(TimeOnly.MinValue));
    }

    private static List<ProductoVendidoViewModel> AgruparPorProducto(IEnumerable<LineaVendida> lineas) =>
        lineas
            .GroupBy(linea => linea.Producto)
            .Select(grupo => new ProductoVendidoViewModel(
                grupo.Key,
                grupo.Sum(linea => linea.Cantidad),
                grupo.Sum(linea => ReglasFacturacion.CalcularImporteLinea(linea.Cantidad, linea.PrecioUnitario))))
            .OrderByDescending(producto => producto.Cantidad)
            .ThenBy(producto => producto.Producto)
            .ToList();

    // El stock bajo es el estado actual del inventario, por eso no depende del período elegido.
    private async Task<List<ProductoStockBajoViewModel>> ListarProductosConStockBajoAsync(CancellationToken cancelacion)
    {
        return await contexto.Productos
            .AsNoTracking()
            .Where(producto => producto.Activo && producto.Stock <= ReglasCatalogo.UmbralStockBajo)
            .OrderBy(producto => producto.Stock)
            .ThenBy(producto => producto.Nombre)
            .Select(producto => new ProductoStockBajoViewModel(producto.Nombre, producto.Stock))
            .ToListAsync(cancelacion);
    }

    private record LineaVendida(string Producto, int Cantidad, decimal PrecioUnitario);
}
