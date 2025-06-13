using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories;

public class ObligationCalculationRepository(EprContext context) : IObligationCalculationRepository
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
		var existingCalculations = await context.ObligationCalculations
			.Where(oc => oc.SubmitterId == submitterId && oc.Year == year && !oc.IsDeleted)
			.ToListAsync();

		if (existingCalculations.Count > 0)
		{
			existingCalculations.ForEach(c => c.IsDeleted = true);
		}

		await context.ObligationCalculations.AddRangeAsync(calculations);
		await context.SaveChangesAsync();
	}
}