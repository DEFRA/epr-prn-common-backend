using EPR.PRN.Backend.Data.Interfaces;
using MediatR;
namespace EPR.PRN.Backend.API.Handlers;

public class UpdateRegulatorApplicationTaskHandler : IRequestHandler<UpdateRegulatorApplicationTaskCommand, bool>
{
    private readonly IRegulatorApplicationTaskStatusRepository _regulatorApplicationTaskStatusRepository;
    public UpdateRegulatorApplicationTaskHandler(IRegulatorApplicationTaskStatusRepository regulatorApplicationTaskStatusRepository)
    {
        _regulatorApplicationTaskStatusRepository = regulatorApplicationTaskStatusRepository;
    }

    public async Task<bool> Handle(UpdateRegulatorApplicationTaskCommand command, CancellationToken cancellationToken)
    {
        await _regulatorApplicationTaskStatusRepository.UpdateStatusAsync(command.Id, command.Status, command.Comment);

        return true;
    }

}