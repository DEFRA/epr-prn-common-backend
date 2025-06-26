using System.Diagnostics.CodeAnalysis;
using System.Net;

using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.PRN.Backend.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
[FeatureGate(FeatureFlags.ReprocessorExporter)]
public class ReprocessingInputsAndOutputsController(IMediator mediator
    , IValidationService validationService
    , ILogger<RegistrationController> logger) : ControllerBase
{


    #region Post Methods

    [HttpPost("registrationReprocessorIO/{Id:guid}/createReprocessorOutput")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatedResult))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
      Summary = "create registration reprocessor output",
      Description = "attempting to create registration reprocessor output"
  )]
    [SwaggerResponse(StatusCodes.Status201Created)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> CreateReprocessorOutput(Guid Id, [FromBody] CreateReprocessorOutputCommand command)
    {
        logger.LogInformation(LogMessages.CreateReprocessorOutput);
        command.ReprocessorOutputId = Id;
        await mediator.Send(command);
        return Ok();
    }

    #endregion Post Methods
}