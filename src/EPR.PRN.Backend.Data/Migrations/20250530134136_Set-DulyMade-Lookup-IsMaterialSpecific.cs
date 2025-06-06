using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetDulyMadeLookupIsMaterialSpecific : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 27,
                column: "IsMaterialSpecific",
                value: true);

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 28,
                column: "IsMaterialSpecific",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 27,
                column: "IsMaterialSpecific",
                value: false);

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 28,
                column: "IsMaterialSpecific",
                value: false);
        }
    }
}
