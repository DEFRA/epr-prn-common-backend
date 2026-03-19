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
		using (logger.BeginScope("{LogPrefix} {Method} {SubmitterId} {Year}", _logPrefix, nameof(UpsertObligationCalculationsForSubmitterYearAsync), submitterId, year))
		{
			await using var transaction = await context.Database.BeginTransactionAsync();

			try
			{
				var existingIds = new List<int>();
				var existingCalculations = await context.ObligationCalculations
					.Where(x => x.SubmitterId == submitterId && x.Year == year && !x.IsDeleted)
					.ToDictionaryAsync(CalculationKey, x => x);
				var calculationsToAdd = new List<ObligationCalculation>();

				var records = existingCalculations.Count;
				logger.LogInformation("existing records {Records}", records);

				foreach (var calculation in calculations)
				{
					var key = CalculationKey(calculation);
					if (existingCalculations.TryGetValue(key, out var existing))
					{
						if (HasChanged(existing, calculation))
						{
							existing.MaterialObligationValue = calculation.MaterialObligationValue;
							existing.CalculatedOn = calculation.CalculatedOn;
							existing.Tonnage = calculation.Tonnage;
							existing.SubmitterTypeId = calculation.SubmitterTypeId;

							logger.LogInformation("changed {Key}", key);
						}

						existingIds.Add(existing.Id);
					}
					else
					{
						calculationsToAdd.Add(calculation);
					}
				}

				await context.ObligationCalculations.AddRangeAsync(calculationsToAdd);

				var idsForDeletion = existingCalculations.Values
					.Where(x => !existingIds.Contains(x.Id))
					.Select(x => x.Id)
					.ToList();

				records = await context.SaveChangesAsync();
				logger.LogInformation("save changes {Records}", records);

				records = await context.ObligationCalculations
					.Where(x => idsForDeletion.Contains(x.Id))
					.ExecuteUpdateAsync(c => c.SetProperty(x => x.IsDeleted, true));
				logger.LogInformation("soft delete {Records}", records);

				await transaction.CommitAsync();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "rolling back - {Message}", ex.Message);
				await transaction.RollbackAsync();
				throw;
			}
		}

		return;

		string CalculationKey(ObligationCalculation x) => $"{x.OrganisationId}_{x.MaterialId}";

		bool HasChanged(ObligationCalculation existing, ObligationCalculation calculation) =>
			existing.MaterialObligationValue != calculation.MaterialObligationValue ||
			existing.CalculatedOn != calculation.CalculatedOn || 
			existing.Tonnage != calculation.Tonnage ||
			existing.SubmitterTypeId != calculation.SubmitterTypeId;
	}
}
