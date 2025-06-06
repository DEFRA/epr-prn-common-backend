using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;
public class RegistrationTaskQueryNoteHandler(IRegulatorRegistrationTaskStatusRepository repository)
    : IRequestHandler<AddRegistrationTaskQueryNoteCommand>
{
    public async Task Handle(AddRegistrationTaskQueryNoteCommand command, CancellationToken cancellationToken)
    {
       
        await repository.AddRegistrationTaskQueryNoteAsync(command.RegulatorRegistrationTaskStatusId, command.CreatedBy, command.Note);
    }
}