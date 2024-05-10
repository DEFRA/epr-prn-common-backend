using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Accreditation.API.Common.Data.Migrations
{
    public partial class Addexternalidtomaterial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid?>(
                name: "ExternalId",
                table: "Material",
                type: "uniqueidentifier",
                nullable: true);

            // add external ids to material, then re-modify so that it is nullable
            migrationBuilder.Sql("UPDATE Material SET ExternalId = NEWID() WHERE ExternalId is NULL;");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExternalId",
                table: "Material",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.NewGuid());

            migrationBuilder.AlterColumn<decimal>(
                name: "AccreditationFee",
                table: "Accreditation",
                type: "decimal(19,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Material");

            migrationBuilder.AlterColumn<decimal>(
                name: "AccreditationFee",
                table: "Accreditation",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,2)",
                oldNullable: true);
        }
    }
}
