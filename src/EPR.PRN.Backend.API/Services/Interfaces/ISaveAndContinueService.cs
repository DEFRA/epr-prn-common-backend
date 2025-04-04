using EPR.PRN.Backend.API.Dto;

namespace EPR.PRN.Backend.API.Services.Interfaces
{
    public interface ISaveAndContinueService
    {
        Task AddAsync(int registrationId, string area, string action, string controller, string parameters);
        Task<SaveAndContinueDto> GetAsync(int registrationId, string controller, string area);
        Task<List<SaveAndContinueDto>> GetAllAsync(int registrationId, string controller, string area);
    }
}
