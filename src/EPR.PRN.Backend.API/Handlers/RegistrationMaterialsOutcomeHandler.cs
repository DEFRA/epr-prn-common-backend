using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Dto.Regulator;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;
using System.Security.Cryptography;
using System;
namespace EPR.PRN.Backend.API.Handlers;
public class RegistrationMaterialsOutcomeHandler : IRequestHandler<RegistrationMaterialsOutcomeCommand, HandlerResponse<string>>
{
    private readonly IRegistrationMaterialRepository _rmRepository;

    public RegistrationMaterialsOutcomeHandler(IRegistrationMaterialRepository rmRepository)
    {
        _rmRepository = rmRepository;
    }

    public async Task<HandlerResponse<string>> Handle(RegistrationMaterialsOutcomeCommand request, CancellationToken cancellationToken)
    {
        var materialData = await _rmRepository.GetMaterialsById(request.Id);
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
            if (registrationmaterialEnum == currentStatus || (currentStatus == RegistrationMaterialStatus.GRANTED && registrationmaterialEnum != RegistrationMaterialStatus.REFUSED))
            {
                return new HandlerResponse<string>(400, string.Empty, "Invalid Outcome transition.");
            }
        }
        string RegistrationReferenceNumber = GenerateRegistrationReferenceNumber(materialData);
        string ReferenceNumber = await _rmRepository.UpdateRegistrationOutCome(request.Id, (int)registrationmaterialEnum, request.Comments, RegistrationReferenceNumber);
        if (String.IsNullOrEmpty(ReferenceNumber))
        {
            return new HandlerResponse<string>(500, string.Empty, "Failed to update the outcome.");
        }

        return new HandlerResponse<string>(200, RegistrationReferenceNumber);
    }
    private string GenerateRegistrationReferenceNumber(RegistrationMaterialDto RegistrationMaterial)
    {
        // Generate a unique registration reference number
        Random _random = new Random();
        string firstLetter = "Registration".FirstOrDefault().ToString();
        string yearCode = (DateTime.Now.Year % 100).ToString("D2");
        string secondLetter = "Eng".FirstOrDefault().ToString();
        string thirdLetter = "Exporter".FirstOrDefault().ToString();
        string orgCode = 1.ToString("D6");
        string randomDigits = _random.Next(1000, 9999).ToString();
        string Materialcode = "PL";
        return $"{firstLetter}{yearCode}{secondLetter}{thirdLetter}{orgCode}{randomDigits}{Materialcode}";
    }
}
