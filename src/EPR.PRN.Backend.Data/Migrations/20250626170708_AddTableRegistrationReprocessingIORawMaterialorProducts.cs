using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTableRegistrationReprocessingIORawMaterialorProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Public.RegistrationReprocessingIORawMaterialorProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationReprocessingIOId = table.Column<int>(type: "int", nullable: false),
                    RawMaterialNameorProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TonneValue = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsInput = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.RegistrationReprocessingIORawMaterialorProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.RegistrationReprocessingIORawMaterialorProducts_Public.RegistrationReprocessingIO_RegistrationReprocessingIOId",
                        column: x => x.RegistrationReprocessingIOId,
                        principalTable: "Public.RegistrationReprocessingIO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationReprocessingIORawMaterialorProducts_RegistrationReprocessingIOId",
                table: "Public.RegistrationReprocessingIORawMaterialorProducts",
                column: "RegistrationReprocessingIOId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Public.RegistrationReprocessingIORawMaterialorProducts");
        }
    }
}
