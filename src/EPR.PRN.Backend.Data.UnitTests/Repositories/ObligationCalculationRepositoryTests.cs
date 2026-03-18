using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Repositories;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories;

[TestClass]
public class ObligationCalculationRepositoryTests
{
    private readonly Guid _drOrganisationId1 = Guid.NewGuid();
    private readonly Guid _subOrganisationId1 = Guid.NewGuid();
    private readonly Guid _drOrganisationId2 = Guid.NewGuid();
    private readonly Guid _subOrganisationId2 = Guid.NewGuid();
    private readonly Guid _drOrganisationId3 = Guid.NewGuid();
    private readonly Guid _csSubmitterId = Guid.NewGuid();
    private readonly int _currentYear = DateTime.UtcNow.Year;
    private readonly int _nextYear = DateTime.UtcNow.Year + 1;

    private List<ObligationCalculation> _obligationCalculations;
    
    private EprContext _context;
    private ObligationCalculationRepository _repository;

    [TestInitialize]
    public void TestInitialize()
    {
        _obligationCalculations =
        [
	        // The .Id values of all records below will be sequential starting from 1
	        // ID values are used in some of these tests
	        
	        // submitter: csSubmitterId
            new ObligationCalculation { OrganisationId = _drOrganisationId1, MaterialId = 5, MaterialObligationValue = 15, Year = _currentYear, SubmitterId = _csSubmitterId, SubmitterTypeId = 1, IsDeleted = false },
            new ObligationCalculation { OrganisationId = _drOrganisationId1, MaterialId = 6, MaterialObligationValue = 25, Year = _currentYear, SubmitterId = _csSubmitterId, SubmitterTypeId = 1, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = _subOrganisationId1, MaterialId = 3, MaterialObligationValue = 35, Year = _currentYear, SubmitterId = _csSubmitterId, SubmitterTypeId = 1, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = _subOrganisationId1, MaterialId = 4, MaterialObligationValue = 45, Year = _currentYear, SubmitterId = _csSubmitterId, SubmitterTypeId = 1, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = _subOrganisationId1, MaterialId = 1, MaterialObligationValue = 55, Year = _currentYear, SubmitterId = _csSubmitterId, SubmitterTypeId = 1, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = _subOrganisationId1, MaterialId = 1, MaterialObligationValue = 55, Year = _currentYear, SubmitterId = _csSubmitterId, SubmitterTypeId = 1, IsDeleted = true  },
	        new ObligationCalculation { OrganisationId = _drOrganisationId1, MaterialId = 5, MaterialObligationValue = 15, Year = _nextYear, SubmitterId = _csSubmitterId, SubmitterTypeId = 1, IsDeleted = false },
	        
	        // submitter: drOrganisationId2
            new ObligationCalculation { OrganisationId = _drOrganisationId2, MaterialId = 5, MaterialObligationValue = 15, Year = _currentYear, SubmitterId = _drOrganisationId2, SubmitterTypeId = 2, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = _drOrganisationId2, MaterialId = 6, MaterialObligationValue = 35, Year = _currentYear, SubmitterId = _drOrganisationId2, SubmitterTypeId = 2, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = _drOrganisationId2, MaterialId = 3, MaterialObligationValue = 45, Year = _currentYear, SubmitterId = _drOrganisationId2, SubmitterTypeId = 2, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = _subOrganisationId2, MaterialId = 4, MaterialObligationValue = 65, Year = _currentYear, SubmitterId = _drOrganisationId2, SubmitterTypeId = 2, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = _subOrganisationId2, MaterialId = 2, MaterialObligationValue = 85, Year = _currentYear, SubmitterId = _drOrganisationId2, SubmitterTypeId = 2, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = _subOrganisationId2, MaterialId = 1, MaterialObligationValue = 85, Year = _currentYear, SubmitterId = _drOrganisationId2, SubmitterTypeId = 2, IsDeleted = false  },
	        
	        // submitter: drOrganisationId3
            new ObligationCalculation { OrganisationId = _drOrganisationId3, MaterialId = 1, MaterialObligationValue = 175, Year = _currentYear, SubmitterId = _drOrganisationId3, SubmitterTypeId = 2, IsDeleted = false  },
            new ObligationCalculation { OrganisationId = _drOrganisationId3, MaterialId = 1, MaterialObligationValue = 175, Year = _currentYear, SubmitterId = _drOrganisationId3, SubmitterTypeId = 2, IsDeleted = true  },
            new ObligationCalculation { OrganisationId = _drOrganisationId3, MaterialId = 2, MaterialObligationValue = 175, Year = _currentYear, SubmitterId = _drOrganisationId3, SubmitterTypeId = 2, IsDeleted = true  },
        ];
        
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<EprContext>().UseSqlite(connection).Options;

        using (var context = new EprContext(options))
        {
	        context.Database.EnsureCreated();
	        context.ObligationCalculations.AddRange(_obligationCalculations);
	        context.SaveChanges();
        }

        _context = new EprContext(options);
        _repository = new ObligationCalculationRepository(_context, NullLogger<ObligationCalculationRepository>.Instance);
    }

    [TestMethod]
    public async Task GetObligationCalculationBySubmitterIdAndYear_ReturnsEmpty_WhenCalledWithInvalidId()
    {
        var result = await _repository.GetObligationCalculationBySubmitterIdAndYear(Guid.NewGuid(), _currentYear);

        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetObligationCalculationBySubmitterIdAndYear_ReturnsObligationCalculation_WhenCalledWithComplianceSchemeId()
    {
        var result = await _repository.GetObligationCalculationBySubmitterIdAndYear(_csSubmitterId, _currentYear);

        result.Should().NotBeNull();
        result.Count.Should().Be(5);
    }

    [TestMethod]
    public async Task GetObligationCalculationBySubmitterIdAndYear_ReturnsResults_WithSubsidiaries_WhenCalledWithDirectRegistrantId()
    {
        var result = await _repository.GetObligationCalculationBySubmitterIdAndYear(_drOrganisationId2, _currentYear);

        result.Should().NotBeNull();
        result.Count.Should().Be(6);
    }

    [TestMethod]
    public async Task GetObligationCalculationBySubmitterIdAndYear_ReturnsResults_WithNoSubsidiaries_WhenCalledWithDirectRegistrantId()
    {
        var result = await _repository.GetObligationCalculationBySubmitterIdAndYear(_drOrganisationId3, _currentYear);

        result.Should().NotBeNull();
        result.Count.Should().Be(1);
    }

    [TestMethod]
    public async Task UpsertObligationCalculationsForSubmitterYearAsync_NoNewCalculation_ShouldRetainExistingRecords()
    {
	    await _repository.UpsertObligationCalculationsForSubmitterYearAsync(_csSubmitterId, _currentYear, []);

	    _context.ObligationCalculations.Count().Should().Be(_obligationCalculations.Count);
    }

    [TestMethod]
    public async Task UpsertObligationCalculationsForSubmitterYearAsync_MatchingCalculation_ShouldUpdateRecord()
    {
	    var newCalculations = new List<ObligationCalculation>
	    {
		    new()
		    {
			    OrganisationId = _subOrganisationId1, 
			    MaterialId = 4,
			    MaterialObligationValue = 100, 
			    Year = _currentYear,
			    CalculatedOn = new DateTime(2026, 3, 18),
			    Tonnage = 200,
			    SubmitterId = _csSubmitterId,
			    SubmitterTypeId = 1
		    }
	    };

	    await _repository.UpsertObligationCalculationsForSubmitterYearAsync(_csSubmitterId, _currentYear, newCalculations);

	    var calculation = await GetCalculation(_obligationCalculations[3].Id);

	    calculation.Should().BeEquivalentTo(new
	    {
		    OrganisationId = _subOrganisationId1,
		    MaterialId = 4,
		    MaterialObligationValue = 100, 
		    Year = _currentYear,
		    CalculatedOn = new DateTime(2026, 3, 18),
		    Tonnage = 200,
		    SubmitterId = _csSubmitterId,
		    SubmitterTypeId = 1
	    });

	    await CalculationIsDeletedShouldBe([
		    _obligationCalculations[0].Id, 
		    _obligationCalculations[1].Id, 
		    _obligationCalculations[2].Id, 
		    _obligationCalculations[4].Id, 
		    _obligationCalculations[5].Id], true);
    }

    [TestMethod]
    public async Task UpsertObligationCalculationsForSubmitterYearAsync_NoMatchingCalculation_ShouldCreateRecord()
    {
	    var newCalculations = new List<ObligationCalculation>
	    {
		    new()
		    {
			    OrganisationId = _subOrganisationId1, 
			    MaterialId = 4,
			    MaterialObligationValue = 100, 
			    Year = _nextYear,
			    CalculatedOn = new DateTime(2026, 3, 18),
			    Tonnage = 200,
			    SubmitterId = _csSubmitterId,
			    SubmitterTypeId = 1
		    }
	    };

	    await _repository.UpsertObligationCalculationsForSubmitterYearAsync(_csSubmitterId, _nextYear, newCalculations);

	    var newRecordId = _obligationCalculations.Count + 1;
	    var calculation = await GetCalculation(newRecordId);

	    calculation.Should().NotBeNull();
	    calculation.Should().BeEquivalentTo(new
	    {
		    OrganisationId = _subOrganisationId1,
		    MaterialId = 4,
		    MaterialObligationValue = 100, 
		    Year = _nextYear,
		    CalculatedOn = new DateTime(2026, 3, 18),
		    Tonnage = 200,
		    SubmitterId = _csSubmitterId,
		    SubmitterTypeId = 1
	    });

	    // IsDeleted state should be unchanged from initial seed
	    await CalculationIsDeletedShouldBe([
		    _obligationCalculations[0].Id, 
		    _obligationCalculations[1].Id, 
		    _obligationCalculations[2].Id, 
		    _obligationCalculations[3].Id, 
		    _obligationCalculations[4].Id], false);
	    await CalculationIsDeletedShouldBe(
		    _obligationCalculations[5].Id, true);
    }

    private async Task<ObligationCalculation> GetCalculation(int id)
    {
	    return await _context.ObligationCalculations.FirstOrDefaultAsync(x => x.Id == id, CancellationToken.None);
    }

    private async Task CalculationIsDeletedShouldBe(int id, bool isDeleted)
    {
	    (await GetCalculation(id)).IsDeleted.Should().Be(isDeleted, $"Record id {id}");
    }

    private async Task CalculationIsDeletedShouldBe(int[] ids, bool isDeleted)
    {
	    foreach (var id in ids)
		    await CalculationIsDeletedShouldBe(id, isDeleted);
    }
}