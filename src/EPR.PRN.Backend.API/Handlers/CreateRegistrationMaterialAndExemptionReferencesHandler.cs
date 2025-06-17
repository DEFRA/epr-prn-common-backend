using AutoMapper;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class CreateRegistrationMaterialAndExemptionReferencesHandler(IRegistrationMaterialRepository repository, 
    IMapper mapper)
    : IRequestHandler<CreateRegistrationMaterialAndExemptionReferencesCommand>
{
    public async Task Handle(CreateRegistrationMaterialAndExemptionReferencesCommand command, CancellationToken cancellationToken)
    {
        var registrationMaterial = mapper.Map<RegistrationMaterial>(command.RegistrationMaterial);
        registrationMaterial.RegistrationId = command.RegistrationId;
        registrationMaterial.ExternalId = Guid.NewGuid();

        var exemptionReferences = command.MaterialExemptionReferences?.Select(x => new MaterialExemptionReference
        {
            ExternalId = Guid.NewGuid(),
            ReferenceNo = x.ReferenceNumber
        }).ToList() ?? new List<MaterialExemptionReference>();

        await repository.CreateRegistrationMaterialWithExemptionsAsync(
            registrationMaterial,
            exemptionReferences
        );
    }        
}
