using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

[FeatureGate(FeatureFlags.ReprocessorExporter)]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/regulatorRegistrationTaskStatus")]

public class RegulatorRegistrationTaskStatusController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<UpdateRegulatorRegistrationTaskCommand> _updateRegulatorRegistrationTaskCommandValidator;

    public RegulatorRegistrationTaskStatusController(IMediator mediator, IValidator<UpdateRegulatorRegistrationTaskCommand> command)
    {
        this._mediator = mediator;
        this._updateRegulatorRegistrationTaskCommandValidator = command;
    }

    #region Patch Methods

    [HttpPatch("{registrationTaskStatusId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateRegistrationTaskStatus(int registrationTaskStatusId,[FromBody] UpdateRegulatorRegistrationTaskCommand command)
    {
        command.Id = registrationTaskStatusId;

        var validationResult = _updateRegulatorRegistrationTaskCommandValidator.Validate(command);

        if (!validationResult.IsValid)
        {
            return new BadRequestObjectResult(validationResult.Errors);
        }

        var result = await _mediator.Send(command);
        return result ? Ok("Update RegistrationTaskStatus recorded successfully") : StatusCode(500, "Failed to process Status");
    }

    #endregion Patch Methods
}