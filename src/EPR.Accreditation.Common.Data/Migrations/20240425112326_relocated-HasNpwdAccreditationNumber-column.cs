using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Accreditation.API.Common.Data.Migrations
{
    public partial class relocatedHasNpwdAccreditationNumbercolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasNpwdAccreditationNumber",
                table: "Accreditation");

            migrationBuilder.AddColumn<bool>(
                name: "HasNpwdAccreditationNumber",
                table: "AccreditationMaterial",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasNpwdAccreditationNumber",
                table: "AccreditationMaterial");

            migrationBuilder.AddColumn<bool>(
                name: "HasNpwdAccreditationNumber",
                table: "Accreditation",
                type: "bit",
                nullable: true);
        }
    }
}
