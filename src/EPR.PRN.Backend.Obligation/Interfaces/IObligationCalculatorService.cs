using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.Models;

namespace EPR.PRN.Backend.Obligation.Interfaces
{
    public interface IObligationCalculatorService
    {
        Task<ObligationCalculationResult> GetObligationCalculation(Guid organisationId, int year);

        Task<ObligationCalculationResult> GetObligationCalculation(IEnumerable<Guid> organisationIds, int year);

        Task<CalculationResult> CalculateAsync(Guid organisationId, List<SubmissionCalculationRequest> request);

        Task UpsertCalculatedPomDataAsync(Guid organisationId, List<ObligationCalculation> calculations);
    }
}