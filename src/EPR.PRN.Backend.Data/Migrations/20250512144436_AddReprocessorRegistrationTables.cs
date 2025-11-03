using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReprocessorRegistrationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lookup.ApplicationType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookup.ApplicationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lookup.FileUploadStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookup.FileUploadStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lookup.FileUploadType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookup.FileUploadType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lookup.JourneyType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookup.JourneyType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lookup.Material",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    MaterialCode = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookup.Material", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lookup.MaterialPermit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookup.MaterialPermit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lookup.Period",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookup.Period", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lookup.RegistrationMaterialStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookup.RegistrationMaterialStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lookup.RegulatorTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsMaterialSpecific = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationTypeId = table.Column<int>(type: "int", nullable: false),
                    JourneyTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookup.RegulatorTask", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lookup.TaskStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookup.TaskStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Public.Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressLine1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TownCity = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    County = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NationId = table.Column<int>(type: "int", nullable: true),
                    GridReference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Public.Registration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationTypeId = table.Column<int>(type: "int", nullable: false),
                    OrganisationId = table.Column<int>(type: "int", nullable: false),
                    RegistrationStatusId = table.Column<int>(type: "int", nullable: false),
                    BusinessAddressId = table.Column<int>(type: "int", nullable: true),
                    ReprocessingSiteAddressId = table.Column<int>(type: "int", nullable: true),
                    LegalDocumentAddressId = table.Column<int>(type: "int", nullable: true),
                    AssignedOfficerId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.Registration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.Registration_Public.Address_BusinessAddressId",
                        column: x => x.BusinessAddressId,
                        principalTable: "Public.Address",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Public.Registration_Public.Address_LegalDocumentAddressId",
                        column: x => x.LegalDocumentAddressId,
                        principalTable: "Public.Address",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Public.Registration_Public.Address_ReprocessingSiteAddressId",
                        column: x => x.ReprocessingSiteAddressId,
                        principalTable: "Public.Address",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Public.RegistrationMaterial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusUpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReasonforNotreg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermitTypeId = table.Column<int>(type: "int", nullable: false),
                    PPCPermitNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    WasteManagementLicenceNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InstallationPermitNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EnvironmentalPermitWasteManagementNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PPCReprocessingCapacityTonne = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WasteManagementReprocessingCapacityTonne = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InstallationReprocessingTonne = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EnvironmentalPermitWasteManagementTonne = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PPCPeriodId = table.Column<int>(type: "int", nullable: true),
                    WasteManagementPeriodId = table.Column<int>(type: "int", nullable: true),
                    InstallationPeriodId = table.Column<int>(type: "int", nullable: true),
                    EnvironmentalPermitWasteManagementPeriodId = table.Column<int>(type: "int", nullable: true),
                    MaximumReprocessingCapacityTonne = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximumReprocessingPeriodId = table.Column<int>(type: "int", nullable: true),
                    IsMaterialRegistered = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.RegistrationMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.RegistrationMaterial_Lookup.MaterialPermit_PermitTypeId",
                        column: x => x.PermitTypeId,
                        principalTable: "Lookup.MaterialPermit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Public.RegistrationMaterial_Lookup.Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Lookup.Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Public.RegistrationMaterial_Lookup.Period_EnvironmentalPermitWasteManagementPeriodId",
                        column: x => x.EnvironmentalPermitWasteManagementPeriodId,
                        principalTable: "Lookup.Period",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Public.RegistrationMaterial_Lookup.Period_InstallationPeriodId",
                        column: x => x.InstallationPeriodId,
                        principalTable: "Lookup.Period",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Public.RegistrationMaterial_Lookup.Period_MaximumReprocessingPeriodId",
                        column: x => x.MaximumReprocessingPeriodId,
                        principalTable: "Lookup.Period",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Public.RegistrationMaterial_Lookup.Period_PPCPeriodId",
                        column: x => x.PPCPeriodId,
                        principalTable: "Lookup.Period",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Public.RegistrationMaterial_Lookup.Period_WasteManagementPeriodId",
                        column: x => x.WasteManagementPeriodId,
                        principalTable: "Lookup.Period",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Public.RegistrationMaterial_Lookup.RegistrationMaterialStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Lookup.RegistrationMaterialStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Public.RegistrationMaterial_Public.Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Public.Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Public.RegulatorRegistrationTaskStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationId = table.Column<int>(type: "int", nullable: true),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: true),
                    TaskStatusId = table.Column<int>(type: "int", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StatusCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusUpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StatusUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.RegulatorRegistrationTaskStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.RegulatorRegistrationTaskStatus_Lookup.RegulatorTask_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Lookup.RegulatorTask",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Public.RegulatorRegistrationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalTable: "Lookup.TaskStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Public.RegulatorRegistrationTaskStatus_Public.Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Public.Registration",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "public.FileUpload",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationMaterialId = table.Column<int>(type: "int", nullable: true),
                    Filename = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateUploaded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileUploadTypeId = table.Column<int>(type: "int", nullable: true),
                    FileUploadStatusId = table.Column<int>(type: "int", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_public.FileUpload", x => x.Id);
                    table.ForeignKey(
                        name: "FK_public.FileUpload_Lookup.FileUploadStatus_FileUploadStatusId",
                        column: x => x.FileUploadStatusId,
                        principalTable: "Lookup.FileUploadStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_public.FileUpload_Lookup.FileUploadType_FileUploadTypeId",
                        column: x => x.FileUploadTypeId,
                        principalTable: "Lookup.FileUploadType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_public.FileUpload_Public.RegistrationMaterial_RegistrationMaterialId",
                        column: x => x.RegistrationMaterialId,
                        principalTable: "Public.RegistrationMaterial",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Public.MaterialExemptionReference",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationMaterialId = table.Column<int>(type: "int", nullable: false),
                    ReferenceNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.MaterialExemptionReference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.MaterialExemptionReference_Public.RegistrationMaterial_RegistrationMaterialId",
                        column: x => x.RegistrationMaterialId,
                        principalTable: "Public.RegistrationMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Public.RegistrationReprocessingIO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationMaterialId = table.Column<int>(type: "int", nullable: false),
                    TypeOfSupplier = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PlantEquipmentUsed = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ReprocessingPackagingWasteLastYearFlag = table.Column<bool>(type: "bit", nullable: false),
                    UKPackagingWasteTonne = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NonUKPackagingWasteTonne = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NotPackingWasteTonne = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SenttoOtherSiteTonne = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContaminantsTonne = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProcessLossTonne = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalInputs = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalOutputs = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.RegistrationReprocessingIO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.RegistrationReprocessingIO_Public.RegistrationMaterial_RegistrationMaterialId",
                        column: x => x.RegistrationMaterialId,
                        principalTable: "Public.RegistrationMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Public.RegulatorApplicationTaskStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationMaterialId = table.Column<int>(type: "int", nullable: true),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: true),
                    TaskStatusId = table.Column<int>(type: "int", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StatusCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusUpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StatusUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.RegulatorApplicationTaskStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.RegulatorApplicationTaskStatus_Lookup.RegulatorTask_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Lookup.RegulatorTask",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Public.RegulatorApplicationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalTable: "Lookup.TaskStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Public.RegulatorApplicationTaskStatus_Public.RegistrationMaterial_RegistrationMaterialId",
                        column: x => x.RegistrationMaterialId,
                        principalTable: "Public.RegistrationMaterial",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Lookup.ApplicationType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Reprocessor" },
                    { 2, "Exporter" }
                });

            migrationBuilder.InsertData(
                table: "Lookup.FileUploadStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Virus check failed" },
                    { 2, "Virus check succeeded" },
                    { 3, "Upload complete" },
                    { 4, "Upload failed" },
                    { 5, "File deleted(Soft delete of record in database – will physically remove from blob storage)" }
                });

            migrationBuilder.InsertData(
                table: "Lookup.FileUploadType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "SamplingAndInspectionPlan" });

            migrationBuilder.InsertData(
                table: "Lookup.JourneyType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Registration" },
                    { 2, "Accreditation" }
                });

            migrationBuilder.InsertData(
                table: "Lookup.Material",
                columns: new[] { "Id", "MaterialCode", "MaterialName" },
                values: new object[,]
                {
                    { 1, "PL", "Plastic" },
                    { 2, "GL", "Steel" },
                    { 3, "AL", "Aluminium" }
                });

            migrationBuilder.InsertData(
                table: "Lookup.MaterialPermit",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Waste Exemption" },
                    { 2, "Pollution,Prevention and Control(PPC) permit" },
                    { 3, "Waste Management Licence" },
                    { 4, "Installation Permit" },
                    { 5, "Environmental permit or waste management licence" }
                });

            migrationBuilder.InsertData(
                table: "Lookup.Period",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Per Year" },
                    { 2, "Per Month" },
                    { 3, "Per Week" }
                });

            migrationBuilder.InsertData(
                table: "Lookup.RegistrationMaterialStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Granted" },
                    { 2, "Refused" }
                });

            migrationBuilder.InsertData(
                table: "Lookup.RegulatorTask",
                columns: new[] { "Id", "ApplicationTypeId", "IsMaterialSpecific", "JourneyTypeId", "Name" },
                values: new object[,]
                {
                    { 1, 1, false, 1, "SiteAddressAndContactDetails" },
                    { 2, 1, false, 1, "MaterialsAuthorisedOnSite" },
                    { 3, 1, false, 1, "RegistrationDulyMade" },
                    { 4, 1, true, 1, "WasteLicensesPermitsAndExemptions" },
                    { 5, 1, true, 1, "ReprocessingInputsAndOutputs" },
                    { 6, 1, true, 1, "SamplingAndInspectionPlan" },
                    { 7, 1, true, 1, "AssignOfficer" },
                    { 8, 2, false, 1, "BusinessAddress" },
                    { 9, 2, false, 1, "WasteLicensesPermitsAndExemptions" },
                    { 10, 2, false, 1, "RegistrationDulyMade" },
                    { 11, 2, true, 1, "SamplingAndInspectionPlan" },
                    { 12, 2, true, 1, "AssignOfficer" },
                    { 13, 2, true, 1, "MaterialDetailsAndContact" },
                    { 14, 2, true, 1, "OverseasReprocessorAndInterimSiteDetails" }
                });

            migrationBuilder.InsertData(
                table: "Lookup.TaskStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "NotStarted" },
                    { 2, "Started" },
                    { 3, "CannotStartYet" },
                    { 4, "Queried" },
                    { 5, "Completed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_public.FileUpload_FileUploadStatusId",
                table: "public.FileUpload",
                column: "FileUploadStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_public.FileUpload_FileUploadTypeId",
                table: "public.FileUpload",
                column: "FileUploadTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_public.FileUpload_RegistrationMaterialId",
                table: "public.FileUpload",
                column: "RegistrationMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.MaterialExemptionReference_RegistrationMaterialId",
                table: "Public.MaterialExemptionReference",
                column: "RegistrationMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.Registration_BusinessAddressId",
                table: "Public.Registration",
                column: "BusinessAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.Registration_LegalDocumentAddressId",
                table: "Public.Registration",
                column: "LegalDocumentAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.Registration_ReprocessingSiteAddressId",
                table: "Public.Registration",
                column: "ReprocessingSiteAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationMaterial_EnvironmentalPermitWasteManagementPeriodId",
                table: "Public.RegistrationMaterial",
                column: "EnvironmentalPermitWasteManagementPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationMaterial_InstallationPeriodId",
                table: "Public.RegistrationMaterial",
                column: "InstallationPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationMaterial_MaterialId",
                table: "Public.RegistrationMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationMaterial_MaximumReprocessingPeriodId",
                table: "Public.RegistrationMaterial",
                column: "MaximumReprocessingPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationMaterial_PermitTypeId",
                table: "Public.RegistrationMaterial",
                column: "PermitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationMaterial_PPCPeriodId",
                table: "Public.RegistrationMaterial",
                column: "PPCPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationMaterial_RegistrationId",
                table: "Public.RegistrationMaterial",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationMaterial_StatusId",
                table: "Public.RegistrationMaterial",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationMaterial_WasteManagementPeriodId",
                table: "Public.RegistrationMaterial",
                column: "WasteManagementPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationReprocessingIO_RegistrationMaterialId",
                table: "Public.RegistrationReprocessingIO",
                column: "RegistrationMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorApplicationTaskStatus_RegistrationMaterialId",
                table: "Public.RegulatorApplicationTaskStatus",
                column: "RegistrationMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorApplicationTaskStatus_TaskId",
                table: "Public.RegulatorApplicationTaskStatus",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorApplicationTaskStatus_TaskStatusId",
                table: "Public.RegulatorApplicationTaskStatus",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorRegistrationTaskStatus_RegistrationId",
                table: "Public.RegulatorRegistrationTaskStatus",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorRegistrationTaskStatus_TaskId",
                table: "Public.RegulatorRegistrationTaskStatus",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorRegistrationTaskStatus_TaskStatusId",
                table: "Public.RegulatorRegistrationTaskStatus",
                column: "TaskStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lookup.ApplicationType");

            migrationBuilder.DropTable(
                name: "Lookup.JourneyType");

            migrationBuilder.DropTable(
                name: "public.FileUpload");

            migrationBuilder.DropTable(
                name: "Public.MaterialExemptionReference");

            migrationBuilder.DropTable(
                name: "Public.RegistrationReprocessingIO");

            migrationBuilder.DropTable(
                name: "Public.RegulatorApplicationTaskStatus");

            migrationBuilder.DropTable(
                name: "Public.RegulatorRegistrationTaskStatus");

            migrationBuilder.DropTable(
                name: "Lookup.FileUploadStatus");

            migrationBuilder.DropTable(
                name: "Lookup.FileUploadType");

            migrationBuilder.DropTable(
                name: "Public.RegistrationMaterial");

            migrationBuilder.DropTable(
                name: "Lookup.RegulatorTask");

            migrationBuilder.DropTable(
                name: "Lookup.TaskStatus");

            migrationBuilder.DropTable(
                name: "Lookup.MaterialPermit");

            migrationBuilder.DropTable(
                name: "Lookup.Material");

            migrationBuilder.DropTable(
                name: "Lookup.Period");

            migrationBuilder.DropTable(
                name: "Lookup.RegistrationMaterialStatus");

            migrationBuilder.DropTable(
                name: "Public.Registration");

            migrationBuilder.DropTable(
                name: "Public.Address");
        }
    }
}
