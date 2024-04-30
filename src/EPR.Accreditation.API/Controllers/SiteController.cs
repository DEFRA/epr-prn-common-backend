using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    [Route("/api/Accreditation/{id}/[controller]")]
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
            Guid id)
        {
            var site = await _accreditationService.GetSite(
                id);

            // it is valid to not have a site
            return Ok(site);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> CreateSite(
            Guid id,
            [FromBody] DTO.Site site)
        {
            var siteid = await _accreditationService.CreateSite(
                id,
                site);

            return Ok(siteid);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSite(
            Guid id,
            [FromBody] DTO.Site site)
        {
            await _accreditationService.UpdateSite(
                id,
                site);

            return Ok();
        }
    }
}