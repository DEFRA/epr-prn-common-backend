using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;
public class UpdateMaterialNotReprocessingReasonHandler(IMaterialRepository repository)
    : IRequestHandler<UpdateMaterialNotReprocessingReasonCommand>
{
    public async Task Handle(UpdateMaterialNotReprocessingReasonCommand command, CancellationToken cancellationToken)
    {
        await repository.UpdateMaterialNotReprocessingReason(command.RegistrationMaterialId, command.MaterialNotReprocessingReason);
    }
}