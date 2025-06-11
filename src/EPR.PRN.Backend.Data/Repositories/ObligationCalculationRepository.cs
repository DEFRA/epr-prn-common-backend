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
							.Where(x => x.SubmitterId == submitterId && x.Year == year)
							.ToListAsync();
	}

    public async Task RemoveAndAddObligationCalculationBySubmitterIdAsync(Guid submitterId, List<ObligationCalculation> calculations)
	{
		if (calculations == null || calculations.Count == 0)
		{
			throw new ArgumentException("The calculations list cannot be null or empty.", nameof(calculations));
		}
		var existingCalculations = await context.ObligationCalculations
												.Where(oc =>
                                                    oc.SubmitterId == submitterId &&
                                                    oc.Year == calculations[0].Year
												 )
												.ToListAsync();
		if (existingCalculations.Count > 0)
		{
			context.ObligationCalculations.RemoveRange(existingCalculations);
		}
		await context.ObligationCalculations.AddRangeAsync(calculations);
		await context.SaveChangesAsync();
	}
}