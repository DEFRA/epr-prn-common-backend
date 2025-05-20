using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Accreditation.API.Common.Data.Migrations
{
    public partial class MakingcountryIdnullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverseasAddress_Country_CountryId",
                table: "OverseasAddress");

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "OverseasAddress",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_OverseasAddress_Country_CountryId",
                table: "OverseasAddress",
                column: "CountryId",
                principalSchema: "Lookup",
                principalTable: "Country",
                principalColumn: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverseasAddress_Country_CountryId",
                table: "OverseasAddress");

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "OverseasAddress",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OverseasAddress_Country_CountryId",
                table: "OverseasAddress",
                column: "CountryId",
                principalSchema: "Lookup",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
