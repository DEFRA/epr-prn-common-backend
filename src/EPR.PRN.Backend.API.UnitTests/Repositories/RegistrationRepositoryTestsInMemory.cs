using AutoFixture;
using EPR.PRN.Backend.Data;
using EPR.PRN.Backend.Data.Repositories;
using EPR.PRN.Backend.Data.DTO.Registration;
using EPR.PRN.Backend.Data.DataModels;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using EPR.PRN.Backend.Data.DataModels.Registrations;
namespace EPR.PRN.Backend.Data.UnitTests.Repositories;
[TestClass]
public class RegistrationRepositoryTestsInMemory
{
    private Mock<ILogger<RegistrationRepository>> _mockLogger;
    private EprContext _context;
    private RegistrationRepository _repository;
    private readonly Fixture _fixture = new();
    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EprContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new EprContext(options);
        _mockLogger = new Mock<ILogger<RegistrationRepository>>();
        _repository = new RegistrationRepository(_context, _mockLogger.Object);
    }
    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
    
    [TestMethod]
    public async Task GetRegistrationsOverviewForOrgIdAsync_ReturnsEmptyList_WhenNoRegistrationsExist()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        // Act
        var result = await _repository.GetRegistrationsOverviewForOrgIdAsync(organisationId);
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
    
    [TestMethod]
    public async Task GetRegistrationsOverviewForOrgIdAsync_ReturnsRegistrations_WithMaterialsAndAccreditations()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var accreditationStatus = new LookupAccreditationStatus
        {
            Id = 1,
            Name = "Accredited"
        };
        var material = new LookupMaterial
        {
            Id = 1,
            MaterialName = "Plastic",
            MaterialCode = "PLS"
        };
        var registration = new Registration
        {
            ExternalId = Guid.NewGuid(),
            OrganisationId = organisationId,
            ApplicationTypeId = 1,
            RegistrationStatusId = 2,
            ReprocessingSiteAddress = new Address
            {
                Id = 1,
                AddressLine1 = "123 Test St",
                AddressLine2 = "Test Area",
                TownCity = "Testville",
                County = "Test County",
                PostCode = "TST 123",
                NationId = 1,
                GridReference = "GB1234567890"
            },
            Materials = new List<RegistrationMaterial>
            {
                new RegistrationMaterial
                {
                    Id = 1,
                    ExternalId = Guid.NewGuid(),
                    Material = material,
                    Accreditations = new List<Accreditation>
                    {
                        new Accreditation
                        {
                            AccreditationStatus = accreditationStatus,
                            AccreditationYear = 2023,
                            ApplicationReferenceNumber = "55599"
                        }
                    }
                }
            }
        };
        _context.Add(accreditationStatus);
        _context.Add(material);
        _context.Add(registration);
        await _context.SaveChangesAsync(CancellationToken.None);
        // Act
        var result = await _repository.GetRegistrationsOverviewForOrgIdAsync(organisationId);
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        var overview = result.First();
        overview.Id.Should().Be(registration.ExternalId);
        overview.Material.Should().Be("Plastic");
        overview.AccreditationStatus.Should().Be(accreditationStatus.Id);
        overview.AccreditationYear.Should().Be(2023);
        overview.ReprocessingSiteAddress.Should().NotBeNull();
        overview.ReprocessingSiteAddress!.AddressLine1.Should().Be("123 Test St");
    }
    
    [TestMethod]
    public async Task GetRegistrationsOverviewForOrgIdAsync_HandlesMultipleRegistrations()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var accreditationStatus1 = new LookupAccreditationStatus
        {
            Id = 1,
            Name = "Accredited"
        };
        var accreditationStatus2 = new LookupAccreditationStatus
        {
            Id = 2,
            Name = "Pending"
        };
        var material = new LookupMaterial
        {
            Id = 1,
            MaterialName = "Plastic",
            MaterialCode = "PLS"
        };
        var registrations = new List<Registration>
        {
            new Registration
            {
                ExternalId = Guid.NewGuid(),
                OrganisationId = organisationId,
                ApplicationTypeId = 1,
                RegistrationStatusId = 2,
                ReprocessingSiteAddress = new Address
                {
                    Id = 1,
                    AddressLine1 = "123 Test St",
                    AddressLine2 = "Test Area",
                    TownCity = "Testville",
                    County = "Test County",
                    PostCode = "TST 123",
                    NationId = 1,
                    GridReference = "GB1234567890"
                },
                Materials = new List<RegistrationMaterial>
                {
                    new RegistrationMaterial
                    {
                        Id = 1,
                        ExternalId = Guid.NewGuid(),
                        Material = material,
                        Accreditations = new List<Accreditation>
                        {
                            new Accreditation
                            {
                                AccreditationStatus = accreditationStatus1,
                                AccreditationYear = 2023,
                                ApplicationReferenceNumber = "76488"
                            }
                        }
                    }
                }
            },
            new Registration
            {
                ExternalId = Guid.NewGuid(),
                OrganisationId = organisationId,
                ApplicationTypeId = 1,
                RegistrationStatusId = 3,
                ReprocessingSiteAddress = new Address
                {
                    Id = 2,
                    AddressLine1 = "456 Another St",
                    AddressLine2 = "Another Area",
                    TownCity = "Anotherville",
                    County = "Another County",
                    PostCode = "ANT 456",
                    NationId = 2,
                    GridReference = "GB9876543210"
                },
                Materials = new List<RegistrationMaterial>
                {
                    new RegistrationMaterial
                    {
                        Id = 2,
                        ExternalId = Guid.NewGuid(),
                        Material = material,
                        Accreditations = new List<Accreditation>
                        {
                            new Accreditation
                            {
                                AccreditationStatus = accreditationStatus2,
                                AccreditationYear = 2024,
                                ApplicationReferenceNumber = "45766"
                            }
                        }
                    }
                }
            }
        };
        _context.Add(accreditationStatus1);
        _context.Add(accreditationStatus2);
        _context.Add(material);
        _context.AddRange(registrations);
        await _context.SaveChangesAsync(CancellationToken.None);
        // Act
        var result = await _repository.GetRegistrationsOverviewForOrgIdAsync(organisationId);
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        var firstOverview = result.First();
        firstOverview.Id.Should().Be(registrations[0].ExternalId);
        firstOverview.Material.Should().Be("Plastic");
        firstOverview.AccreditationStatus.Should().Be(accreditationStatus1.Id);
        firstOverview.AccreditationYear.Should().Be(2023);
        firstOverview.ReprocessingSiteAddress.Should().NotBeNull();
        firstOverview.ReprocessingSiteAddress!.AddressLine1.Should().Be("123 Test St");
        var secondOverview = result.Last();
        secondOverview.Id.Should().Be(registrations[1].ExternalId);
        secondOverview.Material.Should().Be("Plastic");
        secondOverview.AccreditationStatus.Should().Be(accreditationStatus2.Id);
        secondOverview.AccreditationYear.Should().Be(2024);
        secondOverview.ReprocessingSiteAddress.Should().NotBeNull();
        secondOverview.ReprocessingSiteAddress!.AddressLine1.Should().Be("456 Another St");
    }

    [TestMethod]
    public async Task GetRegistrationsOverviewForOrgIdAsync_HandlesNullAndEmptyMaterials()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var registrationWithNullMaterials = new Registration
        {
            ExternalId = Guid.NewGuid(),
            OrganisationId = organisationId,
            ApplicationTypeId = 1,
            RegistrationStatusId = 2,
            ReprocessingSiteAddress = new Address
            {
                Id = 1,
                AddressLine1 = "123 Test St",
                AddressLine2 = "Test Area",
                TownCity = "Testville",
                County = "Test County",
                PostCode = "TST 123",
                NationId = 1,
                GridReference = "GB1234567890"
            },
            Materials = null // Null Materials
        };
        var registrationWithEmptyMaterials = new Registration
        {
            ExternalId = Guid.NewGuid(),
            OrganisationId = organisationId,
            ApplicationTypeId = 1,
            RegistrationStatusId = 2,
            ReprocessingSiteAddress = new Address
            {
                Id = 2,
                AddressLine1 = "456 Another St",
                AddressLine2 = "Another Area",
                TownCity = "Anotherville",
                County = "Another County",
                PostCode = "ANT 456",
                NationId = 2,
                GridReference = "GB9876543210"
            },
            Materials = new List<RegistrationMaterial>() // Empty Materials
        };
        _context.Add(registrationWithNullMaterials);
        _context.Add(registrationWithEmptyMaterials);
        await _context.SaveChangesAsync(CancellationToken.None);
        // Act
        var result = await _repository.GetRegistrationsOverviewForOrgIdAsync(organisationId);
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        var firstOverview = result.First();
        firstOverview.Id.Should().Be(registrationWithNullMaterials.ExternalId);
        firstOverview.Material.Should().Be(string.Empty); // Material should be empty for null Materials
        firstOverview.ReprocessingSiteAddress.Should().NotBeNull();
        firstOverview.ReprocessingSiteAddress!.AddressLine1.Should().Be("123 Test St");
        var secondOverview = result.Last();
        secondOverview.Id.Should().Be(registrationWithEmptyMaterials.ExternalId);
        secondOverview.Material.Should().Be(string.Empty); // Material should be empty for empty Materials
        secondOverview.ReprocessingSiteAddress.Should().NotBeNull();
        secondOverview.ReprocessingSiteAddress!.AddressLine1.Should().Be("456 Another St");
    }
}