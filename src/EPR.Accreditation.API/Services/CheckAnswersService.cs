using EPR.Accreditation.API.Common.Data.DataModels;
using EPR.Accreditation.API.Common.Dtos;
using EPR.Accreditation.API.Common.Enums;
using EPR.Accreditation.API.Helpers;
using EPR.Accreditation.API.Repositories;
using EPR.Accreditation.API.Repositories.Interfaces;
using EPR.Accreditation.API.Services.Interfaces;

namespace EPR.Accreditation.API.Services
{
	public class CheckAnswersService : ICheckAnswersService
	{
		protected readonly IRepository _repository;
		protected readonly IAccreditationService _accreditationService;

		public CheckAnswersService(IRepository repository, IAccreditationService accreditationService)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_accreditationService = accreditationService ?? throw new ArgumentNullException(nameof(accreditationService));
		}

		public CheckAnswersService(IAccreditationService accreditationService)
		{
			_accreditationService = accreditationService;
		}

		public async Task<CheckAnswers> GetCheckAnswers(Guid id, CheckAnswersSection section, Dictionary<string, string> queryParams)
		{
			
			var checkAnswersDto = new CheckAnswers();			
			var accreditation = await _accreditationService.GetAccreditation(id);
			var materialId = Guid.Parse(queryParams["materialId"]);

			switch (section)
			{
				case CheckAnswersSection.AboutMaterialReprocessorActuals:

					if (accreditation.OperatorTypeId != OperatorType.Reprocessor)
					{
						throw new ArgumentException("Can only be for a reprocessor");
					}

					return BuildCyaAboutMaterialReprocessorActuals(
						id,
						materialId,
						await _accreditationService.GetSite(id),
						await _accreditationService.GetMaterial(id, materialId));

				case CheckAnswersSection.AboutMaterialExporter:
					if (accreditation.OperatorTypeId != Common.Enums.OperatorType.Exporter)
					{
						throw new ArgumentException("Can only be for a exporter");
					}

					return BuildCyaAboutMaterialExporter(
						id,
						materialId,
						await _accreditationService.GetMaterial(id, materialId));
			}

			return checkAnswersDto;
		}
		/// <summary>
		/// Retrieves the data for the AboutMaterialReprocessorActuals enum
		/// </summary>
		/// <param name="id">Accreditation ID</param>
		/// <param name="materialId">Site Material ID</param>
		/// <param name="site">The reprocessor site object</param>
		/// <param name="accreditationMaterial">The material for the reprocessing site</param>
		/// <returns>CheckAnswers DTO with the data</returns>
		private CheckAnswers BuildCyaAboutMaterialReprocessorActuals(
			Guid id,
			Guid materialId,
			Common.Dtos.Site site,
			Common.Dtos.AccreditationMaterial accreditationMaterial)
		{
			var siteAddress = site.Address;
			
			var detailsSection = BuildSectionWithRows(
				id,
				materialId,
				"Details",
				new List<string> { "Material", "Uk source of the waste", "Annual total processing capacity", "Average weekly processing capacity" },
				new List<string> { accreditationMaterial.Material.English, accreditationMaterial.WasteSource, accreditationMaterial.AnnualCapacity.ToString(), accreditationMaterial.WeeklyCapacity.ToString() },
				new List<string> { "Material", "WasteSource", "ProcessingCapacity", "ProcessingCapacity" });

			var totalWasteInputsLastCalendarYear = 0;
			if (accreditationMaterial?.MaterialReprocessorDetails != null)
			{
				totalWasteInputsLastCalendarYear = (int)((accreditationMaterial.MaterialReprocessorDetails.UkPackagingWaste ?? 0)
				                                         + (accreditationMaterial.MaterialReprocessorDetails.NonUkPackagingWaste ?? 0)
				                                         + (accreditationMaterial.MaterialReprocessorDetails.NonPackagingWaste ?? 0));
			}

			var wasteInputsLastYearSection = BuildSectionWithRows(
				id,
				materialId,
				"Waste inputs for last calendar year",
				new List<string> { "Uk packaging waste", "Non-UK packaging waste", "Non-packaging waste", "Total" },
				new List<string>
				{
					accreditationMaterial.MaterialReprocessorDetails.UkPackagingWaste.ToString(),
					accreditationMaterial.MaterialReprocessorDetails.NonUkPackagingWaste.ToString(),
					accreditationMaterial.MaterialReprocessorDetails.NonPackagingWaste.ToString(),
					totalWasteInputsLastCalendarYear.ToString()
				},
				
				new List<string> { "MaterialWasteInputs", "MaterialWasteInputs", "MaterialWasteInputs", string.Empty });

			var materialsNotProcessedOnSite = accreditationMaterial.MaterialReprocessorDetails.MaterialsNotProcessedOnSite;
			var contaminents = accreditationMaterial.MaterialReprocessorDetails.Contaminents;
			var processLoss = accreditationMaterial.MaterialReprocessorDetails.ProcessLoss;
			var totalWasteOutputsLastCalendarYear = materialsNotProcessedOnSite + contaminents + processLoss;
			
			var wasteOutputsLastYearSection = BuildSectionWithRows(
				id,
				materialId,
				"Non-waste inputs for last calendar year",
				new List<string> { "Material not processed on site", "Contaminants", "Process loss", "Total" },
				new List<string> { materialsNotProcessedOnSite.ToString(), contaminents.ToString(), processLoss.ToString(), totalWasteOutputsLastCalendarYear.ToString() },
				new List<string> { "MaterialOutputs", "MaterialOutputs", "MaterialOutputs", string.Empty });
			
			var listOfSections = new List<CheckAnswersSectionDto>
			{
				detailsSection,
				wasteInputsLastYearSection,
				wasteOutputsLastYearSection
				
			};
			
			return new CheckAnswers
			{
				Completed = IsComplete(listOfSections),
				SiteAddress = siteAddress,
				Sections = listOfSections
			};
		}

		private CheckAnswersSectionDto BuildSectionWithRows(
			Guid id,
			Guid materialId,
			string sectionTitle,
			IEnumerable<string> rowTitles,
			IReadOnlyList<string> rowValues,
			IReadOnlyList<string> actionNames)
		{
			var rows = rowTitles.Select((title, i) =>
				BuildRow(id, materialId, title, rowValues[i].ToListSingle(), actionNames[i])).ToList();
			
			return BuildSection(sectionTitle, rows);
		}



		/// <summary>
		/// Retrieves the data for the AboutMaterialExporter enum
		/// </summary>
		/// <param name="id">Accreditation ID</param>
		/// <param name="materialId">Overseas site material ID</param>
		/// <param name="accreditationMaterial">The overseas site material</param>
		/// <returns>CheckAnswers DTO with the data</returns>
		private CheckAnswers BuildCyaAboutMaterialExporter(Guid accreditationId, Guid materialId, Common.Dtos.AccreditationMaterial accreditationMaterial)
		{
			var materialName = accreditationMaterial.Material.English;
			var wasteSource = accreditationMaterial.WasteSource;
			var commodityCodes = accreditationMaterial.WasteCodes.Select(x => x.Code).ToList();
			var signatories = GetSignatories();
			var detailsSectionRows = BuildDetailsSectionRows(accreditationId, materialId, materialName, wasteSource, commodityCodes, signatories);
			var detailsSection = BuildSection("Details", detailsSectionRows);
			var sections = new List<CheckAnswersSectionDto> { detailsSection };
			return new CheckAnswers
			{
				Completed = IsComplete(sections),
				SiteAddress = new Common.Dtos.Address(),
				Sections = sections
			};
		}


		private List<string> GetSignatories()
		{
			return new List<string>
			{
				"Andrew Shey, Management Accountant",
				"Gary Law, Strategic Buyer",
				"Scott McAllister, PRN signatory",
				"Shehzad Ismail, Software Developer"
				
			};
		}
		
		private List<CheckAnswersSectionRow> BuildDetailsSectionRows(Guid accreditationId, Guid materialId, string materialName, string wasteSource, List<string> commodityCodes, List<string> signatories)
		{
			return new List<CheckAnswersSectionRow>
			{
				BuildRow(accreditationId, materialId, "Material", new List<string> { materialName }, "Material"),
				BuildRow(accreditationId, materialId, "Uk source of the waste", new List<string> { wasteSource }, "WasteSource"),
				BuildRow(accreditationId, materialId, "Commodity codes", commodityCodes, "CommodityCodes"),
				BuildRow(accreditationId, materialId, "People who have authority to issue PERNs", signatories, "Authority")
			};
		}
		

		/// <summary>
		/// Builds a row for the CYA page within a section
		/// </summary>
		/// <param name="id">Accreditation ID</param>
		/// <param name="materialId">Site material ID</param>
		/// <param name="titleKey">Key of the row title e.g Commodity codes</param>
		/// <param name="value">The value of the answer</param>
		/// <param name="actionName">The name of the action that the link needs to point to</param>
		/// <returns>CheckAnswersSectionRow DTO</returns>
		private static CheckAnswersSectionRow BuildRow(
			Guid id,
			Guid? materialId,
			string titleKey,
			List<string> value,
			string actionName)
		{
			var changeLink = $"/Accreditation/{id}/Site/Material/{materialId}/{actionName}?rtap=y";

			return new CheckAnswersSectionRow
			{
				TitleKey = titleKey,
				Value = value,
				ChangeLink = changeLink
			};
		}

		/// <summary>
		/// Builds a section within a CYA page
		/// </summary>
		/// <param name="sectionTitle">The title of the section e.g. Waste inputs for last calendar year</param>
		/// <param name="rows">The row DTOs for the section</param>
		/// <returns>CheckAnswersSectionDto</returns>
		private static CheckAnswersSectionDto BuildSection(
			string sectionTitle,
			List<CheckAnswersSectionRow> rows)
		{
			var isCompletedSection = IsComplete(rows);

			return new CheckAnswersSectionDto
			{
				Title = sectionTitle,
				Completed = isCompletedSection,
				SectionRows = rows
			};
		}

		/// <summary>
		/// Checks if each question has been answered
		/// </summary>
		/// <param name="rows">The list of rows for a given section</param>
		/// <returns>True or False</returns>
		private static bool IsComplete(List<CheckAnswersSectionRow> rows)
		{
			foreach (var row in rows)
			{
				if (row.Value != null)
				{
					continue;
				}
				else
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Checks if each section has been completed
		/// </summary>
		/// <param name="sections">The list of sections for the CYA page</param>
		/// <returns>True or False</returns>
		private static bool IsComplete(List<CheckAnswersSectionDto> sections)
		{
			foreach (var section in sections)
			{
				if (section.Completed)
				{
					continue;
				}
				else
				{
					return false;
				}
			}
			return true;
		}
	}
}
