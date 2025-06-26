using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsOverdueColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    ALTER TABLE [Public.DeterminationDate]
                    ADD [IsOverdue] AS CASE 
                        WHEN [DeterminateDate] > GETUTCDATE() THEN CAST(0 AS BIT)
                        ELSE CAST(1 AS BIT)
                    END
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    ALTER TABLE [Public.DeterminationDate]
                    DROP COLUMN [IsOverdue] ");
        }

    }
}
