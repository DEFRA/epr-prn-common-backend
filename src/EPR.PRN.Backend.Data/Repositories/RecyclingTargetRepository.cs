using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories;

public class RecyclingTargetRepository(EprContext context) : IRecyclingTargetRepository
{
    public async Task<IEnumerable<RecyclingTarget>> GetAllAsync()
    {
        return await context.RecyclingTargets
                            .AsNoTracking()
                            .ToListAsync();
    }
}
