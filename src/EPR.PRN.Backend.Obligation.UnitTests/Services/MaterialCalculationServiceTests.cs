using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.Obligation.Services;
using FluentAssertions;

namespace EPR.PRN.Backend.Obligation.UnitTests.Services;

[ExcludeFromCodeCoverage]
[TestClass]
public class MaterialCalculationServiceTests
{
	private MaterialCalculationService _service;

	[TestInitialize]
	public void TestInitialize()
	{
		_service = new MaterialCalculationService();
	}

	[TestMethod]
	[DynamicData(nameof(CasesForMaterialsExceptGlass), DynamicDataSourceType.Method)]
	public void CalculateObligation_WhenMaterialIsOtherThanGlass_ThenObligationIsReturned(
		double target, int tonnage, int expected, int? numberOfDaysObligated, int complianceYear)
	{
		var result = _service.Calculate(target, tonnage, numberOfDaysObligated, complianceYear);

		result.Should().Be(expected, "the expected target should match the calculated obligation");
	}

	[TestMethod]
	[DynamicData(nameof(CasesForGlassMaterial), DynamicDataSourceType.Method)]
	public void CalculateObligation_WhenMaterialIsGlass_ThenObligationIsReturned(
		double target, double remeltTarget, int tonnage, int expectedRemelt, int expectedRemainder, int? numberOfDaysObligated, int complianceYear)
	{
		var (remeltResult, remainderResult) = _service.CalculateGlass(target, remeltTarget, tonnage, numberOfDaysObligated, complianceYear);

		remeltResult.Should().Be(expectedRemelt, "the expected remelt target should match the calculated obligation");
		remainderResult.Should().Be(expectedRemainder, "the expected remainder should match the calculated remainder");
	}

	public static IEnumerable<object[]> CasesForMaterialsExceptGlass()
	{
		// target, tonnage, expected, numberOfDaysObligated, complianceYear
		yield return new object[] { 0.61, 100, 61, null, 2026 };
		yield return new object[] { 0.55, 12160, 6688, null, 2026 };
		yield return new object[] { 0.55, 860, 473, null, 2023 };
		yield return new object[] { 0.88, 1000, 880, null, 2024 };
		yield return new object[] { 0.55, 10000, 5500, null, 2025 };
		yield return new object[] { 0.75, 4000, 3000, null, 2024 };
		yield return new object[] { 0.33, 1000, 330, null, 2024 };
		yield return new object[] { 0.99, 5000, 4950, null, 2023 };
		yield return new object[] { 0.61, 2000, 1220, null, 2026 };
		yield return new object[] { 0.88, 501, 441, null, 2025 };
		yield return new object[] { 0.77, 700, 539, null, 2023 };
		yield return new object[] { 0.91, 600, 546, null, 2026 };
		yield return new object[] { 0.05, 10001, 501, null, 2025 };
		yield return new object[] { 0.45, 1001, 451, null, 2024 };
		yield return new object[] { 0.55, 2000, 1100, null, 2024 };

		// Partial year cases (days specified)
		yield return new object[] { 0.61, 120, 37, 180, 2024 };
		yield return new object[] { 0.6, 128, 43, 200, 2023 };
		yield return new object[] { 0.55, 740, 335, 300, 2025 };
		yield return new object[] { 0.55, 340, 77, 150, 2024 };
		yield return new object[] { 0.55, 420, 114, 180, 2026 };
		yield return new object[] { 0.25, 8000, 1096, 200, 2026 };
		yield return new object[] { 0.01, 50000, 500, 366, 2024 };
		yield return new object[] { 0.40, 1500, 165, 100, 2026 };
		yield return new object[] { 0.55, 1001, 302, 200, 2024 };
		yield return new object[] { 0.22, 1001, 61, 100, 2026 };
		yield return new object[] { 0.61, 1001, 503, 300, 2026 };
		yield return new object[] { 0.15, 501, 38, 180, 2024 };
		yield return new object[] { 0.30, 1700, 252, 180, 2026 };
		yield return new object[] { 0.55, 20, 1, 20, 2023 };
		yield return new object[] { 0.55, 40, 2, 30, 2025 };
		yield return new object[] { 0.55, 400, 109, 180, 2026 };
		yield return new object[] { 0.55, 1, 1, 1, 2026 };
		yield return new object[] { 0.55, 21, 1, 21, 2024 };
		yield return new object[] { 0.55, 101, 16, 101, 2025 };
		yield return new object[] { 0.55, 501, 379, 501, 2026 };
		yield return new object[] { 0.45, 883, 268, 245, 2025 };
	}

	public static IEnumerable<object[]> CasesForGlassMaterial()
	{
		// target, remeltTarget, tonnage, expectedRemelt, expectedRemainder, numberOfDaysObligated, complianceYear
		yield return new object[] { 0.74, 0.75, 25, 14, 5, null, 2026 };
		yield return new object[] { 0.7, 0.7, 123, 61, 26, null, 2026 };
		yield return new object[] { 0.74, 0.75, 1051, 584, 194, null, 2024 };
		yield return new object[] { 0.74, 0.74, 456, 250, 88, null, 2025 };
		yield return new object[] { 0.75, 0.75, 5000, 2813, 937, null, 2024 };
		yield return new object[] { 0.6, 0.61, 250, 92, 58, null, 2025 };

		// Partial year cases (days specified)
		yield return new object[] { 0.75, 0.75, 100, 29, 9, 180, 2024 };
		yield return new object[] { 0.74, 0.75, 13005, 3956, 1319, 200, 2023 };
		yield return new object[] { 0.74, 0.75, 4522, 1238, 413, 180, 2026 };
		yield return new object[] { 0.74, 0.74, 101, 16, 6, 100, 2025 };
		yield return new object[] { 0.75, 0.75, 25, 2, 1, 30, 2024 };
		yield return new object[] { 0.6, 0.61, 10, 1, 1, 20, 2025 };
		yield return new object[] { 0.55, 0.55, 300, 45, 37, 180, 2023 };
		yield return new object[] { 0.74, 0.75, 534, 147, 49, 180, 2026 };
	}
}
