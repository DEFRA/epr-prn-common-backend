using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Repositories.Accreditations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories.Accreditations;

[TestClass]
public class AccreditationRepositoryTests
{
    private EprAccreditationContext _dbContext;
    private AccreditationRepository _repository;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EprAccreditationContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase" + Guid.NewGuid().ToString())
            .Options;
        _dbContext = new EprAccreditationContext(options);
        _repository = new AccreditationRepository(_dbContext);

        LookupMaterial material = new()
        {
            Id = 1,
            MaterialName = "steel",
            MaterialCode = "code"
        };

        RegistrationMaterial registrationMaterial = new()
        {
            MaterialId = 1,
            Material = material
        };

        _dbContext.Accreditations.AddRange(
            new List<AccreditationEntity>
            {
                new AccreditationEntity
                {
                    Id = 1,
                    ExternalId = new Guid("11111111-1111-1111-1111-111111111111"), 
                    AccreditationYear = 2026,
                    ApplicationTypeId = 1,
                    ApplicationType = new(),
                    AccreditationStatusId = 1,
                    AccreditationStatus = new(),
                    RegistrationMaterialId = 1,
                    RegistrationMaterial = registrationMaterial
                },
                new AccreditationEntity
                {
                    Id = 2,
                    ExternalId = new Guid("22222222-2222-2222-2222-222222222222"),
                    AccreditationYear = 2026,
                    ApplicationTypeId = 1,
                    ApplicationType = new(),
                    AccreditationStatusId = 1,
                    AccreditationStatus = new(),
                    RegistrationMaterialId = 1,
                    RegistrationMaterial = registrationMaterial
                }
            });
        _dbContext.SaveChangesAsync();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [TestMethod]
    public async Task GetById_ReturnsAccreditation_WhenFound()
    {
        // Arrange
        var accreditationId = new Guid("11111111-1111-1111-1111-111111111111");

        // Act
        var result = await _repository.GetById(accreditationId);

        // Assert
        result.Should().NotBeNull();
        result.ExternalId.Should().Be(accreditationId);
        result.AccreditationYear.Should().Be(2026);
    }

    [TestMethod]
    public async Task GetById_ReturnsNull_WhenNotFound()
    {
        // Act
        var result = await _repository.GetById(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task Create_ShouldAddNewEntity()
    {
        // Arrange
        var accreditation = new AccreditationEntity { AccreditationYear = 2026 };

        // Act
        await _repository.Create(accreditation);

        // Assert
        var accreditationId = accreditation.ExternalId;
        var updatedAccreditation = await _dbContext.Accreditations.SingleAsync(x => x.ExternalId == accreditationId);
        updatedAccreditation.Should().NotBeNull();
        updatedAccreditation.ExternalId.Should().Be(accreditationId);
        updatedAccreditation.AccreditationYear.Should().Be(2026);
    }

    [TestMethod]
    public async Task Update_ShouldModifyExistingEntityValues()
    {
        // Arrange
        var accreditationId = new Guid("11111111-1111-1111-1111-111111111111");
        var accreditation = new AccreditationEntity { Id = 1, ExternalId = accreditationId, AccreditationYear = 2027 };

        // Act
        await _repository.Update(accreditation);

        // Assert
        var updatedAccreditation = await _dbContext.Accreditations.SingleAsync(x => x.ExternalId == accreditationId);
        updatedAccreditation.Should().NotBeNull();
        updatedAccreditation.ExternalId.Should().Be(accreditationId);
        updatedAccreditation.AccreditationYear.Should().Be(2027);
    }
}