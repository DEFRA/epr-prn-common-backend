using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.Models;

namespace EPR.PRN.Backend.Obligation.Dto
{
    public class CalculationRequestDto
    {
        public Guid SubmitterId { get; set; }

        public SubmissionCalculationRequest SubmissionCalculationRequest { get; set; } = new();

        public MaterialType MaterialType { get; set; }

        public List<Material> Materials { get; set; } = [];

        public int SubmitterTypeId { get; set; }

        public Dictionary<int, Dictionary<MaterialType, double>> RecyclingTargets { get; set; } = [];
    }
}
