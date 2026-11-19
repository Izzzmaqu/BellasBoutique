using BellasBoutique.Application.Facturacion.ViewModels;
using BellasBoutique.Data.Facturacion;

namespace BellasBoutique.Application.Facturacion;

public static class VentaMapeos
{
    public static IQueryable<VentaListadoViewModel> ProyectarAListado(this IQueryable<DetalleVenta> detalles) =>
        detalles.Select(detalle => new VentaListadoViewModel(
            detalle.VentaId,
            detalle.Venta.Fecha,
            detalle.Producto.Nombre,
            detalle.Cantidad,
            detalle.PrecioUnitario,
            detalle.Venta.EstadoPago));

    public static IQueryable<FacturaViewModel> ProyectarAFactura(this IQueryable<Venta> ventas) =>
        ventas.Select(venta => new FacturaViewModel(
            venta.Id,
            venta.Fecha,
            venta.ClienteId,
            venta.EstadoPago,
            venta.Detalles
                .OrderBy(detalle => detalle.Id)
                .Select(detalle => new LineaFacturaViewModel(
                    detalle.Producto.Nombre,
                    detalle.Cantidad,
                    detalle.PrecioUnitario))
                .ToList(),
            venta.Subtotal,
            venta.Iva,
            venta.Total));
}
