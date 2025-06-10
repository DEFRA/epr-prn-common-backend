using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class Accreditation_Updates_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegulatorTaskId",
                table: "Public.RegulatorRegistrationTaskStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaskStatusId",
                table: "Public.RegulatorRegistrationTaskStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RegulatorTaskId",
                table: "Public.RegulatorApplicationTaskStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaskStatusId",
                table: "Public.RegulatorApplicationTaskStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RegulatorTaskId",
                table: "Public.RegulatorAccreditationTaskStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaskStatusId",
                table: "Public.RegulatorAccreditationTaskStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RegulatorTaskId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaskStatusId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorRegistrationTaskStatus_RegulatorTaskId",
                table: "Public.RegulatorRegistrationTaskStatus",
                column: "RegulatorTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorRegistrationTaskStatus_TaskStatusId",
                table: "Public.RegulatorRegistrationTaskStatus",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorApplicationTaskStatus_RegulatorTaskId",
                table: "Public.RegulatorApplicationTaskStatus",
                column: "RegulatorTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorApplicationTaskStatus_TaskStatusId",
                table: "Public.RegulatorApplicationTaskStatus",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorAccreditationTaskStatus_RegulatorTaskId",
                table: "Public.RegulatorAccreditationTaskStatus",
                column: "RegulatorTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorAccreditationTaskStatus_TaskStatusId",
                table: "Public.RegulatorAccreditationTaskStatus",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorAccreditationRegistrationTaskStatus_RegulatorTaskId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus",
                column: "RegulatorTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorAccreditationRegistrationTaskStatus_TaskStatusId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus",
                column: "TaskStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegulatorAccreditationRegistrationTaskStatus_Lookup.RegulatorTask_RegulatorTaskId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus",
                column: "RegulatorTaskId",
                principalTable: "Lookup.RegulatorTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegulatorAccreditationRegistrationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus",
                column: "TaskStatusId",
                principalTable: "Lookup.TaskStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegulatorAccreditationTaskStatus_Lookup.RegulatorTask_RegulatorTaskId",
                table: "Public.RegulatorAccreditationTaskStatus",
                column: "RegulatorTaskId",
                principalTable: "Lookup.RegulatorTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegulatorAccreditationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                table: "Public.RegulatorAccreditationTaskStatus",
                column: "TaskStatusId",
                principalTable: "Lookup.TaskStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegulatorApplicationTaskStatus_Lookup.RegulatorTask_RegulatorTaskId",
                table: "Public.RegulatorApplicationTaskStatus",
                column: "RegulatorTaskId",
                principalTable: "Lookup.RegulatorTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegulatorApplicationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                table: "Public.RegulatorApplicationTaskStatus",
                column: "TaskStatusId",
                principalTable: "Lookup.TaskStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegulatorRegistrationTaskStatus_Lookup.RegulatorTask_RegulatorTaskId",
                table: "Public.RegulatorRegistrationTaskStatus",
                column: "RegulatorTaskId",
                principalTable: "Lookup.RegulatorTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegulatorRegistrationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                table: "Public.RegulatorRegistrationTaskStatus",
                column: "TaskStatusId",
                principalTable: "Lookup.TaskStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegulatorAccreditationRegistrationTaskStatus_Lookup.RegulatorTask_RegulatorTaskId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegulatorAccreditationRegistrationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegulatorAccreditationTaskStatus_Lookup.RegulatorTask_RegulatorTaskId",
                table: "Public.RegulatorAccreditationTaskStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegulatorAccreditationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                table: "Public.RegulatorAccreditationTaskStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegulatorApplicationTaskStatus_Lookup.RegulatorTask_RegulatorTaskId",
                table: "Public.RegulatorApplicationTaskStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegulatorApplicationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                table: "Public.RegulatorApplicationTaskStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegulatorRegistrationTaskStatus_Lookup.RegulatorTask_RegulatorTaskId",
                table: "Public.RegulatorRegistrationTaskStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegulatorRegistrationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                table: "Public.RegulatorRegistrationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorRegistrationTaskStatus_RegulatorTaskId",
                table: "Public.RegulatorRegistrationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorRegistrationTaskStatus_TaskStatusId",
                table: "Public.RegulatorRegistrationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorApplicationTaskStatus_RegulatorTaskId",
                table: "Public.RegulatorApplicationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorApplicationTaskStatus_TaskStatusId",
                table: "Public.RegulatorApplicationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorAccreditationTaskStatus_RegulatorTaskId",
                table: "Public.RegulatorAccreditationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorAccreditationTaskStatus_TaskStatusId",
                table: "Public.RegulatorAccreditationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorAccreditationRegistrationTaskStatus_RegulatorTaskId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorAccreditationRegistrationTaskStatus_TaskStatusId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus");

            migrationBuilder.DropColumn(
                name: "RegulatorTaskId",
                table: "Public.RegulatorRegistrationTaskStatus");

            migrationBuilder.DropColumn(
                name: "TaskStatusId",
                table: "Public.RegulatorRegistrationTaskStatus");

            migrationBuilder.DropColumn(
                name: "RegulatorTaskId",
                table: "Public.RegulatorApplicationTaskStatus");

            migrationBuilder.DropColumn(
                name: "TaskStatusId",
                table: "Public.RegulatorApplicationTaskStatus");

            migrationBuilder.DropColumn(
                name: "RegulatorTaskId",
                table: "Public.RegulatorAccreditationTaskStatus");

            migrationBuilder.DropColumn(
                name: "TaskStatusId",
                table: "Public.RegulatorAccreditationTaskStatus");

            migrationBuilder.DropColumn(
                name: "RegulatorTaskId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus");

            migrationBuilder.DropColumn(
                name: "TaskStatusId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus");
        }
    }
}
