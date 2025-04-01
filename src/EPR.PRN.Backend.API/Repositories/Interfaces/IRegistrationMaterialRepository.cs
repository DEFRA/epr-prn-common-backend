
namespace EPR.PRN.Backend.API.Repositories.Interfaces;

using EPR.PRN.Backend.API.Dto;

public interface IRegistrationMaterialRepository
{
    Task<bool> UpdateRegistrationOutCome(int RegistrationMaterialId, int outcome, string? outComeComment);
    Task<RegistrationMaterialDto> GetMaterialsById(int RegistrationId);
    Task<RegistrationOverviewDto> GetRegistrationOverviewDetailById(int RegistrationId);
}
