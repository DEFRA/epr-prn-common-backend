using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.Data.Interfaces
{
    public interface IObligationCalculationRepository
    {
        Task<List<ObligationCalculation>?> GetObligationCalculationByOrganisationId(int id);
        Task AddObligationCalculation(List<ObligationCalculation> calculation);
    }
}