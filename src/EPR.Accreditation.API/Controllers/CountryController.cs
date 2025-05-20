using DTO = EPR.Accreditation.API.Common.Dtos;
using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPR.Accreditation.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CountryController : ControllerBase
    {
        public readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService ?? throw new ArgumentNullException(nameof(countryService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DTO.Country>), 200)]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _countryService.GetCountryList();

            return Ok(countries);
        }
    }
}
