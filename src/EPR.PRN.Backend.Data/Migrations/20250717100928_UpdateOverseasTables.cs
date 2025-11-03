using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOverseasTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Public.OverseasAddressWasteCode_Public.OverseasAddress_OverseasAddressId",
                table: "Public.OverseasAddressWasteCode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Public.OverseasAddressWasteCode",
                table: "Public.OverseasAddressWasteCode");

            migrationBuilder.RenameTable(
                name: "Public.OverseasAddressWasteCode",
                newName: "Public.OverseasAddressWasteCodes");

            migrationBuilder.RenameIndex(
                name: "IX_Public.OverseasAddressWasteCode_OverseasAddressId",
                table: "Public.OverseasAddressWasteCodes",
                newName: "IX_Public.OverseasAddressWasteCodes_OverseasAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Public.OverseasAddressWasteCode_ExternalId",
                table: "Public.OverseasAddressWasteCodes",
                newName: "IX_Public.OverseasAddressWasteCodes_ExternalId");

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalId",
                table: "Public.OverseasAddressContact",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            migrationBuilder.AlterColumn<string>(
                name: "SiteCoordinates",
                table: "Public.OverseasAddress",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganisationId",
                table: "Public.OverseasAddress",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Public.OverseasAddressWasteCodes",
                table: "Public.OverseasAddressWasteCodes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAddressContact_ExternalId",
                table: "Public.OverseasAddressContact",
                column: "ExternalId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Public.OverseasAddressWasteCodes_Public.OverseasAddress_OverseasAddressId",
                table: "Public.OverseasAddressWasteCodes",
                column: "OverseasAddressId",
                principalTable: "Public.OverseasAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Public.OverseasAddressWasteCodes_Public.OverseasAddress_OverseasAddressId",
                table: "Public.OverseasAddressWasteCodes");

            migrationBuilder.DropIndex(
                name: "IX_Public.OverseasAddressContact_ExternalId",
                table: "Public.OverseasAddressContact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Public.OverseasAddressWasteCodes",
                table: "Public.OverseasAddressWasteCodes");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Public.OverseasAddressContact");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Public.OverseasAddress");

            migrationBuilder.RenameTable(
                name: "Public.OverseasAddressWasteCodes",
                newName: "Public.OverseasAddressWasteCode");

            migrationBuilder.RenameIndex(
                name: "IX_Public.OverseasAddressWasteCodes_OverseasAddressId",
                table: "Public.OverseasAddressWasteCode",
                newName: "IX_Public.OverseasAddressWasteCode_OverseasAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Public.OverseasAddressWasteCodes_ExternalId",
                table: "Public.OverseasAddressWasteCode",
                newName: "IX_Public.OverseasAddressWasteCode_ExternalId");

            migrationBuilder.AlterColumn<string>(
                name: "SiteCoordinates",
                table: "Public.OverseasAddress",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Public.OverseasAddressWasteCode",
                table: "Public.OverseasAddressWasteCode",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.OverseasAddressWasteCode_Public.OverseasAddress_OverseasAddressId",
                table: "Public.OverseasAddressWasteCode",
                column: "OverseasAddressId",
                principalTable: "Public.OverseasAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
