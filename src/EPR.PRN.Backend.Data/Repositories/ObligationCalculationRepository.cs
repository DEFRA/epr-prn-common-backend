using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.Data.Repositories;

public class ObligationCalculationRepository(EprContext context, ILogger<ObligationCalculationRepository> logger) : IObligationCalculationRepository
{
    private readonly string _logPrefix = "[EPR.PRN.Backend.Data.Repositories]";

    public async Task<List<ObligationCalculation>> GetObligationCalculationBySubmitterIdAndYear(Guid submitterId, int year)
    {
        logger.LogInformation("{LogPrefix}: ObligationCalculationRepository - GetObligationCalculationBySubmitterIdAndYear: SubmitterId: {SubmitterId}, Year: {Year}", _logPrefix, submitterId, year);
        return await context.ObligationCalculations
                            .AsNoTracking()
                            .Where(oc => oc.SubmitterId == submitterId && oc.Year == year && !oc.IsDeleted)
                            .ToListAsync();
    }

	public async Task UpsertObligationCalculationsForSubmitterYearAsync(Guid submitterId, int year, List<ObligationCalculation> calculations)
	{
		logger.LogInformation("{LogPrefix}: UpsertObligationCalculationsForSubmitterYearAsync - BeginTransactionAsync", _logPrefix);
		await using var transaction = await context.Database.BeginTransactionAsync();

		try
		{
			var existingIds = new List<int>();
			
			foreach (var calculation in calculations)
			{
				// index: Year, OrganisationId, SubmitterId, MaterialId, IsDeleted
				var existing = await context.ObligationCalculations.FirstOrDefaultAsync(x =>
					x.Year == calculation.Year &&
					x.SubmitterId == calculation.SubmitterId && 
					x.MaterialId == calculation.MaterialId && 
					!x.IsDeleted);

				if (existing is null)
				{
					await context.ObligationCalculations.AddAsync(calculation);
				}
				else
				{
					existing.OrganisationId = calculation.OrganisationId;
					existing.MaterialObligationValue = calculation.MaterialObligationValue;
					existing.CalculatedOn = calculation.CalculatedOn;
					existing.Tonnage = calculation.Tonnage;
					existing.SubmitterTypeId = calculation.SubmitterTypeId;
					
					existingIds.Add(existing.Id);
				}
			}

			var records = await context.SaveChangesAsync();
			logger.LogInformation("{LogPrefix}: UpsertObligationCalculationsForSubmitterYearAsync - SaveChangesAsync {Records}", _logPrefix, records);

			// index: will use the one above
			records = await context.ObligationCalculations
				.Where(oc =>
					oc.SubmitterId == submitterId &&
					oc.Year == year &&
					!oc.IsDeleted &&
					!existingIds.Contains(oc.Id))
				.ExecuteUpdateAsync(c => c.SetProperty(x => x.IsDeleted, true));
			logger.LogInformation("{LogPrefix}: UpsertObligationCalculationsForSubmitterYearAsync - ExecuteUpdateAsync {Records}", _logPrefix, records);

			await transaction.CommitAsync();
			logger.LogInformation("{LogPrefix}: UpsertObligationCalculationsForSubmitterYearAsync - Transaction Committed", _logPrefix);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "{LogPrefix}: UpsertObligationCalculationsForSubmitterYearAsync - Exception occurred, rolling back - {Message}", _logPrefix, ex.Message);
			await transaction.RollbackAsync();
			throw;
		}
	}
}
