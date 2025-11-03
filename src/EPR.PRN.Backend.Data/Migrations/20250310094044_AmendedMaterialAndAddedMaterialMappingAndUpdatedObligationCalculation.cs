using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AmendedMaterialAndAddedMaterialMappingAndUpdatedObligationCalculation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Material",
                table: "Material");

            migrationBuilder.DeleteData(
                table: "Material",
                keyColumn: "MaterialName",
                keyValue: "Aluminium");

            migrationBuilder.DeleteData(
                table: "Material",
                keyColumn: "MaterialName",
                keyValue: "FibreComposite");

            migrationBuilder.DeleteData(
                table: "Material",
                keyColumn: "MaterialName",
                keyValue: "Glass");

            migrationBuilder.DeleteData(
                table: "Material",
                keyColumn: "MaterialName",
                keyValue: "Paper");

            migrationBuilder.DeleteData(
                table: "Material",
                keyColumn: "MaterialName",
                keyValue: "Plastic");

            migrationBuilder.DeleteData(
                table: "Material",
                keyColumn: "MaterialName",
                keyValue: "Steel");

            migrationBuilder.DeleteData(
                table: "Material",
                keyColumn: "MaterialName",
                keyValue: "Wood");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Material",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Material",
                table: "Material",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PrnMaterialMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    PRNMaterialId = table.Column<int>(type: "int", nullable: false),
                    NPWDMaterialName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrnMaterialMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrnMaterialMapping_Material_PRNMaterialId",
                        column: x => x.PRNMaterialId,
                        principalTable: "Material",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Material",
                columns: ["Id", "IsCaculable", "IsVisibleToObligation", "MaterialCode", "MaterialName"],
                values: new object[,]
                {
                    { 1, true, true, "PL", "Plastic" },
                    { 2, true, true, "WD", "Wood" },
                    { 3, true, true, "AL", "Aluminium" },
                    { 4, true, true, "ST", "Steel" },
                    { 5, true, true, "PC", "Paper" },
                    { 6, true, true, "GL", "Glass" },
                    { 7, false, true, "GR", "GlassRemelt" },
                    { 8, true, false, "FC", "FibreComposite" }
                });

            migrationBuilder.InsertData(
                table: "PrnMaterialMapping",
                columns: ["Id", "NPWDMaterialName", "PRNMaterialId"],
                values: new object[,]
                {
                    { 1, "Plastic", 1 },
                    { 2, "Wood", 2 },
                    { 3, "Wood Composting", 2 },
                    { 4, "Aluminium", 3 },
                    { 5, "Steel", 4 },
                    { 6, "Paper/board", 5 },
                    { 7, "Paper Composting", 5 },
                    { 8, "Glass Other", 6 },
                    { 9, "Glass Re-melt", 7 }
                });

            // Add MaterialId as Nullable (to prevent errors)
            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "ObligationCalculations",
                nullable: true);

            // Update MaterialId based on MaterialName
            migrationBuilder.Sql(@"
                BEGIN
                    IF COL_LENGTH('ObligationCalculations', 'MaterialName') IS NOT NULL
                    BEGIN
                        EXEC sp_executesql N'
                            UPDATE ObligationCalculations
                            SET MaterialId = (SELECT Id FROM Material WHERE Material.MaterialName = ObligationCalculations.MaterialName)
                            WHERE EXISTS (SELECT 1 FROM Material WHERE Material.MaterialName = ObligationCalculations.MaterialName)
                        ';
                    END
                END;
            ");

            // Make MaterialId Non-Nullable
            migrationBuilder.AlterColumn<int>(
                name: "MaterialId",
                table: "ObligationCalculations",
                nullable: false);

            // Remove MaterialName from Calculation
            migrationBuilder.DropColumn(
                name: "MaterialName",
                table: "ObligationCalculations");

            migrationBuilder.CreateIndex(
                name: "IX_ObligationCalculations_MaterialId",
                table: "ObligationCalculations",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Material_MaterialCode",
                table: "Material",
                column: "MaterialCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrnMaterialMapping_PRNMaterialId",
                table: "PrnMaterialMapping",
                column: "PRNMaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_ObligationCalculations_Material_MaterialId",
                table: "ObligationCalculations",
                column: "MaterialId",
                principalTable: "Material",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Add Back MaterialName Column
            migrationBuilder.AddColumn<string>(
                name: "MaterialName",
                table: "ObligationCalculations",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            // Restore MaterialName Data
            migrationBuilder.Sql(@"
                BEGIN
                    IF COL_LENGTH('ObligationCalculations', 'MaterialId') IS NOT NULL
                    BEGIN
                        EXEC sp_executesql N'
                            UPDATE ObligationCalculations
                            SET MaterialName = (SELECT MaterialName FROM Material WHERE Material.Id = ObligationCalculations.MaterialId)
                            WHERE EXISTS (SELECT 1 FROM Material WHERE Material.Id = ObligationCalculations.MaterialId)
                        ';
                    END
                END;
            ");

            // Drop Foreign Key Constraint
            migrationBuilder.DropForeignKey(
                name: "FK_ObligationCalculations_Material_MaterialId",
                table: "ObligationCalculations");

            // Drop Index
            migrationBuilder.DropIndex(
                name: "IX_ObligationCalculations_MaterialId",
                table: "ObligationCalculations");

            // Drop MaterialId Column
            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "ObligationCalculations");

            migrationBuilder.DropTable(
                name: "PrnMaterialMapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Material",
                table: "Material");

            migrationBuilder.DropIndex(
                name: "IX_Material_MaterialCode",
                table: "Material");

            migrationBuilder.DeleteData(
                table: "Material",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Material",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Material",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Material",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Material",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Material",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Material",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Material",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 8);

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "IsCaculable",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "IsVisibleToObligation",
                table: "Material");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Material",
                table: "Material",
                column: "MaterialName");

            migrationBuilder.InsertData(
                table: "Material",
                columns: ["MaterialName", "MaterialCode"],
                values: new object[,]
                {
                    { "Aluminium", "AL" },
                    { "FibreComposite", "FC" },
                    { "Glass", "GL" },
                    { "Paper", "PC" },
                    { "Plastic", "PL" },
                    { "Steel", "ST" },
                    { "Wood", "WD" }
                });
        }
    }
}
