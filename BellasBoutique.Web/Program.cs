using System.Globalization;
using BellasBoutique.Application.Catalogo;
using BellasBoutique.Data;
using BellasBoutique.Web.Infraestructura;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

var cadenaConexion = builder.Configuration.GetConnectionString("BellasBoutique")
    ?? throw new InvalidOperationException("Falta la cadena de conexión 'BellasBoutique' en appsettings.json.");

builder.Services.AddDatos(cadenaConexion);
builder.Services.AddCatalogo();
builder.Services
    .AddControllersWithViews(opciones => opciones.UsarMensajesDeEnlaceEnEspanol())
    .AddRazorOptions(opciones => opciones.ViewLocationExpanders.Add(new ModuloViewLocationExpander()));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

var culturaCostaRica = new CultureInfo("es-CR");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culturaCostaRica),
    SupportedCultures = [culturaCostaRica],
    SupportedUICultures = [culturaCostaRica]
});

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
