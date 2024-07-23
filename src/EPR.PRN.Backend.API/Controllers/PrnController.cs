using Microsoft.AspNetCore.Mvc;
using EPR.PRN.Backend.API.Services.Interfaces;
using DTO = EPR.PRN.Backend.API.Common.DTO;

namespace EPR.PRN.Backend.API.Controllers
{
    [ApiController]
    [Route("/prn")]
    public class PrnController : Controller
    {
        protected readonly IPrnService _prnService;

        public PrnController(IPrnService prnService)
        {
            _prnService = prnService ?? throw new ArgumentNullException(nameof(prnService));
        }

        #region Get methods
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DTO.PrnDTo), 200)]
        public async Task<IActionResult> GetPrn(Guid id)
        {
            var prn = await _prnService.GetPrnById(id);

            if (prn == null)
                return NotFound();

            return Ok(prn);
        }

        [HttpGet("organisation")]
        [ProducesResponseType(typeof(List<DTO.PrnDTo>), 200)]
        public async Task<IActionResult> GetAllPrnByOrganisationId(Guid orgId)
        {
            var prn = await _prnService.GetAllPrnByOrganisationId(orgId);

            if (prn == null)
                return NotFound();

            return Ok(prn);
        }
        #endregion

        #region Patch Methods
        [HttpPatch("status/{id}")]
        public async Task<IActionResult> AcceptPrn(Guid id)
        {
            var prn = await _prnService.GetPrnById(id);

            if (prn == null)
                return NotFound();

            await _prnService.AcceptPrn(id);

            return Ok();
        }
        #endregion
    }
}