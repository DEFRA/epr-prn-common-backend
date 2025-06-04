using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableDulyMadeAndRegistrationMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReferenceNumber",
                table: "Public.RegistrationMaterial",
                newName: "RegistrationReferenceNumber");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.RenameColumn(
                name: "RegistrationReferenceNumber",
                table: "Public.RegistrationMaterial",
                newName: "ReferenceNumber");
        }
    }
}
