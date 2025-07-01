using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInProgressRegistrationMaterialStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Lookup.RegistrationMaterialStatus",
                columns: new[] { "Id", "Name" },
                values: new object[] { 12, "InProgress" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Lookup.RegistrationMaterialStatus",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
