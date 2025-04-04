using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories
{
    public class SaveAndContinueRepository(EprContext context) : ISaveAndContinueRepository
    {
        public async Task AddAsync(SaveAndContinue model)
        {
            await context.SaveAndContinue.AddAsync(model);
            await context.SaveChangesAsync();
        }

        public async Task<SaveAndContinue> GetAsync(int registrationId, string controller, string area)
        {
            return await context.SaveAndContinue
                .OrderByDescending(x=>x.CreatedOn)
                .Where(x=>x.RegistrationId == registrationId)
                .Where(x => x.Controller == controller)
                .Where(x => x.Area.ToLower() == area.ToLower())
                .FirstOrDefaultAsync();
        }

        public async Task<List<SaveAndContinue>> GetAllAsync(int registrationId, string controller, string area)
        {
            return await context.SaveAndContinue
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => x.RegistrationId == registrationId)
                .Where(x=>x.Controller == controller)
                .Where(x => x.Area.ToLower() == area.ToLower())
                .ToListAsync();
        }
    }
}
