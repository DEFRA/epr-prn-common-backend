using EPR.PRN.Backend.Data.Dto;

namespace EPR.PRN.Backend.Data.Interfaces
{
    public interface IPrnRepository
    {
        Task<List<PrnObligationSummaryDto>> GetObligationSummary(Guid organisationId, int year);
    }
}
