using EPR.PRN.Backend.Data.DTO.Accreditiation;

namespace EPR.PRN.Backend.Data.Interfaces.Accreditation;

public interface IAccreditationRepository
{    
    Task<DataModels.Registrations.Accreditation?> GetById(Guid accreditationId);

    Task<DataModels.Registrations.Accreditation?> GetAccreditationDetails(
        Guid organisationId,
        int materialId,
        int applicationTypeId);

    Task Create(DataModels.Registrations.Accreditation accreditation);

    Task Update(DataModels.Registrations.Accreditation accreditation);

    Task<IEnumerable<AccreditationOverviewDto>> GetAccreditationOverviewForOrgId(Guid organisationId);
}
