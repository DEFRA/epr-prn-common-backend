using System.Net;

using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;

using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static EPR.PRN.Backend.API.Common.Constants.PrnConstants;

namespace EPR.PRN.Backend.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
[FeatureGate(FeatureFlags.ReprocessorExporter)]
public class AccreditationController(IMediator mediator
    , ILogger<RegistrationMaterialController> logger) : ControllerBase
{
    #region Get methods
    [HttpGet("registrations/{id}/accreditations/{year}")]
    [ProducesResponseType(typeof(RegistrationOverviewDto), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
            Summary = "get registration with materials, accreditations and tasks",
            Description = "attempting to get registration with materials, accreditations and tasks."
        )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns registration with materials, accreditations and tasks.", typeof(RegistrationOverviewDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetAccreditationOverviewDetailById(Guid id, int year)
    {
        logger.LogInformation(LogMessages.AccreditationMaterialsTasks);
        var result = await mediator.Send(new GetAccreditationOverviewDetailByIdQuery() { Id = id, Year = year });
        return Ok(result);
    }

    #endregion Get Methods
}