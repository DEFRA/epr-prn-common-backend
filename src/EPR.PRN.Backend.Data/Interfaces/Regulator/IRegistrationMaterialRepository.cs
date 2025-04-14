namespace EPR.PRN.Backend.Data.Interfaces.Regulator;

using EPR.PRN.Backend.Data.DataModels.Registrations;

public interface IRegistrationMaterialRepository
{
    Task<Registration> GetRegistrationById(int registrationId);
    Task<List<RegistrationMaterial>> GetMaterialsByRegistrationId(int registrationId);
    Task<List<LookupRegulatorTask>> GetRequiredTasks(int applicationTypeId, bool isMaterialSpecific);
    Task<List<RegulatorRegistrationTaskStatus>> GetRegistrationTasks(int registrationId);
    Task<List<RegulatorApplicationTaskStatus>> GetMaterialTasks(int registrationMaterialId);
    Task<RegistrationMaterial> GetRegistrationMaterialById(int registrationMaterialId);
    Task UpdateRegistrationOutCome(int registrationMaterialId, int statusId, string? comment, string registrationReferenceNumber);
    Task<LookupAddress?> GetAddressById(int addressId);
    Task<LookupMaterial?> GetMaterialById(int materialId);
}
