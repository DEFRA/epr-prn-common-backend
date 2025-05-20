using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Accreditation.API.Common.Data.Migrations
{
    public partial class ModifyMaterialNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "update [Accreditation].[dbo].[Material] set English = 'paper/board', Welsh = '[Welsh]paper/board' where english = 'Paper/Board'" +
                "update [Accreditation].[dbo].[Material] set English = 'paper composting', Welsh = '[Welsh]paper composting' where english = 'Paper Composting'" +
                "update [Accreditation].[dbo].[Material] set English = 'glass remelt', Welsh = '[Welsh]glass remelt' where english = 'Glass Remelt'" +
                "update [Accreditation].[dbo].[Material] set English = 'glass other', Welsh = '[Welsh]glass other' where english = 'Glass Other'" +
                "update [Accreditation].[dbo].[Material] set English = 'aluminium', Welsh = '[Welsh]aluminium' where english = 'Aluminium'" +
                "update [Accreditation].[dbo].[Material] set English = 'steel', Welsh = '[Welsh]steel' where english = 'Steel'" +
                "update [Accreditation].[dbo].[Material] set English = 'plastic', Welsh = '[Welsh]plastic' where english = 'Plastic'" +
                "update [Accreditation].[dbo].[Material] set English = 'wood', Welsh = '[Welsh]wood' where english = 'Wood'" +
                "update [Accreditation].[dbo].[Material] set English = 'wood composting', Welsh = '[Welsh]wood composting' where english = 'Wood Composting'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "update[Accreditation].[dbo].[Material] set English = 'Paper/Board', Welsh = '[Welsh]Paper/Board' where english = 'Paper/Board'" +
                "update[Accreditation].[dbo].[Material] set English = 'Paper Composting', Welsh = '[Welsh]Paper Composting' where english = 'Paper Composting'" +
                "update[Accreditation].[dbo].[Material] set English = 'Glass Remelt', Welsh = '[Welsh]Glass Remelt' where english = 'Glass Remelt'" +
                "update[Accreditation].[dbo].[Material] set English = 'Glass Other', Welsh = '[Welsh]Glass Other' where english = 'Glass Other'" +
                "update[Accreditation].[dbo].[Material] set English = 'Aluminium', Welsh = '[Welsh]Aluminium' where english = 'Aluminium'" +
                "update[Accreditation].[dbo].[Material] set English = 'Steel', Welsh = '[Welsh]Steel' where english = 'Steel'" +
                "update[Accreditation].[dbo].[Material] set English = 'Plastic', Welsh = '[Welsh]Plastic' where english = 'Plastic'" +
                "update[Accreditation].[dbo].[Material] set English = 'Wood', Welsh = '[Welsh]Wood' where english = 'Wood'" +
                "update[Accreditation].[dbo].[Material] set English = 'Wood Composting', Welsh = '[Welsh]Wood Composting' where english = 'Wood Composting'");
        }
    }
}
