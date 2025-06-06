using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AccreditationTaskStatusQueryNote2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Public.AccreditationTaskStatusQueryNote_Public.RegulatorApplicationTaskStatus_RegulatorAccreditationTaskStatusId",
                table: "Public.AccreditationTaskStatusQueryNote");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.AccreditationTaskStatusQueryNote_Public.RegulatorAccreditationTaskStatus_RegulatorAccreditationTaskStatusId",
                table: "Public.AccreditationTaskStatusQueryNote",
                column: "RegulatorAccreditationTaskStatusId",
                principalTable: "Public.RegulatorAccreditationTaskStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Public.AccreditationTaskStatusQueryNote_Public.RegulatorAccreditationTaskStatus_RegulatorAccreditationTaskStatusId",
                table: "Public.AccreditationTaskStatusQueryNote");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.AccreditationTaskStatusQueryNote_Public.RegulatorApplicationTaskStatus_RegulatorAccreditationTaskStatusId",
                table: "Public.AccreditationTaskStatusQueryNote",
                column: "RegulatorAccreditationTaskStatusId",
                principalTable: "Public.RegulatorApplicationTaskStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
