using EPR.PRN.Backend.API.Commands.ExporterJourney;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Dto.ExporterJourney;
using EPR.PRN.Backend.API.Queries.ExporterJourney;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using FluentValidation;

namespace EPR.PRN.Backend.API.Controllers.ExporterJourney;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
[FeatureGate(FeatureFlags.ReprocessorExporter)]
public class AddressForServiceOfNoticesController(IMediator mediator,
        IValidator<UpsertAddressForServiceOfNoticesCommand> validator) : ControllerBase
{
    [HttpGet("registrations/{registrationId}/address-for-service-of-notices")]
    [ProducesResponseType(typeof(GetAddressForServiceOfNoticesDto), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
           Summary = "get the legal address used for the service of notices",
           Description = "get the legal address used for the service of notices.",
           Tags = new[] { "Registration" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns get legal address used for the service of notices.", typeof(GetCarrierBrokerDealerPermitsResultDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "If no record found.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetAddressForServiceOfNotices(Guid registrationId)
    {
        var result = await mediator.Send(new GetAddressForServiceOfNoticesQuery { RegistrationId = registrationId });
        return result != null ? Ok(result) : NotFound();
    }

    [HttpPut("registrations/{registrationId}/address-for-service-of-notices")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
           Summary = "update the legal address used for the service of notices for a given registration",
           Description = "update the legal address used for the service of notices for a given registration.",
           Tags = new[] { "Registration" }
       )]
    [SwaggerResponse(StatusCodes.Status200OK, "update the legal address used for the service of notices for a given registration.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "If no record found.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> UpdateAddressForServiceOfNotices(
        [FromRoute] Guid registrationId,
        [FromHeader(Name = "X-EPR-USER")] Guid userId,
        [FromBody] UpsertAddressForServiceOfNoticesDto dto)
    {
        try
        {
            var command = new UpsertAddressForServiceOfNoticesCommand
            {
                UserId = userId,
                RegistrationId = registrationId,
                Dto = dto
            };

            await validator.ValidateAndThrowAsync(command);

            await mediator.Send(command);

            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}