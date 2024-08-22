using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories
{
    public class RecyclingTargetRepository : IRecyclingTargetRepository
    {
        private readonly EprContext _context;

        public RecyclingTargetRepository(EprContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RecyclingTarget>> GetAllAsync()
        {
            return await _context.RecyclingTargets.ToListAsync();
        }
    }
}
