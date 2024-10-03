namespace EPR.PRN.Backend.API.Controllers
{
    using EPR.PRN.Backend.API.Common.DTO;
    using EPR.PRN.Backend.API.Helpers;
    using EPR.PRN.Backend.API.Services.Interfaces;
    using EPR.PRN.Backend.Obligation.DTO;
    using EPR.PRN.Backend.Obligation.Interfaces;
    using EPR.PRN.Backend.Obligation.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Net;

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/prn")]
    public class PrnController : Controller
    {
        private readonly IPrnService _prnService;
        private readonly ILogger<PrnController> _logger;
        private readonly IObligationCalculatorService _obligationCalculatorService;

        public PrnController(IPrnService prnService, ILogger<PrnController> logger, IObligationCalculatorService obligationCalculatorService)
        {
            _prnService = prnService;
            _logger = logger;
            _obligationCalculatorService = obligationCalculatorService;
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

        [HttpGet("v1/obligationcalculation/{organisationId}")]
        [ProducesResponseType(typeof(List<PrnDataDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetObligationCalculation([FromRoute] int organisationId)
        {
            if (organisationId <= 0)
            {
                return BadRequest($"Invalid Organisation Id : {organisationId}. Organisation Id must be a positive integer.");
            }

            var obligationCalculation = await _obligationCalculatorService.GetObligationCalculationByOrganisationId(organisationId);

            if (obligationCalculation == null)
            {
                return NotFound($"Obligation calculation not found for Organisation Id : {organisationId}");
            }

            return Ok(obligationCalculation);
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

        [HttpPost("organisation/{id}/calculate")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> CalculateAsync(int id, [FromBody] List<SubmissionCalculationRequest> request)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid Organisation ID." });
            }

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
                var calculationResult = await _obligationCalculatorService.CalculateAsync(id, request);

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
}