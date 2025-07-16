using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class UpdateRegistrationTaskStatusHandler(IRegistrationRepository repository)
    : IRequestHandler<UpdateRegistrationTaskStatusCommand>
{
    public async Task Handle(UpdateRegistrationTaskStatusCommand command, CancellationToken cancellationToken)
    {
        await repository.UpdateRegistrationTaskStatusAsync(command.TaskName, command.RegistrationId, command.Status);
    }
}