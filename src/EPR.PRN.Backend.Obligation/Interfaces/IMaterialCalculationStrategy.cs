using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Enums;

namespace EPR.PRN.Backend.Obligation.Interfaces
{
    public interface IMaterialCalculationStrategy
    {
        bool CanHandle(MaterialType materialType);
        List<ObligationCalculation> Calculate(PomObligtionDto pomObligation, MaterialType materialType, Dictionary<int, Dictionary<MaterialType, double>> recyclingTargets);
    }
}