using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

public class RegistrationMaterialsMarkAsDulyMadeHandler(
    IRegistrationMaterialRepository rmRepository
) : IRequestHandler<RegistrationMaterialsMarkAsDulyMadeCommand>
{
    public async Task Handle(RegistrationMaterialsMarkAsDulyMadeCommand request, CancellationToken cancellationToken)
    {
        var materialEntity = await rmRepository.GetRegistrationMaterialById(request.RegistrationMaterialId);
        if (materialEntity == null)
        {
            throw new KeyNotFoundException("Material not found.");
        }
        await rmRepository.RegistrationMaterialsMarkAsDulyMade(
            request.RegistrationMaterialId,
            (int)RegulatorTaskStatus.Completed,
            request.DeterminationDate,
            request.DulyMadeDate,
             request.DulyMadeBy
        );
    }
}
