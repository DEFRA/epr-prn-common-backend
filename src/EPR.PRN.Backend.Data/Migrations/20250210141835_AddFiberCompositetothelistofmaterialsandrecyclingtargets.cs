using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFiberCompositetothelistofmaterialsandrecyclingtargets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "RecyclingTargets",
            columns: ["Id", "MaterialNameRT", "Target", "Year"],
            values: new object[,]
            {
                { 43, "FibreComposite", 0.75m, 2025 },
                { 44, "FibreComposite", 0.77m, 2026 },
                { 45, "FibreComposite", 0.79m, 2027 },
                { 46, "FibreComposite", 0.81m, 2028 },
                { 47, "FibreComposite", 0.83m, 2029 },
                { 48, "FibreComposite", 0.85m, 2030 }
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
            table: "RecyclingTargets",
            keyColumn: "Id",
            keyValues: [43, 44, 45, 46, 47, 48]);
        }
    }
}
