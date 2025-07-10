using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.Interfaces.Accreditations;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories.Accreditations;

public class OverseasAccreditationSiteRepository(EprAccreditationContext eprContext) : IOverseasAccreditationSiteRepository
{
    public async Task<List<OverseasAccreditationSite>?> GetAllByAccreditationId(Guid accreditationId)
    {
        return await eprContext.OverseasAccreditationSites
            .AsNoTracking()
            .Where(x => x.ExternalId == accreditationId)
            .ToListAsync();
    }

    public async Task PostByAccreditationId(Guid accreditationId, OverseasAccreditationSite overseasAccreditationSite)
    {
        int accreditationIdInt = 0;

        if (eprContext.Accreditations.Any())
            accreditationIdInt = await eprContext.Accreditations.Where(x => x.ExternalId == accreditationId).Select(x => x.Id).SingleAsync();

        overseasAccreditationSite.ExternalId = accreditationId;
        overseasAccreditationSite.AccreditationId = accreditationIdInt; 
        eprContext.OverseasAccreditationSites.Add(overseasAccreditationSite);

        await eprContext.SaveChangesAsync();
    }
}
