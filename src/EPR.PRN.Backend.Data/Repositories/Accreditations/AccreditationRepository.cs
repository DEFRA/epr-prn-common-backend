using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.Interfaces.Accreditations;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories.Accreditations;

public class AccreditationRepository(EprAccreditationContext eprContext) : IAccreditationRepository
{
    public async Task<Accreditation?> GetById(int accreditationId)
    {
        return await eprContext.Accreditations
            .AsNoTracking()
            .Include(x => x.ApplicationType)
            .Include(x => x.AccreditationStatus)
            .Include(x => x.RegistrationMaterial)
                .ThenInclude(x => x.Material)
            .SingleOrDefaultAsync(x => x.Id == accreditationId);
    }

    public async Task Create(Accreditation accreditation)
    {
        eprContext.Accreditations.Add(accreditation);
        await eprContext.SaveChangesAsync();
    }
    public async Task Update(Accreditation accreditation)
    {
        eprContext.Entry(accreditation).State = EntityState.Modified;
        await eprContext.SaveChangesAsync();
    }
}