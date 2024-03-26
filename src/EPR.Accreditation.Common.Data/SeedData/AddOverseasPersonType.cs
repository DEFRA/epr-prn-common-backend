using Microsoft.EntityFrameworkCore.Migrations;

namespace EPR.Accreditation.API.Common.Data.SeedData
{
    public static class AddOverseasPersonType
    {
        public static void Seed(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                "OverseasPersonType",
                schema: "Lookup",
                columns: new[] { "Id", "Name" },
                values: new object[] { (int)Enums.OverseasPersonType.Undefined, "Undefined" });

            migrationBuilder.InsertData(
                "OverseasPersonType",
                schema: "Lookup",
                columns: new[] { "Id", "Name" },
                values: new object[] { (int)Enums.OverseasPersonType.OverseasAgent, "OverseasAgent" });

            migrationBuilder.InsertData(
                "OverseasPersonType",
                schema: "Lookup",
                columns: new[] { "Id", "Name" },
                values: new object[] { (int)Enums.OverseasPersonType.ReprocessorContact, "ReprocessorContact" });
        }
    }
}
