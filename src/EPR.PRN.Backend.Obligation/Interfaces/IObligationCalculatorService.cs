using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Models;

namespace EPR.PRN.Backend.Obligation.Interfaces
{
    public interface IObligationCalculatorService
    {
        Task<List<ObligationCalculationDto>?> GetObligationCalculationByOrganisationId(int id);
        Task ProcessApprovedPomData(Guid id, SubmissionCalculationRequest request);
    }
}