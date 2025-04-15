using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
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
        var registration = await rmRepository.GetRegistrationById(materialEntity.RegistrationId);

        var currentStatus = (RegistrationMaterialStatus?)materialEntity.StatusID;

        if (request.Status == currentStatus ||
            (currentStatus == RegistrationMaterialStatus.Granted && request.Status != RegistrationMaterialStatus.Refused))
        {
            throw new InvalidOperationException("Invalid outcome transition.");
        }

        var orgType = (ApplicationOrganisationType)registration.ApplicationTypeId;
        var addressId = orgType == ApplicationOrganisationType.Exporter
            ? registration.BusinessAddressId
            : registration.ReprocessingSiteAddressId;

        var address = await rmRepository.GetAddressById(addressId);
        var countryCode = address?.Country?.Substring(0, 3).ToUpper() ?? "UNK";

        var material = await rmRepository.GetMaterialById(materialEntity.MaterialId);
        var materialCode = material?.MaterialCode ?? "UNKNOWN";

        var referenceData = new RegistrationReferenceBackendDto
        {
            OrganisationType = orgType.ToString().First().ToString(),
            CountryCode = countryCode,
            MaterialCode = materialCode
        };

        var registrationReferenceNumber = GenerateRegistrationReferenceNumber(referenceData, materialEntity.RegistrationId);

        await rmRepository.UpdateRegistrationOutCome(
            request.Id,
            (int)request.Status,
            request.Comments,
            registrationReferenceNumber
        );
    }

    private static string GenerateRegistrationReferenceNumber(RegistrationReferenceBackendDto referenceData, int registrationId)
    {
        var yearCode = (DateTime.UtcNow.Year % 100).ToString("D2");
        var orgCode = registrationId.ToString("D6");
        var randomDigits = Random.Shared.Next(1000, 9999).ToString();

        return $"{referenceData.RegistrationType}{yearCode}{referenceData.CountryCode}{referenceData.OrganisationType}{orgCode}{randomDigits}{referenceData.MaterialCode}";
    }
}
