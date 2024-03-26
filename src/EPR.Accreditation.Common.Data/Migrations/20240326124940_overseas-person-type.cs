using EPR.Accreditation.API.Common.Data.SeedData;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Accreditation.API.Common.Data.Migrations
{
    public partial class overseaspersontype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OverseasAgent");

            migrationBuilder.CreateTable(
                name: "OverseasPersonType",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverseasPersonType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OverseasContactPerson",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OverseasAddressId = table.Column<int>(type: "int", nullable: false),
                    OverseasReprocessingSiteId = table.Column<int>(type: "int", nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OverseasPersonTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverseasContactPerson", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OverseasContactPerson_OverseasAddress_OverseasAddressId",
                        column: x => x.OverseasAddressId,
                        principalTable: "OverseasAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OverseasContactPerson_OverseasPersonType_OverseasPersonTypeId",
                        column: x => x.OverseasPersonTypeId,
                        principalSchema: "Lookup",
                        principalTable: "OverseasPersonType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OverseasContactPerson_OverseasReprocessingSite_OverseasReprocessingSiteId",
                        column: x => x.OverseasReprocessingSiteId,
                        principalTable: "OverseasReprocessingSite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OverseasContactPerson_OverseasAddressId",
                table: "OverseasContactPerson",
                column: "OverseasAddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OverseasContactPerson_OverseasPersonTypeId",
                table: "OverseasContactPerson",
                column: "OverseasPersonTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OverseasContactPerson_OverseasReprocessingSiteId",
                table: "OverseasContactPerson",
                column: "OverseasReprocessingSiteId",
                unique: true);

            AddOverseasPersonType.Seed(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OverseasContactPerson");

            migrationBuilder.DropTable(
                name: "OverseasPersonType",
                schema: "Lookup");

            migrationBuilder.CreateTable(
                name: "OverseasAgent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OverseasAddressId = table.Column<int>(type: "int", nullable: false),
                    OverseasReprocessingSiteId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Fullname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverseasAgent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OverseasAgent_OverseasAddress_OverseasAddressId",
                        column: x => x.OverseasAddressId,
                        principalTable: "OverseasAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OverseasAgent_OverseasReprocessingSite_OverseasReprocessingSiteId",
                        column: x => x.OverseasReprocessingSiteId,
                        principalTable: "OverseasReprocessingSite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OverseasAgent_OverseasAddressId",
                table: "OverseasAgent",
                column: "OverseasAddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OverseasAgent_OverseasReprocessingSiteId",
                table: "OverseasAgent",
                column: "OverseasReprocessingSiteId",
                unique: true);
        }
    }
}
