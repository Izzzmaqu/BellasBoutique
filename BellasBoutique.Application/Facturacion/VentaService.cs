using BellasBoutique.Application.Facturacion.ViewModels;
using BellasBoutique.Data;
using BellasBoutique.Data.Catalogo;
using BellasBoutique.Data.Facturacion;
using Microsoft.EntityFrameworkCore;

namespace BellasBoutique.Application.Facturacion;

public class VentaService(AppDbContext contexto) : IVentaService
{
    private const string MensajeStockAgotado = "El stock cambió mientras se registraba la venta. Intente de nuevo.";

    public async Task<ResultadoRegistroVenta> RegistrarVentaAsync(SolicitudVenta solicitud, CancellationToken cancelacion)
    {
        var errorDeSolicitud = BuscarErrorDeSolicitud(solicitud);
        if (errorDeSolicitud is not null)
        {
            return ResultadoRegistroVenta.Fallido(errorDeSolicitud);
        }

        var cantidadesPorProducto = SumarCantidadesPorProducto(solicitud.Lineas);
        var productos = await BuscarProductosActivosAsync(cantidadesPorProducto.Keys, cancelacion);

        var errorDeDisponibilidad = BuscarErrorDeDisponibilidad(cantidadesPorProducto, productos);
        if (errorDeDisponibilidad is not null)
        {
            return ResultadoRegistroVenta.Fallido(errorDeDisponibilidad);
        }

        var venta = CrearVenta(solicitud, cantidadesPorProducto, productos);
        return await GuardarVentaAsync(venta, cantidadesPorProducto, cancelacion);
    }

    public async Task<VentasViewModel> ListarVentasAsync(FiltroVentas filtro, CancellationToken cancelacion)
    {
        var detalles = contexto.DetallesVenta.AsNoTracking();

        if (filtro.Estado is not null)
        {
            detalles = detalles.Where(detalle => detalle.Venta.EstadoPago == filtro.Estado);
        }

        if (filtro.Desde is not null)
        {
            var inicio = filtro.Desde.Value.ToDateTime(TimeOnly.MinValue);
            detalles = detalles.Where(detalle => detalle.Venta.Fecha >= inicio);
        }

        if (filtro.Hasta is not null)
        {
            // "Hasta" incluye el día completo: se compara contra el inicio del día siguiente.
            var finExclusivo = RangoDeFechas.InicioDelDiaSiguiente(filtro.Hasta.Value);
            detalles = detalles.Where(detalle => detalle.Venta.Fecha < finExclusivo);
        }

        var filas = await detalles
            .OrderByDescending(detalle => detalle.Venta.Fecha)
            .ThenByDescending(detalle => detalle.VentaId)
            .ThenBy(detalle => detalle.Id)
            .ProyectarAListado()
            .ToListAsync(cancelacion);

        return new VentasViewModel(filas, filtro);
    }

    public async Task<FacturaViewModel?> ObtenerFacturaAsync(int ventaId, CancellationToken cancelacion)
    {
        return await contexto.Ventas
            .AsNoTracking()
            .Where(venta => venta.Id == ventaId)
            .ProyectarAFactura()
            .FirstOrDefaultAsync(cancelacion);
    }

    private static string? BuscarErrorDeSolicitud(SolicitudVenta solicitud)
    {
        if (solicitud.ClienteId <= 0)
        {
            return "El cliente no es válido.";
        }

        if (solicitud.Lineas is null || solicitud.Lineas.Count == 0)
        {
            return "La venta no tiene productos.";
        }

        if (solicitud.Lineas.Any(linea => linea.Cantidad <= 0))
        {
            return "Las cantidades deben ser mayores que cero.";
        }

        if (!Enum.IsDefined(solicitud.EstadoPago))
        {
            return "El estado de pago no es válido.";
        }

        return null;
    }

    // Si el carrito manda el mismo producto en dos líneas, se valida y se descuenta el stock por el total.
    private static Dictionary<int, int> SumarCantidadesPorProducto(IEnumerable<LineaSolicitudVenta> lineas) =>
        lineas
            .GroupBy(linea => linea.ProductoId)
            .ToDictionary(grupo => grupo.Key, grupo => grupo.Sum(linea => linea.Cantidad));

    private async Task<Dictionary<int, Producto>> BuscarProductosActivosAsync(
        IEnumerable<int> productoIds,
        CancellationToken cancelacion)
    {
        return await contexto.Productos
            .AsNoTracking()
            .Where(producto => productoIds.Contains(producto.Id) && producto.Activo)
            .ToDictionaryAsync(producto => producto.Id, cancelacion);
    }

    private static string? BuscarErrorDeDisponibilidad(
        Dictionary<int, int> cantidadesPorProducto,
        Dictionary<int, Producto> productos)
    {
        foreach (var (productoId, cantidad) in cantidadesPorProducto)
        {
            if (!productos.TryGetValue(productoId, out var producto))
            {
                return $"El producto {productoId} no existe o está dado de baja.";
            }

            if (producto.Stock < cantidad)
            {
                return $"No hay stock suficiente de \"{producto.Nombre}\" (disponible: {producto.Stock}).";
            }
        }

        return null;
    }

    // El precio sale de la base de datos y no de la solicitud, para que el cliente no pueda fijar su propio precio.
    private static Venta CrearVenta(
        SolicitudVenta solicitud,
        Dictionary<int, int> cantidadesPorProducto,
        Dictionary<int, Producto> productos)
    {
        var detalles = cantidadesPorProducto
            .Select(par => new DetalleVenta
            {
                ProductoId = par.Key,
                Cantidad = par.Value,
                PrecioUnitario = productos[par.Key].Precio
            })
            .ToList();

        var subtotal = detalles.Sum(detalle =>
            ReglasFacturacion.CalcularImporteLinea(detalle.Cantidad, detalle.PrecioUnitario));
        var totales = ReglasFacturacion.CalcularTotales(subtotal);

        return new Venta
        {
            Fecha = DateTime.Now,
            ClienteId = solicitud.ClienteId,
            Subtotal = totales.Subtotal,
            Iva = totales.Iva,
            Total = totales.Total,
            EstadoPago = solicitud.EstadoPago,
            Detalles = detalles
        };
    }

    // Transacción explícita: ExecuteUpdateAsync no participa de la transacción implícita de SaveChangesAsync,
    // así que sin ella la venta podría quedar guardada sin descontar el stock (o al revés).
    private async Task<ResultadoRegistroVenta> GuardarVentaAsync(
        Venta venta,
        Dictionary<int, int> cantidadesPorProducto,
        CancellationToken cancelacion)
    {
        await using var transaccion = await contexto.Database.BeginTransactionAsync(cancelacion);

        contexto.Ventas.Add(venta);
        await contexto.SaveChangesAsync(cancelacion);

        if (venta.EstadoPago == EstadoPago.Exitoso)
        {
            var stockDescontado = await DescontarStockAsync(cantidadesPorProducto, cancelacion);
            if (!stockDescontado)
            {
                await transaccion.RollbackAsync(cancelacion);
                contexto.ChangeTracker.Clear();
                return ResultadoRegistroVenta.Fallido(MensajeStockAgotado);
            }
        }

        await transaccion.CommitAsync(cancelacion);

        var numeroFactura = ReglasFacturacion.FormatearNumeroFactura(venta.Id);
        return ResultadoRegistroVenta.Correcto($"Se registró la venta con la factura N.º {numeroFactura}.", venta.Id);
    }

    // La condición Stock >= cantidad va dentro del UPDATE: si otra compra se llevó el stock después de validarlo,
    // no se actualiza ninguna fila y la venta se revierte en lugar de dejar el stock negativo.
    private async Task<bool> DescontarStockAsync(Dictionary<int, int> cantidadesPorProducto, CancellationToken cancelacion)
    {
        foreach (var (productoId, cantidad) in cantidadesPorProducto)
        {
            var filasActualizadas = await contexto.Productos
                .Where(producto => producto.Id == productoId && producto.Stock >= cantidad)
                .ExecuteUpdateAsync(
                    cambios => cambios.SetProperty(producto => producto.Stock, producto => producto.Stock - cantidad),
                    cancelacion);

            if (filasActualizadas == 0)
            {
                return false;
            }
        }

        return true;
    }
}
