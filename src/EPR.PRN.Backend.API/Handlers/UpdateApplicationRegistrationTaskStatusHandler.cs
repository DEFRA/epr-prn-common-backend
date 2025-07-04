using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;
public class UpdateApplicationRegistrationTaskStatusHandler(IMaterialRepository repository)
    : IRequestHandler<UpdateApplicationRegistrationTaskStatusCommand>
{
    public async Task Handle(UpdateApplicationRegistrationTaskStatusCommand command, CancellationToken cancellationToken)
    {
        await repository.UpdateApplicationRegistrationTaskStatusAsync(command.TaskName, command.RegistrationId, command.Status);
    }
}