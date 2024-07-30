using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedEnumForStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PrnStatus",
                columns: new[] { "Id", "StatusDescription", "StatusName" },
                values: new object[,]
                {
                    { 1, "Prn Accepted", "ACCEPTED" },
                    { 2, "Prn Rejected", "REJECTED" },
                    { 3, "Prn Cancelled", "CANCELED" },
                    { 4, "Prn Awaiting Acceptance", "AWAITINGACCEPTANCE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PrnStatus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PrnStatus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PrnStatus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PrnStatus",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
