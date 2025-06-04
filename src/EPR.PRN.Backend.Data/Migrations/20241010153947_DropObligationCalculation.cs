using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class DropObligationCalculation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
            name: "ObligationCalculations");

            migrationBuilder.CreateTable(
            name: "ObligationCalculations",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
                OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MaterialName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                MaterialObligationValue = table.Column<int>(type: "int", nullable: false),
                Year = table.Column<int>(type: "int", nullable: false),
                CalculatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                MaterialWeight = table.Column<float>(type: "float", nullable: false, defaultValue: 0.0)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ObligationCalculations", x => x.Id);
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "ObligationCalculations",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
                OrganisationId = table.Column<int>(type: "int", nullable: false),
                MaterialName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                MaterialObligationValue = table.Column<int>(type: "int", nullable: false),
                Year = table.Column<int>(type: "int", nullable: false),
                CalculatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                MaterialWeight = table.Column<float>(type: "float", nullable: false, defaultValue: 0.0)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ObligationCalculations", x => x.Id);
            });

            migrationBuilder.DropTable(
            name: "ObligationCalculations");
        }
    }
}
