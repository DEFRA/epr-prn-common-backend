using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRegistrationTaskStatusId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationTaskStatusId",
                table: "Public.RegistrationTaskStatusQueryNote");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegistrationTaskStatusId",
                table: "Public.RegistrationTaskStatusQueryNote",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
