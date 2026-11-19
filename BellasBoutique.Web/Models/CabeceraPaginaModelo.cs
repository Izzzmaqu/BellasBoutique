namespace BellasBoutique.Web.Models;

/// <summary>Datos del partial _CabeceraPagina: título y, si hace falta, un botón de acción principal.</summary>
public record CabeceraPaginaModelo(string Titulo, string? TextoBoton = null, string? UrlBoton = null);
