using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Accreditation.API.Common.Data.Migrations
{
    public partial class nulltonnesfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccreditationId",
                table: "Address",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganisationId",
                table: "Address",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Address_AccreditationId",
                table: "Address",
                column: "AccreditationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Accreditation_AccreditationId",
                table: "Address",
                column: "AccreditationId",
                principalTable: "Accreditation",
                principalColumn: "Id");

            migrationBuilder.DropForeignKey(
                name: "FK_Address_Accreditation_AccreditationId",
                table: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Address_AccreditationId",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "AccreditationId",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Address");

            migrationBuilder.AlterColumn<decimal>(
                name: "Tonnes",
                table: "ReprocessorSupportingInformation",
                type: "decimal(10,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,3)");

            migrationBuilder.CreateIndex(
                name: "IX_Site_ExternalId",
                table: "Site",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accreditation_LegalAddressId",
                table: "Accreditation",
                column: "LegalAddressId",
                unique: true,
                filter: "[LegalAddressId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Accreditation_Address_LegalAddressId",
                table: "Accreditation",
                column: "LegalAddressId",
                principalTable: "Address",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accreditation_Address_LegalAddressId",
                table: "Accreditation");

            migrationBuilder.DropIndex(
                name: "IX_Site_ExternalId",
                table: "Site");

            migrationBuilder.DropIndex(
                name: "IX_Accreditation_LegalAddressId",
                table: "Accreditation");

            migrationBuilder.AlterColumn<decimal>(
                name: "Tonnes",
                table: "ReprocessorSupportingInformation",
                type: "decimal(10,3)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,3)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccreditationId",
                table: "Address",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganisationId",
                table: "Address",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Address_AccreditationId",
                table: "Address",
                column: "AccreditationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Accreditation_AccreditationId",
                table: "Address",
                column: "AccreditationId",
                principalTable: "Accreditation",
                principalColumn: "Id");

            migrationBuilder.DropForeignKey(
                name: "FK_Address_Accreditation_AccreditationId",
                table: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Address_AccreditationId",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "AccreditationId",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Address");
        }
    }
}
