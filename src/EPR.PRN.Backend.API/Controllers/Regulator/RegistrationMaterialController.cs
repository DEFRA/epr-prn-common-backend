using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace EPR.PRN.Backend.API.Controllers.Regulator;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
[FeatureGate(FeatureFlags.ReprocessorExporter)]
public class RegistrationMaterialController(IMediator mediator
    , IValidator<RegistrationMaterialsOutcomeCommand> registrationMaterialsOutcomeCommandValidator
    , IValidator<RegistrationMaterialsMarkAsDulyMadeCommand> registrationMaterialmarkasdulymadecommandvalidator
    , ILogger<RegistrationMaterialController> logger) : ControllerBase
{
    #region Get methods
    [HttpGet("registrations/{Id}")]
    [ProducesResponseType(typeof(RegistrationOverviewDto), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
            Summary = "get registration with materials and tasks",
            Description = "attempting to get registration with materials and tasks."
        )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns registration with materials and tasks.", typeof(RegistrationOverviewDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetRegistrationOverviewDetailById(Guid Id)
    {
        logger.LogInformation(LogMessages.RegistrationMaterialsTasks);
        var result = await mediator.Send(new GetRegistrationOverviewDetailByIdQuery() { Id = Id });
        return Ok(result);
    }

    [HttpGet("registrationMaterials/{Id}")]
    [ProducesResponseType(typeof(RegistrationMaterialDetailsDto), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
            Summary = "get summary info for a material",
            Description = "attempting to get summary info for a material."
        )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns summary info for a material.", typeof(RegistrationMaterialDetailsDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetMaterialDetailById(Guid Id)
    {
        logger.LogInformation(LogMessages.SummaryInfoMaterial);
        var result = await mediator.Send(new GetMaterialDetailByIdQuery() { Id = Id });
        return Ok(result);
    }

    [HttpGet("registrationMaterials/{Id}/wasteLicences")]
    [ProducesResponseType(typeof(RegistrationMaterialWasteLicencesDto), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWasteLicences(Guid Id)
    {
        logger.LogInformation(LogMessages.RegistrationMaterialsTasks);
        var result = await mediator.Send(new GetMaterialWasteLicencesQuery() { Id = Id });
        return Ok(result);
    }

    [HttpGet("registrationMaterials/{Id}/reprocessingIO")]
    [ProducesResponseType(typeof(RegistrationMaterialReprocessingIODto), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReprocessingIO(Guid Id)
    {
        logger.LogInformation(LogMessages.RegistrationMaterialsTasks);
        var result = await mediator.Send(new GetMaterialReprocessingIOQuery() { Id = Id });
        return Ok(result);
    }

    [HttpGet("registrationMaterials/{Id}/samplingPlan")]
    [ProducesResponseType(typeof(RegistrationMaterialSamplingPlanDto), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSamplingPlan(Guid Id)
    {
        logger.LogInformation(LogMessages.RegistrationMaterialsTasks);
        var result = await mediator.Send(new GetMaterialSamplingPlanQuery() { Id = Id });
        return Ok(result);
    }

    [HttpGet("registrations/{Id}/siteAddress")]
    [ProducesResponseType(typeof(RegistrationSiteAddressDto), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
           Summary = "get site address summary for registration",
           Description = "attempting to get site address info for a registration."
       )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns site address summary for registration.", typeof(RegistrationSiteAddressDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetRegistrationSiteAddressById(Guid Id)
    {
        logger.LogInformation(LogMessages.RegistrationSiteAddress, Id); // Added the missing parameter {Id} to match the placeholder in the log message.
        var result = await mediator.Send(new GetRegistrationSiteAddressByIdQuery() { Id = Id });
        return Ok(result);
    }

    [HttpGet("registrations/{Id}/wasteCarrier")]
    [ProducesResponseType(typeof(RegistrationWasteCarrierDto), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "get waste carrier details",
        Description = "attempting to get waste carrier details.  "
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns waste carrier details for registration.", typeof(RegistrationWasteCarrierDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetWasteCarrierDetailsByRegistrationId(Guid Id)
    {
        logger.LogInformation(LogMessages.RegistrationWasteCarrier, Id);
        var result = await mediator.Send(new GetRegistrationWasteCarrierDetailsByIdQuery() { Id = Id });
        return Ok(result);
    }

    [HttpGet("registrations/{Id}/authorisedMaterials")]
    [ProducesResponseType(typeof(MaterialsAuthorisedOnSiteDto), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
           Summary = "Retrieves information about material authorised on site",
           Description = "Retrieves information about materials authorised for a given site registration."
       )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns authorise material for site.", typeof(MaterialsAuthorisedOnSiteDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetAuthorisedMaterial(Guid Id)
    {
        logger.LogInformation(LogMessages.MaterialAuthorization, Id); // Added the 
        var result = await mediator.Send(new GetMaterialsAuthorisedOnSiteByIdQuery() { Id = Id });
        return Ok(result);
    }
    [HttpGet("registrationMaterials/{Id}/paymentFees")]
    [ProducesResponseType(typeof(MaterialPaymentFeeDto), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
          Summary = "get payment fee of registration material",
          Description = "attempting to get payment fee of registration material."
      )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns payment fee detail with other information.", typeof(MaterialPaymentFeeDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetRegistrationMaterialpaymentFeesById(Guid Id)
    {
        logger.LogInformation(LogMessages.RegistrationMaterialpaymentFees, Id);
        var result = await mediator.Send(new GetMaterialPaymentFeeByIdQuery() { Id = Id });
        return Ok(result);
    }
    [HttpGet("registrationMaterials/{Id}/RegistrationAccreditationReference")]
    [ProducesResponseType(typeof(MaterialPaymentFeeDto), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
         Summary = "get  Registration Accreditation Reference row number",
         Description = "attempting to get  Registration Accreditation Reference row number."
     )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns Registration Accreditation Reference row number.", typeof(RegistrationAccreditationReferenceDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetRegistrationAccreditationReference(Guid Id)
    {
        logger.LogInformation(LogMessages.RegistrationMaterialReference, Id);
        var result = await mediator.Send(new GetRegistrationAccreditationReferenceByIdQuery() { Id = Id });
        return Ok(result);
    }
    #endregion Get Methods

    #region Post Methods
    [HttpPost("registrationMaterials/createRegistrationMaterialAndExemptionReferences")]
    public async Task<IActionResult> CreateRegistrationMaterialAndExemptionReferences([FromBody] CreateRegistrationMaterialAndExemptionReferencesCommand command)
    {
        logger.LogInformation(LogMessages.CreateRegistrationMaterialAndExemptionReferences);        
        await mediator.Send(command);
        return Ok();
    }

    [HttpPost("registrationMaterials/{Id}/outcome")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContentResult))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
            Summary = "update the outcome of a material registration",
            Description = "attempting to update the outcome of a material registration."
        )]
    [SwaggerResponse(StatusCodes.Status204NoContent, $"Returns No Content", typeof(NoContentResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> UpdateRegistrationOutcome(Guid Id, [FromBody] RegistrationMaterialsOutcomeCommand command)
    {
        logger.LogInformation(LogMessages.OutcomeMaterialRegistration, Id);
        command.Id = Id;

        await registrationMaterialsOutcomeCommandValidator.ValidateAndThrowAsync(command);

        await mediator.Send(command);

        return NoContent();
    }

    [HttpPost("registrationMaterials/{Id}/markAsDulyMade")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContentResult))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
           Summary = "Mark as dualy mode material registration",
           Description = "Mark as dualy mode material registration."
       )]
    [SwaggerResponse(StatusCodes.Status204NoContent, $"Returns No Content", typeof(NoContentResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> RegistrationMaterialsMarkAsDulyMade(Guid Id, [FromBody] RegistrationMaterialsMarkAsDulyMadeCommand command)
    {
        logger.LogInformation(LogMessages.MarkAsDulyMade, Id);
        command.RegistrationMaterialId = Id;

        await registrationMaterialmarkasdulymadecommandvalidator.ValidateAndThrowAsync(command);

        await mediator.Send(command);

        return NoContent();
    }
    #endregion Post Methods
}