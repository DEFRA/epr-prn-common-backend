using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetAccreditationIdAndDeterminationDateToNotNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegulatorAccreditationTaskStatus_Public.Accreditation_AccreditationId",
                table: "Public.RegulatorAccreditationTaskStatus");

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

            migrationBuilder.AlterColumn<int>(
                name: "AccreditationId",
                table: "Public.RegulatorAccreditationTaskStatus",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeterminationDate",
                table: "Public.AccreditationDeterminationDate",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegulatorAccreditationTaskStatus_Public.Accreditation_AccreditationId",
                table: "Public.RegulatorAccreditationTaskStatus",
                column: "AccreditationId",
                principalTable: "Public.Accreditation",
                principalColumn: "Id");
        }
    }
}
