using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class RecreateRecyclingTargetTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecyclingTargets");

            migrationBuilder.CreateTable(
                name: "RecyclingTargets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    MaterialNameRT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Target = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecyclingTargets", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "RecyclingTargets",
                columns: ["Id", "MaterialNameRT", "Target", "Year"],
                values: new object[,]
                {
                    { 1, "Paper", 0.75m, 2025 },
                    { 2, "Paper", 0.77m, 2026 },
                    { 3, "Paper", 0.79m, 2027 },
                    { 4, "Paper", 0.81m, 2028 },
                    { 5, "Paper", 0.83m, 2029 },
                    { 6, "Paper", 0.85m, 2030 },
                    { 7, "Glass", 0.74m, 2025 },
                    { 8, "Glass", 0.76m, 2026 },
                    { 9, "Glass", 0.78m, 2027 },
                    { 10, "Glass", 0.8m, 2028 },
                    { 11, "Glass", 0.82m, 2029 },
                    { 12, "Glass", 0.85m, 2030 },
                    { 13, "Aluminium", 0.61m, 2025 },
                    { 14, "Aluminium", 0.62m, 2026 },
                    { 15, "Aluminium", 0.63m, 2027 },
                    { 16, "Aluminium", 0.64m, 2028 },
                    { 17, "Aluminium", 0.65m, 2029 },
                    { 18, "Aluminium", 0.67m, 2030 },
                    { 19, "Steel", 0.8m, 2025 },
                    { 20, "Steel", 0.81m, 2026 },
                    { 21, "Steel", 0.82m, 2027 },
                    { 22, "Steel", 0.83m, 2028 },
                    { 23, "Steel", 0.84m, 2029 },
                    { 24, "Steel", 0.85m, 2030 },
                    { 25, "Plastic", 0.55m, 2025 },
                    { 26, "Plastic", 0.57m, 2026 },
                    { 27, "Plastic", 0.59m, 2027 },
                    { 28, "Plastic", 0.61m, 2028 },
                    { 29, "Plastic", 0.63m, 2029 },
                    { 30, "Plastic", 0.65m, 2030 },
                    { 31, "Wood", 0.45m, 2025 },
                    { 32, "Wood", 0.46m, 2026 },
                    { 33, "Wood", 0.47m, 2027 },
                    { 34, "Wood", 0.48m, 2028 },
                    { 35, "Wood", 0.49m, 2029 },
                    { 36, "Wood", 0.5m, 2030 },
                    { 37, "GlassRemelt", 0.75m, 2025 },
                    { 38, "GlassRemelt", 0.76m, 2026 },
                    { 39, "GlassRemelt", 0.77m, 2027 },
                    { 40, "GlassRemelt", 0.78m, 2028 },
                    { 41, "GlassRemelt", 0.79m, 2029 },
                    { 42, "GlassRemelt", 0.8m, 2030 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecyclingTargets");

            migrationBuilder.CreateTable(
                name: "RecyclingTargets",
                columns: table => new
                {
                    Year = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaperTarget = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    GlassTarget = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    AluminiumTarget = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    SteelTarget = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PlasticTarget = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    WoodTarget = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    GlassRemeltTarget = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecyclingTargets", x => x.Year);
                });

            migrationBuilder.InsertData(
                table: "RecyclingTargets",
                columns: ["Year", "AluminiumTarget", "GlassRemeltTarget", "GlassTarget", "PaperTarget", "PlasticTarget", "SteelTarget", "WoodTarget"],
                values: new object[,]
                {
                    { 2025, 0.61m, 0.75m, 0.74m, 0.75m, 0.55m, 0.8m, 0.45m },
                    { 2026, 0.62m, 0.76m, 0.76m, 0.77m, 0.57m, 0.81m, 0.46m },
                    { 2027, 0.63m, 0.77m, 0.78m, 0.79m, 0.59m, 0.82m, 0.47m },
                    { 2028, 0.64m, 0.78m, 0.8m, 0.81m, 0.61m, 0.83m, 0.48m },
                    { 2029, 0.65m, 0.79m, 0.82m, 0.83m, 0.63m, 0.84m, 0.49m },
                    { 2030, 0.67m, 0.8m, 0.85m, 0.85m, 0.65m, 0.85m, 0.5m }
                });
        }
    }
}