using System.Net.Http.Json;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Enums;
using EPR.PRN.Backend.Obligation.Interfaces;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.Obligation.Services
{
    public class ObligationCalculatorService : IObligationCalculatorService
    {
        private readonly ILogger<ObligationCalculatorService> _logger;
        private readonly IRecyclingTargetDataService _recyclingTargetDataService;
        private readonly IObligationCalculationRepository _obligationCalculationRepository;
        private readonly IPomSubmissionData _pomSubmissionData;
        private readonly IMaterialCalculationStrategyResolver _strategyResolver;

        public ObligationCalculatorService(
            ILogger<ObligationCalculatorService> logger,
            IRecyclingTargetDataService recyclingTargetDataService,
            IObligationCalculationRepository obligationCalculationRepository,
            IPomSubmissionData pomSubmissionData,
            IMaterialCalculationStrategyResolver strategyResolver)
        {
            _logger = logger;
            _recyclingTargetDataService = recyclingTargetDataService;
            _obligationCalculationRepository = obligationCalculationRepository;
            _pomSubmissionData = pomSubmissionData;
            _strategyResolver = strategyResolver;
        }

        public async Task ProcessApprovedPomData(string submissionIdString)
        {
            //Worker Service will call this method peridoically 
            var response = await _pomSubmissionData.GetAggregatedPomData(submissionIdString);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Could not retrieve POM data for Submission Id: {0}.", submissionIdString);
                return;
            }

            var pomData = response.Content.ReadFromJsonAsync<List<PomObligtionDto>>().Result;

            if (pomData == null || pomData.Count == 0)
            {
                _logger.LogError("No POM data returned for Submission Id: {0}.", submissionIdString);
                return;
            }

            var recyclingTargets = await _recyclingTargetDataService.GetRecyclingTargetsAsync();

            var calculations = new List<ObligationCalculation>();

            foreach (var material in pomData)
            {
                Enum.TryParse<MaterialType>(material.PackagingMaterial, out var materialType);
                var strategy = _strategyResolver.Resolve(materialType);

                if (strategy == null)
                {
                    _logger.LogError("Skipping material with unknown type: {MaterialType} for SubmissionId: {submissionIdString}.", materialType, submissionIdString);
                    continue;
                }
                calculations.AddRange(strategy.Calculate(material, materialType, recyclingTargets));
            }

            if (calculations == null || !calculations.Any())
            {
                _logger.LogError("No calculations were saved for SubmissionId: {submissionIdString}.", submissionIdString);
                return;
            }

            await _obligationCalculationRepository.AddObligationCalculation(calculations);
        }

        public async Task<List<ObligationCalculationDto>?> GetObligationCalculationByOrganisationId(int organisationId)
        {
            var result = await _obligationCalculationRepository.GetObligationCalculationByOrganisationId(organisationId);

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
