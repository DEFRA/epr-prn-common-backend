namespace EPR.PRN.Backend.API.Controllers;

using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Constants;
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
    private readonly IValidator<UpdateRegulatorApplicationTaskCommand> _validator;
    private readonly ILogger<RegulatorApplicationTaskStatusController> _logger;

    public RegulatorApplicationTaskStatusController(IMediator mediator, IValidator<UpdateRegulatorApplicationTaskCommand> validator, ILogger<RegulatorApplicationTaskStatusController> logger)
    {
        this._mediator = mediator;
        this._validator = validator;
        this._logger = logger;
    }

    [HttpPost()]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateRegistrationTaskStatus([FromBody] UpdateRegulatorApplicationTaskCommand command)
    {
        _logger.LogInformation(LogMessages.UpdateRegulatorApplicationTask);

        await _validator.ValidateAndThrowAsync(command);

        await _mediator.Send(command);

        return NoContent();
    }
}