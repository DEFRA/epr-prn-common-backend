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
	public void CalculateObligation_WhenMaterialIsOtherThanGlass_ThenObligationIsReturned(double target, int tonnage, int expected)
	{
		var result = _service.Calculate(target, tonnage);

		result.Should().Be(expected, "the expected target should match the calculated obligation");
	}

	[TestMethod]
	[DynamicData(nameof(CasesForGlassMaterial), DynamicDataSourceType.Method)]
	public void CalculateObligation_WhenMaterialIsGlass_ThenObligationIsReturned(double target, double remeltTarget, int tonnage, int expectedRemelt, int expectedRemainder)
	{
		var (remeltResult, remainderResult) = _service.CalculateGlass(target, remeltTarget, tonnage);

		remeltResult.Should().Be(expectedRemelt, "the expected remelt target should match the calculated obligation");
		remainderResult.Should().Be(expectedRemainder, "the expected remainder should match the calculated remainder");
	}

	public static IEnumerable<object[]> CasesForMaterialsExceptGlass()
	{
		yield return new object[] { 0.61, 100, 61 };
		yield return new object[] { 0.61, 120, 74 };
		yield return new object[] { 0.6, 128, 77 };
		yield return new object[] { 0.55, 12160, 6688 };
		yield return new object[] { 0.55, 740, 407 };
		yield return new object[] { 0.55, 340, 187 };
		yield return new object[] { 0.55, 860, 473 };
		yield return new object[] { 0.55, 420, 231 };
		yield return new object[] { 0.88, 1000, 880 };
		yield return new object[] { 0.55, 10000, 5500 };
		yield return new object[] { 0.25, 8000, 2000 };
		yield return new object[] { 0.75, 4000, 3000 };
		yield return new object[] { 0.01, 50000, 500 };
		yield return new object[] { 0.10, 6000, 600 };
		yield return new object[] { 0.40, 1500, 600 };
		yield return new object[] { 0.33, 1000, 330 };
		yield return new object[] { 0.99, 5000, 4950 };
		yield return new object[] { 0.61, 2000, 1220 };
		yield return new object[] { 0.55, 1001, 551 };
		yield return new object[] { 0.88, 501, 441 };
		yield return new object[] { 0.22, 1001, 221 };
		yield return new object[] { 0.77, 700, 539 };
		yield return new object[] { 0.61, 1001, 611 };
		yield return new object[] { 0.15, 501, 76 };
		yield return new object[] { 0.91, 600, 546 };
		yield return new object[] { 0.05, 10001, 501 };
		yield return new object[] { 0.45, 1001, 451 };
		yield return new object[] { 0.30, 1700, 510 };
		yield return new object[] { 0.55, 20, 11 };
		yield return new object[] { 0.55, 40, 22 };
		yield return new object[] { 0.55, 400, 220 };
		yield return new object[] { 0.55, 2000, 1100 };
		yield return new object[] { 0.55, 1, 1 };
		yield return new object[] { 0.55, 21, 12 };
		yield return new object[] { 0.55, 101, 56 };
		yield return new object[] { 0.55, 501, 276 };
		yield return new object[] { 0.55, 20000, 11000 };
		yield return new object[] { 0.75, 40000, 30000 };
		yield return new object[] { 0.60, 50000, 30000 };
		yield return new object[] { 0.70, 100000, 70000 };
		yield return new object[] { 0.25, 80000, 20000 };
		yield return new object[] { 0.11, 100000, 11000 };
		yield return new object[] { 0.99, 50000, 49500 };
		yield return new object[] { 0.88, 10000, 8800 };
		yield return new object[] { 0.44, 25000, 11000 };
		yield return new object[] { 0.74, 50000, 37000 };
		yield return new object[] { 0.55, 20001, 11001 };
		yield return new object[] { 0.75, 40001, 30001 };
		yield return new object[] { 0.60, 50001, 30001 };
		yield return new object[] { 0.70, 100001, 70001 };
		yield return new object[] { 0.25, 80001, 20001 };
		yield return new object[] { 0.11, 100001, 11001 };
		yield return new object[] { 0.99, 50001, 49501 };
		yield return new object[] { 0.88, 10001, 8801 };
		yield return new object[] { 0.44, 25001, 11001 };
		yield return new object[] { 0.74, 50001, 37001 };
	}

	public static IEnumerable<object[]> CasesForGlassMaterial()
	{
		yield return new object[] { 0.74, 0.75, 25, 14, 5 };
		yield return new object[] { 0.75, 0.75, 100, 57, 18 };
		yield return new object[] { 0.6, 0.75, 128, 58, 19 };
		yield return new object[] { 0.7, 0.7, 123, 61, 26 };
		yield return new object[] { 0.78, 0.77, 170, 103, 30 };
		yield return new object[] { 0.74, 0.75, 33045, 18340, 6114 };
		yield return new object[] { 0.74, 0.75, 13005, 7218, 2406 };
		yield return new object[] { 0.74, 0.75, 4522, 2510, 837 };
		yield return new object[] { 0.50, 0.50, 99, 25, 25 };
		yield return new object[] { 0.74, 0.74, 1000, 548, 192 };
		yield return new object[] { 0.74, 0.74, 101, 56, 19 };
		yield return new object[] { 0.75, 0.74, 5000, 2775, 975 };
		yield return new object[] { 0.75, 0.76, 25, 15, 4 };
		yield return new object[] { 0.60, 0.61, 10, 4, 2 };
		yield return new object[] { 0.55, 0.55, 300, 91, 74 };
		yield return new object[] { 0.80, 0.80, 120, 77, 19 };
		yield return new object[] { 0.50, 0.50, 1234, 309, 308 };
		yield return new object[] { 0.99, 0.99, 100, 99, 0 };
		yield return new object[] { 0.74, 0.74, 300, 165, 57 };
		yield return new object[] { 0.70, 0.70, 143, 71, 30 };
		yield return new object[] { 0.75, 0.75, 10001, 5626, 1875 };
		yield return new object[] { 0.60, 0.60, 167, 61, 40 };
		yield return new object[] { 0.55, 0.50, 100, 28, 27 };
		yield return new object[] { 0.75, 0.74, 200, 111, 39 };
		yield return new object[] { 0.74, 0.75, 400, 222, 74 };
		yield return new object[] { 0.74, 0.75, 534, 297, 99 };
		yield return new object[] { 0.75, 0.75, 1, 1, 0 };
		yield return new object[] { 0.70, 0.75, 10000, 5250, 1750 };
		yield return new object[] { 0.55, 0.99, 20000, 10890, 110 };
		yield return new object[] { 0.80, 0.50, 10001, 4001, 4000 };
		yield return new object[] { 0.75, 0.10, 40000, 3000, 27000 };
		yield return new object[] { 0.60, 0.60, 50001, 18001, 12000 };
		yield return new object[] { 0.99, 0.50, 10000, 4950, 4950 };
		yield return new object[] { 0.88, 0.75, 10000, 6600, 2200 };
		yield return new object[] { 0.74, 0.74, 50000, 27380, 9620 };
		yield return new object[] { 0.44, 0.50, 25001, 5501, 5500 };
		yield return new object[] { 0.70, 0.30, 100001, 21001, 49000 };
		yield return new object[] { 0.50, 0.50, 100001, 25001, 25000 };
		yield return new object[] { 0.66, 0.90, 10000, 5940, 660 };
		yield return new object[] { 0.75, 0.55, 40001, 16501, 13500 };
		yield return new object[] { 0.90, 0.11, 20000, 1980, 16020 };
		yield return new object[] { 0.95, 0.80, 100000, 76000, 19000 };
		yield return new object[] { 0.55, 0.15, 20001, 1651, 9350 };
		yield return new object[] { 0.77, 0.50, 10000, 3850, 3850 };
		yield return new object[] { 0.61, 0.60, 20000, 7320, 4880 };
		yield return new object[] { 0.33, 0.99, 30000, 9801, 99 };
		yield return new object[] { 0.75, 0.80, 40001, 24001, 6000 };
	}
}