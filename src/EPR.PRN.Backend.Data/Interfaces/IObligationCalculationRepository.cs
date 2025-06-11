using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.Data.Interfaces
{
    public interface IObligationCalculationRepository
    {
        Task<List<ObligationCalculation>> GetObligationCalculationBySubmitterIdAndYear(Guid submitterId, int year);

        Task RemoveAndAddObligationCalculationBySubmitterIdAsync(Guid submitterId, List<ObligationCalculation> calculations);
    }
}