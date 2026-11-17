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
En `Web` las subcarpetas están dentro de `Controllers` y `Views`.

Los estilos comparten los tokens de `wwwroot/css/tokens.css`; `wwwroot/css/base.css` los usa y no define valores propios.

## Ramas

- Una rama por módulo: `feature/<modulo>`.
- Los cambios llegan a `main` mediante pull request.

## Ejecutar

```powershell
dotnet run --project BellasBoutique.Web
```
