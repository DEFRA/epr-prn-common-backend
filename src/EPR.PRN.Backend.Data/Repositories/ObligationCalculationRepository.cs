using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.Data.Repositories;

public class ObligationCalculationRepository(EprContext context, ILogger<ObligationCalculationRepository> logger, IObligationCalculationUpdater updater) : IObligationCalculationRepository
{
    private readonly string logPrefix = "[EPR.PRN.Backend.Data.Repositories]";

    public async Task<List<ObligationCalculation>> GetObligationCalculationBySubmitterIdAndYear(Guid submitterId, int year)
    {
        logger.LogInformation("{Logprefix}: ObligationCalculationRepository - GetObligationCalculationBySubmitterIdAndYear: SubmitterId: {SubmitterId}, Year: {Year}", logPrefix, submitterId, year);
        return await context.ObligationCalculations
                            .AsNoTracking()
                            .Where(oc => oc.SubmitterId == submitterId && oc.Year == year && !oc.IsDeleted)
                            .ToListAsync();
    }

    public async Task UpsertObligationCalculationsForSubmitterYearAsync(Guid submitterId, int year, List<ObligationCalculation> calculations)
    {
        logger.LogInformation("{Logprefix}: ExecuteSoftDeleteAndCalculationsAsync - BeginTransactionAsync", logPrefix);
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            logger.LogInformation("{Logprefix}: ObligationCalculationRepository - SoftDeleteBySubmitterAndYearAsync: SubmitterId: {SubmitterId}, Year: {Year}", logPrefix, submitterId, year);
            await updater.SoftDeleteBySubmitterAndYearAsync(submitterId, year);

            logger.LogInformation("{Logprefix}: ExecuteSoftDeleteAndCalculationsAsync - ObligationCalculations.AddRangeAsync", logPrefix);
            await context.ObligationCalculations.AddRangeAsync(calculations);

            logger.LogInformation("{Logprefix}: ExecuteSoftDeleteAndCalculationsAsync - SaveChangesAsync", logPrefix);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();
            logger.LogInformation("{Logprefix}: ExecuteSoftDeleteAndCalculationsAsync - Transaction Committed", logPrefix);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Logprefix}: ExecuteSoftDeleteAndCalculationsAsync - Exception occurred, rolling back - {Message}", logPrefix, ex.Message);
            await transaction.RollbackAsync();
            throw;
        }
    }
}
