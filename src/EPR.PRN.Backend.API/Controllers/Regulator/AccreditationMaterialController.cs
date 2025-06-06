using System.ComponentModel.DataAnnotations;
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

namespace EPR.PRN.Backend.API.Controllers.Regulator;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
[FeatureGate(FeatureFlags.ReprocessorExporter)]
public class AccreditationController(IMediator mediator,
    IValidator<RegulatorAccreditationMarkAsDulyMadeCommand> validator,
    ILogger<AccreditationController> logger) : ControllerBase
{
    #region Get methods
    [HttpGet("registrations/{id}/accreditations")]
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
    public async Task<IActionResult> GetRegistrationByIdWithAccreditationsAsync(Guid id, [FromQuery] int? year)
    {
        logger.LogInformation(LogMessages.AccreditationMaterialsTasks);
        var result = await mediator.Send(new GetRegistrationOverviewDetailWithAccreditationsByIdQuery() { Id = id, Year = year });
        return Ok(result);
    }

    [HttpGet("accreditations/{Id}/samplingPlan")]
    [ProducesResponseType(typeof(AccreditationSamplingPlanDto), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
            Summary = "get file uploads relating to an accreditation",
            Description = "attempting to get file uploads relating to an accreditation."
        )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns file uploads relating to an accreditation.", typeof(AccreditationSamplingPlanDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetSamplingPlan(Guid Id)
    {
        logger.LogInformation(LogMessages.AccreditationSamplingPlan);
        var result = await mediator.Send(new GetAccreditationSamplingPlanQuery() { Id = Id });
        return Ok(result);
    }

    [HttpGet("accreditations/{Id}/paymentFees")]
    [ProducesResponseType(typeof(AccreditationPaymentFeeDetailsDto), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "get accreditation payment fees",
        Description = "attempting to get payment fees."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns accreditation payment fee details.", typeof(AccreditationPaymentFeeDetailsDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetRegistrationAccreditationPaymentFeeDetailsById(Guid Id)
    {
        logger.LogInformation(LogMessages.AccreditationMaterialsTasks);
        var result = await mediator.Send(new GetRegistrationAccreditationPaymentFeesByIdQuery() { Id = Id });
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
        command.Id = Id;

        await validator.ValidateAndThrowAsync(command);

        await mediator.Send(command);

        return NoContent();
    }
    #endregion Post Methods
}