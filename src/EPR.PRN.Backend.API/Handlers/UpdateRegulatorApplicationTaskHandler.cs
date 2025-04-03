using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;
namespace EPR.PRN.Backend.API.Handlers;

public class UpdateRegulatorApplicationTaskHandler : IRequestHandler<UpdateRegulatorApplicationTaskCommand, Unit>
{
    private readonly IRegulatorApplicationTaskStatusRepository _repository;
    public UpdateRegulatorApplicationTaskHandler(IRegulatorApplicationTaskStatusRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateRegulatorApplicationTaskCommand command, CancellationToken cancellationToken)
    {
        await _repository.UpdateStatusAsync(command.Id, command.Status, command.Comment);

        return Unit.Value;
    }

}