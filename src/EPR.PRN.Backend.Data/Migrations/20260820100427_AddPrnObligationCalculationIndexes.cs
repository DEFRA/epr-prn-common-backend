using EPR.PRN.Backend.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    [DbContext(typeof(EprContext))]
    [Migration("20260820100427_AddPrnObligationCalculationIndexes")]
    public sealed class AddPrnObligationCalculationIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .CreateIndex(
                    name: "IX_Prn_ObligationCalculation_CurrentYear",
                    table: "Prn",
                    columns: ["OrganisationId", "ObligationYear", "PrnStatusId"]
                )
                .Annotation("SqlServer:Include", new[] { "MaterialName", "TonnageValue" });

            migrationBuilder
                .CreateIndex(
                    name: "IX_Prn_ObligationCalculation_PreviousDecemberWaste",
                    table: "Prn",
                    columns: ["OrganisationId", "AccreditationYear", "PrnStatusId", "DecemberWaste"]
                )
                .Annotation(
                    "SqlServer:Include",
                    new[] { "ObligationYear", "MaterialName", "TonnageValue" }
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Prn_ObligationCalculation_CurrentYear",
                table: "Prn"
            );

            migrationBuilder.DropIndex(
                name: "IX_Prn_ObligationCalculation_PreviousDecemberWaste",
                table: "Prn"
            );
        }
    }
}
