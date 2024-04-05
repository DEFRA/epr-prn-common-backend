using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Accreditation.API.Common.Data.Migrations
{
    public partial class MoveWasteLastYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WasteLastYear",
                table: "MaterialReprocessorDetails");

            migrationBuilder.AddColumn<bool>(
                name: "WasteLastYear",
                table: "AccreditationMaterial",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WasteLastYear",
                table: "AccreditationMaterial");

            migrationBuilder.AddColumn<bool>(
                name: "WasteLastYear",
                table: "MaterialReprocessorDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
