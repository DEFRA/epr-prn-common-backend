using AutoMapper;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;
public class UpsertRegistrationMaterialContactHandler(IRegistrationMaterialRepository repository, IMapper mapper)
    : IRequestHandler<UpsertRegistrationMaterialContactCommand, RegistrationMaterialContactDto>
{
    public async Task<RegistrationMaterialContactDto> Handle(UpsertRegistrationMaterialContactCommand command, CancellationToken cancellationToken)
    {
        var registrationMaterialContact = await repository.UpsertRegistrationMaterialContact(command.RegistrationMaterialId, command.UserId);
        return mapper.Map<RegistrationMaterialContactDto>(registrationMaterialContact);
    }
}