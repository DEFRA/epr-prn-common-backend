using EPR.PRN.Backend.Obligation.DTO;

namespace EPR.PRN.Backend.Obligation.Models
{
    public class ObligationCalculationResult
    {
        public bool IsSuccess { get; set; }
        public string Errors { get; set; } = string.Empty;
        public ObligationModel? ObligationModel { get; set; }
    }
}