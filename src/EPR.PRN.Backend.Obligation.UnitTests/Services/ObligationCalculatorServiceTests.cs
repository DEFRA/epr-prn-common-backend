using AutoFixture;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.Constants;
using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Enums;
using EPR.PRN.Backend.Obligation.Helpers;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Models;
using EPR.PRN.Backend.Obligation.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.Obligation.UnitTests.Services;

[TestClass]
public class ObligationCalculatorServiceTests
{
    private Mock<IObligationCalculationRepository> _mockObligationCalculationRepository;
    private Mock<IRecyclingTargetDataService> _mockRecyclingTargetDataService;
    private Mock<IMaterialService> _mockMaterialService;
    private Mock<IMaterialCalculationStrategyResolver> _mockStrategyResolver;
    private Mock<IPrnRepository> _mockPrnRepository;
    private Mock<IMaterialRepository> _mockMaterialRepository;
    private Mock<ILogger<ObligationCalculatorService>> _mockLogger;
    private ObligationCalculatorService _service;
    private Fixture _fixture;

    [TestInitialize]
    public void TestInitialize()
    {
        _fixture = new Fixture();
        _mockObligationCalculationRepository = new Mock<IObligationCalculationRepository>();
        _mockRecyclingTargetDataService = new Mock<IRecyclingTargetDataService>();
        _mockMaterialService = new Mock<IMaterialService>();
        _mockStrategyResolver = new Mock<IMaterialCalculationStrategyResolver>();
        _mockPrnRepository = new Mock<IPrnRepository>();
        _mockMaterialRepository = new Mock<IMaterialRepository>();
        _mockLogger = new Mock<ILogger<ObligationCalculatorService>>();
        _service = new ObligationCalculatorService(
            _mockObligationCalculationRepository.Object,
            _mockRecyclingTargetDataService.Object,
            _mockMaterialService.Object,
            _mockStrategyResolver.Object,
            _mockLogger.Object,
            _mockPrnRepository.Object,
            _mockMaterialRepository.Object);
    }

    [TestMethod]
    public async Task GetObligationCalculation_ShouldReturnError_WhenNoMaterialsFound()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var year = 2024;

        _mockMaterialRepository
            .Setup(repo => repo.GetAllMaterials())
            .ReturnsAsync(Enumerable.Empty<Material>());
        var loggedMessages = MockLogger();

        // Act
        var result = await _service.GetObligationCalculation(organisationId, year);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Be(ObligationConstants.ErrorMessages.NoMaterialsFoundErrorMessage);
        loggedMessages.Should().Contain(ObligationConstants.ErrorMessages.NoMaterialsFoundErrorMessage);
    }

    [TestMethod]
    public async Task GetObligationCalculation_ShouldReturnSuccess_WithExpectedData()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var year = 2024;

        var materials = _fixture.CreateMany<Material>(5).ToList();
        var obligationCalculations = _fixture.CreateMany<ObligationCalculation>(5).ToList();
        var prns = _fixture.CreateMany<EprnResultsDto>(5).ToList();

        _mockMaterialRepository.Setup(repo => repo.GetAllMaterials()).ReturnsAsync(materials);
        _mockObligationCalculationRepository.Setup(repo => repo.GetObligationCalculation(organisationId, year)).ReturnsAsync(obligationCalculations);
        _mockPrnRepository.Setup(repo => repo.GetAcceptedAndAwaitingPrnsByYearAsync(organisationId, year)).ReturnsAsync(prns);

        var acceptedTonnage = _fixture.CreateMany<EprnTonnageResultsDto>(5).ToList();
        var awaitingTonnage = _fixture.CreateMany<EprnTonnageResultsDto>(5).ToList();

        _mockPrnRepository.Setup(repo => repo.GetSumOfTonnageForMaterials(prns, EprnStatus.ACCEPTED.ToString())).Returns(acceptedTonnage);
        _mockPrnRepository.Setup(repo => repo.GetSumOfTonnageForMaterials(prns, EprnStatus.AWAITINGACCEPTANCE.ToString())).Returns(awaitingTonnage);
        _mockPrnRepository.Setup(repo => repo.GetPrnStatusCount(prns, EprnStatus.AWAITINGACCEPTANCE.ToString())).Returns(awaitingTonnage.Count());

        // Act
        var result = await _service.GetObligationCalculation(organisationId, year);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ObligationModel.Should().NotBeNull();
        result.ObligationModel.ObligationData.Should().HaveCount(materials.Count + 1); // +1 for GlassRemelt
        result.ObligationModel.NumberOfPrnsAwaitingAcceptance.Should().Be(awaitingTonnage.Count());



        foreach (var material in materials)
        {
            var obligationData = result.ObligationModel.ObligationData.FirstOrDefault(d => d.MaterialName == material.MaterialName);
            obligationData.Should().NotBeNull();
            obligationData.OrganisationId.Should().Be(organisationId);
            obligationData.MaterialName.Should().Be(material.MaterialName);
            obligationData.ObligationToMeet.Should().Be(obligationCalculations.FirstOrDefault(o => o.MaterialName == material.MaterialName)?.MaterialObligationValue);
            obligationData.TonnageAccepted.Should().Be(acceptedTonnage.FirstOrDefault(t => t.MaterialName == material.MaterialName)?.TotalTonnage ?? 0);
            obligationData.TonnageAwaitingAcceptance.Should().Be(awaitingTonnage.FirstOrDefault(t => t.MaterialName == material.MaterialName)?.TotalTonnage ?? 0);
        }
    }

    [TestMethod]
    public async Task GetObligationCalculation_ShouldHandleGlassRemeltCorrectly()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var year = 2024;

        var materials = _fixture.CreateMany<Material>(5).ToList(); // No GlassRemelt initially
        var obligationCalculations = _fixture.CreateMany<ObligationCalculation>(6).ToList();
        var prns = _fixture.CreateMany<EprnResultsDto>(5).ToList();
        prns[1].Eprn.MaterialName = "GlassRemelt";

        _mockMaterialRepository.Setup(repo => repo.GetAllMaterials()).ReturnsAsync(materials);
        _mockObligationCalculationRepository.Setup(repo => repo.GetObligationCalculation(organisationId, year)).ReturnsAsync(obligationCalculations);
        _mockPrnRepository.Setup(repo => repo.GetAcceptedAndAwaitingPrnsByYearAsync(organisationId, year)).ReturnsAsync(prns);

        var acceptedTonnage = _fixture.CreateMany<EprnTonnageResultsDto>(6).ToList();
        var awaitingTonnage = _fixture.CreateMany<EprnTonnageResultsDto>(6).ToList();

        _mockPrnRepository.Setup(repo => repo.GetSumOfTonnageForMaterials(prns, EprnStatus.ACCEPTED.ToString())).Returns(acceptedTonnage);
        _mockPrnRepository.Setup(repo => repo.GetSumOfTonnageForMaterials(prns, EprnStatus.AWAITINGACCEPTANCE.ToString())).Returns(awaitingTonnage);
        _mockPrnRepository.Setup(repo => repo.GetPrnStatusCount(prns, EprnStatus.AWAITINGACCEPTANCE.ToString())).Returns(awaitingTonnage.Count());

        // Act
        var result = await _service.GetObligationCalculation(organisationId, year);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ObligationModel.ObligationData.Should().Contain(d => d.MaterialName == "GlassRemelt");
        result.ObligationModel.NumberOfPrnsAwaitingAcceptance.Should().Be(awaitingTonnage.Count());

        var glassRemeltData = result.ObligationModel.ObligationData.FirstOrDefault(d => d.MaterialName == "GlassRemelt");
        glassRemeltData.Should().NotBeNull();
        glassRemeltData.ObligationToMeet.Should().BeNull();
        glassRemeltData.TonnageAccepted.Should().Be(0);
        glassRemeltData.TonnageAwaitingAcceptance.Should().Be(0);
        glassRemeltData.Status.Should().Be(ObligationConstants.Statuses.NoDataYet);
    }

    [TestMethod]
    public async Task CalculatePomDataAsync_WhenSubmissionMaterialIsNullOrEmpty_ShouldLogErrorAndSkip()
    {
        var submissionId = Guid.NewGuid();
        var organisationId = Guid.NewGuid();
        var submissions = new List<SubmissionCalculationRequest>
        {
            new SubmissionCalculationRequest { SubmissionId = submissionId, PackagingMaterial = null }
        };
        _mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync(new Dictionary<int, Dictionary<MaterialType, double>>());

        var loggedMessages = MockLogger();

        var result = await _service.CalculateAsync(organisationId, submissions);

        result.Success.Should().BeFalse();
        loggedMessages.Should().Contain($"Material was null or empty for SubmissionId: {submissionId} and OrganisationId: {organisationId}.");
        loggedMessages.Should().Contain($"No calculations for OrganisationId: {organisationId}.");
    }

    [TestMethod]
    public async Task CalculatePomDataAsync_WhenMaterialIsInvalid_ShouldLogErrorAndSkip()
    {
        var submissionId = Guid.NewGuid();
        var organisationId = Guid.NewGuid();
        var packagingMaterial = "InvalidMaterial";
        var submissions = new List<SubmissionCalculationRequest>
        {
            new SubmissionCalculationRequest { SubmissionId = submissionId, PackagingMaterial = packagingMaterial }
        };
        _mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync(new Dictionary<int, Dictionary<MaterialType, double>>());
        _mockMaterialService.Setup(x => x.GetMaterialByCode("InvalidMaterial")).ReturnsAsync((MaterialType?)null);
        var loggedMessages = MockLogger();

        var result = await _service.CalculateAsync(organisationId, submissions);

        result.Success.Should().BeFalse();
        loggedMessages.Should().Contain($"Material provided was not valid: {packagingMaterial} for SubmissionId: {submissionId} and OrganisationId: {organisationId}.");
        loggedMessages.Should().Contain($"No calculations for OrganisationId: {organisationId}.");
    }

    [TestMethod]
    public async Task CalculatePomDataAsync_WhenStrategyIsNull_ShouldLogErrorAndSkip()
    {
        var submissionId = Guid.NewGuid();
        var organisationId = Guid.NewGuid();
        var packagingMaterial = "ValidMaterial";
        var submissions = new List<SubmissionCalculationRequest>
        {
            new SubmissionCalculationRequest { SubmissionId = submissionId, PackagingMaterial = packagingMaterial }
        };
        _mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync(new Dictionary<int, Dictionary<MaterialType, double>>());
        _mockMaterialService.Setup(x => x.GetMaterialByCode("ValidMaterial")).ReturnsAsync(MaterialType.Plastic);
        _mockStrategyResolver.Setup(x => x.Resolve(MaterialType.Plastic)).Returns((IMaterialCalculationStrategy)null);
        var loggedMessages = MockLogger();

        var result = await _service.CalculateAsync(organisationId, submissions);

        result.Success.Should().BeFalse();
        loggedMessages.Should().Contain($"Could not find handler for Material Type: {packagingMaterial} for SubmissionId: {submissionId} and OrganisationId: {organisationId}.");
        loggedMessages.Should().Contain($"No calculations for OrganisationId: {organisationId}.");
    }

    [TestMethod]
    public async Task CalculatePomDataAsync_WhenCalculationsAreEmpty_ShouldLogError()
    {
        string dateString = "2024-P4";
        var extractedYear = DateHelper.ExtractYear(dateString);
        var submissionId = Guid.NewGuid();
        var organisationId = Guid.NewGuid();
        var packagingMaterial = "ValidMaterial";
        var submissions = new List<SubmissionCalculationRequest>
        {
            new SubmissionCalculationRequest { SubmissionId = submissionId, PackagingMaterial = packagingMaterial }
        };
        _mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync(new Dictionary<int, Dictionary<MaterialType, double>>());
        _mockMaterialService.Setup(x => x.GetMaterialByCode("ValidMaterial")).ReturnsAsync(MaterialType.Plastic);
        var mockStrategy = new Mock<IMaterialCalculationStrategy>();
        mockStrategy.Setup(x => x.Calculate(It.IsAny<CalculationRequestDto>())).Returns(new List<ObligationCalculation>());
        _mockStrategyResolver.Setup(x => x.Resolve(MaterialType.Plastic)).Returns(mockStrategy.Object);
        var loggedMessages = MockLogger();

        var result = await _service.CalculateAsync(organisationId, submissions);

        result.Success.Should().BeFalse();
        loggedMessages.Should().Contain($"No calculations for OrganisationId: {organisationId}.");
    }

    [TestMethod]
    public async Task CalculatePomDataAsync_WhenCalculationsAreSuccessful_ShouldReturnSuccess()
    {
        var submissionId = Guid.NewGuid();
        var organisationId = Guid.NewGuid();
        var packagingMaterial = "ValidMaterial";
        var submissions = new List<SubmissionCalculationRequest>
        {
            new SubmissionCalculationRequest { SubmissionId = submissionId, PackagingMaterial = packagingMaterial }
        };
        _mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync(new Dictionary<int, Dictionary<MaterialType, double>>());
        _mockMaterialService.Setup(x => x.GetMaterialByCode("ValidMaterial")).ReturnsAsync(MaterialType.Plastic);
        var mockStrategy = new Mock<IMaterialCalculationStrategy>();
        mockStrategy.Setup(x => x.Calculate(It.IsAny<CalculationRequestDto>())).Returns(new List<ObligationCalculation>
        {
            new ObligationCalculation()
        });
        _mockStrategyResolver.Setup(x => x.Resolve(MaterialType.Plastic)).Returns(mockStrategy.Object);

        var result = await _service.CalculateAsync(organisationId, submissions);

        result.Success.Should().BeTrue();
        result.Calculations.Should().NotBeNullOrEmpty();
    }

    [TestMethod]
    public async Task SaveCalculatedPomDataAsync_Should_ThrowArgumentException_WhenCalculationsAreNull()
    {
        List<ObligationCalculation> calculations = null;

        Func<Task> act = async () => await _service.SaveCalculatedPomDataAsync(calculations);

        var exception = await act.Should().ThrowAsync<ArgumentException>();
        exception.WithMessage("The calculations list cannot be null or empty.*");

        _mockObligationCalculationRepository.Verify(x => x.AddObligationCalculation(It.IsAny<List<ObligationCalculation>>()), Times.Never);
    }

    [TestMethod]
    public async Task SaveCalculatedPomDataAsync_Should_ThrowArgumentException_WhenCalculationsAreEmpty()
    {
        var calculations = new List<ObligationCalculation>();

        Func<Task> act = async () => await _service.SaveCalculatedPomDataAsync(calculations);

        var exception = await act.Should().ThrowAsync<ArgumentException>();
        exception.WithMessage("The calculations list cannot be null or empty.*");

        _mockObligationCalculationRepository.Verify(x => x.AddObligationCalculation(It.IsAny<List<ObligationCalculation>>()), Times.Never);
    }

    [TestMethod]
    public async Task SaveCalculatedPomDataAsync_Should_CallRepository_WhenValidCalculationsAreProvided()
    {
        var calculations = new List<ObligationCalculation>
            {
                new ObligationCalculation ()
            };

        await _service.SaveCalculatedPomDataAsync(calculations);

        _mockObligationCalculationRepository.Verify(x => x.AddObligationCalculation(calculations), Times.Once);
    }

    private List<string> MockLogger()
    {
        var loggedMessages = new List<string>();
        _mockLogger.Setup(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)))
        .Callback((LogLevel logLevel, EventId eventId, object state, Exception exception, Delegate formatter) =>
        {
            if (formatter != null)
            {
                var logMessage = formatter.DynamicInvoke(state, exception) as string;
                if (logMessage != null)
                {
                    loggedMessages.Add(logMessage);
                }
            }
        });
        return loggedMessages;
    }
}