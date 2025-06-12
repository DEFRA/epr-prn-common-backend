using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddtableCarrierBrokerDealerPermits : Migration
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
                    WasteCarrierBrokerDealerRegistrstion = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    WasteManagementorEnvironmentPermitNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    InstallationPermitorPPCNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    WasteExemptionReference = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    RegisteredWasteCarrierBrokerDealerFlag = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegistrationId1 = table.Column<int>(type: "int", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_Public.CarrierBrokerDealerPermits_Public.Registration_RegistrationId1",
                        column: x => x.RegistrationId1,
                        principalTable: "Public.Registration",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Public.CarrierBrokerDealerPermits_RegistrationId",
                table: "Public.CarrierBrokerDealerPermits",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.CarrierBrokerDealerPermits_RegistrationId1",
                table: "Public.CarrierBrokerDealerPermits",
                column: "RegistrationId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Public.CarrierBrokerDealerPermits");
        }
    }
}
