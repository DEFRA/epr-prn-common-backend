using System;
using EPR.Accreditation.API.Common.Data.DataModels;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Accreditation.API.Common.Data.Migrations
{
    public partial class Createaddresstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // need to delete all accreditation materials from the table, otherwise we cannot
            // add the AccreditationID field as a foreign key
            migrationBuilder.Sql("UPDATE [Accreditation].[dbo].[Accreditation] Set SiteId = null");
            migrationBuilder.Sql("DELETE FROM [Accreditation].[dbo].[AccreditationMaterial]");
            migrationBuilder.Sql("DELETE FROM [Accreditation].[dbo].[Site]");

            migrationBuilder.DropIndex(
                name: "IX_Site_ExternalId",
                table: "Site");

            migrationBuilder.DropIndex(
                name: "IX_Site_Postcode_OrganisationId",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "Address1",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "County",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "Postcode",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "Town",
                table: "Site");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Site",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LegalAddressId",
                table: "Accreditation",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Town = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    County = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Site_AddressId",
                table: "Site",
                column: "AddressId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Site_Address_AddressId",
                table: "Site",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Site_Address_AddressId",
                table: "Site");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Site_AddressId",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "LegalAddressId",
                table: "Accreditation");

            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "Site",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Site",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "County",
                table: "Site",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganisationId",
                table: "Site",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Postcode",
                table: "Site",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Town",
                table: "Site",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Site_ExternalId",
                table: "Site",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Site_Postcode_OrganisationId",
                table: "Site",
                columns: new[] { "Postcode", "OrganisationId" },
                unique: true);
        }
    }
}
