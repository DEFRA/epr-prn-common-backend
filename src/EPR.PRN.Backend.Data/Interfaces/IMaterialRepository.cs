using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.Interfaces;

public interface IMaterialRepository
{
    Task<IEnumerable<Material>> GetAllMaterials();

    Task<RegistrationMaterialContact> UpsertRegistrationMaterialContact(Guid registrationMaterialId, Guid userId);
    Task UpdateApplicantRegistrationTaskStatusAsync(string taskName, Guid registrationId, TaskStatuses status);
}
