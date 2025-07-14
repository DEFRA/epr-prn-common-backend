using EPR.PRN.Backend.API.Dto;
using MediatR;

namespace EPR.PRN.Backend.API.Commands;

public class UpdateMaterialNotRegisteringReasonCommand : IRequest
{
    public Guid RegistrationMaterialId { get; set; }

    public string MaterialNotRegisteringReason { get; set; } = string.Empty;
}
