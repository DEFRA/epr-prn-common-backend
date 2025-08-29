using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories;

public class ObligationCalculationRepository(EprContext context, IObligationCalculationUpdater updater) : IObligationCalculationRepository
{
    public async Task<List<ObligationCalculation>> GetObligationCalculationBySubmitterIdAndYear(Guid submitterId, int year)
    {
        return await context.ObligationCalculations
                            .AsNoTracking()
                            .Where(oc => oc.SubmitterId == submitterId && oc.Year == year && !oc.IsDeleted)
                            .ToListAsync();
    }

    public async Task SoftDeleteAndAddObligationCalculationBySubmitterIdAsync(Guid submitterId, int year, List<ObligationCalculation> calculations)
    {
        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Start the transaction
            await using var transaction = await context.Database.BeginTransactionAsync();

            await updater.SoftDeleteBySubmitterAndYearAsync(submitterId, year);

            await context.ObligationCalculations.AddRangeAsync(calculations);
            await context.SaveChangesAsync();

            // Commit the transaction
            await transaction.CommitAsync();
        });
    }
}
