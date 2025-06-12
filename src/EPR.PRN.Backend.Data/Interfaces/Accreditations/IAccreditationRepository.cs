using EPR.PRN.Backend.Data.DataModels.Accreditations;

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
}
