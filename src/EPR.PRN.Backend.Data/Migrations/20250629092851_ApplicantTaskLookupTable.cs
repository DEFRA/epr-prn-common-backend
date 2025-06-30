using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class ApplicantTaskLookupTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegistrationTaskStatus_Lookup.RegulatorTask_TaskId",
                table: "Public.RegistrationTaskStatus");

            migrationBuilder.AddColumn<int>(
                name: "RegistrationMaterialId",
                table: "Public.RegistrationTaskStatus",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Lookup.Task",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsMaterialSpecific = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationTypeId = table.Column<int>(type: "int", nullable: false),
                    JourneyTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookup.Task", x => x.Id);
                });

            // Hate having to comment this out it really needs to be fixed properly
            // But for now commenting out as due to a previous failed merge that has messed up the migrations to a point where the below keeps getting added in
            // when it already exists in the database.
            //migrationBuilder.InsertData(
            //    table: "Lookup.RegulatorTask",
            //    columns: new[] { "Id", "ApplicationTypeId", "IsMaterialSpecific", "JourneyTypeId", "Name" },
            //    values: new object[] { 28, 2, true, 2, "DulyMade" });

            migrationBuilder.InsertData(
                table: "Lookup.Task",
                columns: new[] { "Id", "ApplicationTypeId", "IsMaterialSpecific", "JourneyTypeId", "Name" },
                values: new object[,]
                {
                    { 1, 1, false, 1, "SiteAddressAndContactDetails" },
                    { 2, 1, true, 1, "WasteLicensesPermitsAndExemptions" },
                    { 3, 1, true, 1, "ReprocessingInputsAndOutputs" },
                    { 4, 1, true, 1, "SamplingAndInspectionPlan" },
                    { 5, 2, false, 1, "WasteLicensesPermitsAndExemptions" },
                    { 6, 2, true, 1, "SamplingAndInspectionPlan" },
                    { 7, 2, true, 1, "OverseasReprocessingSites" },
                    { 8, 2, true, 1, "InterimSites" },
                    { 9, 1, true, 2, "PRNsTonnageAndAuthorityToIssuePRNs" },
                    { 10, 1, true, 2, "BusinessPlan" },
                    { 11, 1, true, 2, "AccreditationSamplingAndInspectionPlan" },
                    { 12, 2, true, 2, "PERNsTonnageAndAuthorityToIssuePERNs" },
                    { 13, 2, true, 2, "BusinessPlan" },
                    { 14, 2, true, 2, "AccreditationSamplingAndInspectionPlan" },
                    { 15, 2, true, 2, "OverseasReprocessingSitesAndEvidenceOfBroadlyEquivalentStandards" },
                    { 16, 2, false, 1, "WasteCarrierBrokerDealerNumber" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationTaskStatus_RegistrationMaterialId",
                table: "Public.RegistrationTaskStatus",
                column: "RegistrationMaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegistrationTaskStatus_Lookup.Task_TaskId",
                table: "Public.RegistrationTaskStatus",
                column: "TaskId",
                principalTable: "Lookup.Task",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegistrationTaskStatus_Public.RegistrationMaterial_RegistrationMaterialId",
                table: "Public.RegistrationTaskStatus",
                column: "RegistrationMaterialId",
                principalTable: "Public.RegistrationMaterial",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegistrationTaskStatus_Lookup.Task_TaskId",
                table: "Public.RegistrationTaskStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Public.RegistrationTaskStatus_Public.RegistrationMaterial_RegistrationMaterialId",
                table: "Public.RegistrationTaskStatus");

            migrationBuilder.DropTable(
                name: "Lookup.Task");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegistrationTaskStatus_RegistrationMaterialId",
                table: "Public.RegistrationTaskStatus");

            // Hate having to comment this out it really needs to be fixed properly
            // But for now commenting out as due to a previous failed merge that has messed up the migrations to a point where the below keeps getting added in
            // when it already exists in the database.
            //migrationBuilder.DeleteData(
            //    table: "Lookup.RegulatorTask",
            //    keyColumn: "Id",
            //    keyValue: 28);

            migrationBuilder.DropColumn(
                name: "RegistrationMaterialId",
                table: "Public.RegistrationTaskStatus");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.RegistrationTaskStatus_Lookup.RegulatorTask_TaskId",
                table: "Public.RegistrationTaskStatus",
                column: "TaskId",
                principalTable: "Lookup.RegulatorTask",
                principalColumn: "Id");
        }
    }
}
