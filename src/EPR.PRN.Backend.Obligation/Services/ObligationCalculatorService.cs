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
using Microsoft.IdentityModel.Tokens;

namespace EPR.PRN.Backend.Obligation.Services
{
	public class ObligationCalculatorService(IObligationCalculationRepository obligationCalculationRepository,
		IRecyclingTargetDataService recyclingTargetDataService,
		IMaterialCalculationStrategyResolver strategyResolver,
		ILogger<ObligationCalculatorService> logger,
		IPrnRepository prnRepository,
		IMaterialRepository materialRepository,
		IObligationCalculationOrganisationSubmitterTypeRepository submitterTypeRepository) : IObligationCalculatorService
	{
		public async Task<CalculationResult> CalculateAsync(Guid submitterId, List<SubmissionCalculationRequest> request)
		{
			var recyclingTargets = await recyclingTargetDataService.GetRecyclingTargetsAsync();
			var submitterTypeId = 0;
			if (request[0].SubmitterType.IsNullOrEmpty() ||
				!Enum.TryParse(request[0].SubmitterType, true, out ObligationCalculationOrganisationSubmitterTypeName submitterTypeName))
			{
				logger.LogWarning("SubmitterType provided is not valid: {SubmitterType} for SubmitterId: {SubmitterId}.",
					request[0].SubmitterType, submitterId);
			}
			else
			{
				submitterTypeId = await submitterTypeRepository.GetSubmitterTypeIdByTypeName(submitterTypeName);
			}
			var materials = await materialRepository.GetAllMaterials();
			var result = new CalculationResult();
			var calculations = new List<ObligationCalculation>();

			foreach (var submission in request)
			{
				if (string.IsNullOrEmpty(submission.PackagingMaterial))
				{
					logger.LogError("Material was null or empty for OrganisationId: {OrganisationId} and SubmitterId: {SubmitterId}.", submission.OrganisationId, submitterId);
					result.Success = false;
					continue;
				}

				var materialName = materials.FirstOrDefault(m => m.MaterialCode == submission.PackagingMaterial)?.MaterialName;
                if (materialName.IsNullOrEmpty() || !Enum.TryParse(materialName, true, out MaterialType materialType))
                {
                    logger.LogError("Material provided was not valid: {PackagingMaterial} for OrganisationId: {OrganisationId} and SubmitterId: {SubmitterId}.",
                        submission.PackagingMaterial, submission.OrganisationId, submitterId);
                    result.Success = false;
                    continue;
                }

                var strategy = strategyResolver.Resolve(materialType);
                if (strategy == null)
                {
                    logger.LogError("Could not find handler for Material Type: {PackagingMaterial} for OrganisationId: {OrganisationId} and SubmitterId: {SubmitterId}.",
						submission.PackagingMaterial, submission.OrganisationId, submitterId);
                    result.Success = false;
                    continue;
                }

                var calculationRequest = new CalculationRequestDto
				{
					SubmitterId = submission.SubmitterId,
					SubmissionCalculationRequest = submission,
					MaterialType = materialType,
					Materials = [.. materials],
					SubmitterTypeId = submitterTypeId,
					RecyclingTargets = recyclingTargets
				};

				calculations.AddRange(strategy.Calculate(calculationRequest));
			}

			if (calculations.Count == 0)
			{
				logger.LogError("No calculations for SubmitterId: {SubmitterId}.", submitterId);
				result.Success = false;
			}
			else
			{
				result.Calculations = calculations;
				result.Success = true;
			}

			return result;
		}

		public async Task RemoveAndAddObligationCalculationAsync(Guid submitterId, List<ObligationCalculation> calculations)
		{
			await obligationCalculationRepository.RemoveAndAddObligationCalculationBySubmitterIdAsync(submitterId, calculations);
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

				// Segregate Paper and Fibre obligation data from other material types
				if (material.MaterialName.Contains(MaterialType.Paper.ToString()) || material.MaterialName.Contains(MaterialType.FibreComposite.ToString()))
				{
					paperFibreObligationData.Add(GetObligationData(material.MaterialName, organisationId, obligationMaterialCalculations, tonnageAccepted, tonnageAwaitingAcceptance, recyclingTarget));
				}
				else
				{
					responseObligationData.Add(GetObligationData(material.MaterialName, organisationId, obligationMaterialCalculations, tonnageAccepted, tonnageAwaitingAcceptance, recyclingTarget));
				}
			}

			// Add Paper and Fibre obligation data and return as Paper
			if (paperFibreObligationData.Count > 0)
			{
				responseObligationData.Add(GetPaperFibreCompositeObligationData(paperFibreObligationData));
			}

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
	}
}
