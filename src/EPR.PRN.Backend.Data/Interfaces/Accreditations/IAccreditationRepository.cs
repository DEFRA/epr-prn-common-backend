using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DataModels.Registrations;   

namespace EPR.PRN.Backend.Data.Interfaces.Accreditations;

public interface IAccreditationRepository
{    
    Task<DataModels.Registrations.Accreditation?> GetById(Guid accreditationId);

    Task<DataModels.Registrations.Accreditation?> GetAccreditationDetails(
        Guid organisationId,
        int materialId,
        int applicationTypeId);

    Task Create(DataModels.Registrations.Accreditation accreditation);

    Task Update(DataModels.Registrations.Accreditation accreditation);

    Task ClearDownDatabase();
}
