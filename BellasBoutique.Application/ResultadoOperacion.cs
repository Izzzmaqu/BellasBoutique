namespace BellasBoutique.Application;

/// <summary>
/// Resultado de una operación de escritura: si salió bien y el mensaje para el usuario.
/// Se usa en lugar de excepciones cuando una regla de negocio no se cumple.
/// </summary>
public record ResultadoOperacion(bool Exito, string Mensaje)
{
    public static ResultadoOperacion Correcto(string mensaje) => new(true, mensaje);

    public static ResultadoOperacion Fallido(string mensaje) => new(false, mensaje);
}
