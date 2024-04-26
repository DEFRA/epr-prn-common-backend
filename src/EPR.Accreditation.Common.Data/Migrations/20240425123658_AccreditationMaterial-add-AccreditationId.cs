using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Accreditation.API.Common.Data.Migrations
{
    public partial class AccreditationMaterialaddAccreditationId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // need to delete all accreditation materials from the table, otherwise we cannot
            // add the AccreditationID field as a foreign key
            migrationBuilder.Sql("DELETE FROM [Accreditation].[dbo].[AccreditationMaterial]");

            migrationBuilder.AddColumn<int>(
                name: "AccreditationId",
                table: "AccreditationMaterial",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AccreditationMaterial_AccreditationId",
                table: "AccreditationMaterial",
                column: "AccreditationId");

            migrationBuilder.DropForeignKey(
                name: "FK_AccreditationMaterial_OverseasReprocessingSite_OverseasReprocessingSiteId",
                table: "AccreditationMaterial");

            migrationBuilder.DropForeignKey(
                "FK_AccreditationMaterial_Site_SiteId",
                table: "AccreditationMaterial");

            migrationBuilder.AddForeignKey(
                name: "FK_AccreditationMaterial_Accreditation_AccreditationId",
                table: "AccreditationMaterial",
                column: "AccreditationId",
                principalTable: "Accreditation",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_AccreditationMaterial_OverseasReprocessingSite_OverseasReprocessingSiteId",
                table: "AccreditationMaterial",
                column: "OverseasReprocessingSiteId",
                principalTable: "OverseasReprocessingSite",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_AccreditationMaterial_Site_SiteId",
                table: "AccreditationMaterial",
                column: "SiteId",
                principalTable: "Site",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccreditationMaterial_Accreditation_AccreditationId",
                table: "AccreditationMaterial");

            migrationBuilder.DropIndex(
                name: "IX_AccreditationMaterial_AccreditationId",
                table: "AccreditationMaterial");

            migrationBuilder.DropForeignKey(
                "FK_AccreditationMaterial_Site_SiteId",
                table: "AccreditationMaterial");

            migrationBuilder.DropColumn(
                name: "AccreditationId",
                table: "AccreditationMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_AccreditationMaterial_OverseasReprocessingSite_OverseasReprocessingSiteId",
                table: "AccreditationMaterial");

            migrationBuilder.AddForeignKey(
                name: "FK_AccreditationMaterial_OverseasReprocessingSite_OverseasReprocessingSiteId",
                table: "AccreditationMaterial",
                column: "OverseasReprocessingSiteId",
                principalTable: "OverseasReprocessingSite",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccreditationMaterial_Site_SiteId",
                table: "AccreditationMaterial",
                column: "SiteId",
                principalTable: "Site",
                principalColumn: "Id");
        }
    }
}
