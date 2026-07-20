using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(EprContext))]
    [Migration("20260720120000_AddPrnRawDataIndexes")]
    public partial class AddPrnRawDataIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Prn_SourceSystemId_Id",
                table: "Prn",
                columns: ["SourceSystemId", "Id"]);

            migrationBuilder.CreateIndex(
                name: "IX_PrnStatusHistory_PrnIdFk_CreatedOn_Id",
                table: "PrnStatusHistory",
                columns: ["PrnIdFk", "CreatedOn", "Id"]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Prn_SourceSystemId_Id",
                table: "Prn");

            migrationBuilder.DropIndex(
                name: "IX_PrnStatusHistory_PrnIdFk_CreatedOn_Id",
                table: "PrnStatusHistory");
        }
    }
}
