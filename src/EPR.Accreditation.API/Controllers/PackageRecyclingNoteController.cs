using EPR.Accreditation.API.Services;
using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
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




        #endregion

        #region Post methods
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> CreatePackageRecyclingNote([FromBody] DTO.PackageRecyclingNote accreditation)
        {
            if (accreditation == null)
                return BadRequest("Package Recycling Note data not suppleid");

            var externalId = await _prnService.CreatePackageRecyclingNote(accreditation);

            return Ok(externalId);
        }
        #endregion

        #region Put methods

        #endregion

        #region Delete methods

        #endregion
    }
}
