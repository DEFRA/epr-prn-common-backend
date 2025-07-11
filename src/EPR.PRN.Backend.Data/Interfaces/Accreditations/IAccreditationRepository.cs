using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.DTO.Accreditiation;

namespace EPR.PRN.Backend.Data.Interfaces.Accreditations;

public interface IAccreditationRepository
{    
    Task<AccreditationEntity?> GetById(Guid accreditationId);

    Task<AccreditationEntity?> GetAccreditationDetails(
        Guid organisationId,
        int materialId,
        int applicationTypeId);

    Task Create(AccreditationEntity accreditation);

    Task Update(AccreditationEntity accreditation);

    Task ClearDownDatabase();
    Task<IEnumerable<AccreditationOverviewDto>> GetAccreditationOverviewForOrgId(Guid organisationId);
}
