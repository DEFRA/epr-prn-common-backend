using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
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

        public ObligationCalculatorService(IObligationCalculationRepository obligationCalculationRepository,
            IRecyclingTargetDataService recyclingTargetDataService,
            IMaterialService materialService,
            IMaterialCalculationStrategyResolver strategyResolver,
            ILogger<ObligationCalculatorService> logger)
        {
            _obligationCalculationRepository = obligationCalculationRepository;
            _recyclingTargetDataService = recyclingTargetDataService;
            _materialService = materialService;
            _strategyResolver = strategyResolver;
            _logger = logger;
        }

        public async Task<CalculationResult> CalculatePomDataAsync(int id, List<SubmissionCalculationRequest> submissions)
        {
            var recyclingTargets = await _recyclingTargetDataService.GetRecyclingTargetsAsync();
            var result = new CalculationResult();
            var calculations = new List<ObligationCalculation>();

            foreach (var submission in submissions)
            {
                if (string.IsNullOrEmpty(submission.PackagingMaterial))
                {
                    var error = $"Material was null or empty for SubmissionId: {submission.SubmissionId} and OrganisationId: {id}.";
                    _logger.LogError(error);
                    result.Success = false;
                    continue;
                }

                var material = _materialService.GetMaterialByCode(submission.PackagingMaterial);
                if (!material.HasValue)
                {
                    var error = $"Material provided was not valid: {submission.PackagingMaterial} for SubmissionId: {submission.SubmissionId} and OrganisationId: {id}.";
                    _logger.LogError(error);
                    result.Success = false;
                    continue;
                }

                var strategy = _strategyResolver.Resolve(material!.Value);
                if (strategy == null)
                {
                    var error = $"Could not find handler for Material Type: {submission.PackagingMaterial} for SubmissionId: {submission.SubmissionId} and OrganisationId: {id}.";
                    _logger.LogError(error);
                    result.Success = false;
                    continue;
                }

                var calculationRequest = new CalculationRequestDto
                {
                    OrganisationId = id,
                    SubmissionCalculationRequest = submission,
                    MaterialType = material.Value,
                    RecyclingTargets = recyclingTargets
                };

                calculations.AddRange(strategy.Calculate(calculationRequest));
            }

            if (!calculations.Any())
            {
                var error = $"No calculations for OrganisationId: {id}.";
                _logger.LogError(error);
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
            if (calculations == null || !calculations.Any())
            {
                throw new ArgumentException("The calculations list cannot be null or empty.", nameof(calculations));
            }

            await _obligationCalculationRepository.AddObligationCalculation(calculations);
        }

        public async Task<List<ObligationCalculationDto>?> GetObligationCalculationByOrganisationId(int id)
        {
            var result = await _obligationCalculationRepository.GetObligationCalculationByOrganisationId(id);

            return result?.Select(item => new ObligationCalculationDto
            {
                MaterialName = item.MaterialName,
                MaterialObligationValue = item.MaterialObligationValue,
                OrganisationId = item.OrganisationId,
                Year = item.Year
            }).ToList();
        }
    }
}
