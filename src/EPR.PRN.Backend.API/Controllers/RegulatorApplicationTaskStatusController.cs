using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/RegulatorApplicationTaskStatus")]

public class RegulatorApplicationTaskStatusController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public RegulatorApplicationTaskStatusController(IMediator mediator, IMapper mapper)
    {
        this._mediator = mediator;
        this._mapper = mapper;
    }

    [HttpPatch("{registrationTaskStatusId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateRegistrationTaskStatus(int registrationTaskStatusId,[FromBody] RegistrationTaskStatusDto dto)
    {
        var command = _mapper.Map<UpdateRegulatorApplicationTaskCommand>(dto);
        command.Id = registrationTaskStatusId;

        if (!Enum.IsDefined(typeof(StatusTypes), command.Status))
            return BadRequest("Invalid Status value");
        
        var result = await _mediator.Send(command);
        return result ? Ok("Update RegistrationTaskStatus recorded successfully") : StatusCode(500, "Failed to process Status");
    }
}