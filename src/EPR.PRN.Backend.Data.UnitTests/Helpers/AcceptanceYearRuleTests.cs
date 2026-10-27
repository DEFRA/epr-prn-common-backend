using AwesomeAssertions;
using EPR.PRN.Backend.Data.Helpers;

namespace EPR.PRN.Backend.Data.UnitTests.Helpers;

[TestClass]
public class AcceptanceYearRuleTests
{
    private static readonly DateTimeOffset DecemberChoiceWindow = new(2026, 12, 15, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset BeforeChoiceWindow = new(2026, 6, 15, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset JanuaryChoiceWindow = new(2027, 1, 20, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset AfterChoiceWindow = new(2027, 2, 15, 12, 0, 0, TimeSpan.Zero);

    [TestMethod]
    [DataRow(2026, false, new[] { 2026 })]
    [DataRow(2027, false, new int[0])]
    [DataRow(2025, false, new int[0])]
    public void StandardNote_IsAcceptableOnlyIntoTheCurrentComplianceYear(int obligationYear, bool decemberWaste, int[] expected)
    {
        AcceptanceYearRule.AvailableYears(obligationYear, decemberWaste, DecemberChoiceWindow).Should().Equal(expected);
    }

    [TestMethod]
    public void DecemberWaste_InsideChoiceWindow_OffersItsOwnYearAndTheNext()
    {
        AcceptanceYearRule.AvailableYears(2026, true, DecemberChoiceWindow).Should().Equal(2026, 2027);
        AcceptanceYearRule.AvailableYears(2026, true, JanuaryChoiceWindow).Should().Equal(2026, 2027);
    }

    [TestMethod]
    public void DecemberWaste_BeforeChoiceWindow_OffersOnlyItsOwnYear()
    {
        AcceptanceYearRule.AvailableYears(2026, true, BeforeChoiceWindow).Should().Equal(2026);
    }

    [TestMethod]
    public void DecemberWaste_AfterChoiceWindow_OffersOnlyTheNextYear()
    {
        AcceptanceYearRule.AvailableYears(2026, true, AfterChoiceWindow).Should().Equal(2027);
    }

    [TestMethod]
    public void DecemberWaste_TwoYearsOld_HasExpired()
    {
        AcceptanceYearRule.AvailableYears(2024, true, AfterChoiceWindow).Should().BeEmpty();
    }

    [TestMethod]
    public void December2025Waste_NeverOffersAChoice()
    {
        AcceptanceYearRule.AvailableYears(2025, true, BeforeChoiceWindow).Should().Equal(2026);
        AcceptanceYearRule.AvailableYears(2025, true, DecemberChoiceWindow).Should().Equal(2026);
    }

    [TestMethod]
    public void NoteFromAFutureYear_IsNeverAcceptable()
    {
        AcceptanceYearRule.AvailableYears(2028, false, DecemberChoiceWindow).Should().BeEmpty();
        AcceptanceYearRule.AvailableYears(2028, true, DecemberChoiceWindow).Should().BeEmpty();
    }

    [TestMethod]
    public void ComplianceYear_RunsFebruaryToJanuaryInUkTime()
    {
        AcceptanceYearRule.ComplianceYear(JanuaryChoiceWindow).Should().Be(2026);
        AcceptanceYearRule.ComplianceYear(AfterChoiceWindow).Should().Be(2027);
        AcceptanceYearRule.ComplianceYear(new DateTimeOffset(2026, 12, 31, 23, 30, 0, TimeSpan.Zero)).Should().Be(2026);
        AcceptanceYearRule.ComplianceYear(new DateTimeOffset(2027, 1, 31, 20, 0, 0, TimeSpan.FromHours(-5))).Should().Be(2027);
    }

    [TestMethod]
    public void ObligationYearsFor_ReturnsTheNoteYearsThatCanBeAcceptedIntoTheTargetYear()
    {
        AcceptanceYearRule.ObligationYearsFor(2027, false, DecemberChoiceWindow).Should().BeEmpty();
        AcceptanceYearRule.ObligationYearsFor(2026, false, DecemberChoiceWindow).Should().Equal(2026);
        AcceptanceYearRule.ObligationYearsFor(2027, true, DecemberChoiceWindow).Should().Equal(2026);
        AcceptanceYearRule.ObligationYearsFor(2026, true, DecemberChoiceWindow).Should().BeEquivalentTo([2025, 2026]);
        AcceptanceYearRule.ObligationYearsFor(2027, true, BeforeChoiceWindow).Should().BeEmpty();
    }
}
