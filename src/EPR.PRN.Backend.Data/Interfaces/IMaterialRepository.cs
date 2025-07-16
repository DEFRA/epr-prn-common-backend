using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;

namespace EPR.PRN.Backend.Data.Interfaces;

public interface IMaterialRepository
{
    Task<IEnumerable<Material>> GetAllMaterials();

    Task<RegistrationMaterialContact> UpsertRegistrationMaterialContact(Guid registrationMaterialId, Guid userId);
    Task UpdateApplicationRegistrationTaskStatusAsync(string taskName, Guid registrationMaterialId, TaskStatuses status);

    Task UpsertRegistrationReprocessingDetailsAsync(Guid registrationMaterialId, RegistrationReprocessingIO registrationReprocessingIO);

    Task<IEnumerable<Material>> GetMaterialsByRegistrationIdQuery(Guid registrationId);
    Task SaveOverseasReprocessingSites(UpdateOverseasAddressDto overseasAddressSubmission);
    Task UpdateMaterialNotReprocessingReason(Guid registrationMaterialId, string materialNotReprocessingReason);
    Task<IList<OverseasMaterialReprocessingSite>> GetOverseasMaterialReprocessingSites(Guid registrationMaterialId);
    Task SaveInterimSitesAsync(SaveInterimSitesRequestDto requestDto);
}
