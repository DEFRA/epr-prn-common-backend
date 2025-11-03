using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    public partial class AddAccreditationDulyMade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Public.AccreditationDulyMade",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(nullable: false),
                    AccreditationId = table.Column<int>(nullable: false),
                    TaskStatusId = table.Column<int>(nullable: false),
                    DulyMadeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DulyMadeBy = table.Column<Guid>(nullable: true),
                    DeterminationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DulyMadeNote = table.Column<string>(maxLength: 500, nullable: true),
                    DeterminationNote = table.Column<string>(maxLength: 500, nullable: true),
                    DeterminationUpdatedBy = table.Column<Guid>(nullable: true),
                    DeterminationUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.AccreditationDulyMade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.AccreditationDulyMade_Public.Accreditation_AccreditationId",
                        column: x => x.AccreditationId,
                        principalTable: "Public.Accreditation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Public.AccreditationDulyMade_Lookup.TaskStatus_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalTable: "Lookup.TaskStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });


            migrationBuilder.CreateIndex(
                name: "IX_Public.AccreditationDulyMade_AccreditationId",
                table: "Public.AccreditationDulyMade",
                column: "AccreditationId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.AccreditationDulyMade_TaskStatusId",
                table: "Public.AccreditationDulyMade",
                column: "TaskStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Public.AccreditationDulyMade");
        }

    }
}
