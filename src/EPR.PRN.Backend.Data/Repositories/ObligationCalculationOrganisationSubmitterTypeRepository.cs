using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories;

public class ObligationCalculationOrganisationSubmitterTypeRepository(EprContext context) : IObligationCalculationOrganisationSubmitterTypeRepository
{
	public async Task<int> GetSubmitterTypeIdByTypeName(ObligationCalculationOrganisationSubmitterTypeName typeName)
	{
		var id = await context.ObligationCalculationOrganisationSubmitterType
						.AsNoTracking()
						.Where(t => t.TypeName == typeName.ToString())
						.Select(t => t.Id)
						.FirstAsync();
		return id;
	}
}
