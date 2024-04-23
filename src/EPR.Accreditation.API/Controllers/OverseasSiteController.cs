using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    [Route("/api/Accreditation/{id}/[controller]")]
    public class OverseasSiteController : ControllerBase
    {
        protected readonly IAccreditationService _accreditationService;

        public OverseasSiteController(
            IAccreditationService accreditationService)
        {
            _accreditationService = accreditationService ?? throw new ArgumentNullException(nameof(accreditationService));
        }

        [HttpGet("{siteid}")]
        [ProducesResponseType(typeof(DTO.OverseasReprocessingSite), 200)]
        public async Task<IActionResult> GetSite(
            Guid id,
            Guid siteid)
        {
            var site = await _accreditationService.GetOverseasSite(
                id,
                siteid);

            if (site == null)
                return NotFound();

            return Ok(site);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> CreateSite(
            Guid id,
            DTO.OverseasReprocessingSite overseasSite)
        {
            var siteid = await _accreditationService.CreateOverseasSite(
                id,
                overseasSite);

            return Ok(siteid);
        }

        [HttpPut("{siteid}")]
        public async Task<IActionResult> UpdateSite(
            Guid id,
            Guid siteid,
            DTO.OverseasReprocessingSite overseasSite)
        {
            await _accreditationService.UpdateOverseasSite(
                id,
                siteid,
                overseasSite);

            return Ok();
        }
    }
}
