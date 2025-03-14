using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TownCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    County = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NationId = table.Column<int>(type: "int", nullable: false),
                    GridReference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeesAmount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeesAmount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileUploadStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUploadStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileUploadType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUploadType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialPermitType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialPermitType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Period",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Period", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskName", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Registration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationTypeId = table.Column<int>(type: "int", nullable: false),
                    RegistrationStatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessAddressId = table.Column<int>(type: "int", nullable: true),
                    ReprocessingSiteAddressId = table.Column<int>(type: "int", nullable: true),
                    LegalDocumentAddressId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registration_Address_BusinessAddressId",
                        column: x => x.BusinessAddressId,
                        principalTable: "Address",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Registration_Address_LegalDocumentAddressId",
                        column: x => x.LegalDocumentAddressId,
                        principalTable: "Address",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Registration_Address_ReprocessingSiteAddressId",
                        column: x => x.ReprocessingSiteAddressId,
                        principalTable: "Address",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Registration_ApplicationType_ApplicationTypeId",
                        column: x => x.ApplicationTypeId,
                        principalTable: "ApplicationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registration_RegistrationStatus_RegistrationStatusId",
                        column: x => x.RegistrationStatusId,
                        principalTable: "RegistrationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppRefPerMaterial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    ReferenceNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRefPerMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppRefPerMaterial_Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileUpload",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateUploaded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedBy = table.Column<Guid>(type: "uniqueidentifier", maxLength: 200, nullable: false),
                    FileUploadTypeId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    FileUploadStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUpload", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileUpload_FileUploadStatus_FileUploadStatusId",
                        column: x => x.FileUploadStatusId,
                        principalTable: "FileUploadStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileUpload_FileUploadType_FileUploadTypeId",
                        column: x => x.FileUploadTypeId,
                        principalTable: "FileUploadType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileUpload_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileUpload_Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationContact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    RegistrationContactId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationContact_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrationContact_RegistrationContact_RegistrationContactId",
                        column: x => x.RegistrationContactId,
                        principalTable: "RegistrationContact",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegistrationContact_Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationMaterial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    FeesId = table.Column<int>(type: "int", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    PermitTypeId = table.Column<int>(type: "int", nullable: false),
                    MaximumReprocessingCapacityTonne = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    MaximumReprocessingPeriodId = table.Column<int>(type: "int", nullable: false),
                    PPCReprocessingCapacityTonne = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PPCPeriodId = table.Column<int>(type: "int", nullable: true),
                    WasteManagementReprocessingCapacityTonne = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    WasteManagementPeriodId = table.Column<int>(type: "int", nullable: true),
                    InstallationReprocessingTonne = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    InstallationPeriodId = table.Column<int>(type: "int", nullable: true),
                    EnvironmentalPermitWasteManagementTonne = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    EnvironmentalPermitWasteManagementPeriodId = table.Column<int>(type: "int", nullable: true),
                    WasteCarrierBrokerDealerRegistration = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegisteredWasteCarrierBrokerDealerFlag = table.Column<bool>(type: "bit", nullable: false),
                    IsMaterialRegistered = table.Column<bool>(type: "bit", nullable: false),
                    ReasonForNotRegistration = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    MaxCapacityTonne = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    MaxPeriodId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationMaterial_FeesAmount_FeesId",
                        column: x => x.FeesId,
                        principalTable: "FeesAmount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrationMaterial_MaterialPermitType_PermitTypeId",
                        column: x => x.PermitTypeId,
                        principalTable: "MaterialPermitType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrationMaterial_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrationMaterial_Period_EnvironmentalPermitWasteManagementPeriodId",
                        column: x => x.EnvironmentalPermitWasteManagementPeriodId,
                        principalTable: "Period",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegistrationMaterial_Period_InstallationPeriodId",
                        column: x => x.InstallationPeriodId,
                        principalTable: "Period",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegistrationMaterial_Period_MaxPeriodId",
                        column: x => x.MaxPeriodId,
                        principalTable: "Period",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegistrationMaterial_Period_MaximumReprocessingPeriodId",
                        column: x => x.MaximumReprocessingPeriodId,
                        principalTable: "Period",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrationMaterial_Period_PPCPeriodId",
                        column: x => x.PPCPeriodId,
                        principalTable: "Period",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegistrationMaterial_Period_WasteManagementPeriodId",
                        column: x => x.WasteManagementPeriodId,
                        principalTable: "Period",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegistrationMaterial_Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationReprocessingIO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    TypeOfSuppliers = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ReprocessingPackgagingWasteLastYearFlag = table.Column<bool>(type: "bit", nullable: false),
                    UKPackgagingWasteTonne = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    NonUKPackgagingWasteTonne = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    NotUKPackgagingWasteTonne = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SentToOtherSiteTonne = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ContaminantsTonne = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ProcessLossTonne = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PlantEquipmentUsed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationReprocessingIO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationReprocessingIO_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrationReprocessingIO_Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaveAndContinue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Controller = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Parameters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveAndContinue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaveAndContinue_Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationTaskStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationContactId = table.Column<int>(type: "int", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    TaskStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationTaskStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationTaskStatus_RegistrationContact_RegistrationContactId",
                        column: x => x.RegistrationContactId,
                        principalTable: "RegistrationContact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrationTaskStatus_TaskName_TaskId",
                        column: x => x.TaskId,
                        principalTable: "TaskName",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrationTaskStatus_TaskStatus_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalTable: "TaskStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationProcessingIORawMaterial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationReprocessingIOId = table.Column<int>(type: "int", nullable: false),
                    RawMaterialName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TonneValue = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsInput = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationProcessingIORawMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationProcessingIORawMaterial_RegistrationReprocessingIO_RegistrationReprocessingIOId",
                        column: x => x.RegistrationReprocessingIOId,
                        principalTable: "RegistrationReprocessingIO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ApplicationType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Reprocessor" },
                    { 2, "Exporter" }
                });

            migrationBuilder.InsertData(
                table: "FileUploadStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Virus check failed" },
                    { 2, "Virus check succeeded" },
                    { 3, "Upload complete" },
                    { 4, "Upload failed" },
                    { 5, "File deleted" }
                });

            migrationBuilder.InsertData(
                table: "FileUploadType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "SamplingAndInspectionPlan" });

            migrationBuilder.InsertData(
                table: "MaterialPermitType",
                columns: new[] { "Id", "Name", "NationId" },
                values: new object[,]
                {
                    { 1, "Environmental permit or waste management licence", 0 },
                    { 2, "Installation Permit", 0 },
                    { 3, "Pollution, Prevention, and Control (PPC) permit", 0 },
                    { 4, "Waste Exemption", 0 },
                    { 5, "Waste Management Licence", 0 }
                });

            migrationBuilder.InsertData(
                table: "Period",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Per Week" },
                    { 2, "Per Month" },
                    { 3, "Per Year" }
                });

            migrationBuilder.InsertData(
                table: "RegistrationStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Accepted" },
                    { 2, "Cancelled" },
                    { 3, "Granted" },
                    { 4, "Queried" },
                    { 5, "Refused" },
                    { 6, "Started" },
                    { 7, "Submitted" },
                    { 8, "Suspended" },
                    { 9, "Updated" },
                    { 10, "Withdrawn" }
                });

            migrationBuilder.InsertData(
                table: "TaskName",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "SiteAddressAndContactDetails" },
                    { 2, "WasteLicensesPermitsAndExemption" },
                    { 3, "ReprocessingInputandOutput" },
                    { 4, "SamplingAndInspectionPlanPerMaterial" }
                });

            migrationBuilder.InsertData(
                table: "TaskStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Not started" },
                    { 2, "Started" },
                    { 3, "Completed" },
                    { 4, "Cannot start yet" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppRefPerMaterial_RegistrationId",
                table: "AppRefPerMaterial",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_FeesAmount_MaterialId",
                table: "FeesAmount",
                column: "MaterialId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileUpload_FileUploadStatusId",
                table: "FileUpload",
                column: "FileUploadStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUpload_FileUploadTypeId",
                table: "FileUpload",
                column: "FileUploadTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUpload_MaterialId",
                table: "FileUpload",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUpload_RegistrationId",
                table: "FileUpload",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_ApplicationTypeId",
                table: "Registration",
                column: "ApplicationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_BusinessAddressId",
                table: "Registration",
                column: "BusinessAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_LegalDocumentAddressId",
                table: "Registration",
                column: "LegalDocumentAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_RegistrationStatusId",
                table: "Registration",
                column: "RegistrationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_ReprocessingSiteAddressId",
                table: "Registration",
                column: "ReprocessingSiteAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationContact_MaterialId",
                table: "RegistrationContact",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationContact_RegistrationContactId",
                table: "RegistrationContact",
                column: "RegistrationContactId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationContact_RegistrationId",
                table: "RegistrationContact",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationMaterial_EnvironmentalPermitWasteManagementPeriodId",
                table: "RegistrationMaterial",
                column: "EnvironmentalPermitWasteManagementPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationMaterial_FeesId",
                table: "RegistrationMaterial",
                column: "FeesId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationMaterial_InstallationPeriodId",
                table: "RegistrationMaterial",
                column: "InstallationPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationMaterial_MaterialId",
                table: "RegistrationMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationMaterial_MaximumReprocessingPeriodId",
                table: "RegistrationMaterial",
                column: "MaximumReprocessingPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationMaterial_MaxPeriodId",
                table: "RegistrationMaterial",
                column: "MaxPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationMaterial_PermitTypeId",
                table: "RegistrationMaterial",
                column: "PermitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationMaterial_PPCPeriodId",
                table: "RegistrationMaterial",
                column: "PPCPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationMaterial_RegistrationId",
                table: "RegistrationMaterial",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationMaterial_WasteManagementPeriodId",
                table: "RegistrationMaterial",
                column: "WasteManagementPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationProcessingIORawMaterial_RegistrationReprocessingIOId",
                table: "RegistrationProcessingIORawMaterial",
                column: "RegistrationReprocessingIOId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationReprocessingIO_MaterialId",
                table: "RegistrationReprocessingIO",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationReprocessingIO_RegistrationId",
                table: "RegistrationReprocessingIO",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationTaskStatus_RegistrationContactId",
                table: "RegistrationTaskStatus",
                column: "RegistrationContactId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationTaskStatus_TaskId",
                table: "RegistrationTaskStatus",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationTaskStatus_TaskStatusId",
                table: "RegistrationTaskStatus",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_SaveAndContinue_RegistrationId",
                table: "SaveAndContinue",
                column: "RegistrationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppRefPerMaterial");

            migrationBuilder.DropTable(
                name: "FileUpload");

            migrationBuilder.DropTable(
                name: "RegistrationMaterial");

            migrationBuilder.DropTable(
                name: "RegistrationProcessingIORawMaterial");

            migrationBuilder.DropTable(
                name: "RegistrationTaskStatus");

            migrationBuilder.DropTable(
                name: "SaveAndContinue");

            migrationBuilder.DropTable(
                name: "FileUploadStatus");

            migrationBuilder.DropTable(
                name: "FileUploadType");

            migrationBuilder.DropTable(
                name: "FeesAmount");

            migrationBuilder.DropTable(
                name: "MaterialPermitType");

            migrationBuilder.DropTable(
                name: "Period");

            migrationBuilder.DropTable(
                name: "RegistrationReprocessingIO");

            migrationBuilder.DropTable(
                name: "RegistrationContact");

            migrationBuilder.DropTable(
                name: "TaskName");

            migrationBuilder.DropTable(
                name: "TaskStatus");

            migrationBuilder.DropTable(
                name: "Registration");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "ApplicationType");

            migrationBuilder.DropTable(
                name: "RegistrationStatus");
        }
    }
}
