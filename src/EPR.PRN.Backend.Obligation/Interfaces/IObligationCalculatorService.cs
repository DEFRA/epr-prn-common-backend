using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.Dto;
using EPR.PRN.Backend.Obligation.Models;

namespace EPR.PRN.Backend.Obligation.Interfaces
{
    public interface IObligationCalculatorService
    {
        Task<List<ObligationCalculationRecord>> GetCalculationRecords(Guid submitterId, int year);
        Task<ObligationCalculationResult> GetObligationCalculation(Guid organisationId, int year);

        Task<CalculationResult> CalculateAsync(Guid submitterId, List<SubmissionCalculationRequest> request);

        Task SoftDeleteAndAddObligationCalculationAsync(Guid submitterId, List<ObligationCalculation> calculations);
	}
}