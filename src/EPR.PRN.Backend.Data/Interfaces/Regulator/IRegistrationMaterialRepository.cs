
namespace EPR.PRN.Backend.Data.Interfaces.Regulator;
using EPR.PRN.Backend.API.Common.Dto.Regulator;


public interface IRegistrationMaterialRepository
{
    Task<bool> UpdateRegistrationOutCome(int RegistrationMaterialId, int outcome, string? outComeComment);
    Task<RegistrationMaterialDto> GetMaterialsById(int RegistrationId);
    Task<RegistrationOverviewDto> GetRegistrationOverviewDetailById(int RegistrationId);
}
