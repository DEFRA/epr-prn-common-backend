using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExporterTaskNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 23,
                column: "Name",
                value: "PERNsTonnageAndAuthorityToIssuePERNs");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "OverseasReprocessingSitesAndEvidenceOfBroadlyEquivalentStandards");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 23,
                column: "Name",
                value: "PRNsTonnageAndAuthorityToIssuePRNs");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "OverseasReprocessingSites");

            migrationBuilder.InsertData(
                table: "Lookup.RegulatorTask",
                columns: new[] { "Id", "ApplicationTypeId", "IsMaterialSpecific", "JourneyTypeId", "Name" },
                values: new object[] { 29, 2, true, 2, "EvidenceOfBroadlyEquivalentStandards" });
        }
    }
}
