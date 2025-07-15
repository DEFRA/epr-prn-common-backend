using AutoMapper;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace EPR.PRN.Backend.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
[FeatureGate(FeatureFlags.ReprocessorExporter)]
public class RegistrationMaterialController(
    IMediator mediator,
    IValidationService validationService,
    ILogger<RegistrationMaterialController> logger,
    IMapper mapper) : ControllerBase
{
    [HttpGet("registrations/{registrationId:guid}/materials")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ApplicantRegistrationMaterialDto>))]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "gets existing registration materials associated with a registration.",
        Description = "attempting to get existing registration materials associated with a registration."
    )]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetAllRegistrationMaterials([FromRoute]Guid registrationId)
    {
        logger.LogInformation(LogMessages.GetAllRegistrationMaterials, registrationId);

        var registrationMaterials = await mediator.Send(new GetAllRegistrationMaterialsQuery
        {
            RegistrationId = registrationId
        });

        return Ok(registrationMaterials);
    }

    [HttpPost("registrationMaterials/create")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatedResult))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "create a material registration",
        Description = "attempting to create a material registration."
    )]
    [SwaggerResponse(StatusCodes.Status201Created)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> CreateRegistrationMaterial([FromBody] CreateRegistrationMaterialCommand command)
    {
        logger.LogInformation(LogMessages.CreateRegistrationMaterial, command.RegistrationId);

        var registrationMaterial = await mediator.Send(command);

        return new CreatedResult(string.Empty, registrationMaterial);
    }

    [HttpPost("registrationMaterials/{Id:guid}/createExemptionReferences")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "create exemption references",
        Description = "attempting to create exemption references"
    )]
    [SwaggerResponse(StatusCodes.Status201Created)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> CreateExemptionReferences(Guid Id, [FromBody] CreateExemptionReferencesCommand command)
    {
        logger.LogInformation(LogMessages.CreateExemptionReferences);
        command.RegistrationMaterialId = Id;
        await mediator.Send(command);
        return Ok();
    }

    [HttpPost("registrationMaterials/{id:Guid}/permits")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "updates an existing registration material permits",
        Description = "attempting to update the registration material permits."
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Returns No Content")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "If an existing registration is not found", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> UpdateRegistrationMaterialPermits([FromRoute] Guid id, [FromBody] UpdateRegistrationMaterialPermitsCommand command)
    {

        logger.LogInformation(LogMessages.UpdateRegistrationMaterialPermits, id);

        command.RegistrationMaterialId = id;

        await validationService.ValidateAndThrowAsync(command);

        await mediator.Send(command);

        return NoContent();
    }

    [HttpPost("registrationMaterials/{id:Guid}/permitCapacity")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "updates an existing registration material permit capacity",
        Description = "attempting to update the registration material permit capacity."
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Returns No Content")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "If an existing registration is not found", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> UpdateRegistrationMaterialPermitCapacity([FromRoute] Guid id, [FromBody] UpdateRegistrationMaterialPermitCapacityCommand command)
    {

        logger.LogInformation(LogMessages.UpdateRegistrationMaterialPermitCapacity, id);

        command.RegistrationMaterialId = id;

        await validationService.ValidateAndThrowAsync(command);

        await mediator.Send(command);

        return NoContent();
    }

    [HttpGet("registrationMaterials/permitTypes")]
    [ProducesResponseType(typeof(List<MaterialsPermitTypeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ContentResult), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Retrieves list of material permit types",
        Description = "Returns a list of material permit types used during registration."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved permit types.", typeof(List<MaterialsPermitTypeDto>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An unexpected error occurred.", typeof(ContentResult))]
    public async Task<IActionResult> GetMaterialsPermitTypes()
    {
        logger.LogInformation(LogMessages.GetMaterialsPermitTypes);

        var result = await mediator.Send(new GetMaterialsPermitTypesQuery());
        return Ok(result);
    }

    [HttpDelete("registrationMaterials/{registrationMaterialId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResult))]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "delete a registration material",
        Description = "attempting to delete a material registration."
    )]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> DeleteRegistrationMaterial([FromRoute]Guid registrationMaterialId)
    {
        logger.LogInformation(LogMessages.DeleteRegistrationMaterial, registrationMaterialId);

        await mediator.Send(new DeleteRegistrationMaterialCommand
        {
            RegistrationMaterialId = registrationMaterialId
        });

        return Ok();
    }

	[HttpPost("registrationMaterials/UpdateIsMaterialRegistered")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
	[SwaggerOperation(
	Summary = "updates an existing registration material IsMaterialRegistered flag",
	Description = "attempting to update the registration material IsMaterialRegistered flag."
	)]
	public async Task<IActionResult> UpdateIsMaterialRegisteredAsync([FromBody] List<UpdateIsMaterialRegisteredDto> request)
	{
		logger.LogInformation(LogMessages.UpdateIsMaterialRegistered);

		await mediator.Send(new UpdateIsMaterialRegisteredCommand { UpdateIsMaterialRegisteredDto = request });

		return NoContent();
	}

    [HttpPost("registrationMaterials/{id:Guid}/contact")]
    [ProducesResponseType(typeof(RegistrationMaterialContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "upserts an registration material contact",
        Description = "attempting to upsert the registration material contact."
    )]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "If an existing registration is not found", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> UpsertRegistrationMaterialContactAsync([FromRoute] Guid id, [FromBody] RegistrationMaterialContactDto registrationMaterialContact)
    {
        logger.LogInformation(LogMessages.UpsertRegistrationMaterialContact, id);

        var command = new UpsertRegistrationMaterialContactCommand
        {
            RegistrationMaterialId = id,
            UserId = registrationMaterialContact.UserId
        };
        
        await validationService.ValidateAndThrowAsync(command);

        var result = await mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("registrationMaterials/{registrationMaterialId:Guid}/registrationReprocessingDetails")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResult))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "upserts a registration reprocessing details for a registration material",
        Description = "attempting to upsert the registration reprocessing details for a registration material."
    )]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "If an existing registration is not found", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> UpsertRegistrationReprocessingDetailsAsync([FromRoute] Guid registrationMaterialId, [FromBody] RegistrationReprocessingIORequestDto registrationReprocessingDetailsRequest)
    {
        logger.LogInformation(LogMessages.UpsertRegistrationReprocessingDetails, registrationMaterialId);

        await validationService.ValidateAndThrowAsync(registrationReprocessingDetailsRequest);

        var command = mapper.Map<RegistrationReprocessingIOCommand>(registrationReprocessingDetailsRequest);
        command.RegistrationMaterialId = registrationMaterialId;
        await mediator.Send(command);

        return Ok();
    }

    [HttpPost("registrationMaterials/{registrationMaterialId:guid}/overseasReprocessingSites")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OverseasAddressSubmissionDto))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "Submit and save created overseasReprocessingSites",
        Description = "attempting to save newly created overseasReprocessingSites"
    )]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    [ExcludeFromCodeCoverage(Justification = "TODO: To be done as part of Check your answers overseas reprocessors site(s)")]
    public async Task<IActionResult> SaveOverseasReprocessingSites(Guid registrationMaterialId, [FromBody] OverseasAddressSubmissionDto overseasAddressSubmission)
    {
        logger.LogInformation(LogMessages.SaveOverseasReprocessingSites, registrationMaterialId);
        var command = new CreateOverseasMaterialReprocessingSiteCommand
        {
            UpdateOverseasAddress = new UpdateOverseasAddressDto
            {
                OverseasAddresses = overseasAddressSubmission.OverseasAddresses,
                RegistrationMaterialId = registrationMaterialId
            }
        };

        await mediator.Send(command);

        return NoContent();
    }

    [HttpPost("registrationMaterials/{registrationMaterialId:Guid}/materialNotReprocessingReason")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResult))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
      Summary = "Update the reason for not reprocessing a registration material",
      Description = "attempting to update the reason for not reprocessing a registration material."
   )]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "If an existing registration material is not found", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> UpdateMaterialNotReprocessingReasonAsync([FromRoute] Guid registrationMaterialId, [FromBody] string materialNotReprocessingReason)
    {
        logger.LogInformation(LogMessages.UpdateMaterialNotReprocessingReason, registrationMaterialId);

        var command = new UpdateMaterialNotReprocessingReasonCommand
        {
            RegistrationMaterialId = registrationMaterialId,
            MaterialNotReprocessingReason = materialNotReprocessingReason
        };

        await validationService.ValidateAndThrowAsync(command);
        await mediator.Send(command);

        return Ok();
    }
    
    [HttpGet("registrationMaterials/{registrationMaterialId:guid}/overseasMaterialReprocessingSites")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OverseasMaterialReprocessingSiteDto>))]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "gets overseas reprocessing sites including corresponding interim sites associated with a registrationMaterial.",
        Description = "attempting to get overseas reprocessing sites including corresponding interim sites associated with a registrationMaterial."
    )]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> GetOverseasMaterialReprocessingSites([FromRoute] Guid registrationMaterialId)
    {
        logger.LogInformation(LogMessages.GetOverseasMaterialReprocessingSites, registrationMaterialId);

        var registrationMaterials = await mediator.Send(new GetOverseasMaterialReprocessingSitesQuery()
        {
            RegistrationMaterialId = registrationMaterialId
        });

        return Ok(registrationMaterials);
    }
}