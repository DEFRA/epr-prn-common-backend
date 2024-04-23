using EPR.Accreditation.API.Services;
using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    [Route("/api/prn")]
    public class PackageRecyclingNoteController : ControllerBase
    {
        protected readonly IPackageRecyclingNoteService _prnService;

        public PackageRecyclingNoteController(IPackageRecyclingNoteService prnService)
        {
            _prnService = prnService ?? throw new ArgumentNullException(nameof(prnService));
        }

        #region Get methods

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DTO.PackageRecyclingNoteResponse), 200)]
        public async Task<IActionResult> GetPackageRecyclingNote(Guid id)
        {
            var prn = await _prnService.GetPackageRecyclingNote(id);

            if (prn == null)
                return NotFound();

            return Ok(prn);
        }

        [HttpGet("organisation/{id}")]
        [ProducesResponseType(typeof(IEnumerable<Guid>), 200)]
        public async Task<IActionResult> GetOrganisationPrns(Guid id)
        {
            var prn = await _prnService.GetPrnsForOrganisation(id);

            if (prn == null)
                return NotFound();

            return Ok(prn);
        }

        #endregion

        #region Post methods
        [HttpPost()]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> CreatePackageRecyclingNote([FromBody] DTO.PackageRecyclingNoteResponse prn)
        {
            if (prn == null)
                return BadRequest("Package Recycling Note data not suppleid");

            try
            {
                var id = await _prnService.CreatePackageRecyclingNote(prn);
                return Ok(id);
            }
            catch
            {

                return BadRequest();
            }
        }

        [HttpPost("status{id}")]
        public async Task<IActionResult> UpdatePrnStatus(Guid id, DTO.PrnStatusHistoryRequest status)
        {
            try
            {
                await _prnService.UpdatePrnStatus(id, status);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        #endregion

        #region Put methods

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackageRecyclingNote(Guid id, DTO.PrnUpdateRequest prn)
        {
            if (prn == null)
                return BadRequest("Package Recycling Note data not supplied");

            try
            {
                await _prnService.UpdatePrn(id, prn);
                return Ok();
            }
            catch
            {

                return BadRequest();
            }
        }

        #endregion

        #region Delete methods

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackagingRecyclingNote(Guid id)
        {
            try
            {
                await _prnService.DeletePrn(id);
                return Ok();
            }
            catch
            {
                return BadRequest($"Unable to delete {id}");
            }
        }

        #endregion
    }
}
