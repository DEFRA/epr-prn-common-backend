using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AccreditationTaskStatusQueryNote : Migration
    {
        /// <inheritdoc />
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
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Public.AccreditationDulyMade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccreditationId = table.Column<int>(type: "int", nullable: false),
                    TaskStatusId = table.Column<int>(type: "int", nullable: false),
                    DulyMadeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DulyMadeBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeterminationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DulyMadeNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DeterminationNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DeterminationUpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeterminationUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.AccreditationDulyMade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.AccreditationDulyMade_Lookup.TaskStatus_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalTable: "Lookup.TaskStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Public.AccreditationTaskStatusQueryNote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QueryNoteId = table.Column<int>(type: "int", nullable: false),
                    RegulatorAccreditationTaskStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.AccreditationTaskStatusQueryNote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.AccreditationTaskStatusQueryNote_Public.Note_QueryNoteId",
                        column: x => x.QueryNoteId,
                        principalTable: "Public.Note",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Public.AccreditationTaskStatusQueryNote_Public.RegulatorApplicationTaskStatus_RegulatorAccreditationTaskStatusId",
                        column: x => x.RegulatorAccreditationTaskStatusId,
                        principalTable: "Public.RegulatorApplicationTaskStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Public.AccreditationDulyMade_TaskStatusId",
                table: "Public.AccreditationDulyMade",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.AccreditationTaskStatusQueryNote_QueryNoteId",
                table: "Public.AccreditationTaskStatusQueryNote",
                column: "QueryNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.AccreditationTaskStatusQueryNote_RegulatorAccreditationTaskStatusId",
                table: "Public.AccreditationTaskStatusQueryNote",
                column: "RegulatorAccreditationTaskStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Public.AccreditationDulyMade");

            migrationBuilder.DropTable(
                name: "Public.AccreditationTaskStatusQueryNote");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "PRNTonnage",
                table: "Public.Accreditation");
        }
    }
}
