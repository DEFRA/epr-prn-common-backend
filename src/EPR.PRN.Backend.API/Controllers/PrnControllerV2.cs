using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using BackendAccountService.Core.Models.Request;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Profiles;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DataModels;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EPR.PRN.Backend.API.Controllers;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/prn")]
public class PrnControllerV2(
    IPrnService prnService,
    ILogger<PrnControllerV2> logger,
    IValidator<SavePrnDetailsRequest> savePrnDetailsRequestV2Validator
) : ControllerBase
{
    private readonly IMapper _mapper = PrnMapper.CreateMapper();

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PrnDto>> SaveAsync([FromBody] SavePrnDetailsRequest requestV2)
    {
        var validationResult = await savePrnDetailsRequestV2Validator.ValidateAsync(requestV2);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        try
        {
            var eprn = _mapper.Map<Eprn>(requestV2);
            var createdDto = _mapper.Map<PrnDto>(await prnService.SaveEprnDetails(eprn));

            return Created($"api/v1/prn/{createdDto.Id}", createdDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Recieved Unhandled exception");
            return Problem("Internal Server Error", null, (int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("modified-prns")]
    [ProducesResponseType(typeof(List<PrnUpdateStatus>), 200)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<PrnUpdateStatus>>> GetModifiedPrns(
        [FromQuery] ModifiedPrnsbyDateRequest request
    )
    {
        if (request.Validate(new ValidationContext(request)).Any())
            return BadRequest(ModelState);

        return Ok(await prnService.GetModifiedPrnsbyDate(request.From, request.To));
    }
}
