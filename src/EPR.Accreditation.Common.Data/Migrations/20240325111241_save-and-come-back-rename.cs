using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Accreditation.API.Common.Data.Migrations
{
    public partial class saveandcomebackrename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaveAndContinue");

            migrationBuilder.CreateTable(
                name: "SaveAndComeBack",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Area = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Controller = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Action = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Parameters = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    AccreditationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveAndComeBack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaveAndComeBack_Accreditation_AccreditationId",
                        column: x => x.AccreditationId,
                        principalTable: "Accreditation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaveAndComeBack_AccreditationId",
                table: "SaveAndComeBack",
                column: "AccreditationId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaveAndComeBack");

            migrationBuilder.CreateTable(
                name: "SaveAndContinue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccreditationId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Area = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Controller = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Parameters = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveAndContinue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaveAndContinue_Accreditation_AccreditationId",
                        column: x => x.AccreditationId,
                        principalTable: "Accreditation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaveAndContinue_AccreditationId",
                table: "SaveAndContinue",
                column: "AccreditationId",
                unique: true);
        }
    }
}
