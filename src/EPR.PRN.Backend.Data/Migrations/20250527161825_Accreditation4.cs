using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class Accreditation4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Lookup.RegistrationMaterialStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 3, "Started" },
                    { 4, "Submitted" },
                    { 5, "RegulatorReviewing" },
                    { 6, "Queried" },
                    { 7, "Updated" },
                    { 8, "Withdrawn" },
                    { 9, "Suspended" },
                    { 10, "Cancelled" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Lookup.RegistrationMaterialStatus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Lookup.RegistrationMaterialStatus",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Lookup.RegistrationMaterialStatus",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Lookup.RegistrationMaterialStatus",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Lookup.RegistrationMaterialStatus",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Lookup.RegistrationMaterialStatus",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Lookup.RegistrationMaterialStatus",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Lookup.RegistrationMaterialStatus",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
