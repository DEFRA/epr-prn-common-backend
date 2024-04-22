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
        [ProducesResponseType(typeof(DTO.PackageRecyclingNote), 200)]
        public async Task<IActionResult> GetPackageRecyclingNote(Guid id)
        {
            var prn = await _prnService.GetPackageRecyclingNote(id);

            if (prn == null)
                return NotFound();

            return Ok(prn);
        }

        [HttpGet("prn/organisation/{id}")]
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
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> CreatePackageRecyclingNote([FromBody] DTO.PackageRecyclingNote accreditation)
        {
            if (accreditation == null)
                return BadRequest("Package Recycling Note data not suppleid");

            try
            {
                await _prnService.CreatePackageRecyclingNote(accreditation);
                return Ok();
            }
            catch
            {

                return BadRequest();
            }
            
        }
        #endregion

        #region Put methods

        [HttpPut]
        public async Task<IActionResult> UpdatePrnStatus(DTO.PrnStatusHistory status)
        {
            try
            {
                await _prnService.UpdatePrnStatus(status);
                return Ok();
            }
            catch 
            {
                return BadRequest();
            }
        }

        #endregion

        #region Delete methods

        [HttpDelete]
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
