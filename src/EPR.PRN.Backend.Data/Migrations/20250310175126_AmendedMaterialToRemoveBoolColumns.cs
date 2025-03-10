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
                columns: new[] { "IsCaculable", "IsVisibleToObligation" },
                values: new object[] { true, true });

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsCaculable", "IsVisibleToObligation" },
                values: new object[] { true, true });

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsCaculable", "IsVisibleToObligation" },
                values: new object[] { true, true });

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "IsCaculable", "IsVisibleToObligation" },
                values: new object[] { true, true });

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "IsCaculable", "IsVisibleToObligation" },
                values: new object[] { true, true });

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "IsCaculable", "IsVisibleToObligation" },
                values: new object[] { true, true });

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "IsCaculable", "IsVisibleToObligation" },
                values: new object[] { false, true });

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "IsCaculable", "IsVisibleToObligation" },
                values: new object[] { true, false });
        }
    }
}
