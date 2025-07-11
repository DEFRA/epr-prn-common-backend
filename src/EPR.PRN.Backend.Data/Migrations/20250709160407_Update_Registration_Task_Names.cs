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
            migrationBuilder.DeleteData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.UpdateData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "WasteLicensesPermitsAndExemption");

            migrationBuilder.UpdateData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "AboutthePackagingYouAreRegistering");

            migrationBuilder.UpdateData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "AddressForServiceofNotices");

            migrationBuilder.UpdateData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "IsMaterialSpecific", "Name" },
                values: new object[] { false, "CarrierBrokerDealerNumberAndOtherPermits" });

            migrationBuilder.UpdateData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "IsMaterialSpecific", "Name" },
                values: new object[] { false, "AboutThePackagingWasteYouExport" });

            migrationBuilder.UpdateData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "IsMaterialSpecific", "Name" },
                values: new object[] { false, "OverseasReprocessingSitesYouExportTo" });

            migrationBuilder.UpdateData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ApplicationTypeId", "IsMaterialSpecific", "JourneyTypeId", "Name" },
                values: new object[] { 2, false, 1, "InterimSites" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "WasteLicensesPermitsAndExemptions");

            migrationBuilder.UpdateData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "ReprocessingInputsAndOutputs");

            migrationBuilder.UpdateData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "WasteLicensesPermitsAndExemptions");

            migrationBuilder.UpdateData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "IsMaterialSpecific", "Name" },
                values: new object[] { true, "SamplingAndInspectionPlan" });

            migrationBuilder.UpdateData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "IsMaterialSpecific", "Name" },
                values: new object[] { true, "OverseasReprocessingSites" });

            migrationBuilder.UpdateData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "IsMaterialSpecific", "Name" },
                values: new object[] { true, "InterimSites" });

            migrationBuilder.UpdateData(
                table: "Lookup.Task",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ApplicationTypeId", "IsMaterialSpecific", "JourneyTypeId", "Name" },
                values: new object[] { 1, true, 2, "PRNsTonnageAndAuthorityToIssuePRNs" });

            migrationBuilder.InsertData(
                table: "Lookup.Task",
                columns: new[] { "Id", "ApplicationTypeId", "IsMaterialSpecific", "JourneyTypeId", "Name" },
                values: new object[,]
                {
                    { 10, 1, true, 2, "BusinessPlan" },
                    { 11, 1, true, 2, "AccreditationSamplingAndInspectionPlan" },
                    { 12, 2, true, 2, "PERNsTonnageAndAuthorityToIssuePERNs" },
                    { 13, 2, true, 2, "BusinessPlan" },
                    { 14, 2, true, 2, "AccreditationSamplingAndInspectionPlan" },
                    { 15, 2, true, 2, "OverseasReprocessingSitesAndEvidenceOfBroadlyEquivalentStandards" },
                    { 16, 2, false, 1, "WasteCarrierBrokerDealerNumber" }
                });
        }
    }
}
