using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Dto.Exporter;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace EPR.PRN.Backend.API.Controllers.Exporter
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    [FeatureGate(FeatureFlags.ReprocessorExporter)]
    public class RegistrationController : ControllerBase
    {
        [HttpPost("fees/registrations/save/overseasReprocessingSites")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IEnumerable<OverseasReprocessingSiteDto>))]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(
        Summary = "Submit and save created overseasReprocessingSites",
        Description = "attempting to save newly created overseasReprocessingSites"
    )]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
        [ExcludeFromCodeCoverage(Justification = "TODO: To be done as part of Check your answers overseas reprocessors site(s)")]
        public async Task<IActionResult> SaveOverseasReprocessingSites([FromBody] IEnumerable<OverseasReprocessingSiteDto> overseasReprocessingSites)
        {
            throw new NotImplementedException();
        }
    }
}
