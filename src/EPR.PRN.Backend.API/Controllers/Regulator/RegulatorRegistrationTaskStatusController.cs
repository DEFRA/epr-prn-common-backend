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
[Route("api/v{version:apiVersion}/RegulatorRegistrationTaskStatus")]

public class RegulatorRegistrationTaskStatusController(
    IMediator mediator,
    IValidator<UpdateRegulatorRegistrationTaskCommand> validator,
    ILogger<RegulatorRegistrationTaskStatusController> logger)
    : ControllerBase
{
    [HttpPost()]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateRegistrationTaskStatus([FromBody] UpdateRegulatorRegistrationTaskCommand command)
    {
        logger.LogInformation(LogMessages.UpdateRegulatorRegistrationTask);

        await validator.ValidateAndThrowAsync(command);

        await mediator.Send(command);

        return NoContent();
    }
    [HttpPost("{Id}/queryNote")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> RegistrationTaskQueryNote(Guid Id, [FromBody] AddRegistrationTaskQueryNoteCommand command)
    {
        logger.LogInformation(LogMessages.UpdateRegulatorApplicationTask);

        command.RegulatorRegistrationTaskStatusId = Id;

        var RegistrationTaskQueryNoteVvalidator = new AddRegistrationTaskQueryNoteCommandValidator();
        await RegistrationTaskQueryNoteVvalidator.ValidateAndThrowAsync(command);

        await mediator.Send(command);

        return NoContent();
    }
}