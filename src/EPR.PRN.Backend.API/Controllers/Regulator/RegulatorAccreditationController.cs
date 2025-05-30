using System.Net;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Services.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.PRN.Backend.API.Controllers.Regulator;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
[FeatureGate(FeatureFlags.ReprocessorExporter)]
public class RegulatorAccreditationController(IMediator mediator,
    IValidator<RegulatorAccreditationMarkAsDulyMadeCommand> validator,
     IValidationService validationService,
     ILogger<RegulatorAccreditationController> logger) : ControllerBase
{
    #region Get methods
    [HttpGet("accreditations/{1}/paymentFees")]
    [ProducesResponseType(typeof(AccreditationPaymentFeeDetailsDto), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
            Summary = "get registration accreditations with materials and tasks",
            Description = "attempting to get registration accreditations with materials and tasks."
        )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns registration accreditations with materials and tasks.", typeof(AccreditationPaymentFeeDetailsDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetRegistrationAccreditationById(int Id)
    {
        logger.LogInformation(LogMessages.AccreditationMaterialsTasks);
        var result = await mediator.Send(new GetRegistrationAccreditationSummaryByIdQuery() { Id = Id });
        return Ok(result);
    }
    #endregion Get Methods

    #region Post methods
    [HttpPost("accreditations/{Id}/markAsDulyMade")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContentResult))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
       Summary = "mark an accreditation as duly made.",
       Description = "attempting to mark an accreditation as duly made."
   )]
    [SwaggerResponse(StatusCodes.Status204NoContent, $"Returns No Content", typeof(NoContentResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> RegulatorAccreditationMarkAsDulyMade(Guid Id, [FromBody] RegulatorAccreditationMarkAsDulyMadeCommand command)
    {
        logger.LogInformation(LogMessages.MarkAccreditationAsDulyMade, Id);
        command.DulyMadeBy = Id;

        await validator.ValidateAndThrowAsync(command);

        await mediator.Send(command);

        return NoContent();
    }
    #endregion Post Methods
}
