using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/regulatorRegistrationTaskStatus")]

public class RegulatorRegistrationTaskStatusController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateTaskStatusRequestDto> _updateTaskStatusRequestDtoValidator;

    public RegulatorRegistrationTaskStatusController(IMediator mediator, IMapper mapper, IValidator<UpdateTaskStatusRequestDto> updateTaskStatusRequestDtoValidator)
    {
        this._mediator = mediator;
        this._mapper = mapper;
        this._updateTaskStatusRequestDtoValidator = updateTaskStatusRequestDtoValidator;
    }

    #region Patch Methods

    [HttpPatch("{registrationTaskStatusId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateRegistrationTaskStatus(int registrationTaskStatusId,[FromBody] UpdateTaskStatusRequestDto dto)
    {
        var command = _mapper.Map<UpdateRegulatorRegistrationTaskCommand>(dto);
        command.Id = registrationTaskStatusId;

        if (!Enum.IsDefined(typeof(StatusTypes), command.Status))
            return BadRequest("Invalid Status value");
        
        var result = await _mediator.Send(command);
        return result ? Ok("Update RegistrationTaskStatus recorded successfully") : StatusCode(500, "Failed to process Status");
    }

    #endregion Patch Methods
}