using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddQueryNoteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Public.RegulatorRegistrationTaskStatus");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Public.RegulatorApplicationTaskStatus");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Public.RegulatorAccreditationTaskStatus");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus");

            migrationBuilder.CreateTable(
                name: "Public.QueryNote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.QueryNote", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Public.ApplicationTaskStatusQueryNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QueryNoteId = table.Column<int>(type: "int", nullable: false),
                    RegulatorApplicationTaskStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.ApplicationTaskStatusQueryNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.ApplicationTaskStatusQueryNotes_Public.QueryNote_QueryNoteId",
                        column: x => x.QueryNoteId,
                        principalTable: "Public.QueryNote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Public.ApplicationTaskStatusQueryNotes_Public.RegulatorApplicationTaskStatus_RegulatorApplicationTaskStatusId",
                        column: x => x.RegulatorApplicationTaskStatusId,
                        principalTable: "Public.RegulatorApplicationTaskStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Public.RegistrationTaskStatusQueryNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QueryNoteId = table.Column<int>(type: "int", nullable: false),
                    RegulatorRegistrationTaskStatusId = table.Column<int>(type: "int", nullable: false),
                    RegistrationTaskStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.RegistrationTaskStatusQueryNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.RegistrationTaskStatusQueryNotes_Public.QueryNote_QueryNoteId",
                        column: x => x.QueryNoteId,
                        principalTable: "Public.QueryNote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Public.RegistrationTaskStatusQueryNotes_Public.RegulatorRegistrationTaskStatus_RegulatorRegistrationTaskStatusId",
                        column: x => x.RegulatorRegistrationTaskStatusId,
                        principalTable: "Public.RegulatorRegistrationTaskStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Public.ApplicationTaskStatusQueryNotes_QueryNoteId",
                table: "Public.ApplicationTaskStatusQueryNotes",
                column: "QueryNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.ApplicationTaskStatusQueryNotes_RegulatorApplicationTaskStatusId",
                table: "Public.ApplicationTaskStatusQueryNotes",
                column: "RegulatorApplicationTaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationTaskStatusQueryNotes_QueryNoteId",
                table: "Public.RegistrationTaskStatusQueryNotes",
                column: "QueryNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationTaskStatusQueryNotes_RegulatorRegistrationTaskStatusId",
                table: "Public.RegistrationTaskStatusQueryNotes",
                column: "RegulatorRegistrationTaskStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Public.ApplicationTaskStatusQueryNotes");

            migrationBuilder.DropTable(
                name: "Public.RegistrationTaskStatusQueryNotes");

            migrationBuilder.DropTable(
                name: "Public.QueryNote");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Public.RegulatorRegistrationTaskStatus",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Public.RegulatorApplicationTaskStatus",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Public.RegulatorAccreditationTaskStatus",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
