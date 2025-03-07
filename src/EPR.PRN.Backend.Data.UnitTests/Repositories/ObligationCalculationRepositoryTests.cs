using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories;

[TestClass]
public class ObligationCalculationRepositoryTests
{
    private Mock<EprContext> _mockEprContext;
    private Guid organisationId = Guid.NewGuid();
    private List<ObligationCalculation> obligationCalculation;
    private List<Guid> organisationIds = [];

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

        organisationIds.Add(Guid.NewGuid());
        organisationIds.Add(Guid.NewGuid());
    }

    [TestMethod]
    public async Task GetObligationCalculationByOrganisationId_WhenCalledWithInvalidId_ReturnsEmpty()
    {
        // Arrange
        var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);
        var year = 2024;

        // Act
        var result = await obligationCalculationRepository.GetObligationCalculation(organisationIds, year);

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
        var result = await obligationCalculationRepository.GetObligationCalculation(organisationIds, year);

        // Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public async Task AddObligationCalculation_WhenCalledWithObligationCalculations_ShouldSaveObligationCalculation()
    {
        // Arrange
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
                Tonnage = 2000,
                CalculatedOn = calculatedOn
            },
            new ObligationCalculation
            {
                OrganisationId = organisationId,
                MaterialName = "GlassRemelt",
                MaterialObligationValue = 75,
                Year = 2024,
                Tonnage = 20023,
                CalculatedOn = calculatedOn
            }
        ];

        // Act
        await obligationCalculationRepository.AddObligationCalculation(obligationCalculationAdd);

        // Assert
        _mockEprContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task UpsertObligationCalculationAsync_AllNewCalculations_ShouldInsertAll()
    {
        // Arrange
        var newOrganisationId = Guid.NewGuid();
        var calculatedOn = DateTime.UtcNow;
        var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);

        List<ObligationCalculation> newCalculations = new List<ObligationCalculation>
        {
            new ObligationCalculation
            {
                OrganisationId = newOrganisationId,
                MaterialName = "Metal",
                MaterialObligationValue = 50,
                Year = 2024,
                Tonnage = 5000,
                CalculatedOn = calculatedOn
            },
            new ObligationCalculation
            {
                OrganisationId = newOrganisationId,
                MaterialName = "Wood",
                MaterialObligationValue = 30,
                Year = 2024,
                Tonnage = 3000,
                CalculatedOn = calculatedOn
            }
        };

        // Act
        await obligationCalculationRepository.UpsertObligationCalculationAsync(newOrganisationId, newCalculations);

        // Assert
        _mockEprContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockEprContext.Verify(x => x.ObligationCalculations.AddRange(It.IsAny<List<ObligationCalculation>>()), Times.Once);
    }

    [TestMethod]
    public async Task UpsertObligationCalculationAsync_AllExistingCalculations_ShouldUpdateAll()
    {
        // Arrange
        var calculatedOn = DateTime.UtcNow;
        var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);

        List<ObligationCalculation> updatedCalculations = new List<ObligationCalculation>
    {
        new ObligationCalculation
        {
            OrganisationId = organisationId,
            MaterialName = "Paper",
            MaterialObligationValue = 80, // Updated value
            Year = 2024,
            Tonnage = 1200,
            CalculatedOn = calculatedOn
        },
        new ObligationCalculation
        {
            OrganisationId = organisationId,
            MaterialName = "Glass",
            MaterialObligationValue = 90, // Updated value
            Year = 2024,
            Tonnage = 2500,
            CalculatedOn = calculatedOn
        }
    };

        // Act
        await obligationCalculationRepository.UpsertObligationCalculationAsync(organisationId, updatedCalculations);

        // Assert

        _mockEprContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task UpsertObligationCalculationAsync_MixedCalculations_ShouldUpdateAndInsert()
    {
        // Arrange
        var calculatedOn = DateTime.UtcNow;
        var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);

        List<ObligationCalculation> mixedCalculations = new List<ObligationCalculation>
        {
            new ObligationCalculation
            {
                OrganisationId = organisationId,
                MaterialName = "Paper", // Existing
                MaterialObligationValue = 100, // Updated value
                Year = 2024,
                Tonnage = 1500,
                CalculatedOn = calculatedOn
            },
            new ObligationCalculation
            {
                OrganisationId = organisationId,
                MaterialName = "Plastic", // Existing
                MaterialObligationValue = 80, // Updated value
                Year = 2024,
                Tonnage = 2100,
                CalculatedOn = calculatedOn
            },
            new ObligationCalculation
            {
                OrganisationId = organisationId,
                MaterialName = "Copper", // New
                MaterialObligationValue = 70,
                Year = 2024,
                Tonnage = 3000,
                CalculatedOn = calculatedOn
            }
        };

        // Act
        await obligationCalculationRepository.UpsertObligationCalculationAsync(organisationId, mixedCalculations);

        // Assert
        _mockEprContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockEprContext.Verify(x => x.ObligationCalculations.AddRange(It.Is<IEnumerable<ObligationCalculation>>(c => c.Any(cal => cal.MaterialName == "Copper"))), Times.Once);
    }

    [TestMethod]
    public async Task UpsertObligationCalculationAsync_NullCalculations_ShouldThrowArgumentException()
    {
        // Arrange
        var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
            obligationCalculationRepository.UpsertObligationCalculationAsync(organisationId, null));
    }
}