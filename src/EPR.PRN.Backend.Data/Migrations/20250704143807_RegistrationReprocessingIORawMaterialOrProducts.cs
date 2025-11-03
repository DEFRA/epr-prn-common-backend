using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class RegistrationReprocessingIORawMaterialOrProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Public.RegistrationReprocessingIORawMaterialOrProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationReprocessingIOId = table.Column<int>(type: "int", nullable: false),
                    RawMaterialOrProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TonneValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsInput = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.RegistrationReprocessingIORawMaterialOrProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.RegistrationReprocessingIORawMaterialOrProducts_Public.RegistrationReprocessingIO_RegistrationReprocessingIOId",
                        column: x => x.RegistrationReprocessingIOId,
                        principalTable: "Public.RegistrationReprocessingIO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationReprocessingIORawMaterialOrProducts_ExternalId",
                table: "Public.RegistrationReprocessingIORawMaterialOrProducts",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationReprocessingIORawMaterialOrProducts_RegistrationReprocessingIOId",
                table: "Public.RegistrationReprocessingIORawMaterialOrProducts",
                column: "RegistrationReprocessingIOId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Public.RegistrationReprocessingIORawMaterialOrProducts");
        }
    }
}
