using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDulyMadeDeterminationDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
