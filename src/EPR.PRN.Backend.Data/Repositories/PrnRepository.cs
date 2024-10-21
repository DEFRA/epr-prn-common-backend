using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;

namespace EPR.PRN.Backend.Data.Repositories
{
    public class PrnRepository(EprContext context) : IPrnRepository
    {
        public IQueryable<EprnResultsDto> GetAcceptedAndAwaitingPrnsByYear(Guid organisationId)
        {
            return context.Prn.Join(
                        context.PrnStatus,
                        eprn => eprn.PrnStatusId,
                        status => status.Id,
                        (eprn, status) => new EprnResultsDto { Eprn = eprn, Status = status }
                    )
                    .Where(joined => joined.Eprn.OrganisationId == organisationId && (joined.Status.StatusName == EprnStatus.ACCEPTED.ToString()
                    || joined.Status.StatusName == EprnStatus.AWAITINGACCEPTANCE.ToString()));
        }
    }
}
