using DTO = EPR.Accreditation.API.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using EPR.Accreditation.API.Services.Interfaces;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class MaterialController : ControllerBase
    {
        public readonly IMaterialService _materialService;

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService ?? throw new ArgumentNullException(nameof(materialService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DTO.Material>), 200)]
        public async Task<IActionResult> GetMaterials()
        {
            return Ok(await _materialService.GetMaterialList());
        }
    }
}
