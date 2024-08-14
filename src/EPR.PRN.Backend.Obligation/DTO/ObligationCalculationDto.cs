using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Obligation.DTO
{
    [ExcludeFromCodeCoverage]
    public class ObligationCalculationDto
    {
        public int OrganisationId { get; set; }

        public string MaterialName { get; set; }

        public int MaterialObligationValue { get; set; }

        public int Year { get; set; }
    }
}
