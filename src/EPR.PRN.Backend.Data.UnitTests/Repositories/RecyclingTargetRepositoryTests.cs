using EPR.PRN.Backend.Data.DataModels;
using Moq;
using Moq.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories.Tests
{
    [TestClass]
    public class RecyclingTargetRepositoryTests
    {
        private Mock<EprContext> _mockEprContext;
        private IEnumerable<RecyclingTarget> recyclingTarget = new List<RecyclingTarget>
            {
                new RecyclingTarget { Year = 2025, PaperTarget = 0.75, GlassTarget = 0.74, AluminiumTarget = 0.61, SteelTarget = 0.8, PlasticTarget = 0.55, WoodTarget = 0.45, GlassRemeltTarget = 0.75 },
                new RecyclingTarget { Year = 2026, PaperTarget = 0.77, GlassTarget = 0.76, AluminiumTarget = 0.62, SteelTarget = 0.81, PlasticTarget = 0.57, WoodTarget = 0.46, GlassRemeltTarget = 0.76 },
                new RecyclingTarget { Year = 2027, PaperTarget = 0.79, GlassTarget = 0.78, AluminiumTarget = 0.63, SteelTarget = 0.82, PlasticTarget = 0.59, WoodTarget = 0.47, GlassRemeltTarget = 0.77 },
                new RecyclingTarget { Year = 2028, PaperTarget = 0.81, GlassTarget = 0.80, AluminiumTarget = 0.64, SteelTarget = 0.83, PlasticTarget = 0.61, WoodTarget = 0.48, GlassRemeltTarget = 0.78 },
                new RecyclingTarget { Year = 2029, PaperTarget = 0.83, GlassTarget = 0.82, AluminiumTarget = 0.65, SteelTarget = 0.84, PlasticTarget = 0.63, WoodTarget = 0.49, GlassRemeltTarget = 0.79 },
                new RecyclingTarget { Year = 2030, PaperTarget = 0.85, GlassTarget = 0.85, AluminiumTarget = 0.67, SteelTarget = 0.85, PlasticTarget = 0.65, WoodTarget = 0.50, GlassRemeltTarget = 0.80 }
            };

        [TestInitialize]
        public void TestInitialize()
        {
            _mockEprContext = new Mock<EprContext>();
            _mockEprContext.Setup(context => context.RecyclingTargets).ReturnsDbSet(recyclingTarget);
        }

        [TestMethod]
        public async Task GetAllAsync_WhenCalled_ShouldReturnAllRecyclingTargets()
        {
            // Arrange
            var recyclingTargetRepository = new RecyclingTargetRepository(_mockEprContext.Object);

            // Act
            var result = await recyclingTargetRepository.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Count());
        }
    }
}