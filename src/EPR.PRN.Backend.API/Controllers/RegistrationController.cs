using System.Net;

using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Constants;
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
    , ILogger<RegistrationController> logger) : ControllerBase
{
    #region Post Methods
    [HttpPost("registrations/{id:int}/siteAddress")]
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
    public async Task<IActionResult> UpdateSiteAddress([FromRoute] int id, [FromBody] UpdateRegistrationSiteAddressCommand command)
    {
        logger.LogInformation(LogMessages.UpdateSiteAddress);
        command.Id = id;

        await mediator.Send(command);

        return NoContent();
    }
    #endregion Post Methods
}