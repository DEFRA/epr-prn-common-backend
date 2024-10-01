using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEprnStatusCancelledCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
            migrationBuilder.UpdateData(
                table: "PrnStatus",
                keyColumn: "Id",
                keyValue: 3,
                column: "StatusName",
                value: "CANCELED");
        }
    }
}
