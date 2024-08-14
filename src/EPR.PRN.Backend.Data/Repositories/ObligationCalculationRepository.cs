using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories
{
    public class ObligationCalculationRepository : IObligationCalculationRepository
    {
        private readonly EprContext _context;

        public ObligationCalculationRepository(EprContext context)
        {
            _context = context;
        }

        public async Task<List<ObligationCalculation>?> GetObligationCalculationById(int id)
        {
            return await _context.ObligationCalculations.Where(x => x.Id == id).ToListAsync();
        }
    }
}
