using EPR.Accreditation.API.Common.Dtos;
using EPR.Accreditation.API.Common.Enums;

namespace EPR.Accreditation.API.Services.Interfaces
{
	public interface ICheckAnswersService
	{

		/// <summary>
		/// Get the CYA page for the specified section
		/// </summary>
		/// <param name="id">Accreditation ID</param>
		/// <param name="section">The name of the CYA page we want</param>
		/// <param name="queryParams">A dictionary of query string parameters to be included in the request URL</param>
		/// <returns>The view result</returns>

		Task<CheckAnswers> GetCheckAnswers(Guid id, CheckAnswersSection section, Dictionary<string, string> queryParams);
	}
}
