using AutoFixture.MSTest;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Services;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.Obligation.UnitTests.Services
{
    [TestClass]
    public class ObligationCalculatorServiceTests
    {
        private Mock<IRecyclingTargetDataService> _mockRecyclingTargetDataService;
        private Mock<IObligationCalculationRepository> _mockObligationCalculationRepository;
        private ObligationCalculatorService _service;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRecyclingTargetDataService = new Mock<IRecyclingTargetDataService>();
            _mockObligationCalculationRepository = new Mock<IObligationCalculationRepository>();
            _service = new ObligationCalculatorService(_mockRecyclingTargetDataService.Object, _mockObligationCalculationRepository.Object);
        }

        [TestMethod]
        [DataRow(0.61, 100, 61)]
        [DataRow(0.61, 120, 73)]
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

        [TestMethod]
        [AutoData]
        public async Task GetObligationCalculationById_ReturnsExpectedDtoList(List<ObligationCalculation> obligationCalculations)
        {
            var organisationId = 1;
            _mockObligationCalculationRepository.Setup(repo => repo.GetObligationCalculationByOrganisationId(organisationId)).ReturnsAsync(obligationCalculations);

            var result = await _service.GetObligationCalculationByOrganisationId(organisationId);

            result.Should().NotBeNull();
            result.Should().HaveCount(obligationCalculations.Count(), "the expected count of calculations should match the actual count of calculations");
            result[0].MaterialName.Should().Be(obligationCalculations[0].MaterialName, "the expected material name for the first calculation should match actual material name");
            result[0].MaterialObligationValue.Should().Be(obligationCalculations[0].MaterialObligationValue, "the expected material obligation value for the first calculation should match actual material obligation value");
            result[0].OrganisationId.Should().Be(obligationCalculations[0].OrganisationId, "the expected organisation id for the first calculation should match actual organisation id");
            result[0].Year.Should().Be(obligationCalculations[0].Year, "the expected Year for the first calculation should match actual Year");

            result[1].MaterialName.Should().Be(obligationCalculations[1].MaterialName, "the expected material name for the second calculation should match actual material name");
            result[1].MaterialObligationValue.Should().Be(obligationCalculations[1].MaterialObligationValue, "the expected material obligation value for the second calculation should match actual material obligation value");
            result[1].OrganisationId.Should().Be(obligationCalculations[1].OrganisationId, "the expected organisation id for the second calculation should match actual organisation id");
            result[1].Year.Should().Be(obligationCalculations[1].Year, "the expected Year for the second calculation should match actual Year");
        }

        [TestMethod]
        public async Task GetObligationCalculationById_ReturnsNull_WhenNoDataFound()
        {
            var organisationId = 2;
            _mockObligationCalculationRepository.Setup(repo => repo.GetObligationCalculationByOrganisationId(organisationId)).ReturnsAsync((List<ObligationCalculation>?)null);

            var result = await _service.GetObligationCalculationByOrganisationId(organisationId);

            result.Should().BeNull();
        }
    }
}
