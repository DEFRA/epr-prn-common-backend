using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

[ExcludeFromCodeCoverage(Justification = "TODO: To be done as part of create registration user story")]
public class CreateRegistrationMaterialHandler(IRegistrationMaterialRepository repository, IMapper mapper)
    : IRequestHandler<CreateRegistrationMaterialCommand, CreateRegistrationMaterialDto>
{
    public async Task<CreateRegistrationMaterialDto> Handle(CreateRegistrationMaterialCommand command, CancellationToken cancellationToken)
    {
        var material = await repository.CreateAsync(command.RegistrationId, command.Material);
        return mapper.Map<CreateRegistrationMaterialDto>(material);
    }
}