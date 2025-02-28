using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories;

public class ObligationCalculationRepository(EprContext context) : IObligationCalculationRepository
{
    public async Task<List<ObligationCalculation>> GetObligationCalculation(IEnumerable<Guid> organisationIds, int year)
    {
        return await context.ObligationCalculations
            .AsNoTracking()
            .Where(x => organisationIds.Contains(x.OrganisationId) && x.Year == year)
            .ToListAsync();
    }

    public async Task AddObligationCalculation(List<ObligationCalculation> calculation)
    {
        await context.ObligationCalculations.AddRangeAsync(calculation);
        await context.SaveChangesAsync();
    }

    public async Task UpsertObligationCalculationAsync(Guid organisationId, List<ObligationCalculation> calculations)
    {
        if (calculations == null || calculations.Count == 0)
        {
            throw new ArgumentException("The calculations list cannot be null or empty.", nameof(calculations));
        }

        List<Guid> organisationIds = [];
        organisationIds.Add(organisationId);

        var obligationCalculations = await GetObligationCalculation(organisationIds, calculations[0].Year);

        var newCalculations = new List<ObligationCalculation>();

        if (obligationCalculations.Count == 0)
        {
            context.ObligationCalculations.AddRange(calculations);
        }
        else
        {
            foreach (var calculation in calculations)
            {
                var existingCalculation = obligationCalculations.FirstOrDefault(c => c.MaterialName == calculation.MaterialName);

                if (existingCalculation != null)
                {
                    context.ObligationCalculations.Attach(existingCalculation);
                    existingCalculation.OrganisationId = organisationIds[0];
                    existingCalculation.MaterialName = calculation.MaterialName;
                    existingCalculation.MaterialObligationValue = calculation.MaterialObligationValue;
                    existingCalculation.Tonnage = calculation.Tonnage;
                    existingCalculation.CalculatedOn = DateTime.Now;
                    existingCalculation.Year = calculation.Year;
                }
                else
                {
                    newCalculations.Add(calculation);
                }
            }
        }

        if (newCalculations.Count != 0)
        {
            context.ObligationCalculations.AddRange(newCalculations);
        }

        await context.SaveChangesAsync();
    }
}