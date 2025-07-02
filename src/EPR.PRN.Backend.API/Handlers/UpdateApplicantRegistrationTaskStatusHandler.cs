using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;
public class UpdateApplicantRegistrationTaskStatusHandler(IMaterialRepository repository)
    : IRequestHandler<UpdateApplicantRegistrationTaskStatusCommand>
{
    public async Task Handle(UpdateApplicantRegistrationTaskStatusCommand command, CancellationToken cancellationToken)
    {
        await repository.UpdateApplicantRegistrationTaskStatusAsync(command.TaskName, command.RegistrationId, command.Status);
    }
}