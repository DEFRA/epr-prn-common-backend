using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories
{
    public class RegistrationRepository(EprContext context): IRegistrationRepository
    {
        public async Task<Registration> GetByIdAsync(int id)
        {
            return await context.Registration
                .Include(x=>x.ApplicationType)
                .Include(x=>x.RegistrationStatus)
                .Include(x=>x.BusinessAddress)
                .Include(x=>x.LegalDocumentAddress)
                .Include(x=>x.ReprocessingSiteAddress)
                .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
