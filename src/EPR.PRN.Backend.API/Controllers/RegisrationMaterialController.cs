using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Configs;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Validators;
using EPR.PRN.Backend.Obligation.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
public class RegisrationMaterialController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RegisrationMaterialController> _logger;

    public RegisrationMaterialController(IMediator mediator, ILogger<RegisrationMaterialController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
   
    #region Get methods
    [HttpGet("registrations/{Id}")]
    [ProducesResponseType(typeof(RegistrationOverviewDto), 200)]
    [ProducesResponseType(400)]
    public async Task<RegistrationOverviewDto> GetRegistrationOverviewDetailById(int Id)
    {
        var result = await _mediator.Send(new GetRegistrationOverviewDetailByIdQuery() { Id = Id });
        return result;
    }
    [HttpGet("registrationMaterials/{Id}")]
    [ProducesResponseType(typeof(RegistrationMaterialDto), 200)]
    [ProducesResponseType(400)]
    public async Task<RegistrationMaterialDto> GetMaterialDetailById(int Id)
    {
        var result = await _mediator.Send(new GetMaterialDetailByIdQuery() { Id = Id });
        return result;
    }

    #endregion Get Methods

    #region Patch Methods

    [HttpPatch("registrationMaterials/{Id}/outcome")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateRegistrationOutcome( int Id,[FromBody] RegistrationOutcomeCommand command)
    {
       
        _logger.LogInformation("UpdateRegistrationOutcome called with Id: {Id}", Id);

    if (command == null)
    {
        _logger.LogWarning("UpdateRegistrationOutcome received a null command.");
        return BadRequest("Invalid request body.");
    }

        command.Id = Id;
     var validator = new RegistrationOutcomeValidator();
    var result = await validator.ValidateAsync(command);

    if (!result.IsValid)
    {
        _logger.LogWarning("Validation failed for Id: {Id}. Errors: {Errors}", Id, result.Errors);
        return BadRequest(result.Errors);
    }


    try
    {
       var response = await _mediator.Send(command);
        _logger.LogInformation("Registration outcome updated successfully for Id: {Id}", Id);
         return StatusCode(response.StatusCode, response.Message ?? (object)response.Data);
     }
    catch (Exception ex)
    {
        _logger.LogError(ex, "An error occurred while updating the registration outcome for Id: {Id}", Id);
        return StatusCode(500, "An unexpected error occurred.");
    }
}
    #endregion Patch Methods
}