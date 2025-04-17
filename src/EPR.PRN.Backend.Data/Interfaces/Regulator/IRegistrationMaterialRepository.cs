using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.Interfaces.Regulator;

public interface IRegistrationMaterialRepository
{
    Task<Registration> GetRegistrationById(int registrationId);
    
    Task<List<LookupRegulatorTask>> GetRequiredTasks(int applicationTypeId, bool isMaterialSpecific);

    Task<RegistrationMaterial> GetRegistrationMaterialById(int registrationMaterialId);

    Task UpdateRegistrationOutCome(int registrationMaterialId, int statusId, string? comment, string? registrationReferenceNumber);
}
