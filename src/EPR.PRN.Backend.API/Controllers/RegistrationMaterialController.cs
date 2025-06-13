using System.Net;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Constants;
using FluentValidation;
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
}