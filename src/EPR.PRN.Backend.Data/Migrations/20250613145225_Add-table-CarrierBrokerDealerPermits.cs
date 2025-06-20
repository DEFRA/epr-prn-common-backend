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
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RegistrationId = table.Column<int>(type: "int", nullable: false),
                    WasteCarrierBrokerDealerRegistration = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    WasteManagementorEnvironmentPermitNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    InstallationPermitorPPCNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    WasteExemptionReference = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    RegisteredWasteCarrierBrokerDealerFlag = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
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
        }
    }
}
