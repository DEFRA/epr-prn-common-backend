using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccreditationPrns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SiteCoordinates",
                table: "Public.OverseasAddress",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "OverseasSiteId",
                table: "public.AccreditationFileUpload",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubmissionId",
                table: "public.AccreditationFileUpload",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "'00000000-0000-0000-0000-000000000000'");

            migrationBuilder.AlterColumn<int>(
                name: "PRNTonnage",
                table: "Public.Accreditation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationReferenceNumber",
                table: "Public.Accreditation",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<int>(
                name: "AccreditationYear",
                table: "Public.Accreditation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Public.Accreditation",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "'00000000-0000-0000-0000-000000000000'");

            migrationBuilder.AddColumn<string>(
                name: "DecFullName",
                table: "Public.Accreditation",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DecJobTitle",
                table: "Public.Accreditation",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "Public.Accreditation",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "'00000000-0000-0000-0000-000000000000'");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Public.Accreditation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Lookup.MeetConditionsOfExport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookup.MeetConditionsOfExport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lookup.SiteCheckStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookup.SiteCheckStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Public.AccreditationPrnIssueAuth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccreditationExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccreditationId = table.Column<int>(type: "int", nullable: false),
                    PersonExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.AccreditationPrnIssueAuth", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.AccreditationPrnIssueAuth_Public.Accreditation_AccreditationId",
                        column: x => x.AccreditationId,
                        principalTable: "Public.Accreditation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Public.AccreditationTaskStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    AccreditationId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.AccreditationTaskStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.AccreditationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalTable: "Lookup.TaskStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Public.AccreditationTaskStatus_Lookup.Task_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Lookup.Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Public.AccreditationTaskStatus_Public.Accreditation_AccreditationId",
                        column: x => x.AccreditationId,
                        principalTable: "Public.Accreditation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Public.OverseasAccreditationSite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccreditationId = table.Column<int>(type: "int", nullable: false),
                    OverseasAddressId = table.Column<int>(type: "int", nullable: false),
                    OrganisationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeetConditionsOfExportId = table.Column<int>(type: "int", nullable: false),
                    StartDay = table.Column<int>(type: "int", nullable: false),
                    StartMonth = table.Column<int>(type: "int", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    ExpiryDay = table.Column<int>(type: "int", nullable: false),
                    ExpiryMonth = table.Column<int>(type: "int", nullable: false),
                    ExpiryYear = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SiteCheckStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.OverseasAccreditationSite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.OverseasAccreditationSite_Lookup.MeetConditionsOfExport_MeetConditionsOfExportId",
                        column: x => x.MeetConditionsOfExportId,
                        principalTable: "Lookup.MeetConditionsOfExport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Public.OverseasAccreditationSite_Lookup.SiteCheckStatus_SiteCheckStatusId",
                        column: x => x.SiteCheckStatusId,
                        principalTable: "Lookup.SiteCheckStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Public.OverseasAccreditationSite_Public.Accreditation_AccreditationId",
                        column: x => x.AccreditationId,
                        principalTable: "Public.Accreditation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Public.OverseasAccreditationSite_Public.OverseasAddress_OverseasAddressId",
                        column: x => x.OverseasAddressId,
                        principalTable: "Public.OverseasAddress",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Lookup.FileUploadType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "OverseasSiteEvidence" });

            migrationBuilder.InsertData(
                table: "Lookup.MeetConditionsOfExport",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Yes (Don't Upload)" },
                    { 2, "Yes (upload)" },
                    { 3, "No" }
                });

            migrationBuilder.InsertData(
                table: "Lookup.SiteCheckStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "NotStarted" },
                    { 2, "InProgress" },
                    { 3, "Completed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Public.AccreditationPrnIssueAuth_AccreditationId",
                table: "Public.AccreditationPrnIssueAuth",
                column: "AccreditationId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.AccreditationTaskStatus_AccreditationId",
                table: "Public.AccreditationTaskStatus",
                column: "AccreditationId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.AccreditationTaskStatus_ExternalId",
                table: "Public.AccreditationTaskStatus",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.AccreditationTaskStatus_TaskId",
                table: "Public.AccreditationTaskStatus",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.AccreditationTaskStatus_TaskStatusId",
                table: "Public.AccreditationTaskStatus",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAccreditationSite_AccreditationId",
                table: "Public.OverseasAccreditationSite",
                column: "AccreditationId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAccreditationSite_ExternalId",
                table: "Public.OverseasAccreditationSite",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAccreditationSite_MeetConditionsOfExportId",
                table: "Public.OverseasAccreditationSite",
                column: "MeetConditionsOfExportId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAccreditationSite_OverseasAddressId",
                table: "Public.OverseasAccreditationSite",
                column: "OverseasAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAccreditationSite_SiteCheckStatusId",
                table: "Public.OverseasAccreditationSite",
                column: "SiteCheckStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Public.AccreditationPrnIssueAuth");

            migrationBuilder.DropTable(
                name: "Public.AccreditationTaskStatus");

            migrationBuilder.DropTable(
                name: "Public.OverseasAccreditationSite");

            migrationBuilder.DropTable(
                name: "Lookup.MeetConditionsOfExport");

            migrationBuilder.DropTable(
                name: "Lookup.SiteCheckStatus");

            migrationBuilder.DeleteData(
                table: "Lookup.FileUploadType",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "OverseasSiteId",
                table: "public.AccreditationFileUpload");

            migrationBuilder.DropColumn(
                name: "SubmissionId",
                table: "public.AccreditationFileUpload");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "DecFullName",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "DecJobTitle",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Public.Accreditation");

            migrationBuilder.AlterColumn<string>(
                name: "SiteCoordinates",
                table: "Public.OverseasAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PRNTonnage",
                table: "Public.Accreditation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationReferenceNumber",
                table: "Public.Accreditation",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccreditationYear",
                table: "Public.Accreditation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
