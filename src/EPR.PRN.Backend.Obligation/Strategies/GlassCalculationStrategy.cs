using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Enums;
using EPR.PRN.Backend.Obligation.Helpers;
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

        public List<ObligationCalculation> Calculate(CalculationRequestDto calculationRequest)
        {
            var targetYear = DateHelper.ExtractYear(calculationRequest.SubmissionCalculationRequest.SubmissionPeriod);
            var calculatedOn = DateTime.UtcNow;
            var (remelt, remainder) = _calculationService.CalculateGlass(
                calculationRequest.RecyclingTargets[targetYear][MaterialType.Glass],
                calculationRequest.RecyclingTargets[targetYear][MaterialType.GlassRemelt],
                calculationRequest.SubmissionCalculationRequest.PackagingMaterialWeight);

            return new List<ObligationCalculation>
            {
                new ObligationCalculation { MaterialName = MaterialType.Glass.ToString(),
                    CalculatedOn = calculatedOn,
                    OrganisationId = calculationRequest.OrganisationId,
                    MaterialObligationValue = remainder,
                    Year = DateTime.UtcNow.Year },
                new ObligationCalculation { MaterialName = MaterialType.GlassRemelt.ToString(),
                    CalculatedOn = calculatedOn,
                    OrganisationId = calculationRequest.OrganisationId,
                    MaterialObligationValue = remelt,
                    Year = DateTime.UtcNow.Year }
            };
        }
    }
}
