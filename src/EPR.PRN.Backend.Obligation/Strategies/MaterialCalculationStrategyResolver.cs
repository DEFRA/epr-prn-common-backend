using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Obligation.Interfaces;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.Obligation.Strategies
{
    public class MaterialCalculationStrategyResolver : IMaterialCalculationStrategyResolver
    {
        private readonly IEnumerable<IMaterialCalculationStrategy> _strategies;
        private readonly ILogger<MaterialCalculationStrategyResolver> _logger;

        public MaterialCalculationStrategyResolver(IEnumerable<IMaterialCalculationStrategy> strategies,
            ILogger<MaterialCalculationStrategyResolver> logger)
        {
            _strategies = strategies;
            _logger = logger;
        }

        public IMaterialCalculationStrategy? Resolve(MaterialType materialType)
        {
            var strategy = _strategies.FirstOrDefault(s => s.CanHandle(materialType));

            if (strategy == null)
            {
                _logger.LogError("No strategy found for material type: {MaterialType}.", materialType);
                return null;
            }

            return strategy;
        }
    }
}
