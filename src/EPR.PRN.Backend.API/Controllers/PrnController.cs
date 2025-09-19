namespace EPR.PRN.Backend.API.Controllers;

using BackendAccountService.Core.Models.Request;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Configs;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Models;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.Dto;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/prn")]
public class PrnController(IPrnService prnService, 
    ILogger<PrnController> logger, 
    IObligationCalculatorService obligationCalculatorService, 
    IOptions<PrnObligationCalculationConfig> config, 
    IConfiguration configuration,
    IValidator<SavePrnDetailsRequest> savePrnDetailsRequestValidator) : ControllerBase
{
    private readonly PrnObligationCalculationConfig _config = config.Value;
    private readonly string? logPrefix = string.IsNullOrEmpty(configuration["LogPrefix"]) ? "[EPR.PRN.Backend]" : configuration["LogPrefix"];

    #region Get methods

    [HttpGet("{prnId}")]
    [ProducesResponseType(typeof(PrnDto), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPrn([FromHeader(Name = "X-EPR-ORGANISATION")] Guid orgId, [FromRoute] Guid prnId)
    {
        logger.LogInformation("{Logprefix}: PrnController - GetPrn: Api Route api/v1/prn/{PrnId}", logPrefix, prnId);
        logger.LogInformation("{Logprefix}: PrnController - GetPrn: Get Prn request for user organisation {Organisation} and Prn {PrnId}", logPrefix, orgId, prnId);
        var prn = await prnService.GetPrnForOrganisationById(orgId, prnId);

        if (prn == null)
        {
            logger.LogError("{Logprefix}: PrnController - GetPrn: Prn Not Found", logPrefix);
            return NotFound();
        }

        logger.LogInformation("{Logprefix}: PrnController - GetPrn: Prn returned {Prn}", logPrefix, prn);
        return Ok(prn);
    }

    [HttpGet("search/{page?}/{search?}/{filterBy?}/{sortBy?}")]
    [ProducesResponseType(typeof(PaginatedResponseDto<PrnDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetSearchPrns([FromHeader(Name = "X-EPR-ORGANISATION")] Guid orgId, [FromQuery] PaginatedRequestDto request, int? page = null,
    string? search = null,
    string? filterBy = null,
    string? sortBy = null)
    {
        // Parameters are unused intentionally (kept for backward compatibility).
        // Request is handled through PaginatedRequestDto instead.
        logger.LogInformation("{Logprefix}: PrnController - GetSearchPrns: Api Route api/v1/prn/search/", logPrefix);
        logger.LogInformation("{Logprefix}: PrnController - GetSearchPrns: Search Prns request for user organisation {Organisation} and Search criteria {Searchcriteria}", logPrefix, orgId, JsonConvert.SerializeObject(request));
        if (orgId == Guid.Empty)
        {
            logger.LogInformation("{Logprefix}: PrnController - GetSearchPrns: UnAuthorised Request", logPrefix);
            return Unauthorized();
        }

        var result = await prnService.GetSearchPrnsForOrganisation(orgId, request);
        logger.LogInformation("{Logprefix}: PrnController - GetSearchPrns: Prns returned {Prns}", logPrefix, JsonConvert.SerializeObject(result));

        return Ok(result);
    }

    [HttpGet("organisation")]
    [ProducesResponseType(typeof(List<PrnDto>), 200)]
    public async Task<IActionResult> GetAllPrnByOrganisationId([FromHeader(Name = "X-EPR-ORGANISATION")] Guid orgId)
    {
        logger.LogInformation("{Logprefix}: PrnController - GetAllPrnByOrganisationId: Api Route api/v1/prn/organisation", logPrefix);
        logger.LogInformation("{Logprefix}: PrnController - GetAllPrnByOrganisationId: request for user organisation {Organisation}", logPrefix, orgId);

        var prns = await prnService.GetAllPrnByOrganisationId(orgId);
        logger.LogInformation("{Logprefix}: PrnController - GetAllPrnByOrganisationId: Prns returned {Prns}", logPrefix, JsonConvert.SerializeObject(prns));

        return Ok(prns);
    }

    [HttpGet("ModifiedPrnsbyDate")]
    [ProducesResponseType(typeof(List<PrnUpdateStatus>), 200)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetModifiedPrnsbyDate([FromQuery] ModifiedPrnsbyDateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var prns = await prnService.GetModifiedPrnsbyDate(request.From, request.To);
        if (prns == null || prns.Count == 0)
            return StatusCode(StatusCodes.Status204NoContent);

        return Ok(prns);
    }

    [HttpGet("syncstatuses")]
    [ProducesResponseType(typeof(List<PrnStatusSync>), 200)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSyncStatuses([FromQuery] ModifiedPrnsbyDateRequest request)
    {
        if (!ModelState.IsValid)
        { return BadRequest(ModelState); }

        var statusList = await prnService.GetSyncStatuses(request.From, request.To);
        return statusList == null || statusList.Count == 0
            ? StatusCode(StatusCodes.Status204NoContent)
            : Ok(statusList);
    }

    #endregion Get Methods

    #region Post Methods

    [HttpGet("obligationcalculation/{year}")]
    [ProducesResponseType(typeof(List<ObligationData>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetObligationCalculations([FromHeader(Name = "X-EPR-ORGANISATION")] Guid organisationId, [FromRoute] int year)
    {
        logger.LogInformation("{Logprefix}: PrnController - GetObligationCalculation: Api Route api/v1/prn/obligationcalculations/{Year}", logPrefix, year);
        logger.LogInformation("{Logprefix}: PrnController - GetObligationCalculation: request to get Obligation Calculation for {Year}", logPrefix, year);

        if (year < _config.StartYear || year > _config.EndYear)
        {
            logger.LogError("{Logprefix}: PrnController - GetObligationCalculation: Invalid year provided: {Year}.", logPrefix, year);
            return BadRequest($"Invalid year provided: {year}.");
        }

        var obligationCalculation = await obligationCalculatorService.GetObligationCalculation(organisationId, year);

        if (!obligationCalculation.IsSuccess)
        {
            logger.LogError("{Logprefix}: PrnController - GetObligationCalculation: Get Obligation Calculation Failed - Errors {Errors}", logPrefix, JsonConvert.SerializeObject(obligationCalculation.Errors));
            return StatusCode(500, obligationCalculation.Errors);
        }

        logger.LogInformation("{Logprefix}: PrnController - GetObligationCalculation: Obligation Calculation returned {ObligationCalculation}", logPrefix, JsonConvert.SerializeObject(obligationCalculation));
        return Ok(obligationCalculation.ObligationModel);
    }

    [HttpPost("status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePrnStatus([FromHeader(Name = "X-EPR-ORGANISATION")] Guid orgId, [FromHeader(Name = "X-EPR-USER")] Guid userId, [FromBody] List<PrnUpdateStatusDto> prnUpdates)
    {
        logger.LogInformation("{Logprefix}: PrnController - UpdatePrnStatus: Api Route api/v1/prn/status", logPrefix);
        logger.LogInformation("{Logprefix}: PrnController - UpdatePrnStatus: request for user {User} organisation {Organisation} - Prns {PrnUpdates}", logPrefix, userId, orgId, JsonConvert.SerializeObject(prnUpdates));

        try
        {
            await prnService.UpdateStatus(orgId, userId, prnUpdates);

            logger.LogInformation("{Logprefix}: PrnController - UpdatePrnStatus: Prn status updated successfully for {Prns}", logPrefix, JsonConvert.SerializeObject(prnUpdates));
            return Ok();
        }
        catch (NotFoundException ex)
        {
            logger.LogError(ex, "{Logprefix}: PrnController - UpdatePrnStatus: Recieved not found exception", logPrefix);
            return Problem(ex.Message, null, (int)HttpStatusCode.NotFound);
        }
        catch (ConflictException ex)
        {
            logger.LogError(ex, "{Logprefix}: PrnController - UpdatePrnStatus: Recieved conflict exception", logPrefix);
            return Problem(ex.Message, null, (int)HttpStatusCode.Conflict);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Logprefix}: PrnController - UpdatePrnStatus: Recieved Unhandled exception", logPrefix);
            return Problem("Internal Server Error", null, (int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("organisation/{submitterId}/calculate")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
    public async Task<IActionResult> CalculateAsync(Guid submitterId, [FromBody] List<SubmissionCalculationRequest> request)
    {
        logger.LogInformation("{Logprefix}: PrnController - CalculateAsync: Api Route api/v1/prn/organisation/{SubmitterId}/calculate", logPrefix, submitterId);
        logger.LogInformation("{Logprefix}: PrnController - CalculateAsync: request for user organisation {SubmitterId} submissions {SubmissionsRequest}", logPrefix, submitterId, JsonConvert.SerializeObject(request));

        if (request == null || request.Count == 0)
        {
            logger.LogError("{Logprefix}: PrnController - CalculateAsync: Submission calculation request cannot be null or empty.", logPrefix);
            return BadRequest(new { message = "Submission calculation request cannot be null or empty." });
        }

        if (!ModelState.IsValid)
        {
            logger.LogError("{Logprefix}: PrnController - CalculateAsync: Invalid submission calculation request provided: {SubmissionsRequest}.", logPrefix, JsonConvert.SerializeObject(request));
            return BadRequest(ModelState);
        }

        try
        {
            var calculationResult = await obligationCalculatorService.CalculateAsync(submitterId, request);

            if (!calculationResult.Success)
            {
                logger.LogError("{Logprefix}: PrnController - CalculateAsync: Get Calculation Failed - {Errors}", logPrefix, JsonConvert.SerializeObject(calculationResult));
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Calculation failed due to internal errors." });
            }
            logger.LogInformation("{Logprefix}: PrnController - CalculateAsync: Obligation Calculation returned {CalculationResult}", logPrefix, JsonConvert.SerializeObject(calculationResult.Calculations));

            logger.LogInformation("{Logprefix}: PrnController - CalculateAsync: calling SoftDeleteAndAddObligationCalculationAsync ", logPrefix);

            await obligationCalculatorService.SoftDeleteAndAddObligationCalculationAsync(submitterId, calculationResult.Calculations);

            logger.LogInformation("{Logprefix}: PrnController - CalculateAsync: Obligation Calculation Successful {Calculations}", logPrefix, JsonConvert.SerializeObject(calculationResult.Calculations));

            return Accepted(new { message = "Calculation successful.", data = calculationResult.Calculations });
        }
        catch (TimeoutException ex)
        {
            logger.LogError(ex, "{Logprefix}: PrnController - CalculateAsync: Calculation timed out. - {Message}", logPrefix, ex.Message);
            return StatusCode(StatusCodes.Status504GatewayTimeout, new { message = "Calculation timed out.", details = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Logprefix}: PrnController - CalculateAsync: An error occurred during calculation - {Message}", logPrefix, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred during calculation.", details = ex.Message });
        }
    }

    [HttpPost("prn-details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SaveAsync(SavePrnDetailsRequest request)
    {
        try
        {
            var validationResult = savePrnDetailsRequestValidator.Validate(request);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors);
            }

            await prnService.SavePrnDetails(request);

            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Recieved Unhandled exception");
            return Problem("Internal Server Error", null, (int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("updatesyncstatus")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PeprToNpwdSyncedPrns([FromBody] List<InsertSyncedPrn> syncedPrns)
    {
        try
        {
            await prnService.InsertPeprNpwdSyncPrns(syncedPrns);

            return Ok();
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning(ex, "Recieved not found exception");
            return Problem(ex.Message, null, (int)HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Recieved Unhandled exception");
            return Problem("Internal Server Error", null, (int)HttpStatusCode.InternalServerError);
        }
    }

    #endregion Post Methods
}