using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Dto;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.Constants;
using EPR.PRN.Backend.Obligation.Dto;
using EPR.PRN.Backend.Obligation.Helpers;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Mappers;
using EPR.PRN.Backend.Obligation.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

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
            var materialsWithRemelt = AddGlassRemelt(materials.ToList());

            var obligationCalculations = await obligationCalculationRepository.GetObligationCalculation(organisationIds, year);
            var prns = prnRepository.GetAcceptedAndAwaitingPrnsByYear(callingOrganisationId, year);

            // make sure material names match materials table
            prns = Mappers.MaterialsMapper.AdjustPrnMaterialNames(prns);

            var acceptedTonnageForPrns = GetSumOfTonnageForMaterials(prns, EprnStatus.ACCEPTED.ToString());
            var awaitingAcceptanceForPrns = GetSumOfTonnageForMaterials(prns, EprnStatus.AWAITINGACCEPTANCE.ToString());
            var awaitingAcceptanceCount = GetPrnStatusCount(prns, EprnStatus.AWAITINGACCEPTANCE.ToString());
            var materialNames = materialsWithRemelt.Select(material => material.MaterialName);
            var obligationData = new List<ObligationData>();
            var paperFCObligationData = new List<ObligationData>();
            var recyclingTargets = await recyclingTargetDataService.GetRecyclingTargetsAsync();

            foreach (var materialName in materialNames)
            {
                var recyclingTarget = GetRecyclingTarget(year, materialName, recyclingTargets);
                var obligationCalculation = obligationCalculations.Find(x => x.MaterialName == materialName);
                var tonnageAccepted = GetTonnage(materialName, acceptedTonnageForPrns);
                var tonnageAwaitingAcceptance = GetTonnage(materialName, awaitingAcceptanceForPrns);
                var tonnageOutstanding = GetTonnageOutstanding(obligationCalculation?.MaterialObligationValue, tonnageAccepted);

                if ((!materialName.Contains(MaterialType.Paper.ToString()) && !materialName.Contains(MaterialType.FibreComposite.ToString())))
                {
                    obligationData.Add(GetObligationData(materialName, callingOrganisationId, obligationCalculation, tonnageAccepted, tonnageAwaitingAcceptance, tonnageOutstanding, recyclingTarget));
                }

                if (materialName.Contains(MaterialType.Paper.ToString()))
                {
                    paperFCObligationData.Add(GetObligationData(materialName, callingOrganisationId, obligationCalculation, tonnageAccepted, tonnageAwaitingAcceptance, tonnageOutstanding, recyclingTarget));
                }

                if (materialName.Contains(MaterialType.FibreComposite.ToString()))
                {
                    paperFCObligationData.Add(GetObligationData(materialName, callingOrganisationId, obligationCalculation, tonnageAccepted, tonnageAwaitingAcceptance, tonnageOutstanding, recyclingTarget));
                }
            }

            if (paperFCObligationData.Count > 0)
            {
                obligationData.Add(GetPaperFibreCompositeObligationData(paperFCObligationData));
            }

            var obligationModel = new ObligationModel { ObligationData = obligationData, NumberOfPrnsAwaitingAcceptance = awaitingAcceptanceCount };
            return new ObligationCalculationResult { IsSuccess = true, ObligationModel = obligationModel };
        }

        private static ObligationData GetObligationData(string materialName, Guid organisationId, ObligationCalculation? obligationCalculation, int? tonnageAccepted, int? tonnageAwaitingAcceptance, int? tonnageOutstanding, double? recyclingTarget)
        {
            return new ObligationData
            {
                OrganisationId = organisationId,
                MaterialName = materialName,
                ObligationToMeet = obligationCalculation?.MaterialObligationValue ?? 0,
                TonnageAccepted = tonnageAccepted ?? 0,
                TonnageAwaitingAcceptance = tonnageAwaitingAcceptance ?? 0,
                TonnageOutstanding = tonnageOutstanding,
                Status = GetStatus(obligationCalculation?.MaterialObligationValue, tonnageAccepted),
                Tonnage = obligationCalculation?.Tonnage ?? 0,
                MaterialTarget = recyclingTarget ?? 0
            };
        }

        private static ObligationData GetPaperFibreCompositeObligationData(List<ObligationData> pcFiberObligationData)
        {
            var pcfcObligationData = pcFiberObligationData
                    .GroupBy(joined => joined.OrganisationId )
                    .Select(static g => new ObligationData
                    {
                        OrganisationId = g.Key,
                        MaterialName = MaterialType.Paper.ToString(),
                        ObligationToMeet = g.Sum(ob => ob.ObligationToMeet),
                        TonnageAccepted = g.Sum(ta => ta.TonnageAccepted),
                        TonnageAwaitingAcceptance = g.Sum(a => a.TonnageAwaitingAcceptance),
                        TonnageOutstanding = g.Sum(to => to.TonnageOutstanding),
                        Tonnage = g.Sum(t => t.Tonnage),
                        MaterialTarget = g.Max(o => o.MaterialTarget)
                    }).ToList()[0];

            pcfcObligationData.Status = GetStatus(pcfcObligationData.ObligationToMeet, pcfcObligationData.TonnageAccepted);
            return pcfcObligationData;
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

        private static double? GetRecyclingTarget(int year, string? materialName, Dictionary<int, Dictionary<MaterialType, double>> recyclingTargets)
        {
            if (string.IsNullOrWhiteSpace(materialName))
            {
                return null;
            }
            var materialType = EnumHelper.ConvertStringToEnum<MaterialType>(materialName);
            if (!materialType.HasValue)
            {
                return null;
            }
            return recyclingTargets[year][materialType.Value];
        }

        private static List<Material> AddGlassRemelt(List<Material> materials)
        {
            materials.Add(new Material { MaterialCode = "GR", MaterialName = "GlassRemelt" });
            return materials;
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

        private static int? GetTonnageOutstanding(int? materialObligationValue, int? tonnageAccepted)
        {
            if (!materialObligationValue.HasValue || !tonnageAccepted.HasValue)
            {
                return null;
            }

            return materialObligationValue - tonnageAccepted;
        }

        private static int? GetTonnage(string materialName, List<EprnTonnageResultsDto> acceptedTonnageForMaterials)
        {
            return acceptedTonnageForMaterials
                .Where(x => x.MaterialName == materialName)
                .Select(x => x.TotalTonnage)
                .FirstOrDefault();
        }
    }
}
