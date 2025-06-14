using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.Interfaces.Regulator;

public interface IRegistrationMaterialRepository
{
    Task<Registration> GetRegistrationById(Guid registrationId);
    Task<Registration> GetRegistrationByExternalIdAndYear(Guid externalId, int? year);
    Task<RegistrationMaterial> GetRegistrationMaterial_FileUploadById(Guid registrationMaterialId);
    Task<List<LookupRegulatorTask>> GetRequiredTasks(int applicationTypeId, bool isMaterialSpecific, int journeyTypeId);

    Task<RegistrationMaterial> GetRegistrationMaterialById(Guid registrationMaterialId);
    Task<Accreditation> GetAccreditation_FileUploadById(Guid accreditationId);
    Task UpdateRegistrationOutCome(Guid registrationMaterialId, int statusId, string? comment, string? registrationReferenceNumber);
    Task RegistrationMaterialsMarkAsDulyMade(Guid registrationMaterialId, int statusId, DateTime DeterminationDate,
            DateTime DulyMadeDate,Guid DulyMadeBy);
    Task<int> CreateAsync(int registrationId, string material);

    Task CreateExemptionReferencesAsync(Guid registrationMaterialId, List<MaterialExemptionReference> exemptionReferences);
}
