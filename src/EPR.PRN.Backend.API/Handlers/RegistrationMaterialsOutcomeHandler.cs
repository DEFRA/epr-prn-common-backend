using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class RegistrationMaterialsOutcomeHandler(IRegistrationMaterialRepository rmRepository)
    : IRequestHandler<RegistrationMaterialsOutcomeCommand>
{
    public async Task Handle(RegistrationMaterialsOutcomeCommand request, CancellationToken cancellationToken)
    {
        var materialData = await rmRepository.GetMaterialsById(request.Id);

        if ( request.Status == materialData.Status ||
             (materialData.Status == RegistrationMaterialStatus.Granted && request.Status != RegistrationMaterialStatus.Refused))
        {
            throw new InvalidOperationException("Invalid outcome transition.");
        }

        string registrationReferenceNumber = GenerateRegistrationReferenceNumber(materialData.RegistrationId, request.Id);

        await rmRepository.UpdateRegistrationOutCome(
            request.Id,
            (int)request.Status,
            request.Comments,
            registrationReferenceNumber
        );
    }

    private string GenerateRegistrationReferenceNumber(int RegistrationId, int RegistrationMaterialid)
    {
        var ReferenceData = rmRepository.GetRegistrationReferenceDataId(RegistrationId, RegistrationMaterialid).Result;
        Random _random = new Random();
        string yearCode = (DateTime.Now.Year % 100).ToString("D2");
        string orgCode = RegistrationId.ToString("D6");
        string randomDigits = _random.Next(1000, 9999).ToString();
        return $"{ReferenceData.RegistrationType}{yearCode}{ReferenceData.CountryCode}{ReferenceData.OrganisationType}{orgCode}{randomDigits}{ReferenceData.MaterialCode}";
    }
}