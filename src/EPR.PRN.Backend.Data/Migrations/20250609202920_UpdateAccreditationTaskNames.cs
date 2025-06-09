using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccreditationTaskNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 18,
                column: "Name",
                value: "PRNsTonnageAndAuthorityToIssuePRNs");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 19,
                column: "Name",
                value: "BusinessPlan");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 20,
                column: "Name",
                value: "AccreditationSamplingAndInspectionPlan");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 23,
                column: "Name",
                value: "PRNsTonnageAndAuthorityToIssuePRNs");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 24,
                column: "Name",
                value: "BusinessPlan");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 25,
                column: "Name",
                value: "AccreditationSamplingAndInspectionPlan");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "OverseasReprocessingSites");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 29,
                column: "Name",
                value: "EvidenceOfBroadlyEquivalentStandards");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 18,
                column: "Name",
                value: "PRNs tonnage and authority to issue PRNs");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 19,
                column: "Name",
                value: "Business Plan");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 20,
                column: "Name",
                value: "Accreditation sampling and inspection plan");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 23,
                column: "Name",
                value: "PRNs tonnage and authority to issue PRNs");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 24,
                column: "Name",
                value: "Business Plan");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 25,
                column: "Name",
                value: "Accreditation sampling and inspection plan");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "Overseas Reprocessing Sites");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 29,
                column: "Name",
                value: "Evidence of Broadly Equivalent Standards");
        }
    }
}
