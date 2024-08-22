using EPR.PRN.Backend.Obligation.DTO;

namespace EPR.PRN.Backend.Obligation.Interfaces
{
    public interface IObligationCalculatorService
    {
        Task<List<ObligationCalculationDto>?> GetObligationCalculationByOrganisationId(int id);
        Task ProcessApprovedPomData(string submissionIdString);
    }
}