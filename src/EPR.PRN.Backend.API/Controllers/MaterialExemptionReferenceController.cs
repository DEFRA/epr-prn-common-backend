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
public class MaterialExemptionReferenceController(
IMediator mediator,
ILogger<MaterialExemptionReferenceController> logger) : ControllerBase
{
    [HttpPost("createxemptionreferences")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary ="Create material exemption references", Description ="Saves a list of material exemption references for during the reprocessor/exporter journey.")]
    [SwaggerResponse(StatusCodes.Status200OK, "returns true/false", typeof(bool))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> CreateMaterialExemptionReference([FromBody] CreateMaterialExemptionReferenceCommand command)
    {
        logger.LogInformation("Attempting to save material exemption references");
        var result = await mediator.Send(command);

        return Ok(result);
    }
}
