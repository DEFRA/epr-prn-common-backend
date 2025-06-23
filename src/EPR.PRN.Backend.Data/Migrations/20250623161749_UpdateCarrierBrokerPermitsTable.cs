using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCarrierBrokerPermitsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstatallationPermitOrPPCNumber",
                table: "Public.CarrierBrokerDealerPermits");

            migrationBuilder.AlterColumn<string>(
                name: "WasteManagementEnvironmentPermitNumber",
                table: "Public.CarrierBrokerDealerPermits",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WasteCarrierBrokerDealerRegistration",
                table: "Public.CarrierBrokerDealerPermits",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstallationPermitOrPPCNumber",
                table: "Public.CarrierBrokerDealerPermits",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegistrationId1",
                table: "Public.CarrierBrokerDealerPermits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WasteExemptionReference",
                table: "Public.CarrierBrokerDealerPermits",
                type: "varchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.CarrierBrokerDealerPermits_RegistrationId1",
                table: "Public.CarrierBrokerDealerPermits",
                column: "RegistrationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.CarrierBrokerDealerPermits_Public.Registration_RegistrationId1",
                table: "Public.CarrierBrokerDealerPermits",
                column: "RegistrationId1",
                principalTable: "Public.Registration",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Public.CarrierBrokerDealerPermits_Public.Registration_RegistrationId1",
                table: "Public.CarrierBrokerDealerPermits");

            migrationBuilder.DropIndex(
                name: "IX_Public.CarrierBrokerDealerPermits_RegistrationId1",
                table: "Public.CarrierBrokerDealerPermits");

            migrationBuilder.DropColumn(
                name: "InstallationPermitOrPPCNumber",
                table: "Public.CarrierBrokerDealerPermits");

            migrationBuilder.DropColumn(
                name: "RegistrationId1",
                table: "Public.CarrierBrokerDealerPermits");

            migrationBuilder.DropColumn(
                name: "WasteExemptionReference",
                table: "Public.CarrierBrokerDealerPermits");

            migrationBuilder.AlterColumn<string>(
                name: "WasteManagementEnvironmentPermitNumber",
                table: "Public.CarrierBrokerDealerPermits",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WasteCarrierBrokerDealerRegistration",
                table: "Public.CarrierBrokerDealerPermits",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstatallationPermitOrPPCNumber",
                table: "Public.CarrierBrokerDealerPermits",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
