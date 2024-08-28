using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Enums;
using EPR.PRN.Backend.Obligation.Interfaces;

namespace EPR.PRN.Backend.Obligation.Strategies
{
    public class GeneralCalculationStrategy : IMaterialCalculationStrategy
    {
        private readonly List<MaterialType> _generalMaterials;
        private readonly IMaterialCalculationService _calculationService;

        public GeneralCalculationStrategy(IMaterialCalculationService calculationService)
        {
            _generalMaterials = Enum.GetValues(typeof(MaterialType)).Cast<MaterialType>().Where(m => m != MaterialType.Glass && m != MaterialType.GlassRemelt).ToList();
            _calculationService = calculationService;
        }
        public bool CanHandle(MaterialType materialType) => _generalMaterials.Contains(materialType);

        public List<ObligationCalculation> Calculate(PomObligtionDto pomObligation, MaterialType materialType, Dictionary<int, Dictionary<MaterialType, double>> recyclingTargets)
        {
            var calculation = new ObligationCalculation
            {
                MaterialName = materialType.ToString(),
                CalculatedOn = DateTime.UtcNow,
                OrganisationId = pomObligation.OrganisationId,
                MaterialObligationValue = _calculationService.Calculate(recyclingTargets[DateTime.Now.Year][materialType], pomObligation.PackagingMaterialWeight),
                Year = DateTime.UtcNow.Year
            };

            return new List<ObligationCalculation> { calculation };
        }
    }
}
