using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Models.ReadModel;
using EPR.PRN.Backend.API.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/registrations")]

public class RegisrationMaterialController : ControllerBase
{
    private readonly IMediator _mediator;

    public RegisrationMaterialController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    #region Get methods
    [HttpGet("{registrationId}/materials")]
    [ProducesResponseType(typeof(RegistrationMaterialTaskReadModel), 200)]
    [ProducesResponseType(400)]
    public async Task<RegistrationMaterialTaskReadModel> GetAllMaterialsByID(Guid registrationId)
    {
        var result = await _mediator.Send(new GetAllMaterialsByIdQuery() { RegistrationID = registrationId});
        return result;
    }


#endregion Get Methods

#region Post Methods
    [HttpPost("{registrationId}/materials/{materialId}/outcome")]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateRegistrationOutCome( Guid registrationId,Guid materialId,[FromBody] RegistrationOutComeCommand command)
    {
        if (registrationId != command.RegistrationID || materialId != command.MaterialID)
            return BadRequest("Mismatched registration/material IDs");
        if (!Enum.IsDefined(typeof(OutComeTypes), command.OutCome))
            return BadRequest("Invalid OutCome value");
        
        var result = await _mediator.Send(command);
        return result ? Ok("Update registration recorded successfully") : StatusCode(500, "Failed to process outcome");
    }



    #endregion Post Methods
}