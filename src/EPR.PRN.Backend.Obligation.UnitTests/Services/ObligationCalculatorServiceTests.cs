using AutoFixture.MSTest;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Enums;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace EPR.PRN.Backend.Obligation.UnitTests.Services;

[TestClass]
public class ObligationCalculatorServiceTests
{
    private Mock<ILogger<ObligationCalculatorService>> _logger;
    private Mock<IRecyclingTargetDataService> _mockRecyclingTargetDataService;
    private Mock<IObligationCalculationRepository> _mockObligationCalculationRepository;
    private Mock<IPomSubmissionData> _pomSubmissionData;
    private Mock<IMaterialCalculationStrategyResolver> _strategyResolver;
    private ObligationCalculatorService _service;

    [TestInitialize]
    public void TestInitialize()
    {
        _logger = new Mock<ILogger<ObligationCalculatorService>>();
        _mockRecyclingTargetDataService = new Mock<IRecyclingTargetDataService>();
        _mockObligationCalculationRepository = new Mock<IObligationCalculationRepository>();
        _pomSubmissionData = new Mock<IPomSubmissionData>();
        _strategyResolver = new Mock<IMaterialCalculationStrategyResolver>();
        _service = new ObligationCalculatorService(_logger.Object, _mockRecyclingTargetDataService.Object, _mockObligationCalculationRepository.Object, _pomSubmissionData.Object, _strategyResolver.Object);
    }

    [TestMethod]
    [AutoData]
    public async Task GetObligationCalculationById_ReturnsExpectedDtoList(List<ObligationCalculation> obligationCalculations)
    {
        var organisationId = 1;
        _mockObligationCalculationRepository.Setup(repo => repo.GetObligationCalculationByOrganisationId(organisationId)).ReturnsAsync(obligationCalculations);

        var result = await _service.GetObligationCalculationByOrganisationId(organisationId);

        result.Should().NotBeNull();
        result.Should().HaveCount(obligationCalculations.Count, "the expected count of calculations should match the actual count of calculations");
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
        _mockObligationCalculationRepository.Setup(repo => repo.GetObligationCalculationByOrganisationId(organisationId)).ReturnsAsync((List<ObligationCalculation>)null);

        var result = await _service.GetObligationCalculationByOrganisationId(organisationId);

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task ProcessApprovedPomData_ShouldLogError_WhenHttpResponseIsUnsuccessful()
    {
        // Arrange
        var submissionId = "123";
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        _pomSubmissionData.Setup(x => x.GetAggregatedPomData(submissionId)).ReturnsAsync(response);

        // Act
        await _service.ProcessApprovedPomData(submissionId);

        // Assert
        _logger.Verify(
            x => x.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Could not retrieve POM data for Submission Id: {submissionId}")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        _mockObligationCalculationRepository.Verify(x => x.AddObligationCalculation(It.IsAny<List<ObligationCalculation>>()), Times.Never);
    }

    [TestMethod]
    public async Task ProcessApprovedPomData_ShouldLogError_WhenNoPomDataIsReturned()
    {
        // Arrange
        var submissionId = "123";
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new List<PomObligtionDto>())
        };
        _pomSubmissionData.Setup(x => x.GetAggregatedPomData(submissionId)).ReturnsAsync(response);

        // Act
        await _service.ProcessApprovedPomData(submissionId);

        // Assert
        _logger.Verify(
            x => x.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"No POM data returned for Submission Id: {submissionId}")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        _mockObligationCalculationRepository.Verify(x => x.AddObligationCalculation(It.IsAny<List<ObligationCalculation>>()), Times.Never);
    }

    [TestMethod]
    public async Task ProcessApprovedPomData_ShouldLogError_WhenStrategyResolverReturnsNull()
    {
        // Arrange
        var submissionId = "123";
        var pomData = new List<PomObligtionDto>
        {
            new() { PackagingMaterial = "UnknownMaterial", PackagingMaterialWeight = 100, OrganisationId = 1 }
        };
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(pomData)
        };
        _pomSubmissionData.Setup(x => x.GetAggregatedPomData(submissionId)).ReturnsAsync(response);

        var recyclingTargets = new Dictionary<int, Dictionary<MaterialType, double>>();
        _mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync(recyclingTargets);

        _strategyResolver.Setup(x => x.Resolve(It.IsAny<MaterialType>())).Returns((IMaterialCalculationStrategy)null);

        // Act
        await _service.ProcessApprovedPomData(submissionId);

        // Assert
        _logger.Verify(
            x => x.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Skipping material with unknown type")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        _mockObligationCalculationRepository.Verify(x => x.AddObligationCalculation(It.IsAny<List<ObligationCalculation>>()), Times.Never);
    }

    [TestMethod]
    public async Task ProcessApprovedPomData_ShouldAddCalculations_WhenEverythingIsValid()
    {
        // Arrange
        var submissionId = "123";
        var pomData = new List<PomObligtionDto>
        {
            new() { PackagingMaterial = "Plastic", PackagingMaterialWeight = 100, OrganisationId = 1 }
        };
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(pomData)
        };
        _pomSubmissionData.Setup(x => x.GetAggregatedPomData(submissionId)).ReturnsAsync(response);
        var recyclingTargets = new Dictionary<int, Dictionary<MaterialType, double>>();
        _mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync(recyclingTargets);

        var mockStrategy = new Mock<IMaterialCalculationStrategy>();
        mockStrategy.Setup(x => x.Calculate(It.IsAny<PomObligtionDto>(), It.IsAny<MaterialType>(), recyclingTargets))
                    .Returns(new List<ObligationCalculation> { new ObligationCalculation { MaterialName = "Plastic", MaterialObligationValue = 100, OrganisationId = 1, Year = 2024 } });
        _strategyResolver.Setup(x => x.Resolve(It.IsAny<MaterialType>())).Returns(mockStrategy.Object);

        // Act
        await _service.ProcessApprovedPomData(submissionId);

        // Assert
        _mockObligationCalculationRepository.Verify(x => x.AddObligationCalculation(It.Is<List<ObligationCalculation>>(list => list.Count == 1 && list[0].MaterialName == "Plastic")), Times.Once);
    }
}