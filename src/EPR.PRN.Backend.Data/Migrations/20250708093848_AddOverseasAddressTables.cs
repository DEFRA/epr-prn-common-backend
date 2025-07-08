using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOverseasAddressTables : Migration
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
                    RegistrationId = table.Column<int>(type: "int", nullable: false),
                    OrganisationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CityOrTown = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    StateProvince = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                        name: "FK_Public.OverseasAddress_Public.Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Public.Registration",
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
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
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
                name: "OverseasAddressOverseasMaterialReprocessingSite",
                columns: table => new
                {
                    OverseasAddressesId = table.Column<int>(type: "int", nullable: false),
                    OverseasMaterialReprocessingSitesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverseasAddressOverseasMaterialReprocessingSite", x => new { x.OverseasAddressesId, x.OverseasMaterialReprocessingSitesId });
                    table.ForeignKey(
                        name: "FK_OverseasAddressOverseasMaterialReprocessingSite_Public.OverseasAddress_OverseasAddressesId",
                        column: x => x.OverseasAddressesId,
                        principalTable: "Public.OverseasAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OverseasAddressOverseasMaterialReprocessingSite_Public.OverseasMaterialReprocessingSite_OverseasMaterialReprocessingSitesId",
                        column: x => x.OverseasMaterialReprocessingSitesId,
                        principalTable: "Public.OverseasMaterialReprocessingSite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OverseasMaterialReprocessingSiteRegistrationMaterial",
                columns: table => new
                {
                    OverseasMaterialReprocessingSitesId = table.Column<int>(type: "int", nullable: false),
                    RegistrationMaterialsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverseasMaterialReprocessingSiteRegistrationMaterial", x => new { x.OverseasMaterialReprocessingSitesId, x.RegistrationMaterialsId });
                    table.ForeignKey(
                        name: "FK_OverseasMaterialReprocessingSiteRegistrationMaterial_Public.OverseasMaterialReprocessingSite_OverseasMaterialReprocessingSit~",
                        column: x => x.OverseasMaterialReprocessingSitesId,
                        principalTable: "Public.OverseasMaterialReprocessingSite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OverseasMaterialReprocessingSiteRegistrationMaterial_Public.RegistrationMaterial_RegistrationMaterialsId",
                        column: x => x.RegistrationMaterialsId,
                        principalTable: "Public.RegistrationMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Public.InterimOverseasConnections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OverseasMaterialReprocessingSiteId = table.Column<int>(type: "int", nullable: false),
                    InterimSiteId = table.Column<int>(type: "int", nullable: false),
                    ParentOverseasAddressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.InterimOverseasConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.InterimOverseasConnections_Public.OverseasAddress_ParentOverseasAddressId",
                        column: x => x.ParentOverseasAddressId,
                        principalTable: "Public.OverseasAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Public.InterimOverseasConnections_Public.OverseasMaterialReprocessingSite_OverseasMaterialReprocessingSiteId",
                        column: x => x.OverseasMaterialReprocessingSiteId,
                        principalTable: "Public.OverseasMaterialReprocessingSite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OverseasAddressOverseasMaterialReprocessingSite_OverseasMaterialReprocessingSitesId",
                table: "OverseasAddressOverseasMaterialReprocessingSite",
                column: "OverseasMaterialReprocessingSitesId");

            migrationBuilder.CreateIndex(
                name: "IX_OverseasMaterialReprocessingSiteRegistrationMaterial_RegistrationMaterialsId",
                table: "OverseasMaterialReprocessingSiteRegistrationMaterial",
                column: "RegistrationMaterialsId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.InterimOverseasConnections_ExternalId",
                table: "Public.InterimOverseasConnections",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.InterimOverseasConnections_OverseasMaterialReprocessingSiteId",
                table: "Public.InterimOverseasConnections",
                column: "OverseasMaterialReprocessingSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.InterimOverseasConnections_ParentOverseasAddressId",
                table: "Public.InterimOverseasConnections",
                column: "ParentOverseasAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAddress_CountryId",
                table: "Public.OverseasAddress",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAddress_ExternalId",
                table: "Public.OverseasAddress",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAddress_RegistrationId",
                table: "Public.OverseasAddress",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAddressContact_OverseasAddressId",
                table: "Public.OverseasAddressContact",
                column: "OverseasAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAddressWasteCode_ExternalId",
                table: "Public.OverseasAddressWasteCode",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAddressWasteCode_OverseasAddressId",
                table: "Public.OverseasAddressWasteCode",
                column: "OverseasAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasMaterialReprocessingSite_ExternalId",
                table: "Public.OverseasMaterialReprocessingSite",
                column: "ExternalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OverseasAddressOverseasMaterialReprocessingSite");

            migrationBuilder.DropTable(
                name: "OverseasMaterialReprocessingSiteRegistrationMaterial");

            migrationBuilder.DropTable(
                name: "Public.InterimOverseasConnections");

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
