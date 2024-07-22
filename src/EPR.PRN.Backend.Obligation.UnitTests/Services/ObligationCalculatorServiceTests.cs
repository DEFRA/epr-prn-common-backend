using EPR.PRN.Backend.Obligation.Services;
using FluentAssertions;

namespace EPR.PRN.Backend.Obligation.UnitTests.Services
{
    [TestClass]
    public class ObligationCalculatorServiceTests
    {
        [TestMethod]
        [DataRow(0.61, 100, 61)]
        [DataRow(0.61, 120, 73)]
        [DataRow(0.6, 128, 77)]
        public void CalculateObligation_WhenAluminium_ThenObligationIsReturned(double target, int tonnage, int expected)
        {
            var obligationCalculator = new ObligationCalculatorService();

            var result = obligationCalculator.Calculate(target, tonnage);

            result.Should().Be(expected, "the expected target should match the calculated obligation");
        }

        [TestMethod]
        [DataRow(0.75, 100, 61)]
        public void CalculateObligation_WhenGlassRemelt_ThenObligationIsReturned(double target, int tonnage, int expected)
        {
            var obligationCalculator = new ObligationCalculatorService();

            var result = obligationCalculator.Calculate(target, tonnage);

            result.Should().Be(expected, "the expected target should match the calculated obligation");
        }
    }
}
