using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;

namespace EPR.PRN.Backend.API.Services
{
    public class SaveAndContinueService(ISaveAndContinueRepository repository, ILogger<SaveAndContinueService> logger) : ISaveAndContinueService
    {
        public async Task AddAsync(int registrationId, string area, string action, string controller, string parameters)
        {
           var model = new SaveAndContinue() { RegistrationId = registrationId, Area = area, Action = action, Controller = controller, Parameters = parameters };
           await repository.AddAsync(model);
        }
    }
}
