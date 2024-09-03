using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories;

public class ObligationCalculationRepository(EprContext context) : IObligationCalculationRepository
{
    public async Task<List<ObligationCalculation>?> GetObligationCalculationByOrganisationId(int organisationId)
    {
        return await context.ObligationCalculations.Where(x => x.OrganisationId == organisationId).ToListAsync();
    }

    public async Task AddObligationCalculation(List<ObligationCalculation> calculation)
    {
        await context.ObligationCalculations.AddRangeAsync(calculation);
        await context.SaveChangesAsync();
    }
}
