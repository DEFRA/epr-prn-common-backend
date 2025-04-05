namespace EPR.PRN.Backend.API.Controllers;

using Azure;
using EPR.PRN.Backend.API.Commands;
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
        command.Id = Id;
        var validator = new RegistrationOutcomeValidator();
        var result = await validator.ValidateAsync(command);
        var response = await _mediator.Send(command);
        return StatusCode(response.StatusCode, response.Message ?? (object)response.Data);
       }
    #endregion Patch Methods
}