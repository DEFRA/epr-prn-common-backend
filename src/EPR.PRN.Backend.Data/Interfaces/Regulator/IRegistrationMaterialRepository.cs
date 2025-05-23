using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.Interfaces.Regulator;

public interface IRegistrationMaterialRepository
{
    Task<Registration> GetRegistrationById(Guid registrationId);
    Task<Registration> GetRegistrationByExternalIdAndYear(Guid externalId, int year);

    Task<List<LookupRegulatorTask>> GetRequiredTasks(int applicationTypeId, bool isMaterialSpecific, int journeyTypeId);

    Task<RegistrationMaterial> GetRegistrationMaterialById(Guid registrationMaterialId);
    Task<RegistrationMaterial> GetRegistrationMaterial_WasteLicencesById(Guid registrationMaterialId);
    Task<RegistrationMaterial> GetRegistrationMaterial_RegistrationReprocessingIOById(Guid registrationMaterialId);
    Task<RegistrationMaterial> GetRegistrationMaterial_FileUploadById(Guid registrationMaterialId);
    Task UpdateRegistrationOutCome(Guid registrationMaterialId, int statusId, string? comment, string? registrationReferenceNumber);
    Task RegistrationMaterialsMarkAsDulyMade(Guid registrationMaterialId, int statusId, DateTime DeterminationDate,
            DateTime DulyMadeDate,Guid DulyMadeBy);
    
}
