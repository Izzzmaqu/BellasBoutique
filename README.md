# Bellas Boutique

Tienda en línea de ropa y accesorios.

## Stack

- .NET 10
- ASP.NET Core MVC

## Estructura de capas

- `BellasBoutique.Web`: controladores, vistas y archivos estáticos.
- `BellasBoutique.Application`: casos de uso y lógica de negocio.
- `BellasBoutique.Data`: acceso a datos.

Cada capa se organiza en subcarpetas por módulo: `Auth`, `Catalogo`, `Carrito`, `Facturacion`, `Soporte`.
En `Web` las subcarpetas están dentro de `Controllers` y `Views`: un controlador en `Controllers/<Modulo>/` busca sus vistas en `Views/<Modulo>/<Controlador>/` (lo resuelve `Infraestructura/ModuloViewLocationExpander.cs`). Por ejemplo, `Controllers/Catalogo/ProductosAdminController.cs` usa `Views/Catalogo/ProductosAdmin/`.

Los estilos comparten los tokens de `wwwroot/css/tokens.css`; `wwwroot/css/base.css` los usa y no define valores propios.

## Ramas

- Una rama por módulo: `feature/<modulo>`.
- Los cambios llegan a `main` mediante pull request.

## Ejecutar

La primera vez, y después de cada migración nueva, crea o actualiza la base de datos SQLite (`bellas.db`, no se versiona):

```powershell
dotnet ef database update --project BellasBoutique.Data --startup-project BellasBoutique.Web
dotnet run --project BellasBoutique.Web
```
