using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExporterTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "Overseas Reprocessing Sites");

            migrationBuilder.InsertData(
                table: "Lookup.RegulatorTask",
                columns: new[] { "Id", "ApplicationTypeId", "IsMaterialSpecific", "JourneyTypeId", "Name" },
                values: new object[] { 29, 2, true, 2, "Evidence of Broadly Equivalent Standards" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "Overseas reprocessing sites and broadly equivalent evidence");
        }
    }
}
