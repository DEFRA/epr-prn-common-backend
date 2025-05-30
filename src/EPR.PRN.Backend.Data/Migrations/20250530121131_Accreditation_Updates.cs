using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class Accreditation_Updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegulatorAccreditationRegistrationTaskStatus_Lookup.RegulatorTask_TaskId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegulatorAccreditationRegistrationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegulatorAccreditationTaskStatus_Lookup.RegulatorTask_TaskId",
                table: "Public.RegulatorAccreditationTaskStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegulatorAccreditationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                table: "Public.RegulatorAccreditationTaskStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegulatorAccreditationTaskStatus_Public.Accreditation_AccreditationId",
                table: "Public.RegulatorAccreditationTaskStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegulatorApplicationTaskStatus_Lookup.RegulatorTask_TaskId",
                table: "Public.RegulatorApplicationTaskStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegulatorApplicationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                table: "Public.RegulatorApplicationTaskStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegulatorRegistrationTaskStatus_Lookup.RegulatorTask_TaskId",
                table: "Public.RegulatorRegistrationTaskStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegulatorRegistrationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                table: "Public.RegulatorRegistrationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorRegistrationTaskStatus_TaskId",
                table: "Public.RegulatorRegistrationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorRegistrationTaskStatus_TaskStatusId",
                table: "Public.RegulatorRegistrationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorApplicationTaskStatus_TaskId",
                table: "Public.RegulatorApplicationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorApplicationTaskStatus_TaskStatusId",
                table: "Public.RegulatorApplicationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorAccreditationTaskStatus_TaskId",
                table: "Public.RegulatorAccreditationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorAccreditationTaskStatus_TaskStatusId",
                table: "Public.RegulatorAccreditationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorAccreditationRegistrationTaskStatus_TaskId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorAccreditationRegistrationTaskStatus_TaskStatusId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus");

            migrationBuilder.DeleteData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Public.RegulatorRegistrationTaskStatus");

            migrationBuilder.DropColumn(
                name: "TaskStatusId",
                table: "Public.RegulatorRegistrationTaskStatus");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Public.RegulatorApplicationTaskStatus");

            migrationBuilder.DropColumn(
                name: "TaskStatusId",
                table: "Public.RegulatorApplicationTaskStatus");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Public.RegulatorAccreditationTaskStatus");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Public.RegulatorAccreditationTaskStatus");

            migrationBuilder.DropColumn(
                name: "TaskStatusId",
                table: "Public.RegulatorAccreditationTaskStatus");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus");

            migrationBuilder.DropColumn(
                name: "TaskStatusId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus");

            migrationBuilder.AlterColumn<int>(
                name: "AccreditationId",
                table: "Public.RegulatorAccreditationTaskStatus",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeterminationDate",
                table: "Public.AccreditationDeterminationDate",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Lookup.RegulatorTask",
                columns: new[] { "Id", "ApplicationTypeId", "IsMaterialSpecific", "JourneyTypeId", "Name" },
                values: new object[,]
                {
                    { 27, 1, false, 2, "DulyMade" },
                    { 28, 2, false, 2, "DulyMade" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegulatorAccreditationTaskStatus_Public.Accreditation_AccreditationId",
                table: "Public.RegulatorAccreditationTaskStatus",
                column: "AccreditationId",
                principalTable: "Public.Accreditation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegulatorAccreditationTaskStatus_Public.Accreditation_AccreditationId",
                table: "Public.RegulatorAccreditationTaskStatus");

            migrationBuilder.DeleteData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "Public.RegulatorRegistrationTaskStatus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskStatusId",
                table: "Public.RegulatorRegistrationTaskStatus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "Public.RegulatorApplicationTaskStatus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskStatusId",
                table: "Public.RegulatorApplicationTaskStatus",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccreditationId",
                table: "Public.RegulatorAccreditationTaskStatus",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Public.RegulatorAccreditationTaskStatus",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "Public.RegulatorAccreditationTaskStatus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskStatusId",
                table: "Public.RegulatorAccreditationTaskStatus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskStatusId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeterminationDate",
                table: "Public.AccreditationDeterminationDate",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "Lookup.RegulatorTask",
                columns: new[] { "Id", "ApplicationTypeId", "IsMaterialSpecific", "JourneyTypeId", "Name" },
                values: new object[] { 21, 1, true, 2, "Overseas reprocessing sites and broadly equivalent evidence" });

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorRegistrationTaskStatus_TaskId",
                table: "Public.RegulatorRegistrationTaskStatus",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorRegistrationTaskStatus_TaskStatusId",
                table: "Public.RegulatorRegistrationTaskStatus",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorApplicationTaskStatus_TaskId",
                table: "Public.RegulatorApplicationTaskStatus",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorApplicationTaskStatus_TaskStatusId",
                table: "Public.RegulatorApplicationTaskStatus",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorAccreditationTaskStatus_TaskId",
                table: "Public.RegulatorAccreditationTaskStatus",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorAccreditationTaskStatus_TaskStatusId",
                table: "Public.RegulatorAccreditationTaskStatus",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorAccreditationRegistrationTaskStatus_TaskId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorAccreditationRegistrationTaskStatus_TaskStatusId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus",
                column: "TaskStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegulatorAccreditationRegistrationTaskStatus_Lookup.RegulatorTask_TaskId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus",
                column: "TaskId",
                principalTable: "Lookup.RegulatorTask",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegulatorAccreditationRegistrationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus",
                column: "TaskStatusId",
                principalTable: "Lookup.TaskStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegulatorAccreditationTaskStatus_Lookup.RegulatorTask_TaskId",
                table: "Public.RegulatorAccreditationTaskStatus",
                column: "TaskId",
                principalTable: "Lookup.RegulatorTask",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegulatorAccreditationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                table: "Public.RegulatorAccreditationTaskStatus",
                column: "TaskStatusId",
                principalTable: "Lookup.TaskStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegulatorAccreditationTaskStatus_Public.Accreditation_AccreditationId",
                table: "Public.RegulatorAccreditationTaskStatus",
                column: "AccreditationId",
                principalTable: "Public.Accreditation",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegulatorApplicationTaskStatus_Lookup.RegulatorTask_TaskId",
                table: "Public.RegulatorApplicationTaskStatus",
                column: "TaskId",
                principalTable: "Lookup.RegulatorTask",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegulatorApplicationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                table: "Public.RegulatorApplicationTaskStatus",
                column: "TaskStatusId",
                principalTable: "Lookup.TaskStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegulatorRegistrationTaskStatus_Lookup.RegulatorTask_TaskId",
                table: "Public.RegulatorRegistrationTaskStatus",
                column: "TaskId",
                principalTable: "Lookup.RegulatorTask",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegulatorRegistrationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                table: "Public.RegulatorRegistrationTaskStatus",
                column: "TaskStatusId",
                principalTable: "Lookup.TaskStatus",
                principalColumn: "Id");
        }
    }
}
