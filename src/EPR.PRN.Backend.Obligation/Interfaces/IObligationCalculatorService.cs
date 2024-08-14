using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Enums;

namespace EPR.PRN.Backend.Obligation.Interfaces
{
    public interface IObligationCalculatorService
    {
        int Calculate(double target, int tonnage);
        (int remelt, int remainder) CalculateGlass(double target, double remeltTarget, int tonnage);
        Task<List<ObligationCalculationDto>?> GetObligationCalculationById(int id);
        Task ProcessApprovedPomData(int year, MaterialType materialType, int tonnage);
    }
}