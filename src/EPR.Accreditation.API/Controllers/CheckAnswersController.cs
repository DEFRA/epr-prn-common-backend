using EPR.Accreditation.API.Common.Dtos;
using EPR.Accreditation.API.Common.Enums;
using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPR.Accreditation.API.Controllers
{
	[Route("/api/[controller]/{id}/")]
	[ApiController]
	public class CheckAnswersController : ControllerBase
	{
		protected readonly ICheckAnswersService _checkAnswersService;

		public CheckAnswersController(ICheckAnswersService checkAnswersService)
		{
			_checkAnswersService = checkAnswersService ?? throw new ArgumentNullException(nameof(checkAnswersService));
		}

		/// <summary>
		/// Handles the GET request to check answers for a specific section.
		/// </summary>
		/// <param name="id">The unique identifier of the entity.</param>
		/// <param name="section">The section of accreditation referred to by the check answers page.</param>
		/// <returns>A view with the CheckAnswersViewModel.</returns>
		[HttpGet("Section/{section}")]
		[ProducesResponseType(typeof(CheckAnswers), 200)]
		public async Task<IActionResult> CheckAnswers(Guid id, CheckAnswersSection section)
		{
			var queryStringParams = Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());
			var checkYourAnswersDto = await _checkAnswersService.GetCheckAnswers(id, section, queryStringParams);

			return checkYourAnswersDto is null 
				? NotFound() 
				: Ok(checkYourAnswersDto);
		}
	}
}
