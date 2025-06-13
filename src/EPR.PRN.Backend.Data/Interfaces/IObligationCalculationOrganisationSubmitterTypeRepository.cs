using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.Data.Interfaces;

public interface IObligationCalculationOrganisationSubmitterTypeRepository
{
	Task<int> GetSubmitterTypeIdByTypeName(ObligationCalculationOrganisationSubmitterTypeName typeName);
}
