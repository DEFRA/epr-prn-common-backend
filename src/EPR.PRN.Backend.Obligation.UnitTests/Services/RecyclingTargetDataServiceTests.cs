using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.Enums;
using EPR.PRN.Backend.Obligation.Models;
using EPR.PRN.Backend.Obligation.Services;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.Obligation.UnitTests.Services
{
    [TestClass]
    public class RecyclingTargetDataServiceTests
    {
        [TestMethod]
        public async Task GetRecyclingTargetsAsync_WhenInvoked_ReturnsRecyclingTargets()
        {
            var recyclingTargetRepositoryMock = new Mock<IRecyclingTargetRepository>(MockBehavior.Strict);
            recyclingTargetRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync([new() { Year = 1, AluminiumTarget = 0.1, GlassRemeltTarget = 0.2, GlassTarget = 0.3, PaperTarget = 0.4, PlasticTarget = 0.5, SteelTarget = 0.6, WoodTarget = 0.7 }]);

            var recyclingTargetDataService = new RecyclingTargetDataService(recyclingTargetRepositoryMock.Object);

            var annualRecyclingTargets = await recyclingTargetDataService.GetRecyclingTargetsAsync();

            annualRecyclingTargets.Should().NotBeNullOrEmpty("we should have a non empty array of transformed recycling targets");
            annualRecyclingTargets.Should().BeEquivalentTo(new AnnualRecyclingTargets[] { new() { Year = 1, Targets = new Dictionary<MaterialType, double> { { MaterialType.Aluminium, 0.1 }, { MaterialType.Glass, 0.3 }, { MaterialType.GlassRemelt, 0.2 }, { MaterialType.Paper, 0.4 }, { MaterialType.Plastic, 0.5 }, { MaterialType.Steel, 0.6 }, { MaterialType.Wood, 0.7 } } } }, "the transformed data should match the data from the repository");
        }

        [TestMethod]
        public async Task GetRecyclingTargetsAsync_WhenInvokedTwice_RepositoryShouldBeUsedOnlyOnce()
        {
            var recyclingTargetRepositoryMock = new Mock<IRecyclingTargetRepository>(MockBehavior.Strict);
            recyclingTargetRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync([new() { Year = 1, AluminiumTarget = 0.1, GlassRemeltTarget = 0.2, GlassTarget = 0.3, PaperTarget = 0.4, PlasticTarget = 0.5, SteelTarget = 0.6, WoodTarget = 0.7 }]);

            var recyclingTargetDataService = new RecyclingTargetDataService(recyclingTargetRepositoryMock.Object);

            var annualRecyclingTargets = await recyclingTargetDataService.GetRecyclingTargetsAsync();
            annualRecyclingTargets.Should().NotBeNullOrEmpty("we should have a non empty array of transformed recycling targets");
            annualRecyclingTargets.Should().BeEquivalentTo(new AnnualRecyclingTargets[] { new() { Year = 1, Targets = new Dictionary<MaterialType, double> { { MaterialType.Aluminium, 0.1 }, { MaterialType.Glass, 0.3 }, { MaterialType.GlassRemelt, 0.2 }, { MaterialType.Paper, 0.4 }, { MaterialType.Plastic, 0.5 }, { MaterialType.Steel, 0.6 }, { MaterialType.Wood, 0.7 } } } }, "the transformed data should match the data from the repository");

            recyclingTargetRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);

            annualRecyclingTargets = await recyclingTargetDataService.GetRecyclingTargetsAsync();
            annualRecyclingTargets.Should().NotBeNullOrEmpty("we should have a non empty array of transformed recycling targets");
            annualRecyclingTargets.Should().BeEquivalentTo(new AnnualRecyclingTargets[] { new() { Year = 1, Targets = new Dictionary<MaterialType, double> { { MaterialType.Aluminium, 0.1 }, { MaterialType.Glass, 0.3 }, { MaterialType.GlassRemelt, 0.2 }, { MaterialType.Paper, 0.4 }, { MaterialType.Plastic, 0.5 }, { MaterialType.Steel, 0.6 }, { MaterialType.Wood, 0.7 } } } }, "the transformed data should match the data from the repository");

            recyclingTargetRepositoryMock.VerifyNoOtherCalls();
        }
    }
}
