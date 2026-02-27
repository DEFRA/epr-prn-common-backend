using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFibreMaterialMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PrnMaterialMapping",
                columns: ["Id", "NPWDMaterialName", "PRNMaterialId"],
                values: new object[,]
                {
                    { 10, "Fibre", 8 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PrnMaterialMapping",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
