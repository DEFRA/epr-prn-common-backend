using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Accreditation.API.Common.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Lookup");

            migrationBuilder.CreateTable(
                name: "AccreditationStatus",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccreditationStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                schema: "Lookup",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "FileUploadStatus",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUploadStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileUploadType",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUploadType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperatorType",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatorType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReprocessorSupportingInformationType",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReprocessorSupportingInformationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Site",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Town = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    County = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Site", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskName",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskName", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskStatus",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WasteCodeType",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WasteCodeType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OverseasAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverseasAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OverseasAddress_Country_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "Lookup",
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accreditation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperatorTypeId = table.Column<int>(type: "int", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Large = table.Column<bool>(type: "bit", nullable: true),
                    AccreditationStatusId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accreditation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accreditation_AccreditationStatus_AccreditationStatusId",
                        column: x => x.AccreditationStatusId,
                        principalSchema: "Lookup",
                        principalTable: "AccreditationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accreditation_OperatorType_OperatorTypeId",
                        column: x => x.OperatorTypeId,
                        principalSchema: "Lookup",
                        principalTable: "OperatorType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accreditation_Site_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Site",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExemptionReference",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExemptionReference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExemptionReference_Site_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Site",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SiteAuthority",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteAuthority", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteAuthority_Site_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Site",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccreditationTaskProgress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskStatusId = table.Column<int>(type: "int", nullable: false),
                    TaskNameId = table.Column<int>(type: "int", nullable: false),
                    AccreditationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccreditationTaskProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccreditationTaskProgress_Accreditation_AccreditationId",
                        column: x => x.AccreditationId,
                        principalTable: "Accreditation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccreditationTaskProgress_TaskName_TaskNameId",
                        column: x => x.TaskNameId,
                        principalSchema: "Lookup",
                        principalTable: "TaskName",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccreditationTaskProgress_TaskStatus_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalSchema: "Lookup",
                        principalTable: "TaskStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileUpload",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccreditationId = table.Column<int>(type: "int", nullable: false),
                    Filename = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateUploaded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FileUploadTypeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUpload", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileUpload_Accreditation_AccreditationId",
                        column: x => x.AccreditationId,
                        principalTable: "Accreditation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileUpload_FileUploadStatus_Status",
                        column: x => x.Status,
                        principalSchema: "Lookup",
                        principalTable: "FileUploadStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileUpload_FileUploadType_FileUploadTypeId",
                        column: x => x.FileUploadTypeId,
                        principalSchema: "Lookup",
                        principalTable: "FileUploadType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OverseasReprocessingSite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccreditationId = table.Column<int>(type: "int", nullable: false),
                    OverseasAddressId = table.Column<int>(type: "int", nullable: true),
                    UkPorts = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Outputs = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RejectedPlans = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverseasReprocessingSite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OverseasReprocessingSite_Accreditation_AccreditationId",
                        column: x => x.AccreditationId,
                        principalTable: "Accreditation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OverseasReprocessingSite_OverseasAddress_OverseasAddressId",
                        column: x => x.OverseasAddressId,
                        principalTable: "OverseasAddress",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccreditationMaterial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    AnnualCapacity = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    WeeklyCapacity = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    WasteSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    OverseasReprocessingSiteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccreditationMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccreditationMaterial_OverseasReprocessingSite_OverseasReprocessingSiteId",
                        column: x => x.OverseasReprocessingSiteId,
                        principalTable: "OverseasReprocessingSite",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccreditationMaterial_Site_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Site",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OverseasAgent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OverseasAddressId = table.Column<int>(type: "int", nullable: false),
                    OverseasReprocessingSiteId = table.Column<int>(type: "int", nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverseasAgent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OverseasAgent_OverseasAddress_OverseasAddressId",
                        column: x => x.OverseasAddressId,
                        principalTable: "OverseasAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OverseasAgent_OverseasReprocessingSite_OverseasReprocessingSiteId",
                        column: x => x.OverseasReprocessingSiteId,
                        principalTable: "OverseasReprocessingSite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WastePermit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccreditationId = table.Column<int>(type: "int", nullable: false),
                    OverseasReprocessingSiteId = table.Column<int>(type: "int", nullable: true),
                    DealerRegistrationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EnvironmentalPermitNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PartAActivityReferenceNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PartBActivityReferenceNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DischargeConsentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WastePermitExemption = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WastePermit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WastePermit_Accreditation_AccreditationId",
                        column: x => x.AccreditationId,
                        principalTable: "Accreditation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WastePermit_OverseasReprocessingSite_OverseasReprocessingSiteId",
                        column: x => x.OverseasReprocessingSiteId,
                        principalTable: "OverseasReprocessingSite",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccreditationTaskProgressMaterial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccreditationTaskProgressId = table.Column<int>(type: "int", nullable: false),
                    AccreditationMaterialId = table.Column<int>(type: "int", nullable: false),
                    TaskStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccreditationTaskProgressMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccreditationTaskProgressMaterial_AccreditationMaterial_AccreditationMaterialId",
                        column: x => x.AccreditationMaterialId,
                        principalTable: "AccreditationMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccreditationTaskProgressMaterial_AccreditationTaskProgress_AccreditationTaskProgressId",
                        column: x => x.AccreditationTaskProgressId,
                        principalTable: "AccreditationTaskProgress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialReprocessorDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccreditationMaterialId = table.Column<int>(type: "int", nullable: false),
                    WasteLastYear = table.Column<bool>(type: "bit", nullable: false),
                    UkPackagingWaste = table.Column<decimal>(type: "decimal(10,3)", nullable: true),
                    NonUkPackagingWaste = table.Column<decimal>(type: "decimal(10,3)", nullable: true),
                    NonPackagingWaste = table.Column<decimal>(type: "decimal(10,3)", nullable: true),
                    MaterialsNotProcessedOnSite = table.Column<decimal>(type: "decimal(10,3)", nullable: true),
                    Contaminents = table.Column<decimal>(type: "decimal(10,3)", nullable: true),
                    ProcessLoss = table.Column<decimal>(type: "decimal(10,3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialReprocessorDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialReprocessorDetails_AccreditationMaterial_AccreditationMaterialId",
                        column: x => x.AccreditationMaterialId,
                        principalTable: "AccreditationMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WasteCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccreditationMaterialId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WasteCodeTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WasteCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WasteCode_AccreditationMaterial_AccreditationMaterialId",
                        column: x => x.AccreditationMaterialId,
                        principalTable: "AccreditationMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WasteCode_WasteCodeType_WasteCodeTypeId",
                        column: x => x.WasteCodeTypeId,
                        principalSchema: "Lookup",
                        principalTable: "WasteCodeType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReprocessorSupportingInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialReprocessorDetailsId = table.Column<int>(type: "int", nullable: false),
                    ReprocessorSupportingInformationTypeId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Tonnes = table.Column<decimal>(type: "decimal(10,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReprocessorSupportingInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReprocessorSupportingInformation_MaterialReprocessorDetails_MaterialReprocessorDetailsId",
                        column: x => x.MaterialReprocessorDetailsId,
                        principalTable: "MaterialReprocessorDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReprocessorSupportingInformation_ReprocessorSupportingInformationType_ReprocessorSupportingInformationTypeId",
                        column: x => x.ReprocessorSupportingInformationTypeId,
                        principalSchema: "Lookup",
                        principalTable: "ReprocessorSupportingInformationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "AccreditationStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "None" },
                    { 1, "Started" },
                    { 2, "Submitted" },
                    { 3, "Accepted" },
                    { 4, "Queried" },
                    { 5, "Updated" },
                    { 6, "Granted" },
                    { 7, "Refused" },
                    { 8, "Withdrawn" },
                    { 9, "Suspended" },
                    { 10, "Cancelled" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "Country",
                columns: new[] { "CountryId", "CountryCode", "Name" },
                values: new object[,]
                {
                    { 4, "AF", "Afghanistan" },
                    { 8, "AL", "Albania" },
                    { 10, "AQ", "Antarctica" },
                    { 12, "DZ", "Algeria" },
                    { 16, "AS", "American Samoa" },
                    { 20, "AD", "Andorra" },
                    { 24, "AO", "Angola" },
                    { 28, "AG", "Antigua and Barbuda" },
                    { 31, "AZ", "Azerbaijan" },
                    { 32, "AR", "Argentina" },
                    { 36, "AU", "Australia" },
                    { 40, "AT", "Austria" },
                    { 44, "BS", "Bahamas" },
                    { 48, "BH", "Bahrain" },
                    { 50, "BD", "Bangladesh" },
                    { 51, "AM", "Armenia" },
                    { 52, "BB", "Barbados" },
                    { 56, "BE", "Belgium" },
                    { 60, "BM", "Bermuda" },
                    { 64, "BT", "Bhutan" },
                    { 68, "BO", "Bolivia (Plurinational State of)" },
                    { 70, "BA", "Bosnia and Herzegovina" },
                    { 72, "BW", "Botswana" },
                    { 74, "BV", "Bouvet Island" },
                    { 76, "BR", "Brazil" },
                    { 84, "BZ", "Belize" },
                    { 86, "IO", "British Indian Ocean Territory" },
                    { 90, "SB", "Solomon Islands" },
                    { 92, "VG", "Virgin Islands (British)" },
                    { 96, "BN", "Brunei Darussalam" },
                    { 100, "BG", "Bulgaria" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "Country",
                columns: new[] { "CountryId", "CountryCode", "Name" },
                values: new object[,]
                {
                    { 104, "MM", "Myanmar" },
                    { 108, "BI", "Burundi" },
                    { 112, "BY", "Belarus" },
                    { 116, "KH", "Cambodia" },
                    { 120, "CM", "Cameroon" },
                    { 124, "CA", "Canada" },
                    { 132, "CV", "Cabo Verde" },
                    { 136, "KY", "Cayman Islands" },
                    { 140, "CF", "Central African Republic" },
                    { 144, "LK", "Sri Lanka" },
                    { 148, "TD", "Chad" },
                    { 152, "CL", "Chile" },
                    { 156, "CN", "China" },
                    { 158, "TW", "Taiwan, Province of China" },
                    { 162, "CX", "Christmas Island" },
                    { 166, "CC", "Cocos (Keeling) Islands" },
                    { 170, "CO", "Colombia" },
                    { 174, "KM", "Comoros" },
                    { 175, "YT", "Mayotte" },
                    { 178, "CG", "Congo" },
                    { 180, "CD", "Congo (Democratic Republic of the)" },
                    { 184, "CK", "Cook Islands" },
                    { 188, "CR", "Costa Rica" },
                    { 191, "HR", "Croatia" },
                    { 192, "CU", "Cuba" },
                    { 196, "CY", "Cyprus" },
                    { 203, "CZ", "Czech Republic" },
                    { 204, "BJ", "Benin" },
                    { 208, "DK", "Denmark" },
                    { 212, "DM", "Dominica" },
                    { 214, "DO", "Dominican Republic" },
                    { 218, "EC", "Ecuador" },
                    { 222, "SV", "El Salvador" },
                    { 226, "GQ", "Equatorial Guinea" },
                    { 231, "ET", "Ethiopia" },
                    { 232, "ER", "Eritrea" },
                    { 233, "EE", "Estonia" },
                    { 234, "FO", "Faroe Islands" },
                    { 238, "FK", "Falkland Islands (Malvinas)" },
                    { 239, "GS", "South Georgia and the South Sandwich Islands" },
                    { 242, "FJ", "Fiji" },
                    { 246, "FI", "Finland" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "Country",
                columns: new[] { "CountryId", "CountryCode", "Name" },
                values: new object[,]
                {
                    { 248, "AX", "Åland Islands" },
                    { 250, "FR", "France" },
                    { 254, "GF", "French Guiana" },
                    { 258, "PF", "French Polynesia" },
                    { 260, "TF", "French Southern Territories" },
                    { 262, "DJ", "Djibouti" },
                    { 266, "GA", "Gabon" },
                    { 268, "GE", "Georgia" },
                    { 270, "GM", "Gambia" },
                    { 275, "PS", "Palestine, State of" },
                    { 276, "DE", "Germany" },
                    { 288, "GH", "Ghana" },
                    { 292, "GI", "Gibraltar" },
                    { 296, "KI", "Kiribati" },
                    { 300, "GR", "Greece" },
                    { 304, "GL", "Greenland" },
                    { 308, "GD", "Grenada" },
                    { 312, "GP", "Guadeloupe" },
                    { 316, "GU", "Guam" },
                    { 320, "GT", "Guatemala" },
                    { 324, "GN", "Guinea" },
                    { 328, "GY", "Guyana" },
                    { 332, "HT", "Haiti" },
                    { 334, "HM", "Heard Island and McDonald Islands" },
                    { 336, "VA", "Holy See" },
                    { 340, "HN", "Honduras" },
                    { 344, "HK", "Hong Kong" },
                    { 348, "HU", "Hungary" },
                    { 352, "IS", "Iceland" },
                    { 356, "IN", "India" },
                    { 360, "ID", "Indonesia" },
                    { 364, "IR", "Iran (Islamic Republic of)" },
                    { 368, "IQ", "Iraq" },
                    { 372, "IE", "Ireland" },
                    { 376, "IL", "Israel" },
                    { 380, "IT", "Italy" },
                    { 384, "CI", "Côte d'Ivoire" },
                    { 388, "JM", "Jamaica" },
                    { 392, "JP", "Japan" },
                    { 398, "KZ", "Kazakhstan" },
                    { 400, "JO", "Jordan" },
                    { 404, "KE", "Kenya" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "Country",
                columns: new[] { "CountryId", "CountryCode", "Name" },
                values: new object[,]
                {
                    { 408, "KP", "Korea (Democratic People's Republic of)" },
                    { 410, "KR", "Korea (Republic of)" },
                    { 414, "KW", "Kuwait" },
                    { 417, "KG", "Kyrgyzstan" },
                    { 418, "LA", "Lao People's Democratic Republic" },
                    { 422, "LB", "Lebanon" },
                    { 426, "LS", "Lesotho" },
                    { 428, "LV", "Latvia" },
                    { 430, "LR", "Liberia" },
                    { 434, "LY", "Libya" },
                    { 438, "LI", "Liechtenstein" },
                    { 440, "LT", "Lithuania" },
                    { 442, "LU", "Luxembourg" },
                    { 446, "MO", "Macao" },
                    { 450, "MG", "Madagascar" },
                    { 454, "MW", "Malawi" },
                    { 458, "MY", "Malaysia" },
                    { 462, "MV", "Maldives" },
                    { 466, "ML", "Mali" },
                    { 470, "MT", "Malta" },
                    { 474, "MQ", "Martinique" },
                    { 478, "MR", "Mauritania" },
                    { 480, "MU", "Mauritius" },
                    { 484, "MX", "Mexico" },
                    { 492, "MN", "Mongolia" },
                    { 498, "MC", "Monaco" },
                    { 499, "ME", "Montenegro" },
                    { 500, "MS", "Montserrat" },
                    { 504, "MA", "Morocco" },
                    { 508, "MZ", "Mozambique" },
                    { 512, "OM", "Oman" },
                    { 516, "NA", "Namibia" },
                    { 520, "NR", "Nauru" },
                    { 524, "NP", "Nepal" },
                    { 528, "NL", "Netherlands" },
                    { 531, "CW", "Curaçao" },
                    { 533, "AW", "Aruba" },
                    { 534, "SX", "Sint Maarten (Dutch part)" },
                    { 535, "BQ", "Bonaire, Sint Eustatius and Saba" },
                    { 540, "NC", "New Caledonia" },
                    { 548, "VU", "Vanuatu" },
                    { 554, "NZ", "New Zealand" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "Country",
                columns: new[] { "CountryId", "CountryCode", "Name" },
                values: new object[,]
                {
                    { 558, "NI", "Nicaragua" },
                    { 562, "NE", "Niger" },
                    { 566, "NG", "Nigeria" },
                    { 570, "NU", "Niue" },
                    { 574, "NF", "Norfolk Island" },
                    { 578, "NO", "Norway" },
                    { 580, "MP", "Northern Mariana Islands" },
                    { 581, "UM", "United States Minor Outlying Islands" },
                    { 583, "FM", "Micronesia (Federated States of)" },
                    { 584, "MH", "Marshall Islands" },
                    { 585, "PW", "Palau" },
                    { 586, "PK", "Pakistan" },
                    { 591, "PA", "Panama" },
                    { 598, "PG", "Papua New Guinea" },
                    { 600, "PY", "Paraguay" },
                    { 604, "PE", "Peru" },
                    { 608, "PH", "Philippines" },
                    { 612, "PN", "Pitcairn" },
                    { 616, "PL", "Poland" },
                    { 620, "PT", "Portugal" },
                    { 624, "GW", "Guinea-Bissau" },
                    { 626, "TL", "Timor-Leste" },
                    { 630, "PR", "Puerto Rico" },
                    { 634, "QA", "Qatar" },
                    { 638, "RE", "Réunion" },
                    { 642, "RO", "Romania" },
                    { 643, "RU", "Russian Federation" },
                    { 646, "RW", "Rwanda" },
                    { 652, "BL", "Saint Barthélemy" },
                    { 654, "SH", "Saint Helena, Ascension and Tristan da Cunha" },
                    { 659, "KN", "Saint Kitts and Nevis" },
                    { 660, "AI", "Anguilla" },
                    { 662, "LC", "Saint Lucia" },
                    { 663, "MF", "Saint Martin (French part)" },
                    { 666, "PM", "Saint Pierre and Miquelon" },
                    { 670, "VC", "Saint Vincent and the Grenadines" },
                    { 674, "SM", "San Marino" },
                    { 678, "ST", "Sao Tome and Principe" },
                    { 682, "SA", "Saudi Arabia" },
                    { 686, "SN", "Senegal" },
                    { 688, "RS", "Serbia" },
                    { 690, "SC", "Seychelles" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "Country",
                columns: new[] { "CountryId", "CountryCode", "Name" },
                values: new object[,]
                {
                    { 694, "SL", "Sierra Leone" },
                    { 702, "SG", "Singapore" },
                    { 703, "SK", "Slovakia" },
                    { 704, "VN", "Viet Nam" },
                    { 705, "SI", "Slovenia" },
                    { 706, "SO", "Somalia" },
                    { 710, "ZA", "South Africa" },
                    { 716, "ZW", "Zimbabwe" },
                    { 724, "ES", "Spain" },
                    { 728, "SS", "South Sudan" },
                    { 729, "SD", "Sudan" },
                    { 732, "EH", "Western Sahara" },
                    { 740, "SR", "Suriname" },
                    { 744, "SJ", "Svalbard and Jan Mayen" },
                    { 748, "SZ", "Swaziland" },
                    { 752, "SE", "Sweden" },
                    { 756, "CH", "Switzerland" },
                    { 760, "SY", "Syrian Arab Republic" },
                    { 762, "TJ", "Tajikistan" },
                    { 764, "TH", "Thailand" },
                    { 768, "TG", "Togo" },
                    { 772, "TK", "Tokelau" },
                    { 776, "TO", "Tonga" },
                    { 780, "TT", "Trinidad and Tobago" },
                    { 784, "AE", "United Arab Emirates" },
                    { 788, "TN", "Tunisia" },
                    { 792, "TR", "Turkey" },
                    { 795, "TM", "Turkmenistan" },
                    { 796, "TC", "Turks and Caicos Islands" },
                    { 798, "TV", "Tuvalu" },
                    { 800, "UG", "Uganda" },
                    { 804, "UA", "Ukraine" },
                    { 807, "MK", "Macedonia (the former Yugoslav Republic of)" },
                    { 818, "EG", "Egypt" },
                    { 826, "GB", "United Kingdom of Great Britain and Northern Ireland" },
                    { 831, "GG", "Guernsey" },
                    { 832, "JE", "Jersey" },
                    { 833, "IM", "Isle of Man" },
                    { 834, "TZ", "Tanzania, United Republic of" },
                    { 840, "US", "United States of America" },
                    { 850, "VI", "Virgin Islands (U.S.)" },
                    { 854, "BF", "Burkina Faso" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "Country",
                columns: new[] { "CountryId", "CountryCode", "Name" },
                values: new object[,]
                {
                    { 858, "UY", "Uruguay" },
                    { 860, "UZ", "Uzbekistan" },
                    { 862, "VE", "Venezuela (Bolivarian Republic of)" },
                    { 876, "WF", "Wallis and Futuna" },
                    { 882, "WS", "Samoa" },
                    { 887, "YE", "Yemen" },
                    { 894, "ZM", "Zambia" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "FileUploadStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Undefined" },
                    { 1, "Virus check success" },
                    { 2, "Upload complete" },
                    { 3, "Virus check failed" },
                    { 4, "Upload failed" },
                    { 5, "Deleted" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "FileUploadType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Undefined" },
                    { 1, "Business plan" },
                    { 2, "Recording system" },
                    { 3, "Sampling plan" },
                    { 4, "Broadly eviquivalent evidence" },
                    { 5, "Flow diagram" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "OperatorType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Reprocessor" },
                    { 1, "Exporter" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "ReprocessorSupportingInformationType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Undefined" },
                    { 1, "Non waste inputs" },
                    { 2, "Products produced last year" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "TaskName",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Undefined" },
                    { 1, "Waste licence and PRNs" },
                    { 2, "Upload a business plan" },
                    { 3, "About the material" },
                    { 4, "Upload supporting documents" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "TaskStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Undefined" },
                    { 1, "Not started" },
                    { 2, "Started" },
                    { 3, "Completed" },
                    { 4, "Cannot start yet" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "WasteCodeType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Undefined" },
                    { 1, "Material commodity code" },
                    { 2, "Waste description code" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accreditation_AccreditationStatusId",
                table: "Accreditation",
                column: "AccreditationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Accreditation_ExternalId",
                table: "Accreditation",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accreditation_OperatorTypeId",
                table: "Accreditation",
                column: "OperatorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accreditation_ReferenceNumber",
                table: "Accreditation",
                column: "ReferenceNumber",
                unique: true,
                filter: "[ReferenceNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Accreditation_SiteId",
                table: "Accreditation",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_AccreditationMaterial_ExternalId",
                table: "AccreditationMaterial",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccreditationMaterial_OverseasReprocessingSiteId",
                table: "AccreditationMaterial",
                column: "OverseasReprocessingSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_AccreditationMaterial_SiteId",
                table: "AccreditationMaterial",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_AccreditationTaskProgress_AccreditationId",
                table: "AccreditationTaskProgress",
                column: "AccreditationId");

            migrationBuilder.CreateIndex(
                name: "IX_AccreditationTaskProgress_TaskNameId",
                table: "AccreditationTaskProgress",
                column: "TaskNameId");

            migrationBuilder.CreateIndex(
                name: "IX_AccreditationTaskProgress_TaskStatusId",
                table: "AccreditationTaskProgress",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AccreditationTaskProgressMaterial_AccreditationMaterialId",
                table: "AccreditationTaskProgressMaterial",
                column: "AccreditationMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_AccreditationTaskProgressMaterial_AccreditationTaskProgressId",
                table: "AccreditationTaskProgressMaterial",
                column: "AccreditationTaskProgressId");

            migrationBuilder.CreateIndex(
                name: "IX_ExemptionReference_SiteId",
                table: "ExemptionReference",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUpload_AccreditationId",
                table: "FileUpload",
                column: "AccreditationId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUpload_FileUploadTypeId",
                table: "FileUpload",
                column: "FileUploadTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUpload_Status",
                table: "FileUpload",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialReprocessorDetails_AccreditationMaterialId",
                table: "MaterialReprocessorDetails",
                column: "AccreditationMaterialId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OverseasAddress_CountryId",
                table: "OverseasAddress",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_OverseasAgent_OverseasAddressId",
                table: "OverseasAgent",
                column: "OverseasAddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OverseasAgent_OverseasReprocessingSiteId",
                table: "OverseasAgent",
                column: "OverseasReprocessingSiteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OverseasReprocessingSite_AccreditationId",
                table: "OverseasReprocessingSite",
                column: "AccreditationId");

            migrationBuilder.CreateIndex(
                name: "IX_OverseasReprocessingSite_OverseasAddressId",
                table: "OverseasReprocessingSite",
                column: "OverseasAddressId",
                unique: true,
                filter: "[OverseasAddressId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ReprocessorSupportingInformation_MaterialReprocessorDetailsId",
                table: "ReprocessorSupportingInformation",
                column: "MaterialReprocessorDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_ReprocessorSupportingInformation_ReprocessorSupportingInformationTypeId",
                table: "ReprocessorSupportingInformation",
                column: "ReprocessorSupportingInformationTypeId");

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

            migrationBuilder.CreateIndex(
                name: "IX_SiteAuthority_SiteId",
                table: "SiteAuthority",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_WasteCode_AccreditationMaterialId",
                table: "WasteCode",
                column: "AccreditationMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_WasteCode_WasteCodeTypeId",
                table: "WasteCode",
                column: "WasteCodeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WastePermit_AccreditationId",
                table: "WastePermit",
                column: "AccreditationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WastePermit_OverseasReprocessingSiteId",
                table: "WastePermit",
                column: "OverseasReprocessingSiteId",
                unique: true,
                filter: "[OverseasReprocessingSiteId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccreditationTaskProgressMaterial");

            migrationBuilder.DropTable(
                name: "ExemptionReference");

            migrationBuilder.DropTable(
                name: "FileUpload");

            migrationBuilder.DropTable(
                name: "OverseasAgent");

            migrationBuilder.DropTable(
                name: "ReprocessorSupportingInformation");

            migrationBuilder.DropTable(
                name: "SiteAuthority");

            migrationBuilder.DropTable(
                name: "WasteCode");

            migrationBuilder.DropTable(
                name: "WastePermit");

            migrationBuilder.DropTable(
                name: "AccreditationTaskProgress");

            migrationBuilder.DropTable(
                name: "FileUploadStatus",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "FileUploadType",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "MaterialReprocessorDetails");

            migrationBuilder.DropTable(
                name: "ReprocessorSupportingInformationType",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "WasteCodeType",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "TaskName",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "TaskStatus",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "AccreditationMaterial");

            migrationBuilder.DropTable(
                name: "OverseasReprocessingSite");

            migrationBuilder.DropTable(
                name: "Accreditation");

            migrationBuilder.DropTable(
                name: "OverseasAddress");

            migrationBuilder.DropTable(
                name: "AccreditationStatus",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "OperatorType",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "Site");

            migrationBuilder.DropTable(
                name: "Country",
                schema: "Lookup");
        }
    }
}
