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
        IMaterialService materialService,
        IMaterialCalculationStrategyResolver strategyResolver,
        ILogger<ObligationCalculatorService> logger,
        IPrnRepository prnRepository,
        IMaterialRepository materialRepository) : IObligationCalculatorService
    {
        public async Task<CalculationResult> CalculateAsync(Guid organisationId, List<SubmissionCalculationRequest> request)
        {
            var recyclingTargets = await recyclingTargetDataService.GetRecyclingTargetsAsync();
            var result = new CalculationResult();
            var calculations = new List<ObligationCalculation>();

            foreach (var submission in request)
            {
                if (string.IsNullOrEmpty(submission.PackagingMaterial))
                {
                    logger.LogError("Material was null or empty for SubmissionId: {SubmissionId} and OrganisationId: {OrganisationId}.", submission.SubmissionId, organisationId);
                    result.Success = false;
                    continue;
                }

                var material = await materialService.GetMaterialByCode(submission.PackagingMaterial);
                if (!material.HasValue)
                {
                    logger.LogError("Material provided was not valid: {PackagingMaterial} for SubmissionId: {SubmissionId} and OrganisationId: {OrganisationId}.",
                        submission.PackagingMaterial, submission.SubmissionId, organisationId);
                    result.Success = false;
                    continue;
                }

                var strategy = strategyResolver.Resolve(material!.Value);
                if (strategy == null)
                {
                    var error = $"Could not find handler for Material Type: {submission.PackagingMaterial} for SubmissionId: {submission.SubmissionId} and OrganisationId: {organisationId}.";
                    logger.LogError(error, submission.PackagingMaterial, submission.SubmissionId, organisationId);
                    result.Success = false;
                    continue;
                }

                var calculationRequest = new CalculationRequestDto
                {
                    OrganisationId = organisationId,
                    SubmissionCalculationRequest = submission,
                    MaterialType = material.Value,
                    RecyclingTargets = recyclingTargets
                };

                calculations.AddRange(strategy.Calculate(calculationRequest));
            }

            if (!calculations.Any())
            {
                logger.LogError("No calculations for OrganisationId: {OrganisationId}.", organisationId);
                result.Success = false;
            }
            else
            {
                result.Calculations = calculations;
                result.Success = true;
            }

            return result;
        }

        public async Task UpsertCalculatedPomDataAsync(Guid organisationId, List<ObligationCalculation> calculations)
        {
            await obligationCalculationRepository.UpsertObligationCalculationAsync(organisationId, calculations);
        }

        public async Task<ObligationCalculationResult> GetObligationCalculation(Guid callingOrganisationId, IEnumerable<Guid> organisationIds, int year)
        {
            var materials = await materialRepository.GetVisibleToObligationMaterials();
            if (!materials.Any())
            {
                logger.LogError(ObligationConstants.ErrorMessages.NoMaterialsFoundErrorMessage);
                return new ObligationCalculationResult
                {
                    Errors = ObligationConstants.ErrorMessages.NoMaterialsFoundErrorMessage,
                    IsSuccess = false
                };
            }

            var obligationCalculations = await obligationCalculationRepository.GetObligationCalculation(organisationIds, year);

            var prns = prnRepository.GetAcceptedAndAwaitingPrnsByYear(callingOrganisationId, year);

            var acceptedTonnageForPrns = GetSumOfTonnageForMaterials(prns, EprnStatus.ACCEPTED.ToString());
            var awaitingAcceptanceForPrns = GetSumOfTonnageForMaterials(prns, EprnStatus.AWAITINGACCEPTANCE.ToString());
            var awaitingAcceptanceCount = GetPrnStatusCount(prns, EprnStatus.AWAITINGACCEPTANCE.ToString());
            
            var recyclingTargets = await recyclingTargetDataService.GetRecyclingTargetsAsync();

            var obligationData = new List<ObligationData>();
            var paperFCObligationData = new List<ObligationData>();

            foreach (var material in materials)
            {
                var materialName = material.MaterialName;
				var npwdMaterialNames = material.PrnMaterialMappings.Select(npwdm => npwdm.NPWDMaterialName);
				var recyclingTarget = GetRecyclingTarget(year, materialName, recyclingTargets);
                var tonnageAccepted = GetTonnage(npwdMaterialNames, acceptedTonnageForPrns);
                var tonnageAwaitingAcceptance = GetTonnage(npwdMaterialNames, awaitingAcceptanceForPrns);
                var obligationMaterialCalculations = obligationCalculations.FindAll(x => x.MaterialName == materialName);

                if (materialName.Contains(MaterialType.Paper.ToString()) || materialName.Contains(MaterialType.FibreComposite.ToString()))
                {
                    paperFCObligationData.Add(GetObligationData(materialName, callingOrganisationId, obligationMaterialCalculations, tonnageAccepted, tonnageAwaitingAcceptance, recyclingTarget));
                }
                else
                {
                    obligationData.Add(GetObligationData(materialName, callingOrganisationId, obligationMaterialCalculations, tonnageAccepted, tonnageAwaitingAcceptance, recyclingTarget));
                }
            }

            if (paperFCObligationData.Count > 0)
            {
                obligationData.Add(GetPaperFibreCompositeObligationData(paperFCObligationData));
            }

            var obligationModel = new ObligationModel { ObligationData = obligationData, NumberOfPrnsAwaitingAcceptance = awaitingAcceptanceCount };
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
                ObligationToMeet = obligationMaterialCalculations.Count > 0 ? (int?)obligationMaterialCalculations.Sum(x => x.MaterialObligationValue) : null,
                Tonnage = obligationMaterialCalculations.Sum(x => x.Tonnage)
            };

            obligationData.Status = GetStatus(obligationData.ObligationToMeet, obligationData.TonnageAccepted);
            return obligationData;
        }

        private static ObligationData GetPaperFibreCompositeObligationData(List<ObligationData> pcFiberObligationData)
        {
            ObligationData obligationData = new()
            {
                OrganisationId = pcFiberObligationData[0].OrganisationId,
                MaterialName = MaterialType.Paper.ToString(),
                MaterialTarget = pcFiberObligationData[0].MaterialTarget,
                ObligationToMeet = pcFiberObligationData.Exists(x => x.ObligationToMeet.HasValue) ? (int?)pcFiberObligationData.Sum(x => x.ObligationToMeet ?? 0) : null,
                TonnageAccepted = pcFiberObligationData.Sum(x => x.TonnageAccepted),
                TonnageAwaitingAcceptance = pcFiberObligationData.Sum(x => x.TonnageAwaitingAcceptance),
                TonnageOutstanding = pcFiberObligationData.Exists(x => x.TonnageOutstanding.HasValue) ? (int?)pcFiberObligationData.Sum(x => x.TonnageOutstanding ?? 0) : null,
                Tonnage = pcFiberObligationData.Sum(x => x.Tonnage)
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

        private static string GetStatus(int? materialObligationValue, int? tonnageAccepted)
        {
            if (!materialObligationValue.HasValue || !tonnageAccepted.HasValue)
            {
                return ObligationConstants.Statuses.NoDataYet;
            }

            if (tonnageAccepted >= materialObligationValue)
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
