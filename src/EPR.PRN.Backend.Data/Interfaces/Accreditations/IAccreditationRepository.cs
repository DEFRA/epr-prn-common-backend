using EPR.PRN.Backend.Data.DataModels.Accreditations;

namespace EPR.PRN.Backend.Data.Interfaces.Accreditations;

public interface IAccreditationRepository
{
    Task<Accreditation?> GetById(int accreditationId);
    
    Task Create(Accreditation accreditation);

    Task Update(Accreditation accreditation);
}
