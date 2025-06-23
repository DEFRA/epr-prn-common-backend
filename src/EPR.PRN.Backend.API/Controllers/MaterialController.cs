using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Dto;
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
public class MaterialController(
    IMediator mediator,
    ILogger<MaterialController> logger) : ControllerBase
{
    [HttpGet("materials")]
    [ProducesResponseType(typeof(IList<MaterialDto>), 200)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Get all materials",
        Description = "Retrieves a list of all materials that can be applied for during the reprocessor/exporter journey."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of materials found.", typeof(IList<MaterialDto>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetAllMaterials()
    {
        logger.LogInformation("Attempting to get all materials");

        var result = await mediator.Send(new GetAllMaterialsQuery());
        return Ok(result);
    }
}