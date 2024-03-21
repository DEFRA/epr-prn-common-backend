using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    [Route("/api/Accreditation/{externalId}/[controller]")]
    public class OverseasSiteController : ControllerBase
    {
        protected readonly IAccreditationService _accreditationService;

        public OverseasSiteController(
            IAccreditationService accreditationService)
        {
            _accreditationService = accreditationService ?? throw new ArgumentNullException(nameof(accreditationService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(DTO.OverseasReprocessingSite), 200)]
        public async Task<IActionResult> GetSite(
            Guid externalId,
            Guid siteExternalId)
        {
            var site = await _accreditationService.GetSite(
                externalId,
                siteExternalId);

            if (site == null)
                return NotFound();

            return Ok(site);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> CreateSite(
            Guid externalId,
            DTO.OverseasReprocessingSite overseasSite)
        {
            var siteExternalId = await _accreditationService.CreateOverseasSite(
                externalId,
                overseasSite);

            return Ok(siteExternalId);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSite(DTO.OverseasReprocessingSite oeverseasSite)
        {
            await _accreditationService.UpdateOverseasSite(oeverseasSite);

            return Ok();
        }
    }
}
