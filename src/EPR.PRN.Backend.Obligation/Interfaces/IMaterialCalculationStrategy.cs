using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.Dto;

namespace EPR.PRN.Backend.Obligation.Interfaces
{
    public interface IMaterialCalculationStrategy
    {
        bool CanHandle(MaterialType materialType);
        List<ObligationCalculation> Calculate(CalculationRequestDto calculationRequest);
    }
}