using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrationMaterialContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Public.RegistrationMaterialContact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationMaterialId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.RegistrationMaterialContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.RegistrationMaterialContact_Public.RegistrationMaterial_RegistrationMaterialId",
                        column: x => x.RegistrationMaterialId,
                        principalTable: "Public.RegistrationMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationMaterialContact_ExternalId",
                table: "Public.RegistrationMaterialContact",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationMaterialContact_RegistrationMaterialId",
                table: "Public.RegistrationMaterialContact",
                column: "RegistrationMaterialId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Public.RegistrationMaterialContact");
        }
    }
}
