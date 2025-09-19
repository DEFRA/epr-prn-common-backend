using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Data.Repositories;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories;

[TestClass]
public class ObligationCalculationRepositoryTests
{
    private Mock<EprContext> _mockEprContext;
    private IObligationCalculationUpdater _mockUpdater;
    private IObligationCalculationRepository _repository;
    private ILogger<ObligationCalculationRepository> _logger;

    private readonly Guid drOrganisationId1 = Guid.NewGuid();
    private readonly Guid subOrganisationId1 = Guid.NewGuid();
    private readonly Guid drOrganisationId2 = Guid.NewGuid();
    private readonly Guid subOrganisationId2 = Guid.NewGuid();
    private readonly Guid drOrganisationId3 = Guid.NewGuid();
    private readonly Guid csSubmitterId = Guid.NewGuid();
    private readonly int currentYear = DateTime.UtcNow.Year;

    // This list will be used to mock the ObligationCalculation DbSet
    private List<ObligationCalculation> obligationCalculations;

    [TestInitialize]
    public void TestInitialize()
    {
        obligationCalculations =
        [
            new ObligationCalculation { OrganisationId = drOrganisationId1, MaterialId = 5, MaterialObligationValue = 15, Year = currentYear, SubmitterId = csSubmitterId, SubmitterTypeId = 1, IsDeleted = false },
            new ObligationCalculation { OrganisationId = drOrganisationId1, MaterialId = 6, MaterialObligationValue = 25, Year = currentYear, SubmitterId = csSubmitterId, SubmitterTypeId = 1, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = subOrganisationId1, MaterialId = 3, MaterialObligationValue = 35, Year = currentYear, SubmitterId = csSubmitterId, SubmitterTypeId = 1, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = subOrganisationId1, MaterialId = 4, MaterialObligationValue = 45, Year = currentYear, SubmitterId = csSubmitterId, SubmitterTypeId = 1, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = subOrganisationId1, MaterialId = 1, MaterialObligationValue = 55, Year = currentYear, SubmitterId = csSubmitterId, SubmitterTypeId = 1, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = subOrganisationId1, MaterialId = 1, MaterialObligationValue = 55, Year = currentYear, SubmitterId = csSubmitterId, SubmitterTypeId = 1, IsDeleted = true  },
            new ObligationCalculation { OrganisationId = drOrganisationId2, MaterialId = 5, MaterialObligationValue = 15, Year = currentYear, SubmitterId = drOrganisationId2, SubmitterTypeId = 2, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = drOrganisationId2, MaterialId = 6, MaterialObligationValue = 35, Year = currentYear, SubmitterId = drOrganisationId2, SubmitterTypeId = 2, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = drOrganisationId2, MaterialId = 3, MaterialObligationValue = 45, Year = currentYear, SubmitterId = drOrganisationId2, SubmitterTypeId = 2, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = subOrganisationId2, MaterialId = 4, MaterialObligationValue = 65, Year = currentYear, SubmitterId = drOrganisationId2, SubmitterTypeId = 2, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = subOrganisationId2, MaterialId = 2, MaterialObligationValue = 85, Year = currentYear, SubmitterId = drOrganisationId2, SubmitterTypeId = 2, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = subOrganisationId2, MaterialId = 1, MaterialObligationValue = 85, Year = currentYear, SubmitterId = drOrganisationId2, SubmitterTypeId = 2, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = drOrganisationId3, MaterialId = 1, MaterialObligationValue = 175, Year = currentYear, SubmitterId = drOrganisationId3, SubmitterTypeId = 2, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = drOrganisationId3, MaterialId = 1, MaterialObligationValue = 175, Year = currentYear, SubmitterId = drOrganisationId3, SubmitterTypeId = 2, IsDeleted = true  },
            new ObligationCalculation { OrganisationId = drOrganisationId3, MaterialId = 2, MaterialObligationValue = 175, Year = currentYear, SubmitterId = drOrganisationId3, SubmitterTypeId = 2, IsDeleted = true  },
        ];

        _logger = new LoggerFactory().CreateLogger<ObligationCalculationRepository>();

        var options = new DbContextOptionsBuilder<EprContext>().Options;
        _mockEprContext = new Mock<EprContext>(options);
        _mockEprContext.Setup(c => c.ObligationCalculations).ReturnsDbSet(obligationCalculations);
        _mockEprContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _mockUpdater = A.Fake<IObligationCalculationUpdater>();

        var mockDatabase = new Mock<DatabaseFacade>(_mockEprContext.Object);

        var mockTransaction = new Mock<IDbContextTransaction>();
        mockDatabase.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockTransaction.Object);
        mockTransaction.Setup(t => t.DisposeAsync()).Returns(ValueTask.CompletedTask);

        _mockEprContext.Setup(c => c.Database).Returns(mockDatabase.Object);
        _repository = new ObligationCalculationRepository(_mockEprContext.Object, _logger, _mockUpdater);
    }

    [TestMethod]
    public async Task GetObligationCalculationBySubmitterIdAndYear_ReturnsEmpty_WhenCalledWithInvalidId()
    {
        // Act
        var result = await _repository.GetObligationCalculationBySubmitterIdAndYear(Guid.NewGuid(), currentYear);

        // Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetObligationCalculationBySubmitterIdAndYear_ReturnsObligationCalculation_WhenCalledWithComplianceSchemeId()
    {
        // Act
        var result = await _repository.GetObligationCalculationBySubmitterIdAndYear(csSubmitterId, currentYear);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(5);
    }

    [TestMethod]
    public async Task GetObligationCalculationBySubmitterIdAndYear_ReturnsResults_WithSubsidiaries_WhenCalledWithDirectRegistrantId()
    {
        // Act
        var result = await _repository.GetObligationCalculationBySubmitterIdAndYear(drOrganisationId2, currentYear);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(6);
    }

    [TestMethod]
    public async Task GetObligationCalculationBySubmitterIdAndYear_ReturnsResults_WithNoSubsidiaries_WhenCalledWithDirectRegistrantId()
    {
        // Act
        var result = await _repository.GetObligationCalculationBySubmitterIdAndYear(drOrganisationId3, currentYear);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(1);
    }

	[TestMethod]
	public async Task SoftDeleteAndAddObligationCalculationBySubmitterIdAsync_ShouldCallUpdaterAndAddNewCalculations()
	{
		// Arrange
		var newCalculations = new List<ObligationCalculation>
	    {
		    new() { SubmitterId = csSubmitterId, Year = currentYear, MaterialId = 4 },
		    new() { SubmitterId = csSubmitterId, Year = currentYear, MaterialId = 5 }
	    };

		var mockTransaction = new Mock<IDbContextTransaction>();
		_mockEprContext.Setup(c => c.Database.BeginTransactionAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(mockTransaction.Object);

		var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object, _logger, _mockUpdater);

		// Act
		await obligationCalculationRepository.UpsertObligationCalculationsForSubmitterYearAsync(csSubmitterId, currentYear, newCalculations);

		// Assert

		A.CallTo(() => _mockUpdater.SoftDeleteBySubmitterAndYearAsync(csSubmitterId, currentYear))
			.MustHaveHappenedOnceExactly();
		_mockEprContext.Verify(c => c.ObligationCalculations.AddRangeAsync(newCalculations, It.IsAny<CancellationToken>()), Moq.Times.Once);
		_mockEprContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Moq.Times.Once);
		mockTransaction.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Moq.Times.Once);
	}

	[TestMethod]
	public async Task SoftDeleteAndAddObligationCalculationBySubmitterIdAsync_ShouldWork_WhenNoExistingCalculations()
	{
		// Arrange
		var newSubmitterId = Guid.NewGuid();
		var newCalculations = new List<ObligationCalculation>
	    {
		    new() { SubmitterId = newSubmitterId, Year = currentYear, MaterialId = 10 }
	    };

		var mockTransaction = new Mock<IDbContextTransaction>();
		_mockEprContext.Setup(c => c.Database.BeginTransactionAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(mockTransaction.Object);

		var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object, _logger, _mockUpdater);

		// Act
		await obligationCalculationRepository.UpsertObligationCalculationsForSubmitterYearAsync(newSubmitterId, currentYear, newCalculations);

		// Assert
		A.CallTo(() => _mockUpdater.SoftDeleteBySubmitterAndYearAsync(newSubmitterId, currentYear))
			.MustHaveHappenedOnceExactly();
		_mockEprContext.Verify(c => c.ObligationCalculations.AddRangeAsync(newCalculations, It.IsAny<CancellationToken>()), Moq.Times.Once);
		_mockEprContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Moq.Times.Once);
		mockTransaction.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Moq.Times.Once);
	}

	[TestMethod]
	public async Task SoftDeleteAndAddObligationCalculationBySubmitterIdAsync_ShouldRollback_WhenExceptionOccurs()
	{
		// Arrange
		var newCalculations = new List<ObligationCalculation>
	    {
		    new() { SubmitterId = csSubmitterId, Year = currentYear, MaterialId = 99 }
	    };

		var mockTransaction = new Mock<IDbContextTransaction>();
		_mockEprContext.Setup(c => c.Database.BeginTransactionAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(mockTransaction.Object);

		// Simulate exception on SaveChangesAsync
		_mockEprContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
			.ThrowsAsync(new InvalidOperationException("Database Errror"));

		var obligationCalculationRepository = new ObligationCalculationRepository(_mockEprContext.Object, _logger, _mockUpdater);

		// Act & Assert
		await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
			await obligationCalculationRepository.UpsertObligationCalculationsForSubmitterYearAsync(csSubmitterId, currentYear, newCalculations));

		// Verify rollback called
		mockTransaction.Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Moq.Times.Once);

		// Verify commit NOT called
		mockTransaction.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Moq.Times.Never);
	}
}