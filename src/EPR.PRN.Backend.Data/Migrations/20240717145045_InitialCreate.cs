using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrnNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganisationName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProducerAgency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReprocessorExporterAgency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PrnStatusId = table.Column<int>(type: "int", nullable: false),
                    TonnageValue = table.Column<int>(type: "int", nullable: false),
                    MaterialName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IssuerNotes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IssuerReference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PrnSignatory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrnSignatoryPosition = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Signature = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessToBeUsed = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DecemberWaste = table.Column<bool>(type: "bit", nullable: false),
                    CancelledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IssuedByOrg = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccreditationNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReprocessingSite = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccreditationYear = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ObligationYear = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PackagingProducer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsExport = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prn", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrnStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StatusDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrnStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrnStatusHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByOrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrnStatusIdFk = table.Column<int>(type: "int", nullable: false),
                    PrnIdFk = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrnStatusHistory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prn_ExternalId",
                table: "Prn",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prn_PrnNumber",
                table: "Prn",
                column: "PrnNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prn");

            migrationBuilder.DropTable(
                name: "PrnStatus");

            migrationBuilder.DropTable(
                name: "PrnStatusHistory");
        }
    }
}
