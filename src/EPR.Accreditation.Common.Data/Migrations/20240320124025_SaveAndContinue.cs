using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Accreditation.API.Common.Data.Migrations
{
    public partial class SaveAndContinue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WasteCode_AccreditationMaterial_AccreditationMaterialId",
                table: "WasteCode");

            migrationBuilder.DropForeignKey(
                name: "FK_WasteCode_WasteCodeType_WasteCodeTypeId",
                table: "WasteCode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WasteCode",
                table: "WasteCode");

            migrationBuilder.RenameTable(
                name: "WasteCode",
                newName: "WasteCodes");

            migrationBuilder.RenameIndex(
                name: "IX_WasteCode_WasteCodeTypeId",
                table: "WasteCodes",
                newName: "IX_WasteCodes_WasteCodeTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_WasteCode_AccreditationMaterialId",
                table: "WasteCodes",
                newName: "IX_WasteCodes_AccreditationMaterialId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WasteCodes",
                table: "WasteCodes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SaveAndContinue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Area = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Controller = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Action = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Parameters = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    AccreditationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveAndContinue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaveAndContinue_Accreditation_AccreditationId",
                        column: x => x.AccreditationId,
                        principalTable: "Accreditation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaveAndContinue_AccreditationId",
                table: "SaveAndContinue",
                column: "AccreditationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WasteCodes_AccreditationMaterial_AccreditationMaterialId",
                table: "WasteCodes",
                column: "AccreditationMaterialId",
                principalTable: "AccreditationMaterial",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WasteCodes_WasteCodeType_WasteCodeTypeId",
                table: "WasteCodes",
                column: "WasteCodeTypeId",
                principalSchema: "Lookup",
                principalTable: "WasteCodeType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WasteCodes_AccreditationMaterial_AccreditationMaterialId",
                table: "WasteCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_WasteCodes_WasteCodeType_WasteCodeTypeId",
                table: "WasteCodes");

            migrationBuilder.DropTable(
                name: "SaveAndContinue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WasteCodes",
                table: "WasteCodes");

            migrationBuilder.RenameTable(
                name: "WasteCodes",
                newName: "WasteCode");

            migrationBuilder.RenameIndex(
                name: "IX_WasteCodes_WasteCodeTypeId",
                table: "WasteCode",
                newName: "IX_WasteCode_WasteCodeTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_WasteCodes_AccreditationMaterialId",
                table: "WasteCode",
                newName: "IX_WasteCode_AccreditationMaterialId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WasteCode",
                table: "WasteCode",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WasteCode_AccreditationMaterial_AccreditationMaterialId",
                table: "WasteCode",
                column: "AccreditationMaterialId",
                principalTable: "AccreditationMaterial",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WasteCode_WasteCodeType_WasteCodeTypeId",
                table: "WasteCode",
                column: "WasteCodeTypeId",
                principalSchema: "Lookup",
                principalTable: "WasteCodeType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
