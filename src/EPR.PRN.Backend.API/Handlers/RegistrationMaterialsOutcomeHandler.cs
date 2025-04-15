using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class RegistrationMaterialsOutcomeHandler(
    IRegistrationMaterialRepository rmRepository
) : IRequestHandler<RegistrationMaterialsOutcomeCommand>
{
    public async Task Handle(RegistrationMaterialsOutcomeCommand request, CancellationToken cancellationToken)
    {
        var materialEntity = await rmRepository.GetRegistrationMaterialById(request.Id);
        
        EnsureStatusTransitionIsValid(request, materialEntity);

        var registrationReferenceNumber = GenerateRegistrationReferenceNumber(materialEntity);

        await rmRepository.UpdateRegistrationOutCome(
            request.Id,
            (int)request.Status,
            request.Comments,
            registrationReferenceNumber
        );
    }

    private static void EnsureStatusTransitionIsValid(RegistrationMaterialsOutcomeCommand request, RegistrationMaterial materialEntity)
    {
        var currentStatus = (RegistrationMaterialStatus?)materialEntity.StatusID;

        if (request.Status == currentStatus ||
            (currentStatus == RegistrationMaterialStatus.Granted &&
             request.Status == RegistrationMaterialStatus.Refused))
        {
            throw new InvalidOperationException("Invalid outcome transition.");
        }
    }

    private static string GenerateRegistrationReferenceNumber(RegistrationMaterial registrationMaterial)
    {
        const string registrationType = "R";

        var orgType = (ApplicationOrganisationType)registrationMaterial.Registration.ApplicationTypeId;
        var address = orgType == ApplicationOrganisationType.Exporter
            ? registrationMaterial.Registration.BusinessAddress
            : registrationMaterial.Registration.ReprocessingSiteAddress;

        var countryCode = address?.Country?.Substring(0, 3).ToUpper() ?? "UNK";
        var materialCode = registrationMaterial.Material.MaterialCode;
        var orgTypeInitial = orgType.ToString().First().ToString();
        var yearCode = (DateTime.UtcNow.Year % 100).ToString("D2");
        var orgCode = registrationMaterial.RegistrationId.ToString("D6");
        var randomDigits = Random.Shared.Next(1000, 9999).ToString();

        return $"{registrationType}{yearCode}{countryCode}{orgTypeInitial}{orgCode}{randomDigits}{materialCode}";
    }
}
