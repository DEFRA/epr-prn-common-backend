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

        public async Task<CalculationResult> CalculateAsync(int organisationId, List<SubmissionCalculationRequest> request)
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

                var material = _materialService.GetMaterialByCode(submission.PackagingMaterial);
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
                    _logger.LogError("Could not find handler for Material Type: {PackagingMaterial} for SubmissionId: {SubmissionId} and OrganisationId: {OrganisationId}.",
                       submission.PackagingMaterial, submission.SubmissionId, organisationId);
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

            if (calculations.Count() == 0)
            {
                _logger.LogError("No calculations for OrganisationId: {organisationId}.", organisationId);
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
            if (calculations == null || calculations.Count() == 0)
            {
                throw new ArgumentException("The calculations list cannot be null or empty.", nameof(calculations));
            }

            await _obligationCalculationRepository.AddObligationCalculation(calculations);
        }

        public async Task<List<PrnDataDto>?> GetObligationCalculationByOrganisationId(int id)
        {
            var result = await _obligationCalculationRepository.GetObligationCalculationByOrganisationId(id);

            return new List<PrnDataDto> { };
        }
    }
}
