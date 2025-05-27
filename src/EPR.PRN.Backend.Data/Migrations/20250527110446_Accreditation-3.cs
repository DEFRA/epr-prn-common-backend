using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class Accreditation3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationReference",
                table: "Public.Accreditation");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationReferenceNumber",
                table: "Public.Accreditation",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationReferenceNumber",
                table: "Public.Accreditation");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationReference",
                table: "Public.Accreditation",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
