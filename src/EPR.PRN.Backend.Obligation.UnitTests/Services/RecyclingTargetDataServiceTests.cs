using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.Services;
using FluentAssertions;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Obligation.UnitTests.Services;

[ExcludeFromCodeCoverage]
[TestClass]
public class RecyclingTargetDataServiceTests
{
    [TestMethod]
    public async Task GetRecyclingTargetsAsync_WhenInvoked_ReturnsRecyclingTargets()
    {
        // Arrange
        var recyclingTargetRepositoryMock = new Mock<IRecyclingTargetRepository>(MockBehavior.Strict);
        recyclingTargetRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<RecyclingTarget>
            {
                new RecyclingTarget { Year = 1, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.1 },
                new RecyclingTarget { Year = 1, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.2 },
                new RecyclingTarget { Year = 1, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.3 },
                new RecyclingTarget { Year = 1, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.4 },
                new RecyclingTarget { Year = 1, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.5 },
                new RecyclingTarget { Year = 1, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.6 },
                new RecyclingTarget { Year = 1, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.7 }
            });

        var recyclingTargetDataService = new RecyclingTargetDataService(recyclingTargetRepositoryMock.Object);

        // Act
        var annualRecyclingTargets = await recyclingTargetDataService.GetRecyclingTargetsAsync();

        // Assert
        annualRecyclingTargets.Should().NotBeNullOrEmpty("we should have a non-empty dictionary of transformed recycling targets");
        annualRecyclingTargets.Should().BeEquivalentTo(
            new Dictionary<int, Dictionary<MaterialType, double>>
            {
                { 1, new Dictionary<MaterialType, double>
                    {
                        { MaterialType.Aluminium, 0.1 },
                        { MaterialType.GlassRemelt, 0.2 },
                        { MaterialType.Glass, 0.3 },
                        { MaterialType.Paper, 0.4 },
                        { MaterialType.Plastic, 0.5 },
                        { MaterialType.Steel, 0.6 },
                        { MaterialType.Wood, 0.7 }
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
        var recyclingTargetRepositoryMock = new Mock<IRecyclingTargetRepository>(MockBehavior.Strict);
        recyclingTargetRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<RecyclingTarget>
            {
                new RecyclingTarget { Year = 1, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.1 },
                new RecyclingTarget { Year = 1, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.2 },
                new RecyclingTarget { Year = 1, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.3 },
                new RecyclingTarget { Year = 1, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.4 },
                new RecyclingTarget { Year = 1, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.5 },
                new RecyclingTarget { Year = 1, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.6 },
                new RecyclingTarget { Year = 1, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.7 }
            });

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
}

