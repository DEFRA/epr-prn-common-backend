using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories;

[TestClass]
public class ObligationCalculationRepositoryTests
{
    private Mock<EprContext> _mockEprContext;
    private Guid organisationId = Guid.NewGuid();
    private List<ObligationCalculation> obligationCalculation;

    [TestInitialize]
    public void TestInitialize()
    {
        var dbContextOptions = new DbContextOptionsBuilder<EprContext>().Options;
        obligationCalculation =
        [
            new ObligationCalculation { OrganisationId = organisationId, MaterialName = "Paper", MaterialObligationValue = 75, Year = 2024 },
            new ObligationCalculation { OrganisationId = organisationId, MaterialName = "Glass", MaterialObligationValue = 75, Year = 2024 },
            new ObligationCalculation { OrganisationId = organisationId, MaterialName = "Aluminium", MaterialObligationValue = 75, Year = 2024 },
            new ObligationCalculation { OrganisationId = organisationId, MaterialName = "Steel", MaterialObligationValue = 75, Year = 2024 },
            new ObligationCalculation { OrganisationId = organisationId, MaterialName = "Plastic", MaterialObligationValue = 75, Year = 2024 }
        ];
        _mockEprContext = new Mock<EprContext>(dbContextOptions);
        _mockEprContext.Setup(context => context.ObligationCalculations).ReturnsDbSet(obligationCalculation);
        _mockEprContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
    }

    [TestMethod]
    public async Task GetObligationCalculationByOrganisationId_WhenCalledWithInvalidId_ReturnsEmpty()
    {
        // Arrange
        var invalidOrganisationId = Guid.NewGuid();
        var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);
        var year = 2024;

        // Act
        var result = await obligationCalculationRepository.GetObligationCalculation(invalidOrganisationId, year);

        // Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetObligationCalculationByOrganisationId_ReturnsObligationCalculation()
    {
        // Arrange
        var year = 2024;
        var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);

        // Act
        var result = await obligationCalculationRepository.GetObligationCalculation(organisationId, year);

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
        var organisationId = Guid.NewGuid();
        var calculatedOn = DateTime.UtcNow;
        var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);

        List<ObligationCalculation> obligationCalculationAdd =
        [
            new ObligationCalculation
            {
                OrganisationId = organisationId,
                MaterialName = "Wood",
                MaterialObligationValue = 75,
                Year = 2024,
                MaterialWeight = 2000,
                CalculatedOn = calculatedOn
            },
            new ObligationCalculation
            {
                OrganisationId = organisationId,
                MaterialName = "GlassRemelt",
                MaterialObligationValue = 75,
                Year = 2024,
                MaterialWeight = 20023,
                CalculatedOn = calculatedOn
            }
        ];

        // Act
        await obligationCalculationRepository.AddObligationCalculation(obligationCalculationAdd);

        // Assert
        _mockEprContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}