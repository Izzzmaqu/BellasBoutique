using Microsoft.AspNetCore.Mvc;

namespace BellasBoutique.Web.Infraestructura;

public static class MensajesDeEnlaceEnEspanol
{
    /// <summary>
    /// Traduce los mensajes que ASP.NET genera por su cuenta cuando un valor no se puede convertir
    /// (por ejemplo, letras en un campo numérico). Sin esto saldrían en inglés.
    /// </summary>
    public static void UsarMensajesDeEnlaceEnEspanol(this MvcOptions opciones)
    {
        var mensajes = opciones.ModelBindingMessageProvider;
        mensajes.SetValueMustBeANumberAccessor(campo => $"El campo {campo} debe ser un número.");
        mensajes.SetAttemptedValueIsInvalidAccessor((valor, campo) => $"El valor \"{valor}\" no es válido para {campo}.");
        mensajes.SetValueIsInvalidAccessor(valor => $"El valor \"{valor}\" no es válido.");
    }
}
