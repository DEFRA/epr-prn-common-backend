using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveunusedDulyMadeFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Public.DulyMade_Lookup.TaskStatus_TaskStatusId",
                table: "Public.DulyMade");

            migrationBuilder.DropIndex(
                name: "IX_Public.DulyMade_TaskStatusId",
                table: "Public.DulyMade");

            migrationBuilder.DropColumn(
                name: "DeterminationNote",
                table: "Public.DulyMade");

            migrationBuilder.DropColumn(
                name: "DeterminationUpdatedBy",
                table: "Public.DulyMade");

            migrationBuilder.DropColumn(
                name: "DeterminationUpdatedDate",
                table: "Public.DulyMade");

            migrationBuilder.DropColumn(
                name: "DulyMadeNote",
                table: "Public.DulyMade");

            migrationBuilder.DropColumn(
                name: "TaskStatusId",
                table: "Public.DulyMade");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "DulyMadeDate",
            //    table: "Public.DulyMade",
            //    type: "datetime2",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime2",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "DulyMadeBy",
            //    table: "Public.DulyMade",
            //    type: "uniqueidentifier",
            //    nullable: false,
            //    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            //    oldClrType: typeof(Guid),
            //    oldType: "uniqueidentifier",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "DeterminateDate",
            //    table: "Public.DeterminationDate",
            //    type: "datetime2",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime2",
            //    oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DulyMadeDate",
                table: "Public.DulyMade",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "DulyMadeBy",
                table: "Public.DulyMade",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "DeterminationNote",
                table: "Public.DulyMade",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeterminationUpdatedBy",
                table: "Public.DulyMade",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeterminationUpdatedDate",
                table: "Public.DulyMade",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DulyMadeNote",
                table: "Public.DulyMade",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskStatusId",
                table: "Public.DulyMade",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeterminateDate",
                table: "Public.DeterminationDate",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_Public.DulyMade_TaskStatusId",
                table: "Public.DulyMade",
                column: "TaskStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.DulyMade_Lookup.TaskStatus_TaskStatusId",
                table: "Public.DulyMade",
                column: "TaskStatusId",
                principalTable: "Lookup.TaskStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
