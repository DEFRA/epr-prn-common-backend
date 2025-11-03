using System.Reflection;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.Services;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.Obligation.UnitTests.Services;

[TestClass]
public class RecyclingTargetDataServiceTests
{
    [TestMethod]
    public async Task GetRecyclingTargetsAsync_WhenInvoked_ReturnsRecyclingTargets()
    {
        // Arrange
        var currentYear = DateTime.UtcNow.Year;
        var recyclingTargetRepositoryMock = new Mock<IRecyclingTargetRepository>(MockBehavior.Strict);
        recyclingTargetRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(
            [
                new RecyclingTarget { Year = currentYear, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.1 },
                new RecyclingTarget { Year = currentYear, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.2 },
                new RecyclingTarget { Year = currentYear, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.3 },
                new RecyclingTarget { Year = currentYear, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.4 },
                new RecyclingTarget { Year = currentYear, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.5 },
                new RecyclingTarget { Year = currentYear , MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.6 },
                new RecyclingTarget { Year = currentYear, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.7 },
                new RecyclingTarget { Year = currentYear, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.8 }
            ]);

        var recyclingTargetDataService = new RecyclingTargetDataService(recyclingTargetRepositoryMock.Object);

        // Act
        var annualRecyclingTargets = await recyclingTargetDataService.GetRecyclingTargetsAsync();

        // Assert
        annualRecyclingTargets.Should().NotBeNullOrEmpty("we should have a non-empty dictionary of transformed recycling targets");
        annualRecyclingTargets.Should().BeEquivalentTo(
            new Dictionary<int, Dictionary<MaterialType, double>>
            {
                {
                    currentYear,
                    new Dictionary<MaterialType, double>
                    {
                        { MaterialType.Aluminium, 0.1 },
                        { MaterialType.GlassRemelt, 0.2 },
                        { MaterialType.Glass, 0.3 },
                        { MaterialType.Paper, 0.4 },
                        { MaterialType.Plastic, 0.5 },
                        { MaterialType.Steel, 0.6 },
                        { MaterialType.Wood, 0.7 },
                        { MaterialType.FibreComposite, 0.8 }
                    }
                }
            },
            "the transformed data should match the data from the repository"
        );
    }

    [TestMethod]
    public async Task GetRecyclingTargetsAsync_WhenInvokedTwice_RepositoryShouldBeUsedOnlyOnce()
    {
        // Arrange
        var targetYear = DateTime.UtcNow.Year;
        var recyclingTargetRepositoryMock = new Mock<IRecyclingTargetRepository>(MockBehavior.Strict);
        recyclingTargetRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(
            [
                new RecyclingTarget { Year = targetYear, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.1 },
                new RecyclingTarget { Year = targetYear, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.2 },
                new RecyclingTarget { Year = targetYear, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.3 },
                new RecyclingTarget { Year = targetYear, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.4 },
                new RecyclingTarget { Year = targetYear, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.5 },
                new RecyclingTarget { Year = targetYear, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.6 },
                new RecyclingTarget { Year = targetYear, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.7 },
                new RecyclingTarget { Year = targetYear, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.8 }
            ]);

        var recyclingTargetDataService = new RecyclingTargetDataService(recyclingTargetRepositoryMock.Object);

        // Act & Assert
        var annualRecyclingTargets = await recyclingTargetDataService.GetRecyclingTargetsAsync();
        annualRecyclingTargets.Should().NotBeNullOrEmpty("we should have a non-empty dictionary of transformed recycling targets");
        recyclingTargetRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);

        annualRecyclingTargets = await recyclingTargetDataService.GetRecyclingTargetsAsync();
        annualRecyclingTargets.Should().NotBeNullOrEmpty("we should have a non-empty dictionary of transformed recycling targets");
        recyclingTargetRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once, "the repository should not be queried again");

        recyclingTargetRepositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetRecyclingTargetsAsync_WhenMaterialNameIsInvalid_ShouldThrowArgumentException()
    {
        // Arrange
        var recyclingTargetRepositoryMock = new Mock<IRecyclingTargetRepository>(MockBehavior.Strict);
        recyclingTargetRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(
            [
                new RecyclingTarget { Year = 1, MaterialNameRT = "InvalidMaterial", Target = 0.1 }
            ]);

        var recyclingTargetDataService = new RecyclingTargetDataService(recyclingTargetRepositoryMock.Object);

        // Act
        Func<Task> act = async () => await recyclingTargetDataService.GetRecyclingTargetsAsync();

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Invalid material name 'InvalidMaterial' in recycling targets.");
    }

    [TestMethod]
    public async Task GetRecyclingTargetsAsync_WhenRepositoryReturnsNoData_ShouldReturnEmptyDictionary()
    {
        // Arrange
        var recyclingTargetRepositoryMock = new Mock<IRecyclingTargetRepository>(MockBehavior.Strict);
        recyclingTargetRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync([]);

        var recyclingTargetDataService = new RecyclingTargetDataService(recyclingTargetRepositoryMock.Object);

        // Act
        var annualRecyclingTargets = await recyclingTargetDataService.GetRecyclingTargetsAsync();

        // Assert
        annualRecyclingTargets.Should().BeEmpty("no data in repository should result in an empty dictionary");
        recyclingTargetRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [TestMethod]
    public async Task GetRecyclingTargetsAsync_WhenDataSpansMultipleYears_ShouldGroupByYearCorrectly()
    {
        // Arrange
        var recyclingTargetRepositoryMock = new Mock<IRecyclingTargetRepository>(MockBehavior.Strict);
        recyclingTargetRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(
            [
                new RecyclingTarget { Year = 1, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.1 },
                new RecyclingTarget { Year = 2, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.2 }
            ]);

        var recyclingTargetDataService = new RecyclingTargetDataService(recyclingTargetRepositoryMock.Object);

        // Act
        var annualRecyclingTargets = await recyclingTargetDataService.GetRecyclingTargetsAsync();

        // Assert
        annualRecyclingTargets.Should().BeEquivalentTo(
            new Dictionary<int, Dictionary<MaterialType, double>>
            {
                {
                    1,
                    new Dictionary<MaterialType, double>
                    {
                        { MaterialType.Aluminium, 0.1 }
                    }
                },
                {
                    2,
                    new Dictionary<MaterialType, double>
                    {
                        { MaterialType.Plastic, 0.2 }
                    }
                }
            },
            "the data should be grouped by year and transformed correctly"
        );
    }

    [TestMethod]
    public async Task GetRecyclingTargetsAsync_WhenRepositoryReturnsNull_ShouldReturnEmptyDictionary()
    {
        // Arrange
        var recyclingTargetRepositoryMock = new Mock<IRecyclingTargetRepository>(MockBehavior.Strict);
        recyclingTargetRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync((List<RecyclingTarget>)null);

        var recyclingTargetDataService = new RecyclingTargetDataService(recyclingTargetRepositoryMock.Object);

        // Act
        var annualRecyclingTargets = await recyclingTargetDataService.GetRecyclingTargetsAsync();

        // Assert
        annualRecyclingTargets.Should().BeEmpty("null response from repository should result in an empty dictionary");
        recyclingTargetRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [TestMethod]
    public void RecyclingTargetDataService_Constructor_ShouldInitializeWithEmptyDictionary()
    {
        // Arrange
        var recyclingTargetRepositoryMock = new Mock<IRecyclingTargetRepository>();

        // Act
        var recyclingTargetDataService = new RecyclingTargetDataService(recyclingTargetRepositoryMock.Object);

        // Assert
        var targetsField = typeof(RecyclingTargetDataService)
            .GetField("_recyclingTargets", BindingFlags.NonPublic | BindingFlags.Instance);
        var targetsValue = (Dictionary<int, Dictionary<MaterialType, double>>)targetsField.GetValue(recyclingTargetDataService);

        targetsValue.Should().NotBeNull("the dictionary should be initialized");
        targetsValue.Should().BeEmpty("the dictionary should be empty upon initialization");
    }
}

