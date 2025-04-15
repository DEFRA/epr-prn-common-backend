using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;

namespace EPR.PRN.Backend.API.Handlers;

public class UpdateRegulatorRegistrationTaskHandler : UpdateRegulatorTaskHandlerBase<UpdateRegulatorRegistrationTaskCommand, IRegulatorRegistrationTaskStatusRepository, RegulatorRegistrationTaskStatus>
{
    public UpdateRegulatorRegistrationTaskHandler(IRegulatorRegistrationTaskStatusRepository repository)
        : base(repository)
    {
    }
}