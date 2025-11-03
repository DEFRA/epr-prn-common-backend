using EPR.PRN.Backend.API.Common.Enums;

namespace EPR.PRN.Backend.Data.Interfaces;

public interface IObligationCalculationOrganisationSubmitterTypeRepository
{
    Task<int> GetSubmitterTypeIdByTypeName(ObligationCalculationOrganisationSubmitterTypeName typeName);
}
