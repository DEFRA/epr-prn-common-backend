using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveColumnsFromAccreditationDulyMade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeterminationDate",
                table: "Public.AccreditationDulyMade");

            migrationBuilder.DropColumn(
                name: "DeterminationNote",
                table: "Public.AccreditationDulyMade");

            migrationBuilder.DropColumn(
                name: "DeterminationUpdatedBy",
                table: "Public.AccreditationDulyMade");

            migrationBuilder.DropColumn(
                name: "DeterminationUpdatedDate",
                table: "Public.AccreditationDulyMade");

            migrationBuilder.DropColumn(
                name: "DulyMadeNote",
                table: "Public.AccreditationDulyMade");

            migrationBuilder.AddColumn<int>(
                name: "DeterminationDateId",
                table: "Public.AccreditationDulyMade",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.AccreditationDulyMade_DeterminationDateId",
                table: "Public.AccreditationDulyMade",
                column: "DeterminationDateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.AccreditationDulyMade_Public.AccreditationDeterminationDate_DeterminationDateId",
                table: "Public.AccreditationDulyMade",
                column: "DeterminationDateId",
                principalTable: "Public.AccreditationDeterminationDate",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Public.AccreditationDulyMade_Public.AccreditationDeterminationDate_DeterminationDateId",
                table: "Public.AccreditationDulyMade");

            migrationBuilder.DropIndex(
                name: "IX_Public.AccreditationDulyMade_DeterminationDateId",
                table: "Public.AccreditationDulyMade");

            migrationBuilder.DropColumn(
                name: "DeterminationDateId",
                table: "Public.AccreditationDulyMade");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeterminationDate",
                table: "Public.AccreditationDulyMade",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeterminationNote",
                table: "Public.AccreditationDulyMade",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeterminationUpdatedBy",
                table: "Public.AccreditationDulyMade",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeterminationUpdatedDate",
                table: "Public.AccreditationDulyMade",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DulyMadeNote",
                table: "Public.AccreditationDulyMade",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
