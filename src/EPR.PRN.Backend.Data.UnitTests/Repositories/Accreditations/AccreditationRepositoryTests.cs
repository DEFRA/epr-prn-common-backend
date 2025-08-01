﻿using AutoMapper;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Repositories.Accreditations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories.Accreditations;

[TestClass]
public class AccreditationRepositoryTests
{
    private EprContext _dbContext;
    private AccreditationRepository _repository;
    private Mock<IMapper> _mapper;
    private Mock<ILogger<AccreditationRepository>> _mockLogger;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EprContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase" + Guid.NewGuid().ToString())
            .Options;
        _mapper = new Mock<IMapper> ();
        _dbContext = new EprContext(options);
        _mockLogger = new Mock<ILogger<AccreditationRepository>>();
        _repository = new AccreditationRepository(_dbContext, _mapper.Object, _mockLogger.Object);

        LookupMaterial material = new()
        {
            Id = 1,
            MaterialName = "steel",
            MaterialCode = "code"
        };

        Registration registration = new()
        {
            Id = 1,
            ExternalId = new Guid("11111111-1111-1111-1111-111111111111"),
            OrganisationId = Guid.NewGuid(),
            ApplicationTypeId = 1,
            CreatedBy = Guid.NewGuid(),
            CreatedDate = DateTime.UtcNow,
            UpdatedBy = Guid.NewGuid(),
            UpdatedDate = DateTime.UtcNow
        };

        RegistrationMaterial registrationMaterial = new()
        {
            MaterialId = 1,
            Material = material,
            Registration = registration
        };

        LookupAccreditationStatus accreditationStatus = new()
        {
            Id = 1,
            Name = "Active"
      
        };


        _dbContext.Accreditations.AddRange(
            new List<Accreditation>
            {
                new Accreditation
                {
                    Id = 1,
                    ExternalId = new Guid("11111111-1111-1111-1111-111111111111"),
                    
                    AccreditationYear = 2026,
           
                    //ApplicationType = new(),
                    AccreditationStatusId = 1,
                
                    AccreditationStatus = accreditationStatus,
                    RegistrationMaterial = registrationMaterial,
                    ApplicationReferenceNumber = "APP-123456",
                },
                new Accreditation
                {
                    Id = 2,
                    ExternalId = new Guid("22222222-2222-2222-2222-222222222222"),
              
                    AccreditationYear = 2026,
                    //ApplicationTypeId = 1,
                    //ApplicationType = new(),
                    AccreditationStatusId = 1,
                    AccreditationStatus = accreditationStatus,
                    RegistrationMaterialId = 1,
                    RegistrationMaterial = registrationMaterial,
                    ApplicationReferenceNumber = "APP-123456",
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
        var accreditation = new Accreditation { AccreditationYear = 2026, ApplicationReferenceNumber = "APP-123456", };

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
        var accreditation = new Accreditation { Id = 1, ExternalId = accreditationId, AccreditationYear = 2027, ApplicationReferenceNumber = "APP-123456", };

        // Act
        await _repository.Update(accreditation);

        // Assert
        var updatedAccreditation = await _dbContext.Accreditations.SingleAsync(x => x.ExternalId == accreditationId);
        updatedAccreditation.Should().NotBeNull();
        updatedAccreditation.ExternalId.Should().Be(accreditationId);
        updatedAccreditation.AccreditationYear.Should().Be(2027);
    }

    [TestMethod]
    public async Task GetAccreditationDetails_NoRecords_ReturnsNull()
    {
        // Arrange
        
        // Act
        var result = await _repository.GetAccreditationDetails(new Guid(), It.IsAny<int>(), It.IsAny<int>());

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetAccreditationOverviewForOrgId_NoRecordsFound_ReturnsEmptyList()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var orgId = Guid.NewGuid();
        var accreditations = new List<Accreditation>
        {
            new Accreditation
            {
                ExternalId = Guid.NewGuid(),
             
            },
            new Accreditation
            {
                ExternalId = Guid.NewGuid(),
                
            },
            new Accreditation
            {
                ExternalId = Guid.NewGuid(),
       
            },
            new Accreditation
            {
                ExternalId = Guid.NewGuid(),
        
            }
        };

        await _dbContext.AddRangeAsync(accreditations);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAccreditationOverviewForOrgId(orgId);

        // Assert
        result.Should().BeNull();
    }
}