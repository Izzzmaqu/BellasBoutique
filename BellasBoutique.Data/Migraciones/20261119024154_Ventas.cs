using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BellasBoutique.Data.Migraciones
{
    /// <inheritdoc />
    public partial class Ventas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ventas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    Subtotal = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Iva = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    EstadoPago = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ventas", x => x.Id);
                    table.CheckConstraint("CK_Ventas_Iva_NoNegativo", "CAST(\"Iva\" AS REAL) >= 0");
                    table.CheckConstraint("CK_Ventas_Subtotal_NoNegativo", "CAST(\"Subtotal\" AS REAL) >= 0");
                    table.CheckConstraint("CK_Ventas_Total_NoNegativo", "CAST(\"Total\" AS REAL) >= 0");
                });

            migrationBuilder.CreateTable(
                name: "DetallesVenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VentaId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Cantidad = table.Column<int>(type: "INTEGER", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesVenta", x => x.Id);
                    table.CheckConstraint("CK_DetallesVenta_Cantidad_Positiva", "\"Cantidad\" > 0");
                    table.CheckConstraint("CK_DetallesVenta_PrecioUnitario_NoNegativo", "CAST(\"PrecioUnitario\" AS REAL) >= 0");
                    table.ForeignKey(
                        name: "FK_DetallesVenta_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetallesVenta_Ventas_VentaId",
                        column: x => x.VentaId,
                        principalTable: "Ventas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Ventas",
                columns: new[] { "Id", "ClienteId", "EstadoPago", "Fecha", "Iva", "Subtotal", "Total" },
                values: new object[,]
                {
                    { 1, 1, "Exitoso", new DateTime(2026, 9, 5, 10, 15, 0, 0, DateTimeKind.Unspecified), 6877m, 52900m, 59777m },
                    { 2, 2, "Fallido", new DateTime(2026, 9, 18, 16, 40, 0, 0, DateTimeKind.Unspecified), 5330m, 41000m, 46330m },
                    { 3, 3, "Exitoso", new DateTime(2026, 9, 27, 11, 5, 0, 0, DateTimeKind.Unspecified), 8047m, 61900m, 69947m },
                    { 4, 1, "Pendiente", new DateTime(2026, 10, 3, 9, 30, 0, 0, DateTimeKind.Unspecified), 3575m, 27500m, 31075m },
                    { 5, 4, "Exitoso", new DateTime(2026, 10, 14, 14, 20, 0, 0, DateTimeKind.Unspecified), 6201m, 47700m, 53901m },
                    { 6, 2, "Exitoso", new DateTime(2026, 10, 14, 18, 45, 0, 0, DateTimeKind.Unspecified), 7735m, 59500m, 67235m },
                    { 7, 5, "Fallido", new DateTime(2026, 10, 29, 12, 10, 0, 0, DateTimeKind.Unspecified), 7774m, 59800m, 67574m },
                    { 8, 3, "Fallido", new DateTime(2026, 11, 11, 10, 0, 0, 0, DateTimeKind.Unspecified), 5330m, 41000m, 46330m },
                    { 9, 4, "Pendiente", new DateTime(2026, 11, 12, 15, 30, 0, 0, DateTimeKind.Unspecified), 4810m, 37000m, 41810m },
                    { 10, 5, "Exitoso", new DateTime(2026, 11, 12, 17, 50, 0, 0, DateTimeKind.Unspecified), 4160m, 32000m, 36160m },
                    { 11, 1, "Exitoso", new DateTime(2026, 11, 13, 9, 20, 0, 0, DateTimeKind.Unspecified), 13442m, 103400m, 116842m },
                    { 12, 2, "Pendiente", new DateTime(2026, 11, 13, 19, 5, 0, 0, DateTimeKind.Unspecified), 4134m, 31800m, 35934m }
                });

            migrationBuilder.InsertData(
                table: "DetallesVenta",
                columns: new[] { "Id", "Cantidad", "PrecioUnitario", "ProductoId", "VentaId" },
                values: new object[,]
                {
                    { 1, 2, 18500m, 1, 1 },
                    { 2, 1, 15900m, 6, 1 },
                    { 3, 1, 41000m, 4, 2 },
                    { 4, 1, 32000m, 2, 3 },
                    { 5, 1, 29900m, 5, 3 },
                    { 6, 1, 27500m, 3, 4 },
                    { 7, 3, 15900m, 6, 5 },
                    { 8, 1, 41000m, 4, 6 },
                    { 9, 1, 18500m, 1, 6 },
                    { 10, 2, 29900m, 5, 7 },
                    { 11, 1, 41000m, 4, 8 },
                    { 12, 2, 18500m, 1, 9 },
                    { 13, 1, 32000m, 2, 10 },
                    { 14, 1, 18500m, 1, 11 },
                    { 15, 2, 27500m, 3, 11 },
                    { 16, 1, 29900m, 5, 11 },
                    { 17, 2, 15900m, 6, 12 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallesVenta_ProductoId",
                table: "DetallesVenta",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesVenta_VentaId",
                table: "DetallesVenta",
                column: "VentaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_Fecha",
                table: "Ventas",
                column: "Fecha");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesVenta");

            migrationBuilder.DropTable(
                name: "Ventas");
        }
    }
}
