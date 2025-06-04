using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AmendedMaterialToRemoveBoolColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCaculable",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "IsVisibleToObligation",
                table: "Material");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCaculable",
                table: "Material",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisibleToObligation",
                table: "Material",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 1,
                columns: ["IsCaculable", "IsVisibleToObligation"],
                values: [true, true]);

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 2,
                columns: ["IsCaculable", "IsVisibleToObligation"],
                values: [true, true]);

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 3,
                columns: ["IsCaculable", "IsVisibleToObligation"],
                values: [true, true]);

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 4,
                columns: ["IsCaculable", "IsVisibleToObligation"],
                values: [true, true]);

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 5,
                columns: ["IsCaculable", "IsVisibleToObligation"],
                values: [true, true]);

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 6,
                columns: ["IsCaculable", "IsVisibleToObligation"],
                values: [true, true]);

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 7,
                columns: ["IsCaculable", "IsVisibleToObligation"],
                values: [false, true]);

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 8,
                columns: ["IsCaculable", "IsVisibleToObligation"],
                values: [true, false]);
        }
    }
}
