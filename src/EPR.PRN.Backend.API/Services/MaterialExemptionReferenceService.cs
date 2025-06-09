using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.Models;

namespace EPR.PRN.Backend.API.Services;

public class MaterialExemptionReferenceService(
    ILogger<MaterialExemptionReferenceService> logger, 
    IMaterialExemptionReferenceRepository exemptionReferenceRepository) : IMaterialExemptionReferenceService
{
    private readonly ILogger<MaterialExemptionReferenceService> _logger = logger;
    private readonly IMaterialExemptionReferenceRepository _materialRepository = exemptionReferenceRepository;
   
    public Task<bool> CreateMaterialExemptionReferenceAsync(List<MaterialExemptionReferenceRequest> 
        materialExemptionReferences, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating material exemption references");
        var exemptions = materialExemptionReferences.Select(exemption => new MaterialExemptionReference
        {
            ExternalId = exemption.ExternalId,
            RegistrationMaterialId = exemption.RegistrationMaterialId,
            ReferenceNo = exemption.ReferenceNumber,
        }).ToList();

        return _materialRepository.CreateMaterialExemptionReference(exemptions);
    }
}