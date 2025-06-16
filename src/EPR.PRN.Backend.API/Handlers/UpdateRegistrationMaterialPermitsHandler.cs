using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class UpdateRegistrationMaterialPermitsHandler(IRegistrationMaterialRepository repository)
    : IRequestHandler<UpdateRegistrationMaterialPermitsCommand>
{
    public async Task Handle(UpdateRegistrationMaterialPermitsCommand command, CancellationToken cancellationToken)
    {
        var registrationMaterial = await repository.GetRegistrationMaterialByExternalId(command.ExternalId);
        if (registrationMaterial is null)
        {
            throw new KeyNotFoundException($"Registration Material not found for external id: {command.ExternalId}");
        }

        // Permit Type
        SetPermitType(command, registrationMaterial);

        // Save
        await repository.SaveAsync(registrationMaterial);
    }

    private static void SetPermitType(UpdateRegistrationMaterialPermitsCommand command, RegistrationMaterial registrationMaterial)
    {
        if (!command.PermitTypeId.HasValue || command.PermitTypeId.GetValueOrDefault() == 0)
        {
            return;
        }

        // Permit Type Id
        registrationMaterial.PermitTypeId = command.PermitTypeId.Value;

        // Permit Number
        switch ((MaterialPermitType)registrationMaterial.PermitTypeId)
        {
            case MaterialPermitType.WasteExemption:
                break;
            case MaterialPermitType.PollutionPreventionAndControlPermit:
                registrationMaterial.PPCPermitNumber = command.PermitNumber;
                break;
            case MaterialPermitType.WasteManagementLicence:
                registrationMaterial.WasteManagementLicenceNumber = command.PermitNumber;
                break;
            case MaterialPermitType.InstallationPermit:
                registrationMaterial.InstallationPermitNumber = command.PermitNumber;
                break;
            case MaterialPermitType.EnvironmentalPermitOrWasteManagementLicence:
                registrationMaterial.EnvironmentalPermitWasteManagementNumber = command.PermitNumber;
                break;
        }
    }
}