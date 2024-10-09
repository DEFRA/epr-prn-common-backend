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

    [TestInitialize]
    public void TestInitialize()
    {
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
    public async Task GetObligationCalculationById_ReturnsExpectedDtoList_WhenGetObligationCalculationHasResult()
    {
        var organisationId = Guid.NewGuid();
        int year = 2024;
        var obligationCalculation = GetObligationCalculation(organisationId, year);
        var eprnAcceptedResult = GetEprnAcceptedResultDto();
        var eprnAwaitedAcceptanceResult = GetEprnAwaitingAcceptanceResultDto();
        var materials = GetMaterials();
        SetupRepositories(organisationId, year, obligationCalculation, eprnAcceptedResult, eprnAwaitedAcceptanceResult, materials);

        var result = await _service.GetObligationCalculation(organisationId, year);

        AddGlassRemelt(materials);
        AssertResults(materials, obligationCalculation, eprnAcceptedResult, eprnAwaitedAcceptanceResult, result);
    }

    [TestMethod]
    public async Task GetObligationCalculationById_ReturnsExpectedDtoList_WhenGetObligationCalculationHasNoResult()
    {
        var organisationId = Guid.NewGuid();
        int year = 2024;
        var obligationCalculation = new List<ObligationCalculation>();
        var eprnAcceptedResult = GetEprnAcceptedResultDto();
        var eprnAwaitedAcceptanceResult = GetEprnAwaitingAcceptanceResultDto();
        var materials = GetMaterials();
        SetupRepositories(organisationId, year, obligationCalculation, eprnAcceptedResult, eprnAwaitedAcceptanceResult, materials);

        var result = await _service.GetObligationCalculation(organisationId, year);

        AddGlassRemelt(materials);
        AssertResults(materials, obligationCalculation, eprnAcceptedResult, eprnAwaitedAcceptanceResult, result);
    }

    [TestMethod]
    public async Task GetObligationCalculationById_ReturnsExpectedDtoList_WhenGetEprnAcceptedHasNoResult()
    {
        var organisationId = Guid.NewGuid();
        int year = 2024;
        var obligationCalculation = GetObligationCalculation(organisationId, year);
        var eprnAcceptedResult = new List<EprnResultsDto>();
        var eprnAwaitedAcceptanceResult = GetEprnAwaitingAcceptanceResultDto();
        var materials = GetMaterials();
        SetupRepositories(organisationId, year, obligationCalculation, eprnAcceptedResult, eprnAwaitedAcceptanceResult, materials);

        var result = await _service.GetObligationCalculation(organisationId, year);

        AddGlassRemelt(materials);
        AssertResults(materials, obligationCalculation, eprnAcceptedResult, eprnAwaitedAcceptanceResult, result);
    }

    [TestMethod]
    public async Task GetObligationCalculationById_ReturnsExpectedDtoList_WhenGetAwaitedAcceptanceHasNoResult()
    {
        var organisationId = Guid.NewGuid();
        int year = 2024;
        var obligationCalculation = GetObligationCalculation(organisationId, year);
        var eprnAcceptedResult = GetEprnAcceptedResultDto();
        var eprnAwaitedAcceptanceResult = new List<EprnResultsDto>();
        var materials = GetMaterials();
        SetupRepositories(organisationId, year, obligationCalculation, eprnAcceptedResult, eprnAwaitedAcceptanceResult, materials);

        var result = await _service.GetObligationCalculation(organisationId, year);

        AddGlassRemelt(materials);
        AssertResults(materials, obligationCalculation, eprnAcceptedResult, eprnAwaitedAcceptanceResult, result);
    }

    [TestMethod]
    public async Task GetObligationCalculationById_ReturnsExpectedDtoList_WhenThereIsNoResult()
    {
        var organisationId = Guid.NewGuid();
        int year = 2024;
        var obligationCalculation = new List<ObligationCalculation>();
        var eprnAcceptedResult = new List<EprnResultsDto>();
        var eprnAwaitedAcceptanceResult = new List<EprnResultsDto>();
        var materials = GetMaterials();
        SetupRepositories(organisationId, year, obligationCalculation, eprnAcceptedResult, eprnAwaitedAcceptanceResult, materials);

        var result = await _service.GetObligationCalculation(organisationId, year);

        AddGlassRemelt(materials);
        AssertResults(materials, obligationCalculation, eprnAcceptedResult, eprnAwaitedAcceptanceResult, result);
    }

    [TestMethod]
    public async Task GetObligationCalculationById_WhenMaterialsIsNull_ShouldLogError()
    {
        var organisationId = Guid.NewGuid();
        int year = 2024;
        var loggedMessages = MockLogger();

        var result = await _service.GetObligationCalculation(organisationId, year);

        result.Should().NotBeNull();
        loggedMessages.Should().Contain("No Materials found in PRN BAckend Database");
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

    private void SetupRepositories(
        Guid organisationId,
        int year,
        List<ObligationCalculation> obligationCalculation,
        List<EprnResultsDto> eprnAcceptedResult,
        List<EprnResultsDto> eprnAwaitedAcceptanceResult,
        List<Material> materials)
    {
        _mockObligationCalculationRepository.Setup(repo => repo.GetObligationCalculation(organisationId, year))
            .ReturnsAsync(obligationCalculation);
        _mockPrnRepository.Setup(repo => repo.GetSumOfTonnageForMaterials(organisationId, EprnStatus.ACCEPTED.ToString()))
            .ReturnsAsync(eprnAcceptedResult);
        _mockPrnRepository.Setup(repo => repo.GetSumOfTonnageForMaterials(organisationId, EprnStatus.AWAITINGACCEPTANCE.ToString()))
            .ReturnsAsync(eprnAwaitedAcceptanceResult);
        _mockMaterialRepository.Setup(repo => repo.GetAllMaterials()).ReturnsAsync(materials);
    }

    private static void AssertResults(
        List<Material> materials,
        List<ObligationCalculation> obligationCalculation,
        List<EprnResultsDto> eprnAcceptedResult,
        List<EprnResultsDto> eprnAwaitedAcceptanceResult,
        List<PrnDataDto> result)
    {
        result.Should().NotBeNull();
        result.Should().HaveCount(materials.Count, "the expected count of calculations should match the actual count of calculations");

        for (int i = 0; i < result.Count; i++)
        {
            var obligation = obligationCalculation.FirstOrDefault(obligationCalculationItem => obligationCalculationItem.MaterialName == result[i].MaterialName);

            result[i].MaterialName.Should().Be(materials[i].MaterialName,
                $"the expected material name for calculation {i} should match the actual material name");

            result[i].ObligationToMeet.Should().Be(obligation?.MaterialObligationValue,
                $"the expected obligation value for material {i} should match");

            var expectedAcceptedTonnage = GetTonnage(result[i].MaterialName, eprnAcceptedResult);
            result[i].TonnageAccepted.Should().Be(expectedAcceptedTonnage,
                $"the expected tonnage accepted for material {i} should match");

            var expectedAwaitingTonnage = GetTonnage(result[i].MaterialName, eprnAwaitedAcceptanceResult);
            result[i].TonnageAwaitingAcceptance.Should().Be(expectedAwaitingTonnage,
                $"the expected tonnage awaiting acceptance for material {i} should match");

            var expectedTonnageOutstanding = GetTonnageOutstanding(obligation?.MaterialObligationValue, expectedAcceptedTonnage);
            result[i].TonnageOutstanding.Should().Be(expectedTonnageOutstanding,
                $"the expected tonnage outstanding for material {i} should match");

            var expectedStatus = GetStatus(obligation?.MaterialObligationValue, expectedAcceptedTonnage);
            result[i].Status.Should().Be(expectedStatus,
                $"the expected status for material {i} should match");
        }
    }

    private void AddGlassRemelt(List<Material> materials)
    {
        materials.Add(new Material { MaterialCode = "GR", MaterialName = "GlassRemelt" });
    }

    private static int? GetTonnage(string materialName, List<EprnResultsDto> acceptedTonnageForMaterials)
    {
        return acceptedTonnageForMaterials
            .Where(x => x.MaterialName == materialName)
            .Select(x => x.TotalTonnage)
            .FirstOrDefault();
    }

    private static List<Material> GetMaterials()
    {
        return new List<Material>
            {
                new Material { MaterialCode = "PL", MaterialName = "Plastic" },
                new Material { MaterialCode = "WD", MaterialName = "Wood" },
                new Material { MaterialCode = "AL", MaterialName = "Aluminium" },
                new Material { MaterialCode = "ST", MaterialName = "Steel" },
                new Material { MaterialCode = "PC", MaterialName = "Paper" },
                new Material { MaterialCode = "GL", MaterialName = "Glass" },
            };
    }

    private static List<ObligationCalculation> GetObligationCalculation(Guid organisationId, int year)
    {
        return new List<ObligationCalculation>()
        {
            new ObligationCalculation()
            {
                Id = 1,
                MaterialName = MaterialType.Glass.ToString(),
                OrganisationId = organisationId,
                Year = year,
                CalculatedOn = DateTime.UtcNow,
                MaterialObligationValue = 2000,
                MaterialWeight = 120
            },
            new ObligationCalculation()
            {
                Id = 2,
                MaterialName = MaterialType.Steel.ToString(),
                OrganisationId = organisationId,
                Year = year,
                CalculatedOn = DateTime.UtcNow,
                MaterialObligationValue = 2000,
                MaterialWeight = 120
            },
            new ObligationCalculation()
            {
                Id = 3,
                MaterialName = MaterialType.Wood.ToString(),
                OrganisationId = organisationId,
                Year = year,
                CalculatedOn = DateTime.UtcNow,
                MaterialObligationValue = 2000,
                MaterialWeight = 120
            }
        };
    }

    private static List<EprnResultsDto> GetEprnAcceptedResultDto()
    {
        return new List<EprnResultsDto>()
        {
            new EprnResultsDto()
            {
                MaterialName = MaterialType.Glass.ToString(),
                StatusName = EprnStatus.AWAITINGACCEPTANCE.ToString(),
                TotalTonnage = 180
            },
            new EprnResultsDto()
            {
                MaterialName = MaterialType.Steel.ToString(),
                StatusName = EprnStatus.AWAITINGACCEPTANCE.ToString(),
                TotalTonnage = 180
            },
            new EprnResultsDto()
            {
                MaterialName = MaterialType.Wood.ToString(),
                StatusName = EprnStatus.AWAITINGACCEPTANCE.ToString(),
                TotalTonnage = 180
            }
        };
    }

    private static List<EprnResultsDto> GetEprnAwaitingAcceptanceResultDto()
    {
        return new List<EprnResultsDto>()
        {
            new EprnResultsDto()
            {
                MaterialName = MaterialType.Glass.ToString(),
                StatusName = EprnStatus.ACCEPTED.ToString(),
                TotalTonnage = 180
            },
            new EprnResultsDto()
            {
                MaterialName = MaterialType.Steel.ToString(),
                StatusName = EprnStatus.ACCEPTED.ToString(),
                TotalTonnage = 180
            },
            new EprnResultsDto()
            {
                MaterialName = MaterialType.Wood.ToString(),
                StatusName = EprnStatus.ACCEPTED.ToString(),
                TotalTonnage = 180
            }
        };
    }

    private static string GetStatus(int? materialObligationValue, int? tonnageAccepted)
    {
        if (!materialObligationValue.HasValue || !tonnageAccepted.HasValue)
        {
            return ObligationConstants.Statuses.NoDataYet;
        }

        if (tonnageAccepted >= materialObligationValue)
        {
            return ObligationConstants.Statuses.Met;
        }
        return ObligationConstants.Statuses.NoMet;
    }

    private static int? GetTonnageOutstanding(int? materialObligationValue, int? tonnageAccepted)
    {
        if (!materialObligationValue.HasValue || !tonnageAccepted.HasValue)
        {
            return null;
        }

        return materialObligationValue - tonnageAccepted;
    }
}