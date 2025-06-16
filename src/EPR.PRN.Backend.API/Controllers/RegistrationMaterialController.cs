using System.Net;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.PRN.Backend.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
[FeatureGate(FeatureFlags.ReprocessorExporter)]
public class RegistrationMaterialController(
    IMediator mediator,
    ILogger<RegistrationMaterialController> logger) : ControllerBase
{
    [HttpPost("registrationMaterials/create")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatedResult))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "create a material registration",
        Description = "attempting to create a material registration."
    )]
    [SwaggerResponse(StatusCodes.Status201Created)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> CreateRegistrationMaterial([FromBody] CreateRegistrationMaterialCommand command)
    {
        logger.LogInformation(LogMessages.OutcomeMaterialRegistration, command.RegistrationId);

        var registrationMaterialId = await mediator.Send(command);

        return new CreatedResult(string.Empty, registrationMaterialId);
    }

    [HttpPost("registrationMaterials/createRegistrationMaterialAndExemptionReferences")]
    public async Task<IActionResult> CreateRegistrationMaterialAndExemptionReferences([FromBody] CreateRegistrationMaterialAndExemptionReferencesCommand command)
    {
        logger.LogInformation(LogMessages.CreateRegistrationMaterialAndExemptionReferences);
        await mediator.Send(command);
        return Ok();
    }

    [HttpPost("registrationMaterials/{externalId:Guid}/permits")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResult))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "updates an existing registration material",
        Description = "attempting to update the registration material."
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Returns No Content")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "If an existing registration is not found", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> UpdateRegistrationMaterialPermits([FromRoute] Guid externalId, [FromBody] UpdateRegistrationMaterialPermitsCommand command)
    {

        logger.LogInformation(LogMessages.UpdateRegistrationMaterial, externalId);

        command.ExternalId = externalId;

        await mediator.Send(command);

        return NoContent();
    }

    [HttpGet("registrationMaterials/permitTypes")]
    [ProducesResponseType(typeof(List<MaterialsPermitTypeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ContentResult), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Retrieves list of material permit types",
        Description = "Returns a list of material permit types used during registration."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved permit types.", typeof(List<MaterialsPermitTypeDto>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An unexpected error occurred.", typeof(ContentResult))]
    public async Task<IActionResult> GetMaterialsPermitTypes()
    {
        logger.LogInformation(LogMessages.GetMaterialsPermitTypes);

        var result = await mediator.Send(new GetMaterialsPermitTypesQuery());
        return Ok(result);
    }
}