using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Interfaces;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.Obligation.Services
{
    public class ObligationCalculatorService : IObligationCalculatorService
    {
        private readonly ILogger<ObligationCalculatorService> _logger;
        private readonly IRecyclingTargetDataService _recyclingTargetDataService;
        private readonly IObligationCalculationRepository _obligationCalculationRepository;
        private readonly IMaterialCalculationStrategyResolver _strategyResolver;

        public ObligationCalculatorService(
            ILogger<ObligationCalculatorService> logger,
            IRecyclingTargetDataService recyclingTargetDataService,
            IObligationCalculationRepository obligationCalculationRepository,
            IMaterialCalculationStrategyResolver strategyResolver)
        {
            _logger = logger;
            _recyclingTargetDataService = recyclingTargetDataService;
            _obligationCalculationRepository = obligationCalculationRepository;
            _strategyResolver = strategyResolver;
        }

        public async Task ProcessApprovedPomData(string submissionIdString)
        {
            //Pom Data will be provided by the function app request - still work in progress

            var recyclingTargets = await _recyclingTargetDataService.GetRecyclingTargetsAsync();

            var calculations = new List<ObligationCalculation>();

            //foreach (var material in pomData)
            //{
            //    Enum.TryParse<MaterialType>(material.PackagingMaterial, out var materialType);
            //    var strategy = _strategyResolver.Resolve(materialType);

            //    if (strategy == null)
            //    {
            //        _logger.LogError("Skipping material with unknown type: {0} for SubmissionId: {1}.", materialType, submissionIdString);
            //        continue;
            //    }
            //    calculations.AddRange(strategy.Calculate(material, materialType, recyclingTargets));
            //}

            //if (calculations.Count <= 0)
            //{
            //    _logger.LogError("No calculations were saved for SubmissionId: {0}.", submissionIdString);
            //    return;
            //}

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
