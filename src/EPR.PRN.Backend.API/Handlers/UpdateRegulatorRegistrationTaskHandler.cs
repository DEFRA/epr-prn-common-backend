using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;
namespace EPR.PRN.Backend.API.Handlers;

public class UpdateRegulatorRegistrationTaskHandler : IRequestHandler<UpdateRegulatorRegistrationTaskCommand, Unit>
{
    private readonly IRegulatorRegistrationTaskStatusRepository _repository;
    public UpdateRegulatorRegistrationTaskHandler(IRegulatorRegistrationTaskStatusRepository repository)
    {
        _repository = repository;
    }
        
    public async Task<Unit> Handle(UpdateRegulatorRegistrationTaskCommand command, CancellationToken cancellationToken)
    {
        await _repository.UpdateStatusAsync(command.Id, command.Status, command.Comment);

        return Unit.Value;
    }

}