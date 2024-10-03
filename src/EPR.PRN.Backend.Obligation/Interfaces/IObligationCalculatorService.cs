using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Models;

namespace EPR.PRN.Backend.Obligation.Interfaces
{
    public interface IObligationCalculatorService
    {
        Task<List<PrnDataDto>?> GetObligationCalculationByOrganisationId(int id);
        Task<CalculationResult> CalculateAsync(int organisationId, List<SubmissionCalculationRequest> request);
        Task SaveCalculatedPomDataAsync(List<ObligationCalculation> calculations);
    }
}