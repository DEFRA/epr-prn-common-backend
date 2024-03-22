using Microsoft.EntityFrameworkCore.Migrations;

namespace EPR.Accreditation.API.Common.Data.SeedData
{
    public static class AddMaterialData
    {
        public static void Seed(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                "Material",
                columns: new[] { "English", "Welsh" },
                values: new object[] { "Paper/Board", "[Welsh]Paper/Board" });

            migrationBuilder.InsertData(
                "Material",
                columns: new[] { "English", "Welsh" },
                values: new object[] { "Paper Composting", "[Welsh]Paper Composting" });

            migrationBuilder.InsertData(
                "Material",
                columns: new[] { "English", "Welsh" },
                values: new object[] { "Glass Remelt", "[Welsh]Glass Remelt" });

            migrationBuilder.InsertData(
                "Material",
                columns: new[] { "English", "Welsh" },
                values: new object[] { "Glass Other", "[Welsh]Glass Other" });

            migrationBuilder.InsertData(
                "Material",
                columns: new[] { "English", "Welsh" },
                values: new object[] { "Aluminium", "[Welsh]Aluminium" });

            migrationBuilder.InsertData(
                "Material",
                columns: new[] { "English", "Welsh" },
                values: new object[] { "Steel", "[Welsh]Steel" });

            migrationBuilder.InsertData(
                "Material",
                columns: new[] { "English", "Welsh" },
                values: new object[] { "Plastic", "[Welsh]Plastic" });

            migrationBuilder.InsertData(
                "Material",
                columns: new[] { "English", "Welsh" },
                values: new object[] { "Wood", "[Welsh]Wood" });

            migrationBuilder.InsertData(
                "Material",
                columns: new[] { "English", "Welsh" },
                values: new object[] { "Wood Composting", "[Welsh]Wood Composting" });
        }
    }
}
