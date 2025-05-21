using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDulyMadeTableAndUpdatedLookUpMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationReferenceNumber",
                table: "Public.RegistrationMaterial",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Public.RegistrationMaterial",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Public.DulyMade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationMaterialId = table.Column<int>(type: "int", nullable: false),
                    TaskStatusId = table.Column<int>(type: "int", nullable: false),
                    DulyMadeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DulyMadeBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeterminationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.DulyMade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.DulyMade_Lookup.TaskStatus_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalTable: "Lookup.TaskStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Public.DulyMade_Public.RegistrationMaterial_RegistrationMaterialId",
                        column: x => x.RegistrationMaterialId,
                        principalTable: "Public.RegistrationMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Lookup.Material",
                keyColumn: "Id",
                keyValue: 2,
                column: "MaterialCode",
                value: "ST");

            migrationBuilder.InsertData(
                table: "Lookup.Material",
                columns: new[] { "Id", "MaterialCode", "MaterialName" },
                values: new object[,]
                {
                    { 4, "GL", "Glass" },
                    { 5, "PA", "Paper/Board" },
                    { 6, "WO", "Wood" }
                });

            migrationBuilder.InsertData(
                table: "Lookup.RegulatorTask",
                columns: new[] { "Id", "ApplicationTypeId", "IsMaterialSpecific", "JourneyTypeId", "Name" },
                values: new object[,]
                {
                    { 15, 1, true, 1, "CheckRegistrationStatus" },
                    { 16, 2, true, 1, "CheckRegistrationStatus" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Public.DulyMade_RegistrationMaterialId",
                table: "Public.DulyMade",
                column: "RegistrationMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.DulyMade_TaskStatusId",
                table: "Public.DulyMade",
                column: "TaskStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Public.DulyMade");

            migrationBuilder.DeleteData(
                table: "Lookup.Material",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Lookup.Material",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Lookup.Material",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DropColumn(
                name: "ApplicationReferenceNumber",
                table: "Public.RegistrationMaterial");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Public.RegistrationMaterial");

            migrationBuilder.UpdateData(
                table: "Lookup.Material",
                keyColumn: "Id",
                keyValue: 2,
                column: "MaterialCode",
                value: "GL");
        }
    }
}
