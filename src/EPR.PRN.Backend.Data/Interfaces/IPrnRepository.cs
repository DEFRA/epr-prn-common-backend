using EPR.PRN.Backend.Data.DTO;

namespace EPR.PRN.Backend.Data.Interfaces
{
    public interface IPrnRepository
    {
        IQueryable<EprnResultsDto> GetAcceptedAndAwaitingPrnsByYear(Guid organisationId);
    }
}
