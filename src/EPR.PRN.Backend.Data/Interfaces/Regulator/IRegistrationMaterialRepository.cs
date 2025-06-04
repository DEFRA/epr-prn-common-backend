using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.Interfaces.Regulator;

public interface IRegistrationMaterialRepository
{
    Task<Registration> GetRegistrationById(int registrationId);
    
    Task<List<LookupRegulatorTask>> GetRequiredTasks(int applicationTypeId, bool isMaterialSpecific);

    Task<RegistrationMaterial> GetRegistrationMaterialById(int registrationMaterialId);
    Task<RegistrationMaterial> GetRegistrationMaterial_WasteLicencesById(int registrationMaterialId);
    Task<RegistrationMaterial> GetRegistrationMaterial_RegistrationReprocessingIOById(int registrationMaterialId);
    Task<RegistrationMaterial> GetRegistrationMaterial_FileUploadById(int registrationMaterialId);
    Task UpdateRegistrationOutCome(int registrationMaterialId, int statusId, string? comment, string? registrationReferenceNumber);
    Task RegistrationMaterialsMarkAsDulyMade(int registrationMaterialId, int statusId, DateTime DeterminationDate,
            DateTime DulyMadeDate,Guid DulyMadeBy);
    
}
