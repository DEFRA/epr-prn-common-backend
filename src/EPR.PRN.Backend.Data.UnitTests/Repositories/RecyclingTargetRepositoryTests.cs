using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories;

[ExcludeFromCodeCoverage]
[TestClass]
public class RecyclingTargetRepositoryTests
{
    private Mock<EprContext> _mockEprContext;
    private readonly IEnumerable<RecyclingTarget> recyclingTarget =
        [
            new RecyclingTarget { MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.75, Year = 2025 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.77, Year = 2026 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.79, Year = 2027 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.81, Year = 2028 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.83, Year = 2029 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.85, Year = 2030 },

            new RecyclingTarget { MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.74, Year = 2025 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.76, Year = 2026 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.78, Year = 2027 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.80, Year = 2028 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.82, Year = 2029 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.85, Year = 2030 },

            new RecyclingTarget { MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.61, Year = 2025 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.62, Year = 2026 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.63, Year = 2027 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.64, Year = 2028 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.65, Year = 2029 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.67, Year = 2030 },

            new RecyclingTarget { MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.8, Year = 2025 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.81, Year = 2026 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.82, Year = 2027 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.83, Year = 2028 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.84, Year = 2029 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.85, Year = 2030 },

            new RecyclingTarget { MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.55, Year = 2025 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.57, Year = 2026 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.59, Year = 2027 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.61, Year = 2028 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.63, Year = 2029 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.65, Year = 2030 },

            new RecyclingTarget { MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.45, Year = 2025 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.46, Year = 2026 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.47, Year = 2027 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.48, Year = 2028 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.49, Year = 2029 },
            new RecyclingTarget { MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.50, Year = 2030 },

            new RecyclingTarget { MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.75, Year = 2025 },
            new RecyclingTarget { MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.76, Year = 2026 },
            new RecyclingTarget { MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.77, Year = 2027 },
            new RecyclingTarget { MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.78, Year = 2028 },
            new RecyclingTarget { MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.79, Year = 2029 },
            new RecyclingTarget { MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.80, Year = 2030 }
        ];

    [TestInitialize]
    public void TestInitialize()
    {
        var dbContextOptions = new DbContextOptionsBuilder<EprContext>().Options;
        _mockEprContext = new Mock<EprContext>(dbContextOptions);
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
        Assert.AreEqual(42, result.Count());
    }
}