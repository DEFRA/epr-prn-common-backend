using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;

namespace EPR.PRN.Backend.API.Handlers;

public class UpdateRegulatorApplicationTaskHandler(IRegulatorApplicationTaskStatusRepository repository)
    : UpdateRegulatorTaskHandlerBase<UpdateRegulatorApplicationTaskCommand, IRegulatorApplicationTaskStatusRepository,
        RegulatorApplicationTaskStatus>(repository);
