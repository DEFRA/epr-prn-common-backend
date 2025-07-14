using System.Diagnostics.CodeAnalysis;
using System.Net;

using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Dto;
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
public class RegistrationController(IMediator mediator
    , IValidationService validationService
    , ILogger<RegistrationController> logger) : ControllerBase
{
    #region Get Methods

    [HttpGet("registrations/{applicationTypeId:int}/organisations/{organisationId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegistrationDto))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "gets an existing registration by the organisation ID.",
        Description = "attempting to get an existing registration using the organisation ID."
    )]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    [ExcludeFromCodeCoverage(Justification = "TODO: To be done as part of create registration user story")]
    public async Task<IActionResult> GetRegistrationByOrganisation([FromRoute]int applicationTypeId, [FromRoute]Guid organisationId)
    {
        logger.LogInformation(string.Format(LogMessages.GetRegistrationByOrganisation, applicationTypeId, organisationId));

        var registration = await mediator.Send(new GetRegistrationByOrganisationQuery
        {
            ApplicationTypeId = applicationTypeId,
            OrganisationId = organisationId
        });

        if (registration is null)
        {
            return NotFound();
        }

        return Ok(registration);
    }

    #endregion

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

        var registration = await mediator.Send(command);

        return new CreatedResult(string.Empty, registration);
    }

    [HttpPost("registrations/{registrationId:guid}/update")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OkResult))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "updates an existing registration",
        Description = "attempting to update the registration application."
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Returns No Content", typeof(int))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "If an existing registration is not found", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    [ExcludeFromCodeCoverage(Justification = "TODO: To be done as part of create registration user story")]
    public async Task<IActionResult> UpdateRegistration([FromRoute]Guid registrationId, [FromBody] UpdateRegistrationCommand command)
    {
        try
        {
            logger.LogInformation(string.Format(LogMessages.UpdateRegistration, registrationId.ToString()));
            command.RegistrationId = registrationId;

            await validationService.ValidateAndThrowAsync(command);

            await mediator.Send(command);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "Could not find registration with ID {RegistrationId}", registrationId);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating registration with ID {RegistrationId}", registrationId);
            return StatusCode(StatusCodes.Status500InternalServerError, new ContentResult
            {
                Content = "An unexpected error occurred while processing your request.",
                ContentType = "text/plain"
            });
        }

        return NoContent();
    }

    [HttpPost("registrations/{registrationId:guid}/taskStatus")]
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
    public async Task<IActionResult> UpdateRegistrationTaskStatus([FromRoute] Guid registrationId, [FromBody] UpdateRegistrationTaskStatusCommand command)
    {
        logger.LogInformation(LogMessages.UpdateRegistrationTaskStatus);
        command.RegistrationId = registrationId;

        await validationService.ValidateAndThrowAsync(command);

        await mediator.Send(command);

        return NoContent();
    }

    [HttpPost("registrations/{registrationId:guid}/applicationTaskStatus")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContentResult))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "update the application registration task status",
        Description = "attempting to update the application registration task status."
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, $"Returns No Content", typeof(NoContentResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> UpdateApplicationRegistrationTaskStatus([FromRoute] Guid registrationId, [FromBody] UpdateApplicationRegistrationTaskStatusCommand command)
    {
        logger.LogInformation(LogMessages.UpdateApplicationRegistrationTaskStatus);
        command.RegistrationId = registrationId;

        await validationService.ValidateAndThrowAsync(command);

        await mediator.Send(command);

        return NoContent();
    }


    [HttpPost("registrations/{registrationId:guid}/siteAddress")]
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
    public async Task<IActionResult> UpdateSiteAddress([FromRoute] Guid registrationId, [FromBody] UpdateRegistrationSiteAddressCommand command)
    {
        logger.LogInformation(LogMessages.UpdateRegistrationSiteAddress);
        command.RegistrationId = registrationId;

        await validationService.ValidateAndThrowAsync(command);

        await mediator.Send(command);

        return NoContent();
    }

    #endregion Post Methods

    [HttpGet("registrations/{organisationId:guid}/overview")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RegistrationOverviewDto>))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "return the registrations overview for a given organisation id",
        Description = "attempting to return registrations."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the registrations overview for the given organisation ID", typeof(List<RegistrationOverviewDto>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetRegistrationsOverviewForOrgId([FromRoute] Guid organisationId)
    {
        logger.LogInformation(LogMessages.RegistrationsOverview, organisationId);
        
        var request = new GetRegistrationsOverviewByOrgIdQuery { OrganisationId = organisationId };
        await validationService.ValidateAndThrowAsync(request);
        
        var result = await mediator.Send(request);

        return Ok(result);
    }
}