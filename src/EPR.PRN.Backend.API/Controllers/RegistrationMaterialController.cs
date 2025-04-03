using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Validators;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
[FeatureGate(FeatureFlags.ReprocessorExporter)]
public class RegistrationMaterialController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RegistrationMaterialController> _logger;
    

    public RegistrationMaterialController(IMediator mediator, ILogger<RegistrationMaterialController> logger)
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
     public async Task<IActionResult> UpdateRegistrationOutcome(int Id, [FromBody] RegistrationMaterialsOutcomeCommand command)
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