using EPR.PRN.Backend.Obligation.Enums;

namespace EPR.PRN.Backend.Obligation.Models
{
    public class AnnualRecyclingTargets
    {
        public int Year { get; set; }
        public Dictionary<MaterialType, double> Targets { get; set; } = [];
    }
}
