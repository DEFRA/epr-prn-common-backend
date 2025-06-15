using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccreditationBusinessPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessCollectionsNotes",
                table: "Public.Accreditation",
                type: "varchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BusinessCollectionsPercentage",
                table: "Public.Accreditation",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommunicationsNotes",
                table: "Public.Accreditation",
                type: "varchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CommunicationsPercentage",
                table: "Public.Accreditation",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InfrastructureNotes",
                table: "Public.Accreditation",
                type: "varchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InfrastructurePercentage",
                table: "Public.Accreditation",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewMarketsNotes",
                table: "Public.Accreditation",
                type: "varchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NewMarketsPercentage",
                table: "Public.Accreditation",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewUsersRecycledPackagingWasteNotes",
                table: "Public.Accreditation",
                type: "varchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NewUsersRecycledPackagingWastePercentage",
                table: "Public.Accreditation",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotCoveredOtherCategoriesNotes",
                table: "Public.Accreditation",
                type: "varchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NotCoveredOtherCategoriesPercentage",
                table: "Public.Accreditation",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecycledWasteNotes",
                table: "Public.Accreditation",
                type: "varchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RecycledWastePercentage",
                table: "Public.Accreditation",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPercentage",
                table: "Public.Accreditation",
                type: "decimal(10,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessCollectionsNotes",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "BusinessCollectionsPercentage",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "CommunicationsNotes",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "CommunicationsPercentage",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "InfrastructureNotes",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "InfrastructurePercentage",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "NewMarketsNotes",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "NewMarketsPercentage",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "NewUsersRecycledPackagingWasteNotes",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "NewUsersRecycledPackagingWastePercentage",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "NotCoveredOtherCategoriesNotes",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "NotCoveredOtherCategoriesPercentage",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "RecycledWasteNotes",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "RecycledWastePercentage",
                table: "Public.Accreditation");

            migrationBuilder.DropColumn(
                name: "TotalPercentage",
                table: "Public.Accreditation");
        }
    }
}
