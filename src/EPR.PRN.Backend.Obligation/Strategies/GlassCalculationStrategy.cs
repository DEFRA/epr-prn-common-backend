using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.Dto;
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
            targetYear = targetYear < calculationRequest.RecyclingTargets.Keys.Min() ? calculationRequest.RecyclingTargets.Keys.Min() : targetYear;
            var calculatedOn = DateTime.UtcNow;
            var (remelt, remainder) = _calculationService.CalculateGlass(
                calculationRequest.RecyclingTargets[targetYear][MaterialType.Glass],
                calculationRequest.RecyclingTargets[targetYear][MaterialType.GlassRemelt],
                calculationRequest.SubmissionCalculationRequest.PackagingMaterialWeight);

            return
			[
				new ObligationCalculation { MaterialId = calculationRequest.Materials.First(m => m.MaterialName == MaterialType.Glass.ToString()).Id,
                    CalculatedOn = calculatedOn,
                    OrganisationId = calculationRequest.OrganisationId,
                    MaterialObligationValue = remainder,
                    Year = DateTime.UtcNow.Year,
                    Tonnage = calculationRequest.SubmissionCalculationRequest.PackagingMaterialWeight },
                new ObligationCalculation { MaterialId = calculationRequest.Materials.First(m => m.MaterialName == MaterialType.GlassRemelt.ToString()).Id,
                    CalculatedOn = calculatedOn,
                    OrganisationId = calculationRequest.OrganisationId,
                    MaterialObligationValue = remelt,
                    Year = DateTime.UtcNow.Year,
                    Tonnage = calculationRequest.SubmissionCalculationRequest.PackagingMaterialWeight }
            ];
        }
    }
}
