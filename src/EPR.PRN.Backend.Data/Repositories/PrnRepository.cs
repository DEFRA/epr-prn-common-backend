using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories
{
    public class PrnRepository(EprContext context) : IPrnRepository
    {
        public async Task<List<EprnResultsDto>> GetSumOfTonnageForMaterials(Guid organisationId, string status)
        {
            return await context.Prn.Join(
                context.PrnStatus,
                eprn => eprn.PrnStatusId,
                status => status.Id,
                (eprn, status) => new { eprn, status }
                ).Where(joined => joined.eprn.OrganisationId == organisationId && joined.status.StatusName == status)
                .GroupBy(joined => new { joined.eprn.MaterialName, joined.status.StatusName })
                .Select(g => new EprnResultsDto
                {
                    MaterialName = g.Key.MaterialName,
                    StatusName = g.Key.StatusName,
                    TotalTonnage = g.Sum(x => x.eprn.TonnageValue)
                })
                .ToListAsync();
        }
    }
}
