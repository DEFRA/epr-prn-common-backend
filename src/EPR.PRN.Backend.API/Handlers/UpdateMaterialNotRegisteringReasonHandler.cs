using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;
public class UpdateMaterialNotRegisteringReasonHandler(IMaterialRepository repository)
    : IRequestHandler<UpdateMaterialNotRegisteringReasonCommand>
{
    public async Task Handle(UpdateMaterialNotRegisteringReasonCommand command, CancellationToken cancellationToken)
    {
        await repository.UpdateMaterialNotRegisteringReason(command.RegistrationMaterialId, command.MaterialNotRegisteringReason);
    }
}