using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class OverseasMaterialReprocessingSite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Public.OverseasAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationMaterialId = table.Column<int>(type: "int", nullable: false),
                    OrganisationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CityOrTown = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    StateProvince = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    PostCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SiteCoordinates = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.OverseasAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.OverseasAddress_Lookup.Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Lookup.Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Public.OverseasAddress_Public.RegistrationMaterial_RegistrationMaterialId",
                        column: x => x.RegistrationMaterialId,
                        principalTable: "Public.RegistrationMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Public.OverseasAddressContact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OverseasAddressId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.OverseasAddressContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.OverseasAddressContact_Public.OverseasAddress_OverseasAddressId",
                        column: x => x.OverseasAddressId,
                        principalTable: "Public.OverseasAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Public.OverseasAddressWasteCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OverseasAddressId = table.Column<int>(type: "int", nullable: false),
                    CodeName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.OverseasAddressWasteCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.OverseasAddressWasteCode_Public.OverseasAddress_OverseasAddressId",
                        column: x => x.OverseasAddressId,
                        principalTable: "Public.OverseasAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Public.OverseasMaterialReprocessingSite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OverseasAddressId = table.Column<int>(type: "int", nullable: false),
                    RegistrationMaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.OverseasMaterialReprocessingSite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.OverseasMaterialReprocessingSite_Public.OverseasAddress_OverseasAddressId",
                        column: x => x.OverseasAddressId,
                        principalTable: "Public.OverseasAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Public.OverseasMaterialReprocessingSite_Public.RegistrationMaterial_RegistrationMaterialId",
                        column: x => x.RegistrationMaterialId,
                        principalTable: "Public.RegistrationMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAddress_CountryId",
                table: "Public.OverseasAddress",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAddress_RegistrationMaterialId",
                table: "Public.OverseasAddress",
                column: "RegistrationMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAddressContact_OverseasAddressId",
                table: "Public.OverseasAddressContact",
                column: "OverseasAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAddressWasteCode_OverseasAddressId",
                table: "Public.OverseasAddressWasteCode",
                column: "OverseasAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasMaterialReprocessingSite_OverseasAddressId",
                table: "Public.OverseasMaterialReprocessingSite",
                column: "OverseasAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasMaterialReprocessingSite_RegistrationMaterialId",
                table: "Public.OverseasMaterialReprocessingSite",
                column: "RegistrationMaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Public.OverseasAddressContact");

            migrationBuilder.DropTable(
                name: "Public.OverseasAddressWasteCode");

            migrationBuilder.DropTable(
                name: "Public.OverseasMaterialReprocessingSite");

            migrationBuilder.DropTable(
                name: "Public.OverseasAddress");
        }
    }
}
