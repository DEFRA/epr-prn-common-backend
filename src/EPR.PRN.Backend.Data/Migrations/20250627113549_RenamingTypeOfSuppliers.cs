using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenamingTypeOfSuppliers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TypeOfSupplier",
                table: "Public.RegistrationReprocessingIO",
                newName: "TypeOfSuppliers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TypeOfSuppliers",
                table: "Public.RegistrationReprocessingIO",
                newName: "TypeOfSupplier");
        }
    }
}
