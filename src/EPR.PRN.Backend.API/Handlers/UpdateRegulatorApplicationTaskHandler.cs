using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;

namespace EPR.PRN.Backend.API.Handlers;

public class UpdateRegulatorApplicationTaskHandler : UpdateRegulatorTaskHandlerBase<UpdateRegulatorApplicationTaskCommand, IRegulatorApplicationTaskStatusRepository, RegulatorApplicationTaskStatus>
{
    public UpdateRegulatorApplicationTaskHandler(IRegulatorApplicationTaskStatusRepository repository, ILogger<UpdateRegulatorApplicationTaskHandler> logger)
        : base(repository, logger)
    {
    }
}


