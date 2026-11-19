using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BellasBoutique.Web.TagHelpers;

/// <summary>
/// Envuelve un control de formulario con su etiqueta y su mensaje de error:
/// <c>&lt;campo-formulario asp-for="Nombre"&gt;&lt;input asp-for="Nombre" /&gt;&lt;/campo-formulario&gt;</c>.
/// Es un tag helper y no un partial porque un partial no puede recibir la expresión de asp-for.
/// </summary>
[HtmlTargetElement("campo-formulario", Attributes = "asp-for")]
public class CampoFormularioTagHelper(IHtmlGenerator generadorHtml) : TagHelper
{
    [HtmlAttributeName("asp-for")]
    public ModelExpression Campo { get; set; } = null!;

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = null!;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var etiqueta = generadorHtml.GenerateLabel(
            ViewContext, Campo.ModelExplorer, Campo.Name, labelText: null, htmlAttributes: null);

        var control = await output.GetChildContentAsync();

        var mensajeDeError = generadorHtml.GenerateValidationMessage(
            ViewContext, Campo.ModelExplorer, Campo.Name, message: null, tag: "span",
            htmlAttributes: new { @class = "campo__error" });

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("class", "campo");
        output.Content.AppendHtml(etiqueta);
        output.Content.AppendHtml(control);
        output.Content.AppendHtml(mensajeDeError);
    }
}
