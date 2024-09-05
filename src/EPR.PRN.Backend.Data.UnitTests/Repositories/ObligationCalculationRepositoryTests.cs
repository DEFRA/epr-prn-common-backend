using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories;

[ExcludeFromCodeCoverage]
[TestClass]
public class ObligationCalculationRepositoryTests
{
    private Mock<EprContext> _mockEprContext;
    private readonly int organisationId = 1;
    private readonly List<ObligationCalculation> obligationCalculation =
    [
        new ObligationCalculation { OrganisationId = 1, MaterialName = "Paper", MaterialObligationValue = 75 },
        new ObligationCalculation { OrganisationId = 1, MaterialName = "Glass", MaterialObligationValue = 75 },
        new ObligationCalculation { OrganisationId = 1, MaterialName = "Aluminium", MaterialObligationValue = 75 },
        new ObligationCalculation { OrganisationId = 1, MaterialName = "Steel", MaterialObligationValue = 75 },
        new ObligationCalculation { OrganisationId = 1, MaterialName = "Plastic", MaterialObligationValue = 75 }
    ];

    [TestInitialize]
    public void TestInitialize()
    {
        var dbContextOptions = new DbContextOptionsBuilder<EprContext>().Options;
        _mockEprContext = new Mock<EprContext>(dbContextOptions);
        _mockEprContext.Setup(context => context.ObligationCalculations).ReturnsDbSet(obligationCalculation);
        _mockEprContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
    }

    [TestMethod]
    public async Task GetObligationCalculationByOrganisationId_WhenCalledWithInvalidId_ReturnsEmpty()
    {
        // Arrange
        var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);
        int invalidOrganisationId = 2;

        // Act
        var result = await obligationCalculationRepository.GetObligationCalculationByOrganisationId(invalidOrganisationId);

        // Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetObligationCalculationByOrganisationId_ReturnsObligationCalculation()
    {
        // Arrange
        var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);

        // Act
        var result = await obligationCalculationRepository.GetObligationCalculationByOrganisationId(organisationId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(5);
        result.Should().Contain(x => x.MaterialName == "Paper");
        result.Should().Contain(x => x.MaterialName == "Glass");
        result.Should().Contain(x => x.MaterialName == "Aluminium");
        result.Should().Contain(x => x.MaterialName == "Steel");
        result.Should().Contain(x => x.MaterialName == "Plastic");
    }

    [TestMethod]
    public async Task AddObligationCalculation_WhenCalledWithObligationCalculations_ShouldSaveObligationCalculation()
    {
        // Arrange
        var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);

        List<ObligationCalculation> obligationCalculationAdd =
        [
            new ObligationCalculation { OrganisationId = 1, MaterialName = "Wood", MaterialObligationValue = 75 },
            new ObligationCalculation { OrganisationId = 1, MaterialName = "GlassRemelt", MaterialObligationValue = 75 }
        ];

        // Act
        await obligationCalculationRepository.AddObligationCalculation(obligationCalculationAdd);

        // Assert
        _mockEprContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}