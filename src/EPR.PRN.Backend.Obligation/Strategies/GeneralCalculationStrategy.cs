using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.Dto;
using EPR.PRN.Backend.Obligation.Interfaces;

namespace EPR.PRN.Backend.Obligation.Strategies;

public class GeneralCalculationStrategy : IMaterialCalculationStrategy
{
    private readonly List<MaterialType> _generalMaterials;
    private readonly IMaterialCalculationService _calculationService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public GeneralCalculationStrategy(IMaterialCalculationService calculationService, IDateTimeProvider dateTimeProvider)
    {
        _generalMaterials = Enum.GetValues(typeof(MaterialType)).Cast<MaterialType>().Where(m => m != MaterialType.Glass && m != MaterialType.GlassRemelt).ToList();
        _calculationService = calculationService;
        _dateTimeProvider = dateTimeProvider;
    }
    public bool CanHandle(MaterialType materialType) => _generalMaterials.Contains(materialType);

    public List<ObligationCalculation> Calculate(CalculationRequestDto calculationRequest)
    {
        var calculatedOn = _dateTimeProvider.UtcNow;
        var currentYear = _dateTimeProvider.CurrentYear;

        var calculation = new ObligationCalculation
        {
            MaterialId = calculationRequest.Materials.First(m => m.MaterialName == calculationRequest.MaterialType.ToString()).Id,
            CalculatedOn = calculatedOn,
            OrganisationId = calculationRequest.SubmissionCalculationRequest.OrganisationId,
            MaterialObligationValue = _calculationService.Calculate
            (
                calculationRequest.RecyclingTargets[currentYear][calculationRequest.MaterialType],
                calculationRequest.SubmissionCalculationRequest.PackagingMaterialWeight
            ),
            Year = currentYear,
            Tonnage = calculationRequest.SubmissionCalculationRequest.PackagingMaterialWeight,
            SubmitterId = calculationRequest.SubmitterId,
            SubmitterTypeId = calculationRequest.SubmitterTypeId
        };

        return [calculation];
    }
}
