using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;

namespace EPR.PRN.Backend.Data.Repositories
{
    public class SaveAndContinueRepository(EprContext context) : ISaveAndContinueRepository
    {
        public async Task AddAsync(SaveAndContinue model)
        {
            await context.SaveAndContinue.AddAsync(model);
            await context.SaveChangesAsync();
        }
    }
}
