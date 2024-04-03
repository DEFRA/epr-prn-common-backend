using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    [Route("/api/Accreditation/{externalId}/[controller]")]
    public class SiteController : ControllerBase
    {
        protected readonly IAccreditationService _accreditationService;

        public SiteController(
            IAccreditationService accreditationService)
        {
            _accreditationService = accreditationService ?? throw new ArgumentNullException(nameof(accreditationService));
        }

        [HttpGet("{siteExternalId}")]
        [ProducesResponseType(typeof(DTO.Site), 200)]
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
            [FromBody] DTO.Site site)
        {
            var siteExternalId = await _accreditationService.CreateSite(
                externalId,
                site);

            return Ok(siteExternalId);
        }

        [HttpPut("{siteExternalId}")]
        public async Task<IActionResult> UpdateSite(
            Guid externalId,
            Guid siteExternalId,
            [FromBody] DTO.Site site)
        {
            await _accreditationService.UpdateSite(
                externalId,
                siteExternalId,
                site);

            return Ok();
        }
    }
}