using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Enums;
using EPR.PRN.Backend.Obligation.Interfaces;

namespace EPR.PRN.Backend.Obligation.Strategies
{
    public class GlassCalculationStrategy : IMaterialCalculationStrategy
    {
        private readonly IMaterialCalculationService _calculationService;

        public GlassCalculationStrategy(IMaterialCalculationService calculationService)
        {
            _calculationService = calculationService;
        }
        public bool CanHandle(MaterialType materialType) => materialType == MaterialType.Glass;

        public List<ObligationCalculation> Calculate(PomObligtionDto pomObligation, MaterialType materialType, Dictionary<int, Dictionary<MaterialType, double>> recyclingTargets)
        {
            var calculatedOn = DateTime.UtcNow;
            var organisationId = pomObligation.OrganisationId;
            var (remelt, remainder) = _calculationService.CalculateGlass(
                recyclingTargets[DateTime.Now.Year][MaterialType.Glass],
                recyclingTargets[DateTime.Now.Year][MaterialType.GlassRemelt],
                pomObligation.PackagingMaterialWeight);

            return new List<ObligationCalculation>
            {
                new ObligationCalculation { MaterialName = MaterialType.Glass.ToString(), CalculatedOn = calculatedOn, OrganisationId = organisationId, MaterialObligationValue = remainder, Year = DateTime.UtcNow.Year },
                new ObligationCalculation { MaterialName = MaterialType.GlassRemelt.ToString(), CalculatedOn = calculatedOn, OrganisationId = organisationId, MaterialObligationValue = remelt, Year = DateTime.UtcNow.Year }
            };
        }
    }
}
