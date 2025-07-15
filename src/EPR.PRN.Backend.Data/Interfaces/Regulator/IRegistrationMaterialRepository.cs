using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;

namespace EPR.PRN.Backend.Data.Interfaces.Regulator;

public interface IRegistrationMaterialRepository
{
    Task<Registration> GetRegistrationById(Guid registrationId);
    Task<Registration> GetRegistrationByExternalIdAndYear(Guid externalId, int? year);
    Task<RegistrationMaterial> GetRegistrationMaterial_FileUploadById(Guid registrationMaterialId);
    Task<List<LookupRegulatorTask>> GetRequiredTasks(int applicationTypeId, bool isMaterialSpecific, int journeyTypeId);

    Task<RegistrationMaterial> GetRegistrationMaterialById(Guid registrationMaterialId);
    Task<Accreditation> GetAccreditation_FileUploadById(Guid accreditationId);
    Task UpdateRegistrationOutCome(Guid registrationMaterialId, int statusId, string? comment, string? registrationReferenceNumber , Guid User);
    Task RegistrationMaterialsMarkAsDulyMade(Guid registrationMaterialId, int statusId, DateTime DeterminationDate,
            DateTime DulyMadeDate,Guid DulyMadeBy);
    Task<RegistrationMaterial> CreateAsync(Guid registrationId, string material);

    Task CreateExemptionReferencesAsync(Guid registrationMaterialId, List<MaterialExemptionReference> exemptionReferences);
    Task<IList<RegistrationMaterial>> GetRegistrationMaterialsByRegistrationId(Guid requestRegistrationId);
    Task UpdateRegistrationMaterialPermits(Guid registrationMaterialId, int permitTypeId, string? permitNumber);
    Task UpdateRegistrationMaterialPermitCapacity(Guid registrationMaterialId, int permitTypeId, decimal? capacityInTonnes, int? periodId);

    Task<IEnumerable<LookupMaterialPermit>> GetMaterialPermitTypes();
    Task DeleteAsync(Guid registrationMaterialId);
	Task UpdateIsMaterialRegisteredAsync(List<UpdateIsMaterialRegisteredDto> updateIsMaterialRegisteredDto);
    Task<IList<OverseasMaterialReprocessingSite>> GetOverseasMaterialReprocessingSites(Guid registrationMaterialId);
}
