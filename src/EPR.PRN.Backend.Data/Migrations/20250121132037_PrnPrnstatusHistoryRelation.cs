using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class PrnPrnstatusHistoryRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PrnStatusHistory_PrnIdFk",
                table: "PrnStatusHistory",
                column: "PrnIdFk");

            migrationBuilder.AddForeignKey(
                name: "FK_PrnStatusHistory_Prn_PrnIdFk",
                table: "PrnStatusHistory",
                column: "PrnIdFk",
                principalTable: "Prn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrnStatusHistory_Prn_PrnIdFk",
                table: "PrnStatusHistory");

            migrationBuilder.DropIndex(
                name: "IX_PrnStatusHistory_PrnIdFk",
                table: "PrnStatusHistory");
        }
    }
}
