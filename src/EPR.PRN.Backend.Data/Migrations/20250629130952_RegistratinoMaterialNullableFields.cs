using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class RegistratinoMaterialNullableFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegistrationMaterial_Lookup.MaterialPermit_PermitTypeId",
                table: "Public.RegistrationMaterial");

            migrationBuilder.AlterColumn<decimal>(
                name: "WasteManagementReprocessingCapacityTonne",
                table: "Public.RegistrationMaterial",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "PermitTypeId",
                table: "Public.RegistrationMaterial",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "PPCReprocessingCapacityTonne",
                table: "Public.RegistrationMaterial",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaximumReprocessingCapacityTonne",
                table: "Public.RegistrationMaterial",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "InstallationReprocessingTonne",
                table: "Public.RegistrationMaterial",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "EnvironmentalPermitWasteManagementTonne",
                table: "Public.RegistrationMaterial",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegistrationMaterial_Lookup.MaterialPermit_PermitTypeId",
                table: "Public.RegistrationMaterial",
                column: "PermitTypeId",
                principalTable: "Lookup.MaterialPermit",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegistrationMaterial_Lookup.MaterialPermit_PermitTypeId",
                table: "Public.RegistrationMaterial");

            migrationBuilder.AlterColumn<decimal>(
                name: "WasteManagementReprocessingCapacityTonne",
                table: "Public.RegistrationMaterial",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PermitTypeId",
                table: "Public.RegistrationMaterial",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PPCReprocessingCapacityTonne",
                table: "Public.RegistrationMaterial",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MaximumReprocessingCapacityTonne",
                table: "Public.RegistrationMaterial",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "InstallationReprocessingTonne",
                table: "Public.RegistrationMaterial",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "EnvironmentalPermitWasteManagementTonne",
                table: "Public.RegistrationMaterial",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegistrationMaterial_Lookup.MaterialPermit_PermitTypeId",
                table: "Public.RegistrationMaterial",
                column: "PermitTypeId",
                principalTable: "Lookup.MaterialPermit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
