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
    [DataRow(0.61, 100, 61)]
    [DataRow(0.61, 120, 74)]
    [DataRow(0.6, 128, 77)]
    [DataRow(0.55, 12160, 6688)]
    [DataRow(0.55, 740, 407)]
    [DataRow(0.55, 340, 187)]
    [DataRow(0.55, 860, 473)]
    [DataRow(0.55, 420, 231)]
	[DataRow(0.88, 1000, 880)]
	[DataRow(0.55, 10000, 5500)]
	[DataRow(0.25, 8000, 2000)]
	[DataRow(0.75, 4000, 3000)]
	[DataRow(0.01, 50000, 500)]
	[DataRow(0.10, 6000, 600)]
	[DataRow(0.40, 1500, 600)]
	[DataRow(0.33, 1000, 330)]
	[DataRow(0.99, 5000, 4950)]
	[DataRow(0.61, 2000, 1220)]
	[DataRow(0.55, 1001, 551)]
	[DataRow(0.88, 501, 441)]
	[DataRow(0.22, 1001, 221)]
	[DataRow(0.77, 700, 539)]
	[DataRow(0.61, 1001, 611)]
	[DataRow(0.15, 501, 76)]
	[DataRow(0.91, 600, 546)]
	[DataRow(0.05, 10001, 501)]
	[DataRow(0.45, 1001, 451)]
	[DataRow(0.30, 1700, 510)]
	[DataRow(0.55, 20, 11)]
	[DataRow(0.55, 40, 22)]
	[DataRow(0.55, 400, 220)]
	[DataRow(0.55, 2000, 1100)]
	[DataRow(0.55, 1, 1)]
	[DataRow(0.55, 21, 12)]
	[DataRow(0.55, 101, 56)]
	[DataRow(0.55, 501, 276)]
	[DataRow(0.55, 20000, 11000)]
	[DataRow(0.75, 40000, 30000)]
	[DataRow(0.60, 50000, 30000)]
	[DataRow(0.70, 100000, 70000)]
	[DataRow(0.25, 80000, 20000)]
	[DataRow(0.11, 100000, 11000)]
	[DataRow(0.99, 50000, 49500)]
	[DataRow(0.88, 10000, 8800)]
	[DataRow(0.44, 25000, 11000)]
	[DataRow(0.74, 50000, 37000)]
	[DataRow(0.55, 20001, 11001)]
	[DataRow(0.75, 40001, 30001)]
	[DataRow(0.60, 50001, 30001)]
	[DataRow(0.70, 100001, 70001)]
	[DataRow(0.25, 80001, 20001)]
	[DataRow(0.11, 100001, 11001)]
	[DataRow(0.99, 50001, 49501)]
	[DataRow(0.88, 10001, 8801)]
	[DataRow(0.44, 25001, 11001)]
	[DataRow(0.74, 50001, 37001)]
	public void CalculateObligation_WhenAluminium_ThenObligationIsReturned(double target, int tonnage, int expected)
    {
        var result = _service.Calculate(target, tonnage);

        result.Should().Be(expected, "the expected target should match the calculated obligation");
    }

    [TestMethod]
	[DataRow(0.74, 0.75, 25, 14, 5)]
	[DataRow(0.75, 0.75, 100, 57, 18)]
    [DataRow(0.6, 0.75, 128, 58, 19)]
    [DataRow(0.7, 0.7, 123, 61, 26)]
    [DataRow(0.78, 0.77, 170, 103, 30)]
	[DataRow(0.74, 0.75, 33045, 18340, 6114)]
	[DataRow(0.74, 0.75, 13005, 7218, 2406)]
	[DataRow(0.74, 0.75, 4522, 2510, 837)]
	[DataRow(0.50, 0.50, 99, 25, 25)]
	[DataRow(0.74, 0.74, 1000, 548, 192)]
	[DataRow(0.74, 0.74, 101, 56, 19)]
	[DataRow(0.75, 0.74, 5000, 2775, 975)]
	[DataRow(0.75, 0.76, 25, 15, 4)]
	[DataRow(0.60, 0.61, 10, 4, 2)]
	[DataRow(0.55, 0.55, 300, 91, 74)]
	[DataRow(0.80, 0.80, 120, 77, 19)]
	[DataRow(0.50, 0.50, 1234, 309, 308)]
	[DataRow(0.99, 0.99, 100, 99, 0)]
	[DataRow(0.74, 0.74, 300, 165, 57)]
	[DataRow(0.70, 0.70, 143, 71, 30)]
	[DataRow(0.75, 0.75, 10001, 5626, 1875)]
	[DataRow(0.60, 0.60, 167, 61, 40)]
	[DataRow(0.55, 0.50, 100, 28, 27)]
	[DataRow(0.75, 0.74, 200, 111, 39)]
	[DataRow(0.74, 0.75, 400, 222, 74)]
	[DataRow(0.74, 0.75, 534, 297, 99)]
	[DataRow(0.75, 0.75, 1, 1, 0)]
	[DataRow(0.70, 0.75, 10000, 5250, 1750)]
	[DataRow(0.55, 0.99, 20000, 10890, 110)]
	[DataRow(0.80, 0.50, 10001, 4001, 4000)]
	[DataRow(0.75, 0.10, 40000, 3000, 27000)]
	[DataRow(0.60, 0.60, 50001, 18001, 12000)]
	[DataRow(0.99, 0.50, 10000, 4950, 4950)]
	[DataRow(0.88, 0.75, 10000, 6600, 2200)]
	[DataRow(0.74, 0.74, 50000, 27380, 9620)]
	[DataRow(0.44, 0.50, 25001, 5501, 5500)]
	[DataRow(0.70, 0.30, 100001, 21001, 49000)]
	[DataRow(0.50, 0.50, 100001, 25001, 25000)]
	[DataRow(0.66, 0.90, 10000, 5940, 660)]
	[DataRow(0.75, 0.55, 40001, 16501, 13500)]
	[DataRow(0.90, 0.11, 20000, 1980, 16020)]
	[DataRow(0.95, 0.80, 100000, 76000, 19000)]
	[DataRow(0.55, 0.15, 20001, 1651, 9350)]
	[DataRow(0.77, 0.50, 10000, 3850, 3850)]
	[DataRow(0.61, 0.60, 20000, 7320, 4880)]
	[DataRow(0.33, 0.99, 30000, 9801, 99)]
	[DataRow(0.75, 0.80, 40001, 24001, 6000)]
	public void CalculateObligation_WhenGlassRemelt_ThenObligationIsReturned(double target, double remeltTarget, int tonnage, int expectedRemelt, int expectedRemainder)
    {
        var (remeltResult, remainderResult) = _service.CalculateGlass(target, remeltTarget, tonnage);

        remeltResult.Should().Be(expectedRemelt, "the expected remelt target should match the calculated obligation");
        remainderResult.Should().Be(expectedRemainder, "the expected remainder should match the calculated remainder");
    }
}
