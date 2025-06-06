using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AccreditationDulyMadeUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Public.AccreditationDulyMade_Lookup.TaskStatus_TaskStatusId",
                table: "Public.AccreditationDulyMade");

            migrationBuilder.DropIndex(
                name: "IX_Public.AccreditationDulyMade_TaskStatusId",
                table: "Public.AccreditationDulyMade");

            migrationBuilder.DropColumn(
                name: "TaskStatusId",
                table: "Public.AccreditationDulyMade");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DulyMadeDate",
                table: "Public.AccreditationDulyMade",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DulyMadeBy",
                table: "Public.AccreditationDulyMade",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DulyMadeDate",
                table: "Public.AccreditationDulyMade",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "DulyMadeBy",
                table: "Public.AccreditationDulyMade",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "TaskStatusId",
                table: "Public.AccreditationDulyMade",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Public.AccreditationDulyMade_TaskStatusId",
                table: "Public.AccreditationDulyMade",
                column: "TaskStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.AccreditationDulyMade_Lookup.TaskStatus_TaskStatusId",
                table: "Public.AccreditationDulyMade",
                column: "TaskStatusId",
                principalTable: "Lookup.TaskStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
