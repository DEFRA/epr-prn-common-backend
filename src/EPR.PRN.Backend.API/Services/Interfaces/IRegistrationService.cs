using EPR.PRN.Backend.API.Dto;

namespace EPR.PRN.Backend.API.Services.Interfaces
{
    public interface IRegistrationService
    {
        Task<RegistrationDto?> GetByIdAsync(int id);
    }
}
