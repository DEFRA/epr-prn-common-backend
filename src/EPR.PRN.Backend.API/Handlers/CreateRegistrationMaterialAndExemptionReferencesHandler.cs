using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class CreateRegistrationMaterialAndExemptionReferencesHandler(IRegistrationMaterialRepository repository) 
    : IRequestHandler<CreateRegistrationMaterialAndExemptionReferencesCommand>
{
    public async Task Handle(CreateRegistrationMaterialAndExemptionReferencesCommand command, CancellationToken cancellationToken)
    {
        var registrationMaterial = new RegistrationMaterial
        {
            ExternalId = Guid.NewGuid(),
            RegistrationId = command.RegistrationId,
            MaterialId = command.RegistrationMaterial.MaterialId,
            StatusId = command.RegistrationMaterial.StatusId,            
            StatusUpdatedDate = command.RegistrationMaterial.StatusUpdatedDate ?? DateTime.UtcNow,            
            PermitTypeId = command.RegistrationMaterial.PermitTypeId,
            PPCReprocessingCapacityTonne = command.RegistrationMaterial.PPCReprocessingCapacityTonne, 
            WasteManagementReprocessingCapacityTonne = command.RegistrationMaterial.WasteManagementReprocessingCapacityTonne, 
            InstallationReprocessingTonne = command.RegistrationMaterial.InstallationReprocessingTonne,
            EnvironmentalPermitWasteManagementTonne = command.RegistrationMaterial.EnvironmentalPermitWasteManagementTonne,
            MaximumReprocessingCapacityTonne = command.RegistrationMaterial.MaximumReprocessingCapacityTonne,
            IsMaterialRegistered = command.RegistrationMaterial.IsMaterialRegistered, 
            CreatedDate = command.RegistrationMaterial.CreatedDate
        };

        var exemptionReferences = command.MaterialExemptionReferences?.Select(x => new MaterialExemptionReference
        {
            ExternalId = registrationMaterial.ExternalId,
            RegistrationMaterialId = registrationMaterial.MaterialId,
            ReferenceNo = x.ReferenceNumber
        }).ToList() ?? new List<MaterialExemptionReference>();

        await repository.CreateRegistrationMaterialWithExemptionsAsync(
            registrationMaterial,
            exemptionReferences
        );
    }
}
