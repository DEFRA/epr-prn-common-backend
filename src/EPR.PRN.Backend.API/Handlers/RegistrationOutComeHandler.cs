using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using MediatR;
namespace EPR.PRN.Backend.API.Handlers;
public class RegistrationOutComeHandler : IRequestHandler<RegistrationOutcomeCommand, HandlerResponse<bool>>
{
    private readonly IRegistrationMaterialRepository _rmRepository;

    public RegistrationOutComeHandler(IRegistrationMaterialRepository rmRepository)
    {
        _rmRepository = rmRepository;
    }

    public async Task<HandlerResponse<bool>> Handle(RegistrationOutcomeCommand request, CancellationToken cancellationToken)
    {
        var materialData = await _rmRepository.GetMaterialsById(request.Id);
        if (materialData == null)
        {
            return new HandlerResponse<bool>(404, false, "Material not found.");
        }

        if (!Enum.TryParse<OutcomeTypes>(request.Outcome, true, out var outcomeEnum))
        {
            return new HandlerResponse<bool>(400, false, "Invalid outcome type.");
        }

        if (!string.IsNullOrEmpty(materialData.Status) && Enum.TryParse<OutcomeTypes>(materialData.Status, true, out var currentStatus))
        {
            if (outcomeEnum == currentStatus || (currentStatus == OutcomeTypes.GRANTED && outcomeEnum != OutcomeTypes.REFUSED))
            {
                return new HandlerResponse<bool>(400, false, "Invalid Outcome transition.");
            }
        }

        bool isUpdated = await _rmRepository.UpdateRegistrationOutCome(request.Id, (int)outcomeEnum, request.Comments);
        if (!isUpdated)
        {
            return new HandlerResponse<bool>(500, false, "Failed to update the outcome.");
        }

        return new HandlerResponse<bool>(204, true);
    }
}
