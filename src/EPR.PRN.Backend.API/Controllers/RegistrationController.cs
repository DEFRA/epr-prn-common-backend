using System.Net;

using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Constants;
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
public class RegistrationController(IMediator mediator
    , IValidationService validationService
    , ILogger<RegistrationController> logger) : ControllerBase
{
    #region Post Methods
    [HttpPost("registrations/{registrationId:int}/taskStatus")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContentResult))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
            Summary = "update the registration task status",
            Description = "attempting to update the registration task status."
        )]
    [SwaggerResponse(StatusCodes.Status204NoContent, $"Returns No Content", typeof(NoContentResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> UpdateRegistrationTaskStatus([FromRoute] int registrationId, [FromBody] UpdateRegistrationTaskStatusCommand command)
    {
        logger.LogInformation(LogMessages.UpdateRegistrationTaskStatus);
        command.RegistrationId = registrationId;

        await validationService.ValidateAndThrowAsync(command);

        await mediator.Send(command);

        return NoContent();
    }


    [HttpPost("registrations/{registrationId:int}/siteAddress")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContentResult))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
            Summary = "update the site address and contact details of an application registration",
            Description = "attempting to update the site address and contact details of an application registration."
        )]
    [SwaggerResponse(StatusCodes.Status204NoContent, $"Returns No Content", typeof(NoContentResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> UpdateSiteAddress([FromRoute] int registrationId, [FromBody] UpdateRegistrationSiteAddressCommand command)
    {
        logger.LogInformation(LogMessages.UpdateRegistrationSiteAddress);
        command.Id = registrationId;

        await mediator.Send(command);

        return NoContent();
    }
    #endregion Post Methods
}