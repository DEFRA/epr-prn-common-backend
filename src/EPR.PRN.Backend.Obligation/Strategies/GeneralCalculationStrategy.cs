using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.Dto;
using EPR.PRN.Backend.Obligation.Interfaces;

namespace EPR.PRN.Backend.Obligation.Strategies;

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
		var calculatedOn = DateTime.UtcNow;
		var submission = calculationRequest.SubmissionCalculationRequest;
		var complianceYear = calculationRequest.ComplianceYear;

		var recyclingTarget = calculationRequest.RecyclingTargets[complianceYear];
		var materialId = calculationRequest.Materials
		.First(m => m.MaterialName == calculationRequest.MaterialType.ToString())
		.Id;

		var materialObligationValue = _calculationService.Calculate(
			recyclingTarget[calculationRequest.MaterialType],
			submission.PackagingMaterialWeight
		);

		var calculation = new ObligationCalculation
		{
			MaterialId = materialId,
			CalculatedOn = calculatedOn,
			OrganisationId = submission.OrganisationId,
			MaterialObligationValue = materialObligationValue,
			Year = complianceYear,
			Tonnage = submission.PackagingMaterialWeight,
			SubmitterId = calculationRequest.SubmitterId,
			SubmitterTypeId = calculationRequest.SubmitterTypeId
		};

        return [calculation];
    }
}
