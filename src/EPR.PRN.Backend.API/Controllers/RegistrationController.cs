using System.Diagnostics.CodeAnalysis;
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
    [HttpPost("registrations")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
            Summary = "create the registration application",
            Description = "attempting to create the registration application."
        )]
    [SwaggerResponse(StatusCodes.Status204NoContent, $"Returns No Content", typeof(int))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    [ExcludeFromCodeCoverage(Justification = "TODO: To be done as part of create registration user story")]
    public async Task<IActionResult> CreateRegistration([FromBody] CreateRegistrationCommand command)
    {
        logger.LogInformation(LogMessages.CreateRegistration);

        var registrationId = await mediator.Send(command);

        return new CreatedResult(string.Empty, registrationId);
    }

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
            Summary = "update the site address of an application registration",
            Description = "attempting to update the site address of an application registration."
        )]
    [SwaggerResponse(StatusCodes.Status204NoContent, $"Returns No Content", typeof(NoContentResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> UpdateSiteAddress([FromRoute] int registrationId, [FromBody] UpdateRegistrationSiteAddressCommand command)
    {
        logger.LogInformation(LogMessages.UpdateRegistrationSiteAddress);
        command.RegistrationId = registrationId;

        await validationService.ValidateAndThrowAsync(command);

        await mediator.Send(command);

        return NoContent();
    }
    #endregion Post Methods

    [HttpGet("registrations/{organisationId:int}/overview")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContentResult))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "return the registrations overview for a given organisation id",
        Description = "attempting to return registrations."
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, $"Returns No Content", typeof(NoContentResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetRegistrationsForOrg([FromRoute] int organisationId)
    {
        var command = new RegistrationsOverviewCommand { OrganisationId = organisationId };
        
        logger.LogInformation(LogMessages.RegistrationsOverview, command.OrganisationId);
        
        await validationService.ValidateAndThrowAsync(command);
        var result = await mediator.Send(command);
        return Ok(result);
    }
}