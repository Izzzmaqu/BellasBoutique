namespace BellasBoutique.Application;

public record ResultadoOperacion(bool Exito, string Mensaje)
{
    public static ResultadoOperacion Correcto(string mensaje) => new(true, mensaje);

    public static ResultadoOperacion Fallido(string mensaje) => new(false, mensaje);
}
