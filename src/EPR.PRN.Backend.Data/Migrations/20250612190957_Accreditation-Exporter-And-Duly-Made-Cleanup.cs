using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AccreditationExporterAndDulyMadeCleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Public.AccreditationDulyMade_Lookup.TaskStatus_TaskStatusId",
                table: "Public.AccreditationDulyMade");

            migrationBuilder.DropForeignKey(
                name: "FK_Public.AccreditationDulyMade_Public.AccreditationDeterminationDate_DeterminationDateId",
                table: "Public.AccreditationDulyMade");

            migrationBuilder.DropIndex(
                name: "IX_Public.AccreditationDulyMade_DeterminationDateId",
                table: "Public.AccreditationDulyMade");

            migrationBuilder.DropIndex(
                name: "IX_Public.AccreditationDulyMade_TaskStatusId",
                table: "Public.AccreditationDulyMade");

            migrationBuilder.DropColumn(
                name: "DeterminationDateId",
                table: "Public.AccreditationDulyMade");

            migrationBuilder.DropColumn(
                name: "TaskStatusId",
                table: "Public.AccreditationDulyMade");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 18,
                column: "Name",
                value: "PRNsTonnageAndAuthorityToIssuePRNs");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 19,
                column: "Name",
                value: "BusinessPlan");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 20,
                column: "Name",
                value: "AccreditationSamplingAndInspectionPlan");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 23,
                column: "Name",
                value: "PERNsTonnageAndAuthorityToIssuePERNs");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 24,
                column: "Name",
                value: "BusinessPlan");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 25,
                column: "Name",
                value: "AccreditationSamplingAndInspectionPlan");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "OverseasReprocessingSitesAndEvidenceOfBroadlyEquivalentStandards");

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorRegistrationTaskStatus_ExternalId",
                table: "Public.RegulatorRegistrationTaskStatus",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorApplicationTaskStatus_ExternalId",
                table: "Public.RegulatorApplicationTaskStatus",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorAccreditationTaskStatus_ExternalId",
                table: "Public.RegulatorAccreditationTaskStatus",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegulatorAccreditationRegistrationTaskStatus_ExternalId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationTaskStatus_ExternalId",
                table: "Public.RegistrationTaskStatus",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationReprocessingIO_ExternalId",
                table: "Public.RegistrationReprocessingIO",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.RegistrationMaterial_ExternalId",
                table: "Public.RegistrationMaterial",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.Registration_ExternalId",
                table: "Public.Registration",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.MaterialExemptionReference_ExternalId",
                table: "Public.MaterialExemptionReference",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.DeterminationDate_ExternalId",
                table: "Public.DeterminationDate",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.AccreditationDulyMade_ExternalId",
                table: "Public.AccreditationDulyMade",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.AccreditationDeterminationDate_ExternalId",
                table: "Public.AccreditationDeterminationDate",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.Accreditation_ExternalId",
                table: "Public.Accreditation",
                column: "ExternalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorRegistrationTaskStatus_ExternalId",
                table: "Public.RegulatorRegistrationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorApplicationTaskStatus_ExternalId",
                table: "Public.RegulatorApplicationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorAccreditationTaskStatus_ExternalId",
                table: "Public.RegulatorAccreditationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegulatorAccreditationRegistrationTaskStatus_ExternalId",
                table: "Public.RegulatorAccreditationRegistrationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegistrationTaskStatus_ExternalId",
                table: "Public.RegistrationTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegistrationReprocessingIO_ExternalId",
                table: "Public.RegistrationReprocessingIO");

            migrationBuilder.DropIndex(
                name: "IX_Public.RegistrationMaterial_ExternalId",
                table: "Public.RegistrationMaterial");

            migrationBuilder.DropIndex(
                name: "IX_Public.Registration_ExternalId",
                table: "Public.Registration");

            migrationBuilder.DropIndex(
                name: "IX_Public.MaterialExemptionReference_ExternalId",
                table: "Public.MaterialExemptionReference");

            migrationBuilder.DropIndex(
                name: "IX_Public.DeterminationDate_ExternalId",
                table: "Public.DeterminationDate");

            migrationBuilder.DropIndex(
                name: "IX_Public.AccreditationDulyMade_ExternalId",
                table: "Public.AccreditationDulyMade");

            migrationBuilder.DropIndex(
                name: "IX_Public.AccreditationDeterminationDate_ExternalId",
                table: "Public.AccreditationDeterminationDate");

            migrationBuilder.DropIndex(
                name: "IX_Public.Accreditation_ExternalId",
                table: "Public.Accreditation");

            migrationBuilder.AddColumn<int>(
                name: "DeterminationDateId",
                table: "Public.AccreditationDulyMade",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskStatusId",
                table: "Public.AccreditationDulyMade",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 18,
                column: "Name",
                value: "PRNs tonnage and authority to issue PRNs");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 19,
                column: "Name",
                value: "Business Plan");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 20,
                column: "Name",
                value: "Accreditation sampling and inspection plan");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 23,
                column: "Name",
                value: "PRNs tonnage and authority to issue PRNs");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 24,
                column: "Name",
                value: "Business Plan");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 25,
                column: "Name",
                value: "Accreditation sampling and inspection plan");

            migrationBuilder.UpdateData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "Overseas reprocessing sites and broadly equivalent evidence");

            migrationBuilder.CreateIndex(
                name: "IX_Public.AccreditationDulyMade_DeterminationDateId",
                table: "Public.AccreditationDulyMade",
                column: "DeterminationDateId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.AccreditationDulyMade_TaskStatusId",
                table: "Public.AccreditationDulyMade",
                column: "TaskStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Public.AccreditationDulyMade_Lookup.TaskStatus_TaskStatusId",
                table: "Public.AccreditationDulyMade",
                column: "TaskStatusId",
                principalTable: "Lookup.TaskStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Public.AccreditationDulyMade_Public.AccreditationDeterminationDate_DeterminationDateId",
                table: "Public.AccreditationDulyMade",
                column: "DeterminationDateId",
                principalTable: "Public.AccreditationDeterminationDate",
                principalColumn: "Id");
        }
    }
}
