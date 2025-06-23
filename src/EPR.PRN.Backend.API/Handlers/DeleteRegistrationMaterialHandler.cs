using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class DeleteRegistrationMaterialHandler(IRegistrationMaterialRepository repository)
    : IRequestHandler<DeleteRegistrationMaterialCommand>
{
    public async Task Handle(DeleteRegistrationMaterialCommand command, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(command.RegistrationMaterialId);
    }
}