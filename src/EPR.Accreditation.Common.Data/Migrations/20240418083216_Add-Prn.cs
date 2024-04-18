using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Accreditation.API.Common.Data.Migrations
{
    public partial class AddPrn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrnStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StatusDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrnStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrnTypeId = table.Column<int>(type: "int", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrnStatusId = table.Column<int>(type: "int", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    IsDecember = table.Column<bool>(type: "bit", nullable: false),
                    TonnageValue = table.Column<int>(type: "int", nullable: false),
                    ProducerId = table.Column<int>(type: "int", nullable: false),
                    ProducerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccreditationId = table.Column<int>(type: "int", nullable: false),
                    AccreditationReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prn_OperatorType_PrnTypeId",
                        column: x => x.PrnTypeId,
                        principalSchema: "Lookup",
                        principalTable: "OperatorType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prn_PrnStatus_PrnStatusId",
                        column: x => x.PrnStatusId,
                        principalTable: "PrnStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prn_Site_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Site",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    OrganisationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PrnStatusId = table.Column<int>(type: "int", nullable: false),
                    PrnId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrnStatusHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrnStatusHistory_Prn_PrnId",
                        column: x => x.PrnId,
                        principalTable: "Prn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrnStatusHistory_PrnStatus_PrnStatusId",
                        column: x => x.PrnStatusId,
                        principalTable: "PrnStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prn_PrnStatusId",
                table: "Prn",
                column: "PrnStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Prn_PrnTypeId",
                table: "Prn",
                column: "PrnTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Prn_SiteId",
                table: "Prn",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_PrnStatusHistory_PrnId",
                table: "PrnStatusHistory",
                column: "PrnId");

            migrationBuilder.CreateIndex(
                name: "IX_PrnStatusHistory_PrnStatusId",
                table: "PrnStatusHistory",
                column: "PrnStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrnStatusHistory");

            migrationBuilder.DropTable(
                name: "Prn");

            migrationBuilder.DropTable(
                name: "PrnStatus");
        }
    }
}
