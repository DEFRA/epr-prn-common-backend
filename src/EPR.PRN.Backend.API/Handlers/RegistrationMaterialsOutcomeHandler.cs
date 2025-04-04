using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;
public class RegistrationMaterialsOutcomeHandler : IRequestHandler<RegistrationMaterialsOutcomeCommand, HandlerResponse<bool>>
{
    private readonly IRegistrationMaterialRepository _rmRepository;

    public RegistrationMaterialsOutcomeHandler(IRegistrationMaterialRepository rmRepository)
    {
        _rmRepository = rmRepository;
    }

    public async Task<HandlerResponse<bool>> Handle(RegistrationMaterialsOutcomeCommand request, CancellationToken cancellationToken)
    {
        var materialData = await _rmRepository.GetMaterialsById(request.Id);
        if (materialData == null)
        {
            return new HandlerResponse<bool>(404, false, "Material not found.");
        }

        if (!Enum.TryParse<OutcomeStatus>(request.Outcome, true, out var outcomeEnum))
        {
            return new HandlerResponse<bool>(400, false, "Invalid outcome type.");
        }

        if (!string.IsNullOrEmpty(materialData.Status) && Enum.TryParse<OutcomeStatus>(materialData.Status, true, out var currentStatus))
        {
            if (outcomeEnum == currentStatus || (currentStatus == OutcomeStatus.GRANTED && outcomeEnum != OutcomeStatus.REFUSED))
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
