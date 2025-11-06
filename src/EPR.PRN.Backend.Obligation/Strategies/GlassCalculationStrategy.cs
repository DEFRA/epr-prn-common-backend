using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.Dto;
using EPR.PRN.Backend.Obligation.Interfaces;

namespace EPR.PRN.Backend.Obligation.Strategies;

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
		var calculatedOn = DateTime.UtcNow;
		var submission = calculationRequest.SubmissionCalculationRequest;
		var complianceYear = calculationRequest.ComplianceYear;

		var recyclingTarget = calculationRequest.RecyclingTargets[complianceYear];
		var (glassRemeltValue, glassRemainderValue) = _calculationService.CalculateGlass(
			recyclingTarget[MaterialType.Glass],
			recyclingTarget[MaterialType.GlassRemelt],
			submission.PackagingMaterialWeight
		);

		var materialsByName = calculationRequest.Materials.ToDictionary(m => m.MaterialName, StringComparer.OrdinalIgnoreCase);
		ObligationCalculation CreateObligation(MaterialType type, int materialObligationValue) => new()
		{
			MaterialId = materialsByName[type.ToString()].Id,
			CalculatedOn = calculatedOn,
			OrganisationId = submission.OrganisationId,
			MaterialObligationValue = materialObligationValue,
			Year = complianceYear,
			Tonnage = submission.PackagingMaterialWeight,
			SubmitterId = calculationRequest.SubmitterId,
			SubmitterTypeId = calculationRequest.SubmitterTypeId
		};

		return
		[
			CreateObligation(MaterialType.Glass, glassRemainderValue),
			CreateObligation(MaterialType.GlassRemelt, glassRemeltValue)
		];
	}
}
