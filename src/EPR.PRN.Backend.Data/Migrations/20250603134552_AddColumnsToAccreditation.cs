using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    public partial class AddCreatedOnAndPRNTonnageToAccreditation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Public.Accreditation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PRNTonnage",
                table: "Public.Accreditation",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "PRNTonnage",
                table: "Public.Accreditation");
        }
    }
}
