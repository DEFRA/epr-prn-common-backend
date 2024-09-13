using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEprnStatusAndChangedCancelledDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CancelledDate",
                table: "Prn",
                newName: "StatusUpdatedOn");

            migrationBuilder.UpdateData(
                table: "PrnStatus",
                keyColumn: "Id",
                keyValue: 3,
                column: "StatusName",
                value: "CANCELLED");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatusUpdatedOn",
                table: "Prn",
                newName: "CancelledDate");

            migrationBuilder.UpdateData(
                table: "PrnStatus",
                keyColumn: "Id",
                keyValue: 3,
                column: "StatusName",
                value: "CANCELED");
        }
    }
}
