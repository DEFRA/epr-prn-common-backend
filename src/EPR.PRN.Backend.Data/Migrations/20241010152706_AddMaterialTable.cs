using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialTable : Migration
    {
        private static readonly string[] _materialColumns = new[] { "MaterialName", "MaterialCode" };

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    MaterialName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaterialCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.MaterialName);
                });

            migrationBuilder.InsertData(
                table: "Material",
                columns: _materialColumns,
                values: new object[,]
                {
                    { "Aluminium", "AL" },
                    { "Glass", "GL" },
                    { "Paper", "PC" },
                    { "Plastic", "PL" },
                    { "Steel", "ST" },
                    { "Wood", "WD" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Material");
        }
    }
}
