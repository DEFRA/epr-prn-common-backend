using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Constants;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace EPR.PRN.Backend.API.Controllers;

[FeatureGate(FeatureFlags.ReprocessorExporter)]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/RegulatorApplicationTaskStatus")]

public class RegulatorApplicationTaskStatusController(
    IMediator mediator,
    IValidator<UpdateRegulatorApplicationTaskCommand> validator,
    ILogger<RegulatorApplicationTaskStatusController> logger)
    : ControllerBase
{
    [HttpPost()]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateRegistrationTaskStatus([FromBody] UpdateRegulatorApplicationTaskCommand command)
    {
        logger.LogInformation(LogMessages.UpdateRegulatorApplicationTask);

        await validator.ValidateAndThrowAsync(command);

        await mediator.Send(command);

        return NoContent();
    }
}