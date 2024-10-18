namespace EPR.PRN.Backend.API.Controllers;

using EPR.PRN.Backend.API.Common.DTO;
using EPR.PRN.Backend.API.Configs;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/prn")]
public class PrnController : Controller
{
    private readonly IPrnService _prnService;
    private readonly ILogger<PrnController> _logger;
    private readonly IObligationCalculatorService _obligationCalculatorService;
    private readonly PrnObligationCalculationConfig _config;

    public PrnController(IPrnService prnService, ILogger<PrnController> logger, IObligationCalculatorService obligationCalculatorService, IOptions<PrnObligationCalculationConfig> config)
    {
        _prnService = prnService;
        _logger = logger;
        _obligationCalculatorService = obligationCalculatorService;
        _config = config.Value;
    }

    #region Get methods
    [HttpGet("{prnId}")]
    [ProducesResponseType(typeof(PrnDto), 200)]
    public async Task<IActionResult> GetPrn([FromHeader(Name = "X-EPR-ORGANISATION")] Guid orgId, [FromRoute] Guid prnId)
    {
        var prn = await _prnService.GetPrnForOrganisationById(orgId, prnId);

        if (prn == null)
            return NotFound();

        return Ok(prn);
    }

    [HttpGet("search/{page?}/{search?}/{filterBy?}/{sortBy?}")]
    [ProducesResponseType(typeof(PaginatedResponseDto<PrnDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetSearchPrns([FromHeader(Name = "X-EPR-ORGANISATION")] Guid orgId,
        [FromQuery] PaginatedRequestDto request)
    {
        if (orgId == Guid.Empty)
            return Unauthorized();

        var result = await _prnService.GetSearchPrnsForOrganisation(orgId, request);

        return Ok(result);
    }

    [HttpGet("organisation")]
    [ProducesResponseType(typeof(List<PrnDto>), 200)]
    public async Task<IActionResult> GetAllPrnByOrganisationId([FromHeader(Name = "X-EPR-ORGANISATION")] Guid orgId)
    {
        var prn = await _prnService.GetAllPrnByOrganisationId(orgId);

        if (prn.Count == 0)
            return NotFound();

        return Ok(prn);
    }

    [HttpGet("obligationcalculation/{year}")]
    [ProducesResponseType(typeof(List<ObligationData>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(424)]
    public async Task<ActionResult<ObligationModel>> GetObligationCalculation([FromHeader(Name = "X-EPR-ORGANISATION")] Guid organisationId, [FromRoute] int year)
    {
        if (year < _config.StartYear || year > _config.EndYear)
        {
            return BadRequest($"Invalid year provided: {year}.");
        }

        var obligationCalculation = await _obligationCalculatorService.GetObligationCalculation(organisationId, year);

        if (obligationCalculation == null || obligationCalculation.Count == 0)
        {
            return NotFound($"Obligation calculation not found for Organisation Id : {organisationId}");
        }

        return Ok(new ObligationModel { ObligationData = obligationCalculation, NumberOfPrnsAwaitingAcceptance = 0 });
    }
    #endregion

    #region Post Methods
    [HttpPost("status")]
    public async Task<IActionResult> UpdatePrnStatus(
        [FromHeader(Name = "X-EPR-ORGANISATION")] Guid orgId,
        [FromHeader(Name = "X-EPR-USER")] Guid userId,
        [FromBody] List<PrnUpdateStatusDto> prnUpdates)
    {
        try
        {
            await _prnService.UpdateStatus(orgId, userId, prnUpdates);

            return Ok();
        }
        catch (NotFoundException ex)
        {
            _logger.LogInformation(ex, "Recieved not found exception");
            return Problem(ex.Message, null, (int)HttpStatusCode.NotFound);
        }
        catch (ConflictException ex)
        {
            _logger.LogInformation(ex, "Recieved conflict exception");
            return Problem(ex.Message, null, (int)HttpStatusCode.Conflict);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Recieved Unhandled exception");
            return Problem("Internal Server Error", null, (int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("organisation/{organisationId}/calculate")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
    public async Task<IActionResult> CalculateAsync(Guid organisationId, [FromBody] List<SubmissionCalculationRequest> request)
    {
        if (request == null || request.Count == 0)
        {
            return BadRequest(new { message = "Submission calculation request cannot be null or empty." });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var calculationResult = await _obligationCalculatorService.CalculateAsync(organisationId, request);

            if (!calculationResult.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Calculation failed due to internal errors." });
            }

            await _obligationCalculatorService.SaveCalculatedPomDataAsync(calculationResult.Calculations);

            return Accepted(new { message = "Calculation successful.", data = calculationResult.Calculations });
        }
        catch (TimeoutException ex)
        {
            return StatusCode(StatusCodes.Status504GatewayTimeout, new { message = "Calculation timed out.", details = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred during calculation.", details = ex.Message });
        }
    }

    #endregion
}