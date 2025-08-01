﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNoteTable : Migration
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

            migrationBuilder.CreateTable(
                name: "Public.Note",
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
                    table.PrimaryKey("PK_Public.Note", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Public.ApplicationTaskStatusQueryNote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QueryNoteId = table.Column<int>(type: "int", nullable: false),
                    RegulatorApplicationTaskStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.ApplicationTaskStatusQueryNote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.ApplicationTaskStatusQueryNote_Public.Note_QueryNoteId",
                        column: x => x.QueryNoteId,
                        principalTable: "Public.Note",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Public.ApplicationTaskStatusQueryNote_Public.RegulatorApplicationTaskStatus_RegulatorApplicationTaskStatusId",
                        column: x => x.RegulatorApplicationTaskStatusId,
                        principalTable: "Public.RegulatorApplicationTaskStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Public.RegistrationTaskStatusQueryNote",
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
                    table.PrimaryKey("PK_Public.RegistrationTaskStatusQueryNote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.RegistrationTaskStatusQueryNote_Public.Note_QueryNoteId",
                        column: x => x.QueryNoteId,
                        principalTable: "Public.Note",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Public.RegistrationTaskStatusQueryNote_Public.RegulatorRegistrationTaskStatus_RegulatorRegistrationTaskStatusId",
                        column: x => x.RegulatorRegistrationTaskStatusId,
                        principalTable: "Public.RegulatorRegistrationTaskStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Public.ApplicationTaskStatusQueryNote_QueryNoteId",
                table: "Public.ApplicationTaskStatusQueryNote",
                column: "QueryNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.ApplicationTaskStatusQueryNote_RegulatorApplicationTaskStatusId",
                table: "Public.ApplicationTaskStatusQueryNote",
                column: "RegulatorApplicationTaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationTaskStatusQueryNote_QueryNoteId",
                table: "Public.RegistrationTaskStatusQueryNote",
                column: "QueryNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationTaskStatusQueryNote_RegulatorRegistrationTaskStatusId",
                table: "Public.RegistrationTaskStatusQueryNote",
                column: "RegulatorRegistrationTaskStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Public.ApplicationTaskStatusQueryNote");

            migrationBuilder.DropTable(
                name: "Public.RegistrationTaskStatusQueryNote");

            migrationBuilder.DropTable(
                name: "Public.Note");

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
        }
    }
}
