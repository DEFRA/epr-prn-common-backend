using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Constants;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace EPR.PRN.Backend.API.Controllers.Regulator;

[FeatureGate(FeatureFlags.ReprocessorExporter)]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/RegulatorAccreditationTaskStatus")]

public class RegulatorAccreditationTaskStatusController(
    IMediator mediator,
    IValidator<UpdateRegulatorAccreditationTaskCommand> validator,
    ILogger<RegulatorAccreditationTaskStatusController> logger)
    : ControllerBase
{
    [HttpPost()]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateAccreditationTaskStatus([FromBody] UpdateRegulatorAccreditationTaskCommand command)
    {
        logger.LogInformation(LogMessages.UpdateRegulatorApplicationTask);

        await validator.ValidateAndThrowAsync(command);

        await mediator.Send(command);

        return NoContent();
    }
}