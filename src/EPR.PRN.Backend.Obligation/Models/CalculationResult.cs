using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.Obligation.Models
{
    public class CalculationResult
    {
        public bool Success { get; set; }
        public List<ObligationCalculation> Calculations { get; set; } = new();
    }

}
