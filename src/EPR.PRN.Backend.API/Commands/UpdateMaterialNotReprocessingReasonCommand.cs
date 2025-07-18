using EPR.PRN.Backend.API.Dto;
using MediatR;

namespace EPR.PRN.Backend.API.Commands;

public class UpdateMaterialNotReprocessingReasonCommand : IRequest
{
    public Guid RegistrationMaterialId { get; set; }

    public string MaterialNotReprocessingReason { get; set; } = string.Empty;
}
