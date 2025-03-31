using AutoMapper;
using Azure.Core;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

[FeatureGate(FeatureFlags.ReprocessorExporter)]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/RegulatorApplicationTaskStatus")]

public class RegulatorApplicationTaskStatusController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateTaskStatusRequestDto> _updateTaskStatusRequestDtoValidator;

    public RegulatorApplicationTaskStatusController(IMediator mediator, IMapper mapper, IValidator<UpdateTaskStatusRequestDto> updateTaskStatusRequestDtoValidator)
    {
        this._mediator = mediator;
        this._mapper = mapper;
        this._updateTaskStatusRequestDtoValidator = updateTaskStatusRequestDtoValidator;
    }

    [HttpPatch("{registrationTaskStatusId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateRegistrationTaskStatus(int registrationTaskStatusId,[FromBody] UpdateTaskStatusRequestDto dto)
    {
        var validationResult = _updateTaskStatusRequestDtoValidator.Validate(dto);

        if (!validationResult.IsValid)
        {
            return new BadRequestObjectResult(validationResult.Errors);
        }

        var command = _mapper.Map<UpdateRegulatorApplicationTaskCommand>(dto);
        command.Id = registrationTaskStatusId;

        //if (!Enum.IsDefined(typeof(StatusTypes), command.Status))
        //    return BadRequest("Invalid Status value");
        
        var result = await _mediator.Send(command);
        return result ? Ok("Update RegistrationTaskStatus recorded successfully") : StatusCode(500, "Failed to process Status");
    }
}