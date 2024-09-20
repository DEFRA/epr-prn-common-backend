using EPR.PRN.Backend.Obligation.Enums;
using EPR.PRN.Backend.Obligation.Models;

namespace EPR.PRN.Backend.Obligation.DTO
{
    public class CalculationRequestDto
    {
        public int OrganisationId { get; set; }
        public SubmissionCalculationRequest SubmissionCalculationRequest { get; set; } = new();
        public MaterialType MaterialType { get; set; }
        public Dictionary<int, Dictionary<MaterialType, double>> RecyclingTargets { get; set; } = new();
    }
}
