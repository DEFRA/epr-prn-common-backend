using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.Dto;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories;

public class PrnRepository(EprContext context) : IPrnRepository
{
    public IQueryable<EprnResultsDto> GetAcceptedAndAwaitingPrnsByYear(Guid organisationId, int year)
    {
        var query = from eprn in context.Prn
            join status in context.PrnStatus on eprn.PrnStatusId equals status.Id
            where eprn.OrganisationId == organisationId && (eprn.PrnStatusId == (int)EprnStatus.ACCEPTED ||
                                                            eprn.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE)
            select new EprnResultsDto
            {
                Eprn = eprn,
                Status = status
            };

        if (year == 2026)
        {
            // This scenario is a one-off and unique to requesting data for 2026 where 
            // 2025 December waste PRNs can also be returned
            query = from eprn in query
                where eprn.Eprn.ObligationYear == year.ToString() || (eprn.Eprn.ObligationYear == "2025" && eprn.Eprn.DecemberWaste == true)
                select eprn;
        }
        else
        {
            query = from eprn in query
                where eprn.Eprn.ObligationYear == year.ToString()
                select eprn;
        }

        return query.AsNoTracking();
    }
}
