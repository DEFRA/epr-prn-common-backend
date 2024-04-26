using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Accreditation.API.Common.Data.Migrations
{
    public partial class AccreditationMaterialdefaultcolumnstonull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "WeeklyCapacity",
                table: "AccreditationMaterial",
                type: "decimal(10,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AnnualCapacity",
                table: "AccreditationMaterial",
                type: "decimal(10,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,3)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "WeeklyCapacity",
                table: "AccreditationMaterial",
                type: "decimal(10,3)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AnnualCapacity",
                table: "AccreditationMaterial",
                type: "decimal(10,3)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,3)",
                oldNullable: true);
        }
    }
}
