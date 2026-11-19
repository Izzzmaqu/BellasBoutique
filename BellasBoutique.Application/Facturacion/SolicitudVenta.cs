using BellasBoutique.Data.Facturacion;

namespace BellasBoutique.Application.Facturacion;

// Contrato que usa el carrito: solo pide qué y cuánto; precios y montos los calcula el servidor.
public record SolicitudVenta(int ClienteId, IReadOnlyList<LineaSolicitudVenta> Lineas, EstadoPago EstadoPago);
