using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class accreditationFileUploadTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "public.FileUpload",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "public.AccreditationFileUpload",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccreditationId = table.Column<int>(type: "int", nullable: true),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Filename = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateUploaded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FileUploadTypeId = table.Column<int>(type: "int", nullable: true),
                    FileUploadStatusId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_public.AccreditationFileUpload", x => x.Id);
                    table.ForeignKey(
                        name: "FK_public.AccreditationFileUpload_Lookup.FileUploadStatus_FileUploadStatusId",
                        column: x => x.FileUploadStatusId,
                        principalTable: "Lookup.FileUploadStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_public.AccreditationFileUpload_Lookup.FileUploadType_FileUploadTypeId",
                        column: x => x.FileUploadTypeId,
                        principalTable: "Lookup.FileUploadType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_public.AccreditationFileUpload_Public.Accreditation_AccreditationId",
                        column: x => x.AccreditationId,
                        principalTable: "Public.Accreditation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_public.AccreditationFileUpload_AccreditationId",
                table: "public.AccreditationFileUpload",
                column: "AccreditationId");

            migrationBuilder.CreateIndex(
                name: "IX_public.AccreditationFileUpload_FileUploadStatusId",
                table: "public.AccreditationFileUpload",
                column: "FileUploadStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_public.AccreditationFileUpload_FileUploadTypeId",
                table: "public.AccreditationFileUpload",
                column: "FileUploadTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "public.AccreditationFileUpload");

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "public.FileUpload",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
