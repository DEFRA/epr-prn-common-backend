using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyObligationCalculationsTableToAddNewCoulmnAndRelatedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			// Remove exisitng rows from ObligationCalculations table
			migrationBuilder.Sql(@"DELETE FROM ObligationCalculations");

			migrationBuilder.AddColumn<Guid>(
                name: "SubmitterId",
                table: "ObligationCalculations",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "SubmitterTypeId",
                table: "ObligationCalculations",
                type: "int",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "ObligationCalculationOrganisationSubmitterType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObligationCalculationOrganisationSubmitterType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ObligationCalculationOrganisationSubmitterType",
                columns: new[] { "Id", "TypeName" },
                values: new object[,]
                {
                    { 1, "ComplianceScheme" },
                    { 2, "DirectRegistrant" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObligationCalculations_SubmitterTypeId",
                table: "ObligationCalculations",
                column: "SubmitterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ObligationCalculationOrganisationSubmitterType_TypeName",
                table: "ObligationCalculationOrganisationSubmitterType",
                column: "TypeName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ObligationCalculations_ObligationCalculationOrganisationSubmitterType_SubmitterTypeId",
                table: "ObligationCalculations",
                column: "SubmitterTypeId",
                principalTable: "ObligationCalculationOrganisationSubmitterType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ObligationCalculations_ObligationCalculationOrganisationSubmitterType_SubmitterTypeId",
                table: "ObligationCalculations");

            migrationBuilder.DropTable(
                name: "ObligationCalculationOrganisationSubmitterType");

            migrationBuilder.DropIndex(
                name: "IX_ObligationCalculations_SubmitterTypeId",
                table: "ObligationCalculations");

            migrationBuilder.DropColumn(
                name: "SubmitterId",
                table: "ObligationCalculations");

            migrationBuilder.DropColumn(
                name: "SubmitterTypeId",
                table: "ObligationCalculations");
        }
    }
}
