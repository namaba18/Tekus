using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tekus.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedSuppliers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Email", "NIT", "Name", "WebPage" },
                values: new object[,]
                {
                    { new Guid("8f1a6b1e-1c2d-4a3e-9f4a-111111111111"), "contacto@suministrosandinos.com", "900123456-7", "Suministros Andinos S.A.S.", "https://suministrosandinos.com" },
                    { new Guid("8f1a6b1e-1c2d-4a3e-9f4a-222222222222"), "ventas@tecnocaribe.com", "900654321-0", "Tecnología y Servicios del Caribe Ltda.", "https://tecnocaribe.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("8f1a6b1e-1c2d-4a3e-9f4a-111111111111"));

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("8f1a6b1e-1c2d-4a3e-9f4a-222222222222"));
        }
    }
}
