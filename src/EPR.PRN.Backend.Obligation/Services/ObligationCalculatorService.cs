using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Dto;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.Constants;
using EPR.PRN.Backend.Obligation.Dto;
using EPR.PRN.Backend.Obligation.Helpers;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Models;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.Obligation.Services
{
	public class ObligationCalculatorService(IObligationCalculationRepository obligationCalculationRepository,
		IRecyclingTargetDataService recyclingTargetDataService,
		IMaterialCalculationStrategyResolver strategyResolver,
		ILogger<ObligationCalculatorService> logger,
		IPrnRepository prnRepository,
		IMaterialRepository materialRepository,
		IObligationCalculationOrganisationSubmitterTypeRepository submitterTypeRepository,
		IDateTimeProvider dateTimeProvider) : IObligationCalculatorService
	{
		public async Task<CalculationResult> CalculateAsync(Guid submitterId, List<SubmissionCalculationRequest> request)
		{
			var recyclingTargets = await recyclingTargetDataService.GetRecyclingTargetsAsync();
			var result = new CalculationResult();
			var calculations = new List<ObligationCalculation>();

			if (!TryParseSubmitterType(request, out var submitterTypeName))
			{
				logger.LogError("SubmitterType provided is not valid: {SubmitterType} for SubmitterId: {SubmitterId}.",
					request[0].SubmitterType, submitterId);
				result.Success = false;
				return result;
			}

			var submitterTypeId = await submitterTypeRepository.GetSubmitterTypeIdByTypeName(submitterTypeName);
			var materials = await materialRepository.GetAllMaterials();

			foreach (var submission in request)
			{
				if (!TryValidateMaterial(submission, submitterId, materials, out var materialType))
					continue;

				if (!TryResolveStrategy(materialType, submission.PackagingMaterial, submitterId, submission.OrganisationId, out var strategy))
					continue;

				var calculationRequest = new CalculationRequestDto
				{
					SubmitterId = submission.SubmitterId,
					SubmissionCalculationRequest = submission,
					MaterialType = materialType,
					Materials = [.. materials],
					SubmitterTypeId = submitterTypeId,
					RecyclingTargets = recyclingTargets
				};

				calculations.AddRange(strategy?.Calculate(calculationRequest)!);
			}

			result.Calculations = calculations;
			result.Success = calculations.Count > 0;

			if (!result.Success)
			{
				logger.LogError("No calculations for SubmitterId: {SubmitterId}.", submitterId);
			}

			return result;
		}

		private static bool TryParseSubmitterType(
			List<SubmissionCalculationRequest> request,
			out ObligationCalculationOrganisationSubmitterTypeName submitterTypeName)
		{
			submitterTypeName = default;

			var submitterType = request.Find(r => !string.IsNullOrEmpty(r.SubmitterType))?.SubmitterType;

			return !string.IsNullOrEmpty(submitterType) &&
				Enum.TryParse(request[0].SubmitterType, true, out submitterTypeName);
		}

		private bool TryValidateMaterial(
			SubmissionCalculationRequest submission,
			Guid submitterId,
			IEnumerable<Material> materials,
			out MaterialType materialType)
		{
			materialType = default;

			if (string.IsNullOrEmpty(submission.PackagingMaterial))
			{
				logger.LogError("Material was null or empty for OrganisationId: {OrganisationId} and SubmitterId: {SubmitterId}.",
					submission.OrganisationId, submitterId);
				return false;
			}

			var materialName = materials.FirstOrDefault(m => m.MaterialCode == submission.PackagingMaterial)?.MaterialName;

			if (string.IsNullOrEmpty(materialName) || !Enum.TryParse(materialName, true, out materialType))
			{
				logger.LogError("Material provided was not valid: {PackagingMaterial} for OrganisationId: {OrganisationId} and SubmitterId: {SubmitterId}.",
					submission.PackagingMaterial, submission.OrganisationId, submitterId);
				return false;
			}

			return true;
		}

		private bool TryResolveStrategy(
			MaterialType materialType,
			string packagingMaterial,
			Guid submitterId,
			Guid organisationId,
			out IMaterialCalculationStrategy? strategy)
		{
			strategy = strategyResolver.Resolve(materialType);
			if (strategy == null)
			{
				logger.LogError("Could not find handler for Material Type: {PackagingMaterial} for OrganisationId: {OrganisationId} and SubmitterId: {SubmitterId}.",
					packagingMaterial, organisationId, submitterId);
				return false;
			}
			return true;
		}

		public async Task SoftDeleteAndAddObligationCalculationAsync(Guid submitterId, List<ObligationCalculation> calculations)
		{
			if (calculations == null || calculations.Count == 0)
			{
				throw new ArgumentException("The calculations list cannot be null or empty.", nameof(calculations));
			}

			await obligationCalculationRepository.UpsertObligationCalculationsForSubmitterYearAsync(submitterId, dateTimeProvider.CurrentYear, calculations);
		}

		public async Task<ObligationCalculationResult> GetObligationCalculation(Guid organisationId, int year)
		{
			var materials = await materialRepository.GetAllMaterials();
			if (!materials.Any())
			{
				logger.LogError(ObligationConstants.ErrorMessages.NoMaterialsFoundErrorMessage);
				return new ObligationCalculationResult
				{
					Errors = ObligationConstants.ErrorMessages.NoMaterialsFoundErrorMessage,
					IsSuccess = false
				};
			}

			var obligationCalculations = await obligationCalculationRepository.GetObligationCalculationBySubmitterIdAndYear(organisationId, year);

			var prns = prnRepository.GetAcceptedAndAwaitingPrnsByYear(organisationId, year);

			var acceptedTonnageForPrns = GetSumOfTonnageForMaterials(prns, EprnStatus.ACCEPTED.ToString());
			var awaitingAcceptanceForPrns = GetSumOfTonnageForMaterials(prns, EprnStatus.AWAITINGACCEPTANCE.ToString());
			var awaitingAcceptanceCount = GetPrnStatusCount(prns, EprnStatus.AWAITINGACCEPTANCE.ToString());
			var recyclingTargets = await recyclingTargetDataService.GetRecyclingTargetsAsync();

			var responseObligationData = new List<ObligationData>();
			var paperFibreObligationData = new List<ObligationData>();

			foreach (var material in materials)
			{
				var npwdMaterialNames = material.PrnMaterialMappings?.Select(npwdm => npwdm.NPWDMaterialName) ?? [];
				var recyclingTarget = GetRecyclingTarget(year, material.MaterialName, recyclingTargets);
				var tonnageAccepted = GetTonnage(npwdMaterialNames, acceptedTonnageForPrns);
				var tonnageAwaitingAcceptance = GetTonnage(npwdMaterialNames, awaitingAcceptanceForPrns);
				var obligationMaterialCalculations = obligationCalculations.FindAll(x => x.MaterialId == material.Id);

				// Segregate Paper and FibreComposite materials to combine later
				if (material.MaterialName.Contains(MaterialType.Paper.ToString()) || material.MaterialName.Contains(MaterialType.FibreComposite.ToString()))
				{
					paperFibreObligationData.Add(GetObligationData(material.MaterialName, organisationId, obligationMaterialCalculations, tonnageAccepted, tonnageAwaitingAcceptance, recyclingTarget));
				}
				else
				{
					responseObligationData.Add(GetObligationData(material.MaterialName, organisationId, obligationMaterialCalculations, tonnageAccepted, tonnageAwaitingAcceptance, recyclingTarget));
				}
			}

			// Combine Paper and Fibre obligation data into a single entry for Paper
			if (paperFibreObligationData.Count > 0)
			{
				responseObligationData.Add(GetPaperFibreCompositeObligationData(paperFibreObligationData));
			}

			// Adjust TonnageOutstanding values including Glass and Glass Re-melt adjustments
			AdjustTonnageOutstandingForObligations(responseObligationData);

			var obligationModel = new ObligationModel { ObligationData = responseObligationData, NumberOfPrnsAwaitingAcceptance = awaitingAcceptanceCount };
			return new ObligationCalculationResult { IsSuccess = true, ObligationModel = obligationModel };
		}

		private static ObligationData GetObligationData(string materialName, Guid organisationId, List<ObligationCalculation> obligationMaterialCalculations, int? tonnageAccepted, int? tonnageAwaitingAcceptance, double? recyclingTarget)
		{
			ObligationData obligationData = new()
			{
				OrganisationId = organisationId,
				MaterialName = materialName,
				TonnageAccepted = tonnageAccepted ?? 0,
				TonnageAwaitingAcceptance = tonnageAwaitingAcceptance ?? 0,
				TonnageOutstanding = (obligationMaterialCalculations.Count > 0 && tonnageAccepted.HasValue) ? obligationMaterialCalculations.Sum(x => x.MaterialObligationValue) - tonnageAccepted : null,
				MaterialTarget = recyclingTarget ?? 0,
				ObligationToMeet = obligationMaterialCalculations.Count > 0 ? obligationMaterialCalculations.Sum(x => x.MaterialObligationValue) : null,
				Tonnage = obligationMaterialCalculations.Sum(x => x.Tonnage)
			};

			obligationData.Status = GetStatus(obligationData.ObligationToMeet, obligationData.TonnageAccepted);
			return obligationData;
		}

		private static ObligationData GetPaperFibreCompositeObligationData(List<ObligationData> paperFibreObligationData)
		{
			ObligationData obligationData = new()
			{
				OrganisationId = paperFibreObligationData[0].OrganisationId,
				MaterialName = MaterialType.Paper.ToString(),
				MaterialTarget = paperFibreObligationData[0].MaterialTarget,
				ObligationToMeet = paperFibreObligationData.Exists(x => x.ObligationToMeet.HasValue) ? (int?)paperFibreObligationData.Sum(x => x.ObligationToMeet ?? 0) : null,
				TonnageAccepted = paperFibreObligationData.Sum(x => x.TonnageAccepted),
				TonnageAwaitingAcceptance = paperFibreObligationData.Sum(x => x.TonnageAwaitingAcceptance),
				TonnageOutstanding = paperFibreObligationData.Exists(x => x.TonnageOutstanding.HasValue) ? (int?)paperFibreObligationData.Sum(x => x.TonnageOutstanding ?? 0) : null,
				Tonnage = paperFibreObligationData.Sum(x => x.Tonnage)
			};

			obligationData.Status = GetStatus(obligationData.ObligationToMeet, obligationData.TonnageAccepted);
			return obligationData;
		}

		private static List<EprnTonnageResultsDto> GetSumOfTonnageForMaterials(IQueryable<EprnResultsDto> prns, string status)
		{
			return [.. prns
						.Where(joined => joined.Status.StatusName == status)
						.GroupBy(joined => new { joined.Eprn.MaterialName, joined.Status.StatusName })
						.Select(g => new EprnTonnageResultsDto
						{
							MaterialName = g.Key.MaterialName,
							StatusName = g.Key.StatusName,
							TotalTonnage = g.Sum(x => x.Eprn.TonnageValue)
						})];
		}

		private static int GetPrnStatusCount(IQueryable<EprnResultsDto> prns, string status)
		{
			return prns.Where(joined => joined.Status.StatusName == status).Count();
		}

		private static double? GetRecyclingTarget(int year, string materialName, Dictionary<int, Dictionary<MaterialType, double>> recyclingTargets)
		{
			var materialType = EnumHelper.ConvertStringToEnum<MaterialType>(materialName);
			if (!materialType.HasValue)
			{
				return null;
			}
			return recyclingTargets[year][materialType.Value];
		}

		private static string GetStatus(int? obligationToMeet, int? tonnageAccepted)
		{
			if (!obligationToMeet.HasValue || !tonnageAccepted.HasValue)
			{
				return ObligationConstants.Statuses.NoDataYet;
			}

			if (tonnageAccepted >= obligationToMeet)
			{
				return ObligationConstants.Statuses.Met;
			}
			return ObligationConstants.Statuses.NotMet;
		}

		private static int? GetTonnage(IEnumerable<string> npwdMaterialNames, List<EprnTonnageResultsDto> acceptedTonnageForMaterials)
		{
			return acceptedTonnageForMaterials
				.Where(x => npwdMaterialNames.Contains(x.MaterialName))
				.Select(x => x.TotalTonnage)
				.FirstOrDefault();
		}

		private static void AdjustTonnageOutstandingForObligations(List<ObligationData> responseObligationData)
		{
			// If Glass Re-melt TonnageOutstanding is negative and Glass TonnageOutstanding is positive, subtract the Re-melt value from Glass.
			ApplySurplusGlassRemeltToRemainingGlass(responseObligationData);

			// Adjust TonnageOutstanding column if TonnageAccepted exceeds ObligationToMeet
			ResetNegativeTonnageOutstandingToZero(responseObligationData);
		}

		private static void ApplySurplusGlassRemeltToRemainingGlass(List<ObligationData> responseObligationData)
		{
			var glassRemeltData = responseObligationData.Find(data => data.MaterialName == MaterialType.GlassRemelt.ToString()
										&& data.TonnageOutstanding.HasValue
										&& data.TonnageOutstanding < 0);

			var glassData = responseObligationData.Find(data => data.MaterialName == MaterialType.Glass.ToString()
										&& data.TonnageOutstanding.HasValue
										&& data.TonnageOutstanding > 0);

			if (glassRemeltData == null || glassData == null)
			{
				return;
			}
			// Apply surplus Glass Re-melt to reduce Glass TonnageOutstanding
			glassData.TonnageOutstanding += glassRemeltData.TonnageOutstanding;

			// Calculate adjusted remaining Glass PRNs after applying surplus Glass Re-melt PRNs
			var adjustedRemainingGlassPRNs = glassData.TonnageAccepted + (-glassRemeltData.TonnageOutstanding);

			// Get updated status for Glass after adjustment of TonnageOutstanding
			glassData.Status = GetStatus(glassData.ObligationToMeet, adjustedRemainingGlassPRNs);
		}

		private static void ResetNegativeTonnageOutstandingToZero(List<ObligationData> responseObligationData)
		{
			foreach (var data in responseObligationData)
			{
				if (data.TonnageOutstanding.HasValue && data.TonnageOutstanding < 0)
				{
					data.TonnageOutstanding = 0;
				}
			}
		}
	}
}
