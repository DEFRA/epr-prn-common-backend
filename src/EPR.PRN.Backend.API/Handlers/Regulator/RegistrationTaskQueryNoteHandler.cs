using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

[ExcludeFromCodeCoverage]
public class RegistrationTaskQueryNoteHandler(IRegulatorRegistrationTaskStatusRepository repository)
    : IRequestHandler<AddRegistrationTaskQueryNoteCommand>
{
    public async Task Handle(AddRegistrationTaskQueryNoteCommand command, CancellationToken cancellationToken)
    {
       
        await repository.AddRegistrationTaskQueryNoteAsync(command.RegulatorRegistrationTaskStatusId, command.CreatedBy, command.Note);
    }
}