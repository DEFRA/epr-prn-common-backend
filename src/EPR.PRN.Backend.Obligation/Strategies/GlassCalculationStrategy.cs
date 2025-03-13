using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.Dto;
using EPR.PRN.Backend.Obligation.Interfaces;

namespace EPR.PRN.Backend.Obligation.Strategies;

public class GlassCalculationStrategy : IMaterialCalculationStrategy
{
    private readonly IMaterialCalculationService _calculationService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public GlassCalculationStrategy(IMaterialCalculationService calculationService, IDateTimeProvider dateTimeProvider)
    {
        _calculationService = calculationService;
        _dateTimeProvider = dateTimeProvider;
    }
    public bool CanHandle(MaterialType materialType) => materialType == MaterialType.Glass;

    public List<ObligationCalculation> Calculate(CalculationRequestDto calculationRequest)
    {
        var calculatedOn = _dateTimeProvider.UtcNow;
        var currentYear = _dateTimeProvider.CurrentYear;

        var (remelt, remainder) = _calculationService.CalculateGlass
        (
            calculationRequest.RecyclingTargets[currentYear][MaterialType.Glass],
            calculationRequest.RecyclingTargets[currentYear][MaterialType.GlassRemelt],
            calculationRequest.SubmissionCalculationRequest.PackagingMaterialWeight
        );

        return
        [
            new ObligationCalculation
            {
                MaterialId = calculationRequest.Materials.First(m => m.MaterialName == MaterialType.Glass.ToString()).Id,
                CalculatedOn = calculatedOn,
                OrganisationId = calculationRequest.OrganisationId,
                MaterialObligationValue = remainder,
                Year = currentYear,
                Tonnage = calculationRequest.SubmissionCalculationRequest.PackagingMaterialWeight
            },
            new ObligationCalculation
            {
                MaterialId = calculationRequest.Materials.First(m => m.MaterialName == MaterialType.GlassRemelt.ToString()).Id,
                CalculatedOn = calculatedOn,
                OrganisationId = calculationRequest.OrganisationId,
                MaterialObligationValue = remelt,
                Year = currentYear,
                Tonnage = calculationRequest.SubmissionCalculationRequest.PackagingMaterialWeight
            }
        ];
    }
}
