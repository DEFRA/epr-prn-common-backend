using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.Dto;
using EPR.PRN.Backend.Obligation.Helpers;
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

        public List<ObligationCalculation> Calculate(CalculationRequestDto calculationRequest)
        {
            var targetYear = DateHelper.ExtractYear(calculationRequest.SubmissionCalculationRequest.SubmissionPeriod);
            targetYear = targetYear < calculationRequest.RecyclingTargets.Keys.Min() ? calculationRequest.RecyclingTargets.Keys.Min() : targetYear;
            var calculation = new ObligationCalculation
            {
                MaterialName = calculationRequest.MaterialType.ToString(),
                CalculatedOn = DateTime.UtcNow,
                OrganisationId = calculationRequest.OrganisationId,
                MaterialObligationValue = _calculationService.Calculate(calculationRequest.RecyclingTargets[targetYear][calculationRequest.MaterialType],
                calculationRequest.SubmissionCalculationRequest.PackagingMaterialWeight),
                Year = DateTime.UtcNow.Year,
                Tonnage = calculationRequest.SubmissionCalculationRequest.PackagingMaterialWeight
            };

            return new List<ObligationCalculation> { calculation };
        }
    }
}
