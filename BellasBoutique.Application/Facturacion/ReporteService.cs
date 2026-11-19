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

        // Una sola consulta (ventas con sus líneas) para que la tarjeta de ingresos y la tabla salgan de la misma lectura.
        var ventasExitosas = await contexto.Ventas
            .AsNoTracking()
            .Where(venta => venta.EstadoPago == EstadoPago.Exitoso
                && venta.Fecha >= inicio
                && venta.Fecha < finExclusivo)
            .Select(venta => new VentaDelPeriodo(
                venta.Subtotal,
                venta.Detalles
                    .Select(detalle => new LineaVendida(detalle.Producto.Nombre, detalle.Cantidad, detalle.PrecioUnitario))
                    .ToList()))
            .ToListAsync(cancelacion);

        var productosVendidos = AgruparPorProducto(ventasExitosas.SelectMany(venta => venta.Lineas));
        var productosConStockBajo = await ListarProductosConStockBajoAsync(cancelacion);

        return new ReporteViewModel(
            periodo,
            fecha,
            productosVendidos.Sum(producto => producto.Cantidad),
            ventasExitosas.Sum(venta => venta.Subtotal),
            productosVendidos,
            productosConStockBajo);
    }

    private static (DateTime Inicio, DateTime FinExclusivo) CalcularRango(PeriodoReporte periodo, DateOnly fecha)
    {
        var primerDia = periodo == PeriodoReporte.Mensual
            ? new DateOnly(fecha.Year, fecha.Month, 1)
            : fecha;

        var ultimoDia = periodo == PeriodoReporte.Mensual
            ? new DateOnly(fecha.Year, fecha.Month, DateTime.DaysInMonth(fecha.Year, fecha.Month))
            : fecha;

        return (primerDia.ToDateTime(TimeOnly.MinValue), RangoDeFechas.InicioDelDiaSiguiente(ultimoDia));
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

    private record VentaDelPeriodo(decimal Subtotal, List<LineaVendida> Lineas);

    private record LineaVendida(string Producto, int Cantidad, decimal PrecioUnitario);
}
