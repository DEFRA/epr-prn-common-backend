using System.Net;

using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;

using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static EPR.PRN.Backend.API.Common.Constants.PrnConstants;

namespace EPR.PRN.Backend.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
[FeatureGate(FeatureFlags.ReprocessorExporter)]
public class RegistrationMaterialController(IMediator mediator
    , IValidator<RegistrationMaterialsOutcomeCommand> registrationMaterialsOutcomeCommandValidator
    , ILogger<RegistrationMaterialController> logger) : ControllerBase
{
    #region Get methods
    [HttpGet("registrations/{Id}")]
    [ProducesResponseType(typeof(RegistrationOverviewDto), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
            Summary = "get registration with materials and tasks",
            Description = "attempting to get registration with materials and tasks."
        )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns registration with materials and tasks.", typeof(RegistrationOverviewDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetRegistrationOverviewDetailById(int Id)
    {
        logger.LogInformation(LogMessages.RegistrationMaterialsTasks);
        var result = await mediator.Send(new GetRegistrationOverviewDetailByIdQuery() { Id = Id });
        return Ok(result);
    }

    [HttpGet("registrationMaterials/{Id}")]
    [ProducesResponseType(typeof(RegistrationMaterialDetailsDto), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
            Summary = "get summary info for a material",
            Description = "attempting to get summary info for a material."
        )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns summary info for a material.", typeof(RegistrationMaterialDetailsDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetMaterialDetailById(int Id)
    {
        logger.LogInformation(LogMessages.SummaryInfoMaterial);
        var result = await mediator.Send(new GetMaterialDetailByIdQuery() { Id = Id });
        return Ok(result);
    }

    [HttpGet("registrations/{Id}/siteAddress")]
    [ProducesResponseType(typeof(RegistrationSiteAddressDto), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
           Summary = "get site address summary for registration",
           Description = "attempting to get site address info for a registration."
       )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns site address summary for registration.", typeof(RegistrationSiteAddressDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetRegistrationSiteAddressById(int Id)
    {
        logger.LogInformation(LogMessages.RegistrationSiteAddress, Id); // Added the missing parameter {Id} to match the placeholder in the log message.
        var result = await mediator.Send(new GetRegistrationSiteAddressByIdQuery() { Id = Id });
        return Ok(result);
    }
    
    [HttpGet("registrations/{Id}/authorisedMaterials")]
    [ProducesResponseType(typeof(MaterialsAuthorisedOnSiteDto), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
           Summary = "Retrieves information about material authorised on site",
           Description = "Retrieves information about materials authorised for a given site registration."
       )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns authorise material for site.", typeof(MaterialsAuthorisedOnSiteDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetAuthorisedMaterial(int Id)
    {
        logger.LogInformation(LogMessages.MeterialAutorisation, Id); // Added the 
        var result = await mediator.Send(new GetMaterialsAuthorisedOnSiteByIdQuery() { Id = Id });
        return Ok(result);
    }

    #endregion Get Methods

    #region Post Methods
    [HttpPost("registrationMaterials/{Id}/outcome")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContentResult))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
            Summary = "update the outcome of a material registration",
            Description = "attempting to update the outcome of a material registration."
        )]
    [SwaggerResponse(StatusCodes.Status204NoContent, $"Returns No Content", typeof(NoContentResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> UpdateRegistrationOutcome(int Id, [FromBody] RegistrationMaterialsOutcomeCommand command)
    {
        logger.LogInformation(LogMessages.OutcomeMaterialRegistration, Id);
        command.Id = Id;

        await registrationMaterialsOutcomeCommandValidator.ValidateAndThrowAsync(command);

        await mediator.Send(command);

        return NoContent();
    }
    #endregion Post Methods
}