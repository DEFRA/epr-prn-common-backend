using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCarrierBrokerDealerPermitsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Public.CarrierBrokerDealerPermits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationId = table.Column<int>(type: "int", nullable: false),
                    WasteCarrierBrokerDealerRegistration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WasteManagementEnvironmentPermitNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstatallationPermitOrPPCNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisteredWasteCarrierBrokerDealerFlag = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.CarrierBrokerDealerPermits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.CarrierBrokerDealerPermits_Public.Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Public.Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Lookup.RegulatorTask",
                columns: new[] { "Id", "ApplicationTypeId", "IsMaterialSpecific", "JourneyTypeId", "Name" },
                values: new object[] { 29, 1, false, 1, "WasteCarrierBrokerDealerNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Public.CarrierBrokerDealerPermits_ExternalId",
                table: "Public.CarrierBrokerDealerPermits",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.CarrierBrokerDealerPermits_RegistrationId",
                table: "Public.CarrierBrokerDealerPermits",
                column: "RegistrationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Public.CarrierBrokerDealerPermits");

            migrationBuilder.DeleteData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 29);
        }
    }
}
