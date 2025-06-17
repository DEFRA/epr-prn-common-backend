using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

public class RegistrationMaterialsOutcomeHandler(
    IRegistrationMaterialRepository rmRepository
) : IRequestHandler<RegistrationMaterialsOutcomeCommand>
{
    public async Task Handle(RegistrationMaterialsOutcomeCommand request, CancellationToken cancellationToken)
    {
        var materialEntity = await rmRepository.GetRegistrationMaterialById(request.Id);

        EnsureStatusTransitionIsValid(request, materialEntity);

        await rmRepository.UpdateRegistrationOutCome(
            request.Id,
            (int)request.Status,
            request.Comments,
            request.RegistrationReferenceNumber,
            request.User
        );
    }

    private static void EnsureStatusTransitionIsValid(RegistrationMaterialsOutcomeCommand request, RegistrationMaterial materialEntity)
    {
        var currentStatus = (RegistrationMaterialStatus?)materialEntity.StatusId;

        if (request.Status == currentStatus ||
            currentStatus == RegistrationMaterialStatus.Granted &&
             request.Status == RegistrationMaterialStatus.Refused)
        {
            throw new InvalidOperationException("Invalid outcome transition.");
        }
    }

}
