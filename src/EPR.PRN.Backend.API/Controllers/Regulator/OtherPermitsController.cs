using EPR.PRN.Backend.API.Commands;
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
    public async Task<IActionResult> GetOtherPermits(Guid id)
    {
        var result = await mediator.Send(new GetOtherPermitsQuery { RegistrationId = id });

        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost("registrations/{Id}/other-permits")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
           Summary = "create other permits of registration",
           Description = "create other permits of registration."
       )]
    [SwaggerResponse(StatusCodes.Status200OK, "Confirms that resource exists.")]
    [SwaggerResponse(StatusCodes.Status201Created, "Creates other permits.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "If registration not found.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> CreateOtherPermits(
        [FromRoute] Guid id,
        [FromHeader(Name = "X-EPR-USER")] Guid userId,
        [FromBody] CreateOtherPermitsDto dto)
    {
        try
        {
            var created = await mediator.Send(new CreateOtherPermitsCommand
            {
                UserId = userId,
                RegistrationId = id,
                Dto = dto
            });

            return created ? CreatedAtAction(nameof(GetOtherPermits), new { id }, null) : Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("registrations/{Id}/other-permits")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
           Summary = "update other permits of registration",
           Description = "update other permits of registration."
       )]
    [SwaggerResponse(StatusCodes.Status200OK, "Updates other permits.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "If other permits not found.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> UpdateOtherPermits(
        [FromRoute] Guid id,
        [FromHeader(Name = "X-EPR-USER")] Guid userId,
        [FromBody] UpdateOtherPermitsDto dto)
    {
        try
        {
            await mediator.Send(new UpdateOtherPermitsCommand
            {
                UserId = userId,
                RegistrationId = id,
                Dto = dto
            });

            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}