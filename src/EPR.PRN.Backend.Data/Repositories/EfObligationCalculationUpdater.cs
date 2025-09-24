using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories
{
    public class EfObligationCalculationUpdater : IObligationCalculationUpdater
    {
        private readonly EprContext _context;

        public EfObligationCalculationUpdater(EprContext context)
        {
            _context = context;
        }

        public async Task<int> SoftDeleteBySubmitterAndYearAsync(Guid submitterId, int year)
        {
            return await _context.ObligationCalculations
                .Where(oc => oc.SubmitterId == submitterId && oc.Year == year && !oc.IsDeleted)
                .ExecuteUpdateAsync(c => c.SetProperty(x => x.IsDeleted, true));
        }
    }
}
