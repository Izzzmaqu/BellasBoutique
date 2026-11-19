using Microsoft.AspNetCore.Mvc;

namespace BellasBoutique.Web.Infraestructura;

public static class MensajesDeEnlaceEnEspanol
{
    public static void UsarMensajesDeEnlaceEnEspanol(this MvcOptions opciones)
    {
        var mensajes = opciones.ModelBindingMessageProvider;
        mensajes.SetValueMustBeANumberAccessor(campo => $"El campo {campo} debe ser un número.");
        mensajes.SetAttemptedValueIsInvalidAccessor((valor, campo) => $"El valor \"{valor}\" no es válido para {campo}.");
        mensajes.SetValueIsInvalidAccessor(valor => $"El valor \"{valor}\" no es válido.");
    }
}
