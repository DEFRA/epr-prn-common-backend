using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Validators.Regulator;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace EPR.PRN.Backend.API.Controllers.Regulator;

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
    [HttpPost("{Id}/queryNote")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> AddApplicationTaskQueryNote(Guid Id, [FromBody] AddApplicationTaskQueryNoteCommand command)
    {
        logger.LogInformation(LogMessages.UpdateRegulatorApplicationTask);

        command.RegulatorApplicationTaskStatusId = Id;

        var applicationTaskValidator = new AddApplicationTaskQueryNoteCommandValidator();
        await applicationTaskValidator.ValidateAndThrowAsync(command);

        await mediator.Send(command);

        return NoContent();

    }
}