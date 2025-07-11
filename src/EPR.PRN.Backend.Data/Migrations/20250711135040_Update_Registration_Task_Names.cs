using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update_Registration_Task_Names : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "OverseasReprocessingSitesYouExportTo");

            migrationBuilder.UpdateData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 16,
                column: "Name",
                value: "CarrierBrokerDealerNumberAndOtherPermits");

            migrationBuilder.InsertData(
                table: "Lookup.Task",
                columns: new[] { "Id", "ApplicationTypeId", "IsMaterialSpecific", "JourneyTypeId", "Name" },
                values: new object[,]
                {
                    { 17, 2, false, 1, "AddressForServiceofNotices" },
                    { 18, 2, false, 1, "AboutThePackagingWasteYouExport" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.UpdateData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "OverseasReprocessingSites");

            migrationBuilder.UpdateData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 16,
                column: "Name",
                value: "WasteCarrierBrokerDealerNumber");
        }
    }
}
