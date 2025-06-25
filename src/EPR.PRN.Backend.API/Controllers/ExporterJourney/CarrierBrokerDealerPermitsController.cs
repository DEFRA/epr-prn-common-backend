using EPR.PRN.Backend.API.Commands.ExporterJourney;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Dto.ExporterJourney;
using EPR.PRN.Backend.API.Queries.ExporterJourney;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.PRN.Backend.API.Controllers.ExporterJourney;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
[FeatureGate(FeatureFlags.ReprocessorExporter)]
public class CarrierBrokerDealerPermitsController(IMediator mediator) : ControllerBase
{
    [HttpGet("registrations/{registrationId}/carrier-broker-dealer-permits")]
    [ProducesResponseType(typeof(GetCarrierBrokerDealerPermitsResultDto), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
           Summary = "get carrier broker dealer permits data for given registration",
           Description = "get carrier broker dealer permits data for given registration.",
           Tags = new[] { "Registration" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns get carrier broker dealer permits data for given registration.", typeof(GetCarrierBrokerDealerPermitsResultDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "If no record found.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetCarrierBrokerDealerPermits(Guid registrationId)
    {
        var result = await mediator.Send(new CarrierBrokerDealerPermitsQuery { RegistrationId = registrationId });
        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost("registrations/{registrationId}/carrier-broker-dealer-permits")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
           Summary = "create carrier broker dealer permits record for given registration",
           Description = "create carrier broker dealer permits record for given registration.",
           Tags = new[] { "Registration" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Confirms that resource already exists.")]
    [SwaggerResponse(StatusCodes.Status201Created, "Create carrier broker dealer permits record for given registration.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "If registration parent record not found.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> CreateCarrierBrokerDealerPermits(
        [FromRoute] Guid registrationId,
        [FromHeader(Name = "X-EPR-USER")] Guid userId,
        [FromBody] CreateCarrierBrokerDealerPermitsDto dto)
    {
        try
        {
            var created = await mediator.Send(new CreateCarrierBrokerDealerPermitsCommand
            {
                UserId = userId,
                RegistrationId = registrationId, 
                WasteCarrierBrokerDealerRegistration = dto.WasteCarrierBrokerDealerRegistration
            });

            return created ? CreatedAtAction(nameof(GetCarrierBrokerDealerPermits), new { registrationId }, null) : Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("registrations/{registrationId}/carrier-broker-dealer-permits")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
           Summary = "update carrier broker dealer permits record for given registration",
           Description = "update carrier broker dealer permits record for given registration.",
           Tags = new[] { "Registration" }
       )]
    [SwaggerResponse(StatusCodes.Status200OK, "update carrier broker dealer permits record for given registration.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "If no record found.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> UpdateCarrierBrokerDealerPermits(
        [FromRoute] Guid registrationId,
        [FromHeader(Name = "X-EPR-USER")] Guid userId,
        [FromBody] UpdateCarrierBrokerDealerPermitsDto dto)
    {
        try
        {
            await mediator.Send(new UpdateCarrierBrokerDealerPermitsCommand
            {
                UserId = userId,
                RegistrationId = registrationId,
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