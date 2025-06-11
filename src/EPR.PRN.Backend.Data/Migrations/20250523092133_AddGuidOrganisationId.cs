using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGuidOrganisationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrganisationId",
                table: "Public.Registration",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Public.Registration");
        }
    }
}
