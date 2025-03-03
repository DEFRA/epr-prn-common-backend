using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.Data.Interfaces
{
    public interface IObligationCalculationRepository
    {
        Task<List<ObligationCalculation>> GetObligationCalculation(IEnumerable<Guid> organisationIds, int year);

        Task AddObligationCalculation(List<ObligationCalculation> calculation);

        Task UpsertObligationCalculationAsync(Guid organisationId, List<ObligationCalculation> calculations);
    }
}