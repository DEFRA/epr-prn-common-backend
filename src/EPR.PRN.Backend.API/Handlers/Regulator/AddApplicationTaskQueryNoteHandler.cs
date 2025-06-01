using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

public class AddApplicationTaskQueryNoteHandler(IRegulatorApplicationTaskStatusRepository repository)
    : IRequestHandler<AddApplicationTaskQueryNoteCommand>
{
    public async Task Handle(AddApplicationTaskQueryNoteCommand command, CancellationToken cancellationToken)
    {
        await
       repository.AddApplicationTaskQueryNoteAsync(command.RegulatorApplicationTaskStatusId,
       command.QueryBy, command.Note);
    }
}