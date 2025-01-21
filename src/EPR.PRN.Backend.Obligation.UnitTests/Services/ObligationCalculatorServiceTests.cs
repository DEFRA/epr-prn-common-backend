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
        _fixture.Customize(new NoCircularReferencesCustomization());
        _fixture.Customize(new IgnoreVirtualMembersCustomization());

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

        var materials = GetMaterialCodes();
        var obligationCalculations = _fixture.CreateMany<ObligationCalculation>(5).ToList();
        obligationCalculations[0].MaterialName = MaterialType.Plastic.ToString();
        obligationCalculations[1].MaterialName = MaterialType.Paper.ToString();
        obligationCalculations[2].MaterialName = MaterialType.Steel.ToString();
        obligationCalculations[3].MaterialName = MaterialType.Wood.ToString();
        obligationCalculations[4].MaterialName = MaterialType.Aluminium.ToString();
        var prnList = _fixture.CreateMany<EprnResultsDto>(5).ToList();
        prnList[0].Eprn.MaterialName = MaterialType.Plastic.ToString();
        prnList[1].Eprn.MaterialName = MaterialType.Paper.ToString();
        prnList[2].Eprn.MaterialName = MaterialType.Steel.ToString();
        prnList[3].Eprn.MaterialName = MaterialType.Wood.ToString();
        prnList[4].Eprn.MaterialName = MaterialType.Aluminium.ToString();
        prnList[0].Eprn.ObligationYear = year.ToString();
        prnList[1].Eprn.ObligationYear = year.ToString();
        prnList[2].Eprn.ObligationYear = year.ToString();
        prnList[3].Eprn.ObligationYear = year.ToString();
        prnList[4].Eprn.ObligationYear = year.ToString();
        var prns = prnList.AsQueryable();
        _mockMaterialRepository.Setup(repo => repo.GetAllMaterials()).ReturnsAsync(materials);
        _mockObligationCalculationRepository.Setup(repo => repo.GetObligationCalculation(organisationId, year)).ReturnsAsync(obligationCalculations);
        _mockPrnRepository.Setup(repo => repo.GetAcceptedAndAwaitingPrnsByYear(organisationId, year)).Returns(prns);
        var acceptedTonnage = _fixture.CreateMany<EprnTonnageResultsDto>(5).ToList();
        var awaitingTonnage = _fixture.CreateMany<EprnTonnageResultsDto>(5).ToList();
        _mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync(GetRecyclingTargets());

        // Act
        var result = await _service.GetObligationCalculation(organisationId, year);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ObligationModel.Should().NotBeNull();
        result.ObligationModel.ObligationData.Should().HaveCount(materials.Count + 1); // +1 for GlassRemelt

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
        var prnList = _fixture.CreateMany<EprnResultsDto>(5).ToList();
        prnList[1].Eprn.MaterialName = "GlassRemelt";
        var prns = prnList.AsQueryable();
        _mockMaterialRepository.Setup(repo => repo.GetAllMaterials()).ReturnsAsync(materials);
        _mockObligationCalculationRepository.Setup(repo => repo.GetObligationCalculation(organisationId, year)).ReturnsAsync(obligationCalculations);
        _mockPrnRepository.Setup(repo => repo.GetAcceptedAndAwaitingPrnsByYear(organisationId, year)).Returns(prns);
        _mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync(GetRecyclingTargets());
        var acceptedTonnage = _fixture.CreateMany<EprnTonnageResultsDto>(6).ToList();
        var awaitingTonnage = _fixture.CreateMany<EprnTonnageResultsDto>(6).ToList();

        // Act
        var result = await _service.GetObligationCalculation(organisationId, year);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ObligationModel.ObligationData.Should().Contain(d => d.MaterialName == "GlassRemelt");

        var glassRemeltData = result.ObligationModel.ObligationData.FirstOrDefault(d => d.MaterialName == "GlassRemelt");
        glassRemeltData.Should().NotBeNull();
        glassRemeltData.ObligationToMeet.Should().BeNull();
        glassRemeltData.TonnageAccepted.Should().Be(0);
        glassRemeltData.TonnageAwaitingAcceptance.Should().Be(0);
        glassRemeltData.Status.Should().Be(ObligationConstants.Statuses.NoDataYet);
    }

    [TestMethod]
    public async Task GetObligationCalculation_ShouldHandlePRNAwaitingAcceptanceCorrectly()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var year = 2024;

        var materials = _fixture.CreateMany<Material>(5).ToList();
        var obligationCalculations = _fixture.CreateMany<ObligationCalculation>(6).ToList();
        var prnList = _fixture.CreateMany<EprnResultsDto>(5).ToList();
        prnList[0].Eprn.PrnStatusId = 4;
        prnList[0].Status.Id = 4;
        prnList[0].Status.StatusName = EprnStatus.AWAITINGACCEPTANCE.ToString();
        prnList[2].Eprn.PrnStatusId = 4;
        prnList[2].Status.Id = 4;
        prnList[2].Status.StatusName = EprnStatus.AWAITINGACCEPTANCE.ToString();
        prnList[4].Eprn.PrnStatusId = 4;
        prnList[4].Status.Id = 4;
        prnList[4].Status.StatusName = EprnStatus.AWAITINGACCEPTANCE.ToString();
        prnList[1].Eprn.MaterialName = "GlassRemelt";
        var prns = prnList.AsQueryable();
        _mockMaterialRepository.Setup(repo => repo.GetAllMaterials()).ReturnsAsync(materials);
        _mockObligationCalculationRepository.Setup(repo => repo.GetObligationCalculation(organisationId, year)).ReturnsAsync(obligationCalculations);
        _mockPrnRepository.Setup(repo => repo.GetAcceptedAndAwaitingPrnsByYear(organisationId, year)).Returns(prns);
        _mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync(GetRecyclingTargets());
        var acceptedTonnage = _fixture.CreateMany<EprnTonnageResultsDto>(6).ToList();
        var awaitingTonnage = _fixture.CreateMany<EprnTonnageResultsDto>(6).ToList();

        // Act
        var result = await _service.GetObligationCalculation(organisationId, year);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ObligationModel.NumberOfPrnsAwaitingAcceptance.Should().Be(3);
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
    public async Task UpsertCalculatedPomDataAsync_Should_CallRepository_WhenValidCalculationsAreProvided()
    {
        var calculations = new List<ObligationCalculation>
            {
                new ObligationCalculation ()
            };
        var organisationId = Guid.NewGuid();

        await _service.UpsertCalculatedPomDataAsync(organisationId, calculations);

        _mockObligationCalculationRepository.Verify(x => x.UpsertObligationCalculationAsync(organisationId, calculations), Times.Once);
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

    public static Dictionary<int, Dictionary<MaterialType, double>> GetRecyclingTargets()
    {
        var targets = new List<RecyclingTarget>()
        {
            new RecyclingTarget { Year = 2024, PaperTarget = 0.75, GlassTarget = 0.74, AluminiumTarget = 0.61, SteelTarget = 0.8, PlasticTarget = 0.55, WoodTarget = 0.45, GlassRemeltTarget = 0.75 },
            new RecyclingTarget { Year = 2025, PaperTarget = 0.77, GlassTarget = 0.76, AluminiumTarget = 0.62, SteelTarget = 0.81, PlasticTarget = 0.57, WoodTarget = 0.46, GlassRemeltTarget = 0.76 },
            new RecyclingTarget { Year = 2026, PaperTarget = 0.79, GlassTarget = 0.78, AluminiumTarget = 0.63, SteelTarget = 0.82, PlasticTarget = 0.59, WoodTarget = 0.47, GlassRemeltTarget = 0.77 },
            new RecyclingTarget { Year = 2027, PaperTarget = 0.81, GlassTarget = 0.80, AluminiumTarget = 0.64, SteelTarget = 0.83, PlasticTarget = 0.61, WoodTarget = 0.48, GlassRemeltTarget = 0.78 },
            new RecyclingTarget { Year = 2028, PaperTarget = 0.83, GlassTarget = 0.82, AluminiumTarget = 0.65, SteelTarget = 0.84, PlasticTarget = 0.63, WoodTarget = 0.49, GlassRemeltTarget = 0.79 },
            new RecyclingTarget { Year = 2029, PaperTarget = 0.85, GlassTarget = 0.85, AluminiumTarget = 0.67, SteelTarget = 0.85, PlasticTarget = 0.65, WoodTarget = 0.50, GlassRemeltTarget = 0.80 }
        };

        return targets.Select(x => new KeyValuePair<int, Dictionary<MaterialType, double>>(x.Year, TransformTargets(x))).ToDictionary();
    }

    private static Dictionary<MaterialType, double> TransformTargets(RecyclingTarget recyclingTarget)
    {
        var dictionary = new Dictionary<MaterialType, double>(7)
            {
                { MaterialType.Aluminium, recyclingTarget.AluminiumTarget },
                { MaterialType.Glass, recyclingTarget.GlassTarget },
                { MaterialType.GlassRemelt, recyclingTarget.GlassRemeltTarget },
                { MaterialType.Paper, recyclingTarget.PaperTarget },
                { MaterialType.Plastic, recyclingTarget.PlasticTarget },
                { MaterialType.Steel, recyclingTarget.SteelTarget },
                { MaterialType.Wood, recyclingTarget.WoodTarget }
            };

        return dictionary;
    }

    private static List<Material> GetMaterialCodes()
    {
        return new List<Material>
        {
            new Material { MaterialCode = "PL", MaterialName = "Plastic" },
            new Material { MaterialCode = "WD", MaterialName = "Wood" },
            new Material { MaterialCode = "AL", MaterialName = "Aluminium" },
            new Material { MaterialCode = "ST", MaterialName = "Steel" },
            new Material { MaterialCode = "PC", MaterialName = "Paper" },
            new Material { MaterialCode = "GL", MaterialName = "Glass" }
        };
    }

}