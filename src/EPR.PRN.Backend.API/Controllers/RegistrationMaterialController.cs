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
    [HttpGet("registrations/{registrationId:guid}/materials")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RegistrationMaterialDto>))]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "gets existing registration materials associated with a registration.",
        Description = "attempting to get existing registration materials associated with a registration."
    )]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetAllRegistrationMaterials([FromRoute]Guid registrationId)
    {
        logger.LogInformation(LogMessages.GetAllRegistrationMaterials, registrationId);

        var registrationMaterials = await mediator.Send(new GetAllRegistrationMaterialsQuery
        {
            RegistrationId = registrationId
        });

        return Ok(registrationMaterials);
    }

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
        logger.LogInformation(LogMessages.CreateRegistrationMaterial, command.RegistrationId);

        var registrationMaterial = await mediator.Send(command);

        return new CreatedResult(string.Empty, registrationMaterial);
    }

    [HttpPost("registrationMaterials/{Id:guid}/createExemptionReferences")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatedResult))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "create exemption references",
        Description = "attempting to create exemption references"
    )]
    [SwaggerResponse(StatusCodes.Status201Created)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> CreateExemptionReferences(Guid Id, [FromBody] CreateExemptionReferencesCommand command)
    {
        logger.LogInformation(LogMessages.CreateExemptionReferences);
        command.RegistrationMaterialId = Id;
        await mediator.Send(command);
        return Ok();
    }

    [HttpPost("registrationMaterials/{id:Guid}/permits")]
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
    public async Task<IActionResult> UpdateRegistrationMaterialPermits([FromRoute] Guid id, [FromBody] UpdateRegistrationMaterialPermitsCommand command)
    {

        logger.LogInformation(LogMessages.UpdateRegistrationMaterial, id);

        command.RegistrationMaterialId = id;

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