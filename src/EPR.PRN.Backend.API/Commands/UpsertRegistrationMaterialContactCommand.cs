using EPR.PRN.Backend.API.Dto;
using MediatR;

namespace EPR.PRN.Backend.API.Commands;

public class UpsertRegistrationMaterialContactCommand : IRequest<RegistrationMaterialContactDto>
{
    public Guid RegistrationMaterialId { get; set; }

    public Guid UserId { get; set; }
}
