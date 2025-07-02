using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Accreditations;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories.Accreditations;

public class AccreditationPrnIssueAuthRepository(EprContext eprContext) : IAccreditationPrnIssueAuthRepository
{
    public async Task<List<AccreditationPrnIssueAuth>?> GetByAccreditationId(Guid accreditationId)
    {
        return await eprContext.AccreditationPrnIssueAuths
            .AsNoTracking()
            .Where(x => x.AccreditationExternalId == accreditationId)
            .ToListAsync();
    }

    public async Task ReplaceAllByAccreditationId(Guid accreditationId, List<AccreditationPrnIssueAuth> accreditationPrnIssueAuths)
    {
        var existingAccreditationPrnIssueAuths = await eprContext.AccreditationPrnIssueAuths
            .Where(x => x.AccreditationExternalId == accreditationId)
            .ToListAsync();
        eprContext.AccreditationPrnIssueAuths.RemoveRange(existingAccreditationPrnIssueAuths);

        int accreditationIdInt = await eprContext.Accreditations.Where(x => x.ExternalId == accreditationId).Select(x => x.Id).SingleAsync();

        foreach (var accreditationPrnIssueAuth in accreditationPrnIssueAuths)
        {
            accreditationPrnIssueAuth.ExternalId = Guid.NewGuid();
            accreditationPrnIssueAuth.AccreditationExternalId = accreditationId;
            accreditationPrnIssueAuth.AccreditationId = accreditationIdInt; 
            eprContext.AccreditationPrnIssueAuths.Add(accreditationPrnIssueAuth);
        }

        await eprContext.SaveChangesAsync();
    }
}