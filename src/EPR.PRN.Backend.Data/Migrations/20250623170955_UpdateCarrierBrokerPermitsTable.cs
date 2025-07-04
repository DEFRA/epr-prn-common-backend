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
            // Rename the column (preserves data)
            migrationBuilder.RenameColumn(
                name: "InstatallationPermitOrPPCNumber",
                table: "Public.CarrierBrokerDealerPermits",
                newName: "InstallationPermitOrPPCNumber");

            // Change the type/length if needed
            migrationBuilder.AlterColumn<string>(
                name: "InstallationPermitOrPPCNumber",
                table: "Public.CarrierBrokerDealerPermits",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
                name: "WasteExemptionReference",
                table: "Public.CarrierBrokerDealerPermits",
                type: "varchar(150)",
                maxLength: 150,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WasteExemptionReference",
                table: "Public.CarrierBrokerDealerPermits");

            migrationBuilder.AlterColumn<string>(
                name: "InstallationPermitOrPPCNumber",
                table: "Public.CarrierBrokerDealerPermits",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

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

            // Rename the column back
            migrationBuilder.RenameColumn(
                name: "InstallationPermitOrPPCNumber",
                table: "Public.CarrierBrokerDealerPermits",
                newName: "InstatallationPermitOrPPCNumber");
        }
    }
}
