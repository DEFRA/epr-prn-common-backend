using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrationTaskStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Public.RegistrationTaskStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: true),
                    TaskStatusId = table.Column<int>(type: "int", nullable: true),
                    RegistrationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.RegistrationTaskStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.RegistrationTaskStatus_Lookup.RegulatorTask_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Lookup.RegulatorTask",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Public.RegistrationTaskStatus_Lookup.TaskStatus_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalTable: "Lookup.TaskStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Public.RegistrationTaskStatus_Public.Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Public.Registration",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationTaskStatus_RegistrationId",
                table: "Public.RegistrationTaskStatus",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationTaskStatus_TaskId",
                table: "Public.RegistrationTaskStatus",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationTaskStatus_TaskStatusId",
                table: "Public.RegistrationTaskStatus",
                column: "TaskStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Public.RegistrationTaskStatus");
        }
    }
}
