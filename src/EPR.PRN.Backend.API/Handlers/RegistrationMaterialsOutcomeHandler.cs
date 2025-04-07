using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;
namespace EPR.PRN.Backend.API.Handlers;
public class RegistrationMaterialsOutcomeHandler(IRegistrationMaterialRepository rmRepository) : IRequestHandler<RegistrationMaterialsOutcomeCommand, HandlerResponse<string>>
{
    public async Task<HandlerResponse<string>> Handle(RegistrationMaterialsOutcomeCommand request, CancellationToken cancellationToken)
    {
        var materialData = await rmRepository.GetMaterialsById(request.Id);
        if (materialData == null)
        {
            return new HandlerResponse<string>(404, string.Empty, "Material not found.");
        }

        if (!Enum.TryParse<RegistrationMaterialStatus>(request.RegistrationMaterialStatus, true, out var registrationmaterialEnum))
        {
            return new HandlerResponse<string>(400, string.Empty, "Invalid outcome type.");
        }

        if (!string.IsNullOrEmpty(materialData.Status.ToString()) && Enum.TryParse<RegistrationMaterialStatus>(materialData.Status.ToString(), true, out var currentStatus))
        {
            if (registrationmaterialEnum == currentStatus || (currentStatus == RegistrationMaterialStatus.Granted && registrationmaterialEnum != RegistrationMaterialStatus.Refused))
            {
                return new HandlerResponse<string>(400, string.Empty, "Invalid Outcome transition.");
            }
        }
        string RegistrationReferenceNumber = GenerateRegistrationReferenceNumber(materialData.RegistrationId, request.Id);
        string ReferenceNumber = await rmRepository.UpdateRegistrationOutCome(request.Id, (int)registrationmaterialEnum, request.Comments, RegistrationReferenceNumber);
        if (String.IsNullOrEmpty(ReferenceNumber))
        {
            return new HandlerResponse<string>(500, string.Empty, "Failed to update the outcome.");
        }

        return new HandlerResponse<string>(200, RegistrationReferenceNumber);
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
