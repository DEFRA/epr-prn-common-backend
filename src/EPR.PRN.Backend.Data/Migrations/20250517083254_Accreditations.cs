using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class Accreditations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lookup.AccreditationStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookup.AccreditationStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Public.RegulatorAccreditationRegistrationTaskStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationId = table.Column<int>(type: "int", nullable: false),
                    AccreditationYear = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Public.RegulatorAccreditationRegistrationTaskStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.RegulatorAccreditationRegistrationTaskStatus_Lookup.RegulatorTask_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Lookup.RegulatorTask",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Public.RegulatorAccreditationRegistrationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalTable: "Lookup.TaskStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Public.RegulatorAccreditationRegistrationTaskStatus_Public.Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Public.Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Public.Accreditation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationMaterialId = table.Column<int>(type: "int", nullable: false),
                    AccreditationYear = table.Column<int>(type: "int", nullable: false),
                    AccreditationStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.Accreditation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.Accreditation_Lookup.AccreditationStatus_AccreditationStatusId",
                        column: x => x.AccreditationStatusId,
                        principalTable: "Lookup.AccreditationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Public.Accreditation_Public.RegistrationMaterial_RegistrationMaterialId",
                        column: x => x.RegistrationMaterialId,
                        principalTable: "Public.RegistrationMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Public.RegulatorAccreditationTaskStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccreditationId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Public.RegulatorAccreditationTaskStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.RegulatorAccreditationTaskStatus_Lookup.RegulatorTask_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Lookup.RegulatorTask",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Public.RegulatorAccreditationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalTable: "Lookup.TaskStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Public.RegulatorAccreditationTaskStatus_Public.Accreditation_AccreditationId",
                        column: x => x.AccreditationId,
                        principalTable: "Public.Accreditation",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Lookup.RegulatorTask",
                columns: new[] { "Id", "ApplicationTypeId", "IsMaterialSpecific", "JourneyTypeId", "Name" },
                values: new object[,]
                {
                    { 15, 1, false, 2, "AssignOfficer" },
                    { 16, 1, true, 2, "PRNs tonnage and authority to issue PRNs" },
                    { 17, 1, true, 2, "Business Plan" },
                    { 18, 1, true, 2, "Accreditation sampling and inspection plan" },
                    { 19, 1, true, 2, "Overseas reprocessing sites and broadly equivalent evidence" },
                    { 20, 2, false, 2, "AssignOfficer" },
                    { 21, 2, true, 2, "PRNs tonnage and authority to issue PRNs" },
                    { 22, 2, true, 2, "Business Plan" },
                    { 23, 2, true, 2, "Accreditation sampling and inspection plan" },
                    { 24, 2, true, 2, "Overseas reprocessing sites and broadly equivalent evidence" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Public.Accreditation_AccreditationStatusId",
                table: "Public.Accreditation",
                column: "AccreditationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.Accreditation_RegistrationMaterialId",
                table: "Public.Accreditation",
                column: "RegistrationMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorAccreditationRegistrationTaskStatus_RegistrationId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorAccreditationRegistrationTaskStatus_TaskId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorAccreditationRegistrationTaskStatus_TaskStatusId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorAccreditationTaskStatus_AccreditationId",
                table: "Public.RegulatorAccreditationTaskStatus",
                column: "AccreditationId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorAccreditationTaskStatus_TaskId",
                table: "Public.RegulatorAccreditationTaskStatus",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorAccreditationTaskStatus_TaskStatusId",
                table: "Public.RegulatorAccreditationTaskStatus",
                column: "TaskStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Public.RegulatorAccreditationRegistrationTaskStatus");

            migrationBuilder.DropTable(
                name: "Public.RegulatorAccreditationTaskStatus");

            migrationBuilder.DropTable(
                name: "Public.Accreditation");

            migrationBuilder.DropTable(
                name: "Lookup.AccreditationStatus");

            migrationBuilder.DeleteData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 24);
        }
    }
}
