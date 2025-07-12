using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "60628d11-b8b2-40d0-bcf3-fb9e60f76f30", "8bccf7f0-8125-4d99-8072-9951990d25a4", "Student", "STUDENT" },
                    { "751f7dfb-f40e-4c20-af72-26012c4b5ef4", "6b6366a6-3a14-4962-afe9-03c50aa87b71", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60628d11-b8b2-40d0-bcf3-fb9e60f76f30");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "751f7dfb-f40e-4c20-af72-26012c4b5ef4");
        }
    }
}
