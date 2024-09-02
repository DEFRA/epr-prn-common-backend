using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialRecyclingTargets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecyclingTargets");
        }
    }
}
