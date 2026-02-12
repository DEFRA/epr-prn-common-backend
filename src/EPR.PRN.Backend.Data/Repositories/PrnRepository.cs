using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.Dto;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories;

public class PrnRepository(EprContext context) : IPrnRepository
{
    public IQueryable<EprnResultsDto> GetAcceptedAndAwaitingPrnsByYear(Guid organisationId, int year)
    {
        var currentYear = year.ToString();
        var previousYear = (year - 1).ToString();

        var query = from eprn in context.Prn
            join status in context.PrnStatus on eprn.PrnStatusId equals status.Id
            where eprn.OrganisationId == organisationId &&
                  (
                      (eprn.PrnStatusId == (int)EprnStatus.ACCEPTED && eprn.ObligationYear == currentYear) ||
                      (eprn.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE && (
                          eprn.ObligationYear == currentYear ||
                          (eprn.AccreditationYear == previousYear && eprn.DecemberWaste == true)
                      )
                  ))
            select new EprnResultsDto
            {
                Eprn = eprn,
                Status = status
            };

        return query.AsNoTracking();
    }
}
