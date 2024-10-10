using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories;

public class ObligationCalculationRepository(EprContext context) : IObligationCalculationRepository
{
    public async Task<List<ObligationCalculation>> GetObligationCalculation(Guid organisationId, int year)
    {
        return await context.ObligationCalculations
            .AsNoTracking()
            .Where(x => x.OrganisationId == organisationId && x.Year == year)
            .ToListAsync();
    }

    public async Task AddObligationCalculation(List<ObligationCalculation> calculation)
    {
        await context.ObligationCalculations.AddRangeAsync(calculation);
        await context.SaveChangesAsync();
    }
}
