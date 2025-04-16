using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Constants;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace EPR.PRN.Backend.API.Controllers;
[FeatureGate(FeatureFlags.ReprocessorExporter)]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/regulatorRegistrationTaskStatus")]

public class RegulatorRegistrationTaskStatusController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<UpdateRegulatorRegistrationTaskCommand> _validator;
    private readonly ILogger<RegulatorRegistrationTaskStatusController> _logger;

    public RegulatorRegistrationTaskStatusController(IMediator mediator, IValidator<UpdateRegulatorRegistrationTaskCommand> validator, ILogger<RegulatorRegistrationTaskStatusController> logger)
    {
        this._mediator = mediator;
        this._validator = validator;
        this._logger = logger;
    }

    [HttpPost()]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateRegistrationTaskStatus([FromBody] UpdateRegulatorRegistrationTaskCommand command)
    {
        _logger.LogInformation(LogMessages.UpdateRegulatorRegistrationTask);

        await _validator.ValidateAndThrowAsync(command);

        await _mediator.Send(command);

        return NoContent();
    }
}