using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;

namespace EPR.PRN.Backend.Data.Interfaces;

public interface IMaterialRepository
{
    Task<IEnumerable<Material>> GetAllMaterials();

    Task<RegistrationMaterialContact> UpsertRegistrationMaterialContact(Guid registrationMaterialId, Guid userId);

    Task<RegistrationReprocessingIOResponseDto> UpsertRegistrationReprocessingDetailsAsync(Guid registrationMaterialId, RegistrationReprocessingIO registrationReprocessingIO);
}
