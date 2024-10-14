using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories
{
    public class PrnRepository(EprContext context) : IPrnRepository
    {
        public async Task<List<EprnResultsDto>> GetAcceptedAndAwaitingPrnsByYearAsync(Guid organisationId, int year)
        {
            return await context.Prn.Join(
                    context.PrnStatus,
                    eprn => eprn.PrnStatusId,
                    status => status.Id,
                    (eprn, status) => new EprnResultsDto { Eprn = eprn, Status = status }
                    )
                    .Where(joined => joined.Eprn.OrganisationId == organisationId && joined.Status.StatusName == EprnStatus.ACCEPTED.ToString()
                    && joined.Status.StatusName == EprnStatus.AWAITINGACCEPTANCE.ToString() && joined.Eprn.ObligationYear == year.ToString())
                    .ToListAsync();
        }

        public int GetPrnStatusCount(List<EprnResultsDto> prns, string status)
        {
            return prns.Where(joined => joined.Status.StatusName == status).Count();
        }

        public List<EprnTonnageResultsDto> GetSumOfTonnageForMaterials(List<EprnResultsDto> prns, string status)
        {
            return prns
                 .Where(joined => joined.Status.StatusName == status)
                .GroupBy(joined => new { joined.Eprn.MaterialName, joined.Status.StatusName })
                .Select(g => new EprnTonnageResultsDto
                {
                    MaterialName = g.Key.MaterialName,
                    StatusName = g.Key.StatusName,
                    TotalTonnage = g.Sum(x => x.Eprn.TonnageValue)
                })
                .ToList();
        }
    }
}
