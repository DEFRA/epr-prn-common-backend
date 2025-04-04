using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Helpers;
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

        public async Task<List<SaveAndContinueDto>> GetAllAsync(int registrationId, string controller, string area)
        {
            var responseModel = new List<SaveAndContinueDto>();

            var model = await repository.GetAllAsync(registrationId, controller, area);

            if (model == null)
            {
                logger.LogInformation("SaveAndContinue GetAllAsync was not found with {registrationId}.", registrationId);
                throw new NotFoundException($"SaveAndContinue with registration id {registrationId} and area {area} was not found in system");
            };

            model.ForEach(x=> responseModel.Add(new SaveAndContinueDto { Action = x.Action, Area= x.Area, Parameters = x.Parameters, RegistrationId = x.RegistrationId, Controller = x.Controller, CreatedOn = x.CreatedOn, Id = x.Id }));
            
            return responseModel;
        }

        public async Task<SaveAndContinueDto> GetAsync(int registrationId, string controller, string area)
        {
            var model = await repository.GetAsync(registrationId, controller, area);

            if (model == null)
            {
                logger.LogInformation("SaveAndContinue GetAsync was not found with {registrationId}.", registrationId);
                throw new NotFoundException($"SaveAndContinue with registration id {registrationId} and area {area} was not found in system");
            };

            return new SaveAndContinueDto { Action = model.Action, Area = model.Area, Parameters = model.Parameters, RegistrationId = model.RegistrationId, Controller = model.Controller, CreatedOn = model.CreatedOn, Id = model.Id };
        }
    }
}
