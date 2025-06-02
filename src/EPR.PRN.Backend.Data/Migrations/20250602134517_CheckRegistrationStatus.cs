using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CheckRegistrationStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Public.DulyMade_RegistrationMaterialId",
                table: "Public.DulyMade");

            migrationBuilder.DropColumn(
                name: "DeterminationDate",
                table: "Public.DulyMade");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "public.FileUpload",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "Public.DeterminationDate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationMaterialId = table.Column<int>(type: "int", nullable: false),
                    DeterminateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.DeterminationDate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.DeterminationDate_Public.RegistrationMaterial_RegistrationMaterialId",
                        column: x => x.RegistrationMaterialId,
                        principalTable: "Public.RegistrationMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Public.DulyMade_RegistrationMaterialId",
                table: "Public.DulyMade",
                column: "RegistrationMaterialId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.DeterminationDate_RegistrationMaterialId",
                table: "Public.DeterminationDate",
                column: "RegistrationMaterialId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Public.DeterminationDate");

            migrationBuilder.DropIndex(
                name: "IX_Public.DulyMade_RegistrationMaterialId",
                table: "Public.DulyMade");

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "public.FileUpload",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeterminationDate",
                table: "Public.DulyMade",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.DulyMade_RegistrationMaterialId",
                table: "Public.DulyMade",
                column: "RegistrationMaterialId");
        }
    }
}
