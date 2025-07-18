using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class InterimOverSeasConnectionRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SiteCoordinates",
                table: "Public.OverseasAddress",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Public.InterimOverseasConnections_InterimSiteId",
                table: "Public.InterimOverseasConnections",
                column: "InterimSiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.InterimOverseasConnections_Public.OverseasAddress_InterimSiteId",
                table: "Public.InterimOverseasConnections",
                column: "InterimSiteId",
                principalTable: "Public.OverseasAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Public.InterimOverseasConnections_Public.OverseasAddress_InterimSiteId",
                table: "Public.InterimOverseasConnections");

            migrationBuilder.DropIndex(
                name: "IX_Public.InterimOverseasConnections_InterimSiteId",
                table: "Public.InterimOverseasConnections");

            migrationBuilder.AlterColumn<string>(
                name: "SiteCoordinates",
                table: "Public.OverseasAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
