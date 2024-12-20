using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.Dto;
using EPR.PRN.Backend.Obligation.Helpers;
using EPR.PRN.Backend.Obligation.Interfaces;

namespace EPR.PRN.Backend.Obligation.Strategies
{
    public class GeneralCalculationStrategy(IMaterialCalculationService calculationService) : IMaterialCalculationStrategy
    {
        private readonly List<MaterialType> _generalMaterials = Enum.GetValues(typeof(MaterialType)).Cast<MaterialType>().Where(m => m != MaterialType.Glass && m != MaterialType.GlassRemelt).ToList();

        public bool CanHandle(MaterialType materialType) => _generalMaterials.Contains(materialType);

        public List<ObligationCalculation> Calculate(CalculationRequestDto calculationRequest)
        {
            var targetYear = DateHelper.ExtractYear(calculationRequest.SubmissionCalculationRequest.SubmissionPeriod);
            var calculation = new ObligationCalculation
            {
                MaterialName = calculationRequest.MaterialType.ToString(),
                CalculatedOn = DateTime.UtcNow,
                OrganisationId = calculationRequest.OrganisationId,
                MaterialObligationValue = calculationService.Calculate(calculationRequest.RecyclingTargets[targetYear][calculationRequest.MaterialType],
                calculationRequest.SubmissionCalculationRequest.PackagingMaterialWeight),
                Year = DateTime.UtcNow.Year,
                Tonnage = calculationRequest.SubmissionCalculationRequest.PackagingMaterialWeight
            };

            return [calculation];
        }
    }
}
