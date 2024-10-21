using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.Constants;
using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Models;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.Obligation.Services
{
    public class ObligationCalculatorService : IObligationCalculatorService
    {
        private readonly IObligationCalculationRepository _obligationCalculationRepository;
        private readonly IRecyclingTargetDataService _recyclingTargetDataService;
        private readonly IMaterialService _materialService;
        private readonly IMaterialCalculationStrategyResolver _strategyResolver;
        private readonly ILogger<ObligationCalculatorService> _logger;
        private readonly IPrnRepository _prnRepository;
        private readonly IMaterialRepository _materialRepository;

        public ObligationCalculatorService(IObligationCalculationRepository obligationCalculationRepository,
            IRecyclingTargetDataService recyclingTargetDataService,
            IMaterialService materialService,
            IMaterialCalculationStrategyResolver strategyResolver,
            ILogger<ObligationCalculatorService> logger,
            IPrnRepository prnRepository,
            IMaterialRepository materialRepository)
        {
            _obligationCalculationRepository = obligationCalculationRepository;
            _recyclingTargetDataService = recyclingTargetDataService;
            _materialService = materialService;
            _strategyResolver = strategyResolver;
            _logger = logger;
            _prnRepository = prnRepository;
            _materialRepository = materialRepository;
        }

        public async Task<CalculationResult> CalculateAsync(Guid organisationId, List<SubmissionCalculationRequest> request)
        {
            var recyclingTargets = await _recyclingTargetDataService.GetRecyclingTargetsAsync();
            var result = new CalculationResult();
            var calculations = new List<ObligationCalculation>();

            foreach (var submission in request)
            {
                if (string.IsNullOrEmpty(submission.PackagingMaterial))
                {
                    _logger.LogError("Material was null or empty for SubmissionId: {SubmissionId} and OrganisationId: {OrganisationId}.", submission.SubmissionId, organisationId);
                    result.Success = false;
                    continue;
                }

                var material = await _materialService.GetMaterialByCode(submission.PackagingMaterial);
                if (!material.HasValue)
                {
                    _logger.LogError("Material provided was not valid: {PackagingMaterial} for SubmissionId: {SubmissionId} and OrganisationId: {OrganisationId}.",
                        submission.PackagingMaterial, submission.SubmissionId, organisationId);
                    result.Success = false;
                    continue;
                }

                var strategy = _strategyResolver.Resolve(material!.Value);
                if (strategy == null)
                {
                    var error = $"Could not find handler for Material Type: {submission.PackagingMaterial} for SubmissionId: {submission.SubmissionId} and OrganisationId: {organisationId}.";
                    _logger.LogError(error, submission.PackagingMaterial, submission.SubmissionId, organisationId);
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
                _logger.LogError("No calculations for OrganisationId: {OrganisationId}.", organisationId);
                result.Success = false;
            }
            else
            {
                result.Calculations = calculations;
                result.Success = true;
            }

            return result;
        }

        public async Task SaveCalculatedPomDataAsync(List<ObligationCalculation> calculations)
        {
            if (calculations == null || calculations.Count == 0)
            {
                throw new ArgumentException("The calculations list cannot be null or empty.", nameof(calculations));
            }

            await _obligationCalculationRepository.AddObligationCalculation(calculations);
        }

        public async Task<ObligationCalculationResult> GetObligationCalculation(Guid organisationId, int year)
        {
            var materials = await _materialRepository.GetAllMaterials();
            if (!materials.Any())
            {
                _logger.LogError(ObligationConstants.ErrorMessages.NoMaterialsFoundErrorMessage);
                return new ObligationCalculationResult
                {
                    Errors = ObligationConstants.ErrorMessages.NoMaterialsFoundErrorMessage,
                    IsSuccess = false
                };
            }
            var materialsWithRemelt = AddGlassRemelt(materials.ToList());
            var obligationCalculations = await _obligationCalculationRepository.GetObligationCalculation(organisationId, year);
            var prns = await _prnRepository.GetAcceptedAndAwaitingPrnsByYearAsync(organisationId, year);
            var acceptedTonnageForPrns = _prnRepository.GetSumOfTonnageForMaterials(prns, EprnStatus.ACCEPTED.ToString());
            var awaitingAcceptanceForPrns = _prnRepository.GetSumOfTonnageForMaterials(prns, EprnStatus.AWAITINGACCEPTANCE.ToString());
            var awaitingAcceptanceCount = _prnRepository.GetPrnStatusCount(prns, EprnStatus.AWAITINGACCEPTANCE.ToString());
            var materialNames = materialsWithRemelt.Select(material => material.MaterialName);
            var obligationData = new List<ObligationData>();
            foreach (var materialName in materialNames)
            {
                var obligationCalculation = obligationCalculations.Find(x => x.MaterialName == materialName);
                var tonnageAccepted = GetTonnage(materialName, acceptedTonnageForPrns);
                var tonnageAwaitingAcceptance = GetTonnage(materialName, awaitingAcceptanceForPrns);
                var tonnageOutstanding = GetTonnageOutstanding(obligationCalculation?.MaterialObligationValue, tonnageAccepted);
                obligationData.Add(new ObligationData
                {
                    OrganisationId = organisationId,
                    MaterialName = materialName,
                    ObligationToMeet = obligationCalculation?.MaterialObligationValue,
                    TonnageAccepted = tonnageAccepted ?? 0,
                    TonnageAwaitingAcceptance = tonnageAwaitingAcceptance ?? 0,
                    TonnageOutstanding = tonnageOutstanding,
                    Status = GetStatus(obligationCalculation?.MaterialObligationValue, tonnageAccepted)
                });
            }
            var obligationModel = new ObligationModel { ObligationData = obligationData, NumberOfPrnsAwaitingAcceptance = awaitingAcceptanceCount };
            return new ObligationCalculationResult { IsSuccess = true, ObligationModel = obligationModel };
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
