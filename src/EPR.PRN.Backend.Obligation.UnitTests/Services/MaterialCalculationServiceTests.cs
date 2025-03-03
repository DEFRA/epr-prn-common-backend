﻿using System.Diagnostics.CodeAnalysis;
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
    public void CalculateObligation_WhenAluminium_ThenObligationIsReturned(double target, int tonnage, int expected)
    {
        var result = _service.Calculate(target, tonnage);

        result.Should().Be(expected, "the expected target should match the calculated obligation");
    }

    [TestMethod]
    [DataRow(0.75, 0.75, 100, 57, 18)]
    [DataRow(0.6, 0.75, 128, 58, 19)]
    [DataRow(0.7, 0.7, 123, 61, 25)]
    [DataRow(0.78, 0.77, 170, 103, 30)]
    public void CalculateObligation_WhenGlassRemelt_ThenObligationIsReturned(double target, double remeltTarget, int tonnage, int expectedRemelt, int expectedRemainder)
    {
        var (remeltResult, remainderResult) = _service.CalculateGlass(target, remeltTarget, tonnage);

        expectedRemelt.Should().Be(remeltResult, "the expected remelt target should match the calculated obligation");
        expectedRemainder.Should().Be(remainderResult, "the expected remainder should match the calculated remainder");
    }
}
