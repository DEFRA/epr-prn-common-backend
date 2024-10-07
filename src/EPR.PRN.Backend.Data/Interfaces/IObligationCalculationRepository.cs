using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.Data.Interfaces
{
    public interface IObligationCalculationRepository
    {
        Task<List<ObligationCalculation>> GetObligationCalculation(Guid organisationId, int year);
        Task AddObligationCalculation(List<ObligationCalculation> calculation);
    }
}