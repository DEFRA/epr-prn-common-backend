using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExternalIdUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
