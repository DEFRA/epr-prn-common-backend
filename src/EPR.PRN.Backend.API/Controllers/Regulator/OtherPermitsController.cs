using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.PRN.Backend.API.Controllers.Regulator;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
[FeatureGate(FeatureFlags.ReprocessorExporter)]
public class OtherPermitsController(IMediator mediator) : ControllerBase
{
    [HttpGet("registrations/{Id}/other-permits")]
    [ProducesResponseType(typeof(GetOtherPermitsResultDto), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
           Summary = "get other permits of registration",
           Description = "get other permits of registration."
       )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns other permits of registration.", typeof(GetOtherPermitsResultDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "If other permits not found.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetOtherPermits(Guid Id)
    {
        var result = await mediator.Send(new GetOtherPermitsQuery() { RegistrationId = Id });

        return result != null ? Ok(result) : NotFound();
    }
}