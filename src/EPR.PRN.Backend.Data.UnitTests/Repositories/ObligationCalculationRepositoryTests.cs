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
	private readonly Guid organisationId = Guid.NewGuid();
	private readonly Guid drOrganisationId1 = Guid.NewGuid();
	private readonly Guid subOrganisationId1 = Guid.NewGuid();
	private readonly Guid drOrganisationId2 = Guid.NewGuid();
	private readonly Guid subOrganisationId2 = Guid.NewGuid();
	private readonly Guid drOrganisationId3 = Guid.NewGuid();
	private readonly Guid csSubmitterId = Guid.NewGuid();
	private readonly int currentYear = DateTime.UtcNow.Year;
	private readonly DateTime calculatedOn = DateTime.UtcNow;

	// This list will be used to mock the ObligationCalculation DbSet
	private List<ObligationCalculation> obligationCalculation;

    [TestInitialize]
    public void TestInitialize()
    {
        var dbContextOptions = new DbContextOptionsBuilder<EprContext>().Options;
        obligationCalculation =
        [
            new ObligationCalculation { OrganisationId = drOrganisationId1, MaterialId = 5, MaterialObligationValue = 15, Year = currentYear, SubmitterId = csSubmitterId, SubmitterTypeId = 1 },
            new ObligationCalculation { OrganisationId = drOrganisationId1, MaterialId = 6, MaterialObligationValue = 25, Year = currentYear, SubmitterId = csSubmitterId, SubmitterTypeId = 1 },
			new ObligationCalculation { OrganisationId = subOrganisationId1, MaterialId = 3, MaterialObligationValue = 35, Year = currentYear, SubmitterId = csSubmitterId, SubmitterTypeId = 1 },
			new ObligationCalculation { OrganisationId = subOrganisationId1, MaterialId = 4, MaterialObligationValue = 45, Year = currentYear, SubmitterId = csSubmitterId, SubmitterTypeId = 1 },
			new ObligationCalculation { OrganisationId = subOrganisationId1, MaterialId = 1, MaterialObligationValue = 55, Year = currentYear, SubmitterId = csSubmitterId, SubmitterTypeId = 1 },
			new ObligationCalculation { OrganisationId = drOrganisationId2, MaterialId = 5, MaterialObligationValue = 15, Year = currentYear, SubmitterId = drOrganisationId2, SubmitterTypeId = 2 },
			new ObligationCalculation { OrganisationId = drOrganisationId2, MaterialId = 6, MaterialObligationValue = 35, Year = currentYear, SubmitterId = drOrganisationId2, SubmitterTypeId = 2 },
			new ObligationCalculation { OrganisationId = drOrganisationId2, MaterialId = 3, MaterialObligationValue = 45, Year = currentYear, SubmitterId = drOrganisationId2, SubmitterTypeId = 2 },
			new ObligationCalculation { OrganisationId = subOrganisationId2, MaterialId = 4, MaterialObligationValue = 65, Year = currentYear, SubmitterId = drOrganisationId2, SubmitterTypeId = 2 },
			new ObligationCalculation { OrganisationId = subOrganisationId2, MaterialId = 2, MaterialObligationValue = 85, Year = currentYear, SubmitterId = drOrganisationId2, SubmitterTypeId = 2 },
			new ObligationCalculation { OrganisationId = subOrganisationId2, MaterialId = 1, MaterialObligationValue = 85, Year = currentYear, SubmitterId = drOrganisationId2, SubmitterTypeId = 2 },
			new ObligationCalculation { OrganisationId = drOrganisationId3, MaterialId = 1, MaterialObligationValue = 175, Year = currentYear, SubmitterId = drOrganisationId3, SubmitterTypeId = 2 }
		];
        _mockEprContext = new Mock<EprContext>(dbContextOptions);
        _mockEprContext.Setup(context => context.ObligationCalculations).ReturnsDbSet(obligationCalculation);
        _mockEprContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
    }

	[TestMethod]
	public async Task GetObligationCalculationBySubmitterIdAndYear_ReturnsEmpty_WhenCalledWithInvalidId()
	{
		// Arrange
		var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);

		// Act
		var result = await obligationCalculationRepository.GetObligationCalculationBySubmitterIdAndYear(Guid.NewGuid(), currentYear);

		// Assert
		result.Should().BeEmpty();
	}

	[TestMethod]
	public async Task GetObligationCalculationBySubmitterIdAndYear_ReturnsObligationCalculation_WhenCalledWithComplianceSchemeId()
	{
		// Arrange
		var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);

		// Act
		var result = await obligationCalculationRepository.GetObligationCalculationBySubmitterIdAndYear(csSubmitterId, currentYear);

		// Assert
		result.Should().NotBeNull();
		result.Count.Should().Be(5);
	}

	[TestMethod]
	public async Task GetObligationCalculationBySubmitterIdAndYear_ReturnsResults_WithSubsidiaries_WhenCalledWithDirectRegistrantId()
	{
		// Arrange
		var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);

		// Act
		var result = await obligationCalculationRepository.GetObligationCalculationBySubmitterIdAndYear(drOrganisationId2, currentYear);

		// Assert
		result.Should().NotBeNull();
		result.Count.Should().Be(6);
	}

	[TestMethod]
	public async Task GetObligationCalculationBySubmitterIdAndYear_ReturnsResults_WithNoSubsidiaries_WhenCalledWithDirectRegistrantId()
	{
		// Arrange
		var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);

		// Act
		var result = await obligationCalculationRepository.GetObligationCalculationBySubmitterIdAndYear(drOrganisationId3, currentYear);

		// Assert
		result.Should().NotBeNull();
		result.Count.Should().Be(1);
	}

    [TestMethod]
    public async Task RemoveAndAddObligationCalculationBySubmitterIdAsync_ShouldRemoveExistingCalculations_ThenInsertNewCalculations()
    {
		// Arrange
		var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);
		List<ObligationCalculation> obligationCalculationAdd =
		[
			new ObligationCalculation
			{
				OrganisationId = organisationId,
				MaterialId = 2,
				MaterialObligationValue = 75,
				Year = currentYear,
				Tonnage = 2000,
				CalculatedOn = calculatedOn,
				SubmitterId = csSubmitterId,
				SubmitterTypeId = 1
			},
			new ObligationCalculation
			{
				OrganisationId = organisationId,
				MaterialId = 7,
				MaterialObligationValue = 75,
				Year = currentYear,
				Tonnage = 20023,
				CalculatedOn = calculatedOn,
				SubmitterId = csSubmitterId,
				SubmitterTypeId = 1
			}
		];

		// Act
		await obligationCalculationRepository.RemoveAndAddObligationCalculationBySubmitterIdAsync(csSubmitterId, obligationCalculationAdd);
		
		// Assert
		_mockEprContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		_mockEprContext.Verify(x => x.ObligationCalculations.RemoveRange(It.IsAny<List<ObligationCalculation>>()), Times.Once);
		_mockEprContext.Verify(x => x.ObligationCalculations.AddRangeAsync(It.IsAny<List<ObligationCalculation>>(), It.IsAny<CancellationToken>()), Times.Once);
	}

	[TestMethod]
	public async Task RemoveAndAddObligationCalculationBySubmitterIdAsync_ShouldInsertNewCalculations_ButNoExistingCalculationsToRemove()
	{
		// Arrange
		var drOrganisationId = Guid.NewGuid();
		var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);
		List<ObligationCalculation> obligationCalculationAdd =
		[
			new ObligationCalculation
			{
				OrganisationId = Guid.NewGuid(),
				MaterialId = 4,
				MaterialObligationValue = 1050,
				Year = currentYear,
				Tonnage = 1500,
				CalculatedOn = calculatedOn,
				SubmitterId = drOrganisationId,
				SubmitterTypeId = 2
			},
			new ObligationCalculation
			{
				OrganisationId = Guid.NewGuid(),
				MaterialId = 3,
				MaterialObligationValue = 123,
				Year = currentYear,
				Tonnage = 200,
				CalculatedOn = calculatedOn,
				SubmitterId = drOrganisationId,
				SubmitterTypeId = 2
			}
		];

		// Act
		await obligationCalculationRepository.RemoveAndAddObligationCalculationBySubmitterIdAsync(drOrganisationId, obligationCalculationAdd);
		
		// Assert
		_mockEprContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		_mockEprContext.Verify(x => x.ObligationCalculations.RemoveRange(It.IsAny<List<ObligationCalculation>>()), Times.Never);
		_mockEprContext.Verify(x => x.ObligationCalculations.AddRangeAsync(It.IsAny<List<ObligationCalculation>>(), It.IsAny<CancellationToken>()), Times.Once);
	}

    [TestMethod]
    public async Task RemoveAndAddObligationCalculationBySubmitterIdAsync_NullCalculations_ShouldThrowArgumentException()
    {
        // Arrange
        var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object);

		// Act & Assert
		await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
			obligationCalculationRepository.RemoveAndAddObligationCalculationBySubmitterIdAsync(Guid.NewGuid(), null));
	}
}