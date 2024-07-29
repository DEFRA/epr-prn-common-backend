namespace EPR.PRN.Backend.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using EPR.PRN.Backend.API.Services.Interfaces;
    using EPR.PRN.Backend.API.Common.DTO;
    using EPR.PRN.Backend.API.Helpers;
    using System.Net;

    [ApiController]
    [Route("/prn")]
    public class PrnController : Controller
    {
        private readonly IPrnService _prnService;

        private readonly ILogger<PrnController> _logger;

        public PrnController(IPrnService prnService, ILogger<PrnController> logger)
        {
            _prnService = prnService ?? throw new ArgumentNullException(nameof(prnService));
            _logger = logger;
        }

        #region Get methods
        [HttpGet("{prnId}")]
        [ProducesResponseType(typeof(PrnDTo), 200)]
        public async Task<IActionResult> GetPrn([FromHeader] Guid organisationId, [FromRoute]Guid prnId)
        {
            var prn = await _prnService.GetPrnForOrganisationById(organisationId, prnId);

            if (prn == null)
                return NotFound();

            return Ok(prn);
        }

        [HttpGet("organisation")]
        [ProducesResponseType(typeof(List<PrnDTo>), 200)]
        public async Task<IActionResult> GetAllPrnByOrganisationId([FromHeader] Guid organisationId)
        {
            var prn = await _prnService.GetAllPrnByOrganisationId(organisationId);

            if (prn.Count == 0)
                return NotFound();

            return Ok(prn);
        }
        #endregion

        #region Post Methods
        [HttpPost("status")]
        public async Task<IActionResult> UpdatePrnStatus([FromHeader] Guid organisationId, [FromBody] List<PrnUpdateStatusDto> prnUpdates)
        {
            try
            {
                await _prnService.UpdateStatus(organisationId, prnUpdates);

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
                return Problem("Internal Server Error", null,(int)HttpStatusCode.InternalServerError);
            }
        }
        #endregion
    }
}