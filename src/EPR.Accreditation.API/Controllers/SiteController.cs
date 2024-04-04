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

        [HttpGet]
        [ProducesResponseType(typeof(DTO.Site), 200)]
        public async Task<IActionResult> GetSite(
            Guid externalId)
        {
            var site = await _accreditationService.GetSite(
                externalId);

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

        [HttpPut]
        public async Task<IActionResult> UpdateSite(
            Guid externalId,
            [FromBody] DTO.Site site)
        {
            await _accreditationService.UpdateSite(
                externalId,
                site);

            return Ok();
        }
    }
}