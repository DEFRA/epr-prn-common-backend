using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

[FeatureGate(FeatureFlags.ReprocessorExporter)]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/RegulatorApplicationTaskStatus")]

public class RegulatorApplicationTaskStatusController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<UpdateRegulatorApplicationTaskCommand> _updateRegulatorApplicationTaskCommandValidator;

    public RegulatorApplicationTaskStatusController(IMediator mediator, IValidator<UpdateRegulatorApplicationTaskCommand> updateRegulatorApplicationTaskCommandValidator)
    {
        this._mediator = mediator;
        this._updateRegulatorApplicationTaskCommandValidator = updateRegulatorApplicationTaskCommandValidator;
    }

    [HttpPatch("{Id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateRegistrationTaskStatus(int Id, [FromBody] UpdateRegulatorApplicationTaskCommand command)
    {
        command.Id = Id;
        
        var validationResult = _updateRegulatorApplicationTaskCommandValidator.Validate(command);

        if (!validationResult.IsValid)
        {
            return new BadRequestObjectResult(validationResult.Errors);
        }

        try
        {
            var result = await _mediator.Send(command);

            return result ? NoContent() : StatusCode(500, "Failed to process Status");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}