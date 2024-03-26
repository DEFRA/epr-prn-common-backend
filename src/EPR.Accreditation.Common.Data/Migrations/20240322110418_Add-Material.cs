using EPR.Accreditation.API.Common.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Accreditation.API.Common.Data.Migrations
{
    public partial class AddMaterial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    English = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Welsh = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccreditationMaterial_MaterialId",
                table: "AccreditationMaterial",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccreditationMaterial_Material_MaterialId",
                table: "AccreditationMaterial",
                column: "MaterialId",
                principalTable: "Material",
            principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            AddMaterialData.Seed(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccreditationMaterial_Material_MaterialId",
                table: "AccreditationMaterial");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropIndex(
                name: "IX_AccreditationMaterial_MaterialId",
                table: "AccreditationMaterial");
        }
    }
}
