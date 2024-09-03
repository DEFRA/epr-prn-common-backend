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
    [Route("/prn")]
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

        [HttpGet("organisation")]
        [ProducesResponseType(typeof(List<PrnDto>), 200)]
        public async Task<IActionResult> GetAllPrnByOrganisationId([FromHeader(Name = "X-EPR-ORGANISATION")] Guid orgId)
        {
            var prn = await _prnService.GetAllPrnByOrganisationId(orgId);

            if (prn.Count == 0)
                return NotFound();

            return Ok(prn);
        }

        [HttpGet("obligationcalculation/{organisationId}")]
        [ProducesResponseType(typeof(List<ObligationCalculationDto>), 200)]
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

        [HttpPost("submissions/{id}/calculate")]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CalculateAsync(Guid id, [FromBody] SubmissionCalculationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await Task.Run(() => _obligationCalculatorService.ProcessApprovedPomData(id, request));

            return Accepted();
        }

        #endregion
    }
}