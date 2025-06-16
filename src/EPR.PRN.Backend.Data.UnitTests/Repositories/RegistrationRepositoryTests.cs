using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories;

[TestClass]
public class RegistrationRepositoryTests
{
    private EprContext _context;
    private Mock<ILogger<RegistrationRepository>> _logger;
    private RegistrationRepository _repository;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EprContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new EprContext(options);
        _logger = new Mock<ILogger<RegistrationRepository>>();
        _repository = new RegistrationRepository(_context, _logger.Object);
    }

    [TestMethod]
    public async Task GetTaskStatusAsync_ShouldReturnNull_WhenNoMatchFound()
    {
        // Act
        var result = await _repository.GetTaskStatusAsync("NonExistentTask", 999);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetTaskStatusAsync_ShouldReturnStatus_WhenMatchExists()
    {
        // Arrange
        var task = new LookupRegulatorTask { Name = "TestTask", ApplicationTypeId = 1 };
        var taskStatusEntity = new LookupTaskStatus { Name = "InProgress" };
        var status = new RegistrationTaskStatus
        {
            RegistrationId = 1,
            Task = task,
            TaskStatus = taskStatusEntity
        };

        await _context.LookupTasks.AddAsync(task);
        await _context.LookupTaskStatuses.AddAsync(taskStatusEntity);
        await _context.RegistrationTaskStatus.AddAsync(status);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetTaskStatusAsync("TestTask", 1);

        // Assert
        result.Should().NotBeNull();
        result!.Task.Name.Should().Be("TestTask");
        result.TaskStatus.Name.Should().Be("InProgress");
    }

    [TestMethod]
    public async Task UpdateRegistrationTaskStatusAsync_ShouldAddNewStatus_WhenNotExists()
    {
        // Arrange
        var registration = new Registration { Id = 1, ApplicationTypeId = 1 };
        var task = new LookupRegulatorTask { Name = "NewTask", ApplicationTypeId = 1, IsMaterialSpecific = false };
        var status = new LookupTaskStatus { Name = TaskStatuses.Completed.ToString() };

        await _context.Registrations.AddAsync(registration);
        await _context.LookupTasks.AddAsync(task);
        await _context.LookupTaskStatuses.AddAsync(status);
        await _context.SaveChangesAsync();

        // Act
        await _repository.UpdateRegistrationTaskStatusAsync("NewTask", 1, TaskStatuses.Completed);

        // Assert
        var result = await _repository.GetTaskStatusAsync("NewTask", 1);
        result.Should().NotBeNull();
        result!.TaskStatus.Name.Should().Be(TaskStatuses.Completed.ToString());
    }

    [TestMethod]
    public async Task UpdateRegistrationTaskStatusAsync_ShouldUpdateExistingStatus()
    {
        // Arrange
        var registration = new Registration { Id = 2, ApplicationTypeId = 1 };
        var task = new LookupRegulatorTask { Name = "ExistingTask", ApplicationTypeId = 1, IsMaterialSpecific = false };
        var oldStatus = new LookupTaskStatus { Name = TaskStatuses.Started.ToString() };
        var newStatus = new LookupTaskStatus { Name = TaskStatuses.Completed.ToString() };

        await _context.Registrations.AddAsync(registration);
        await _context.LookupTasks.AddAsync(task);
        await _context.LookupTaskStatuses.AddRangeAsync(oldStatus, newStatus);

        var taskStatus = new RegistrationTaskStatus
        {
            RegistrationId = 2,
            Task = task,
            TaskStatus = oldStatus
        };

        await _context.RegistrationTaskStatus.AddAsync(taskStatus);
        await _context.SaveChangesAsync();

        // Act
        await _repository.UpdateRegistrationTaskStatusAsync("ExistingTask", 2, TaskStatuses.Completed);

        // Assert
        var result = await _repository.GetTaskStatusAsync("ExistingTask", 2);
        result!.TaskStatus.Name.Should().Be(TaskStatuses.Completed.ToString());
    }

    [TestMethod]
    public async Task UpdateRegistrationTaskStatusAsync_ShouldThrow_WhenRegistrationNotFound()
    {
        // Arrange
        var status = new LookupTaskStatus { Name = TaskStatuses.Completed.ToString() };
        await _context.LookupTaskStatuses.AddAsync(status);
        await _context.SaveChangesAsync();

        // Act
        Func<Task> act = async () => await _repository.UpdateRegistrationTaskStatusAsync("AnyTask", 9999, TaskStatuses.Completed);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [TestMethod]
    public async Task UpdateRegistrationTaskStatusAsync_ShouldThrow_WhenTaskNotFound()
    {
        // Arrange
        var registration = new Registration { Id = 3, ApplicationTypeId = 10 };
        var status = new LookupTaskStatus { Name = TaskStatuses.Completed.ToString() };

        await _context.Registrations.AddAsync(registration);
        await _context.LookupTaskStatuses.AddAsync(status);
        await _context.SaveChangesAsync();

        // Act
        Func<Task> act = async () => await _repository.UpdateRegistrationTaskStatusAsync("MissingTask", 3, TaskStatuses.Completed);

        // Assert
        await act.Should().ThrowAsync<RegulatorInvalidOperationException>()
            .WithMessage("No Valid Task Exists: MissingTask");
    }

    [TestMethod]
    public async Task UpdateSiteAddressAsync_ShouldUpdateRegistrationWithNewAddresses_WhenAddressesHaveNoId()
    {
        // Arrange
        var registration = new Registration { Id = 1, ExternalId = Guid.NewGuid() };
        _context.Registrations.Add(registration);
        await _context.SaveChangesAsync();

        var reprocessingAddress = new AddressDto
        {
            AddressLine1 = "123 Test St",
            AddressLine2 = "Test Area",
            TownCity = "Testville",
            Country = "Testland",
            PostCode = "TST 123",
            GridReference = "GB1234567890",
            County = "UK",
            NationId = 1,
        };

        // Act
        await _repository.UpdateSiteAddressAsync(registration.Id, reprocessingAddress);

        // Assert
        var updatedRegistration = await _context.Registrations.FirstAsync(r => r.Id == registration.Id);
        updatedRegistration.ReprocessingSiteAddressId.Should().NotBeNull();

        var reprocAddress = await _context.LookupAddresses.FindAsync(updatedRegistration.ReprocessingSiteAddressId);

        reprocAddress.AddressLine1.Should().Be("123 Test St");
    }

    [TestMethod]
    public async Task UpdateSiteAddressAsync_ShouldThrowException_WhenRegistrationNotFound()
    {
        // Arrange
        var reprocessingAddress = new AddressDto();

        // Act
        Func<Task> act = async () => await _repository.UpdateSiteAddressAsync(999, reprocessingAddress);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Registration not found.");
    }

    [TestMethod]
    public async Task UpdateSiteAddressAsync_ShouldReuseAddressIds_WhenAddressDtosContainIds()
    {
        // Arrange
        var registration = new Registration { Id = 2, ExternalId = Guid.NewGuid() };
        var lookupAddress = new Address
        {
            Id = 101,
            AddressLine1 = "123 Test St",
            AddressLine2 = "Test Area",
            TownCity = "Testville",
            PostCode = "TST 123",
            GridReference = "GB1234567890",
            County = "UK",
            NationId = 1,
        };

        _context.Registrations.Add(registration);
        _context.LookupAddresses.Add(lookupAddress);
        await _context.SaveChangesAsync();

        var reprocessingAddress = new AddressDto { Id = 101 };

        // Act
        await _repository.UpdateSiteAddressAsync(registration.Id, reprocessingAddress);

        // Assert
        var updatedRegistration = await _context.Registrations.FindAsync(registration.Id);
        updatedRegistration.ReprocessingSiteAddressId.Should().Be(101);
    }

    [TestMethod]
    public async Task GetRegistrationsOverviewForOrgIdAsync_ShouldReturnEmptyList_WhenNoRegistrationsExist()
    {
        // Act
        var result = await _repository.GetRegistrationsOverviewForOrgIdAsync(Guid.NewGuid());
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetRegistrationsOverviewForOrgIdAsync_ShouldReturnRegistrations_WhenRegistrationsExist()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
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
                    Material = new LookupMaterial
                    {
                        Id = 1,
                        MaterialName = "Plastic",
                        MaterialCode = "PLS" // Set the required MaterialCode property
                    }
                }
            }
        };
        await _context.Registrations.AddAsync(registration);
        await _context.SaveChangesAsync();
        // Act
        var result = await _repository.GetRegistrationsOverviewForOrgIdAsync(organisationId);
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        var overview = result.First();
        overview.RegistrationId.Should().Be(registration.ExternalId);
        overview.Material.Should().Be("Plastic");
        overview.ReprocessingSiteAddress.Should().NotBeNull();
        overview.ReprocessingSiteAddress!.AddressLine1.Should().Be("123 Test St");
    }


    [TestMethod]
    public async Task GetRegistrationsOverviewForOrgIdAsync_ShouldHandleNullMaterials()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var registration = new Registration
        {
            ExternalId = Guid.NewGuid(),
            OrganisationId = organisationId,
            ApplicationTypeId = 1,
            RegistrationStatusId = 2,
            Materials = null
        };
        await _context.Registrations.AddAsync(registration);
        await _context.SaveChangesAsync();
        // Act
        var result = await _repository.GetRegistrationsOverviewForOrgIdAsync(organisationId);
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        var overview = result.First();
        overview.RegistrationId.Should().Be(registration.ExternalId);
        overview.Material.Should().Be(string.Empty);
    }

    [TestMethod]
    public async Task GetRegistrationsOverviewForOrgIdAsync_ShouldHandleNullReprocessingSiteAddress()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var registration = new Registration
        {
            ExternalId = Guid.NewGuid(),
            OrganisationId = organisationId,
            ApplicationTypeId = 1,
            RegistrationStatusId = 2,
            ReprocessingSiteAddress = null,
            Materials = new List<RegistrationMaterial>
            {
                new RegistrationMaterial
                {
                    Id = 1,
                    Material = new LookupMaterial
                    {
                        Id = 1,
                        MaterialName = "Glass",
                        MaterialCode = "GLS" // Set the required MaterialCode property
                    }
                }
            }
        };
        await _context.Registrations.AddAsync(registration);
        await _context.SaveChangesAsync();
        // Act
        var result = await _repository.GetRegistrationsOverviewForOrgIdAsync(organisationId);
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        var overview = result.First();
        overview.RegistrationId.Should().Be(registration.ExternalId);
        overview.Material.Should().Be("Glass");
        overview.ReprocessingSiteAddress.Should().BeNull();
    }


    [TestMethod]
    public async Task GetRegistrationsOverviewForOrgIdAsync_ShouldReturnMultipleRegistrations()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var registrations = new List<Registration>
        {
            new Registration
            {
                ExternalId = Guid.NewGuid(),
                OrganisationId = organisationId,
                ApplicationTypeId = 1,
                RegistrationStatusId = 2,
                Materials = new List<RegistrationMaterial>
                {
                    new RegistrationMaterial
                    {
                        Id = 1,
                        Material = new LookupMaterial
                        {
                            Id = 1,
                            MaterialName = "Metal",
                            MaterialCode = "MTL" // Set the required MaterialCode property
                        }
                    }
                }
            },
            new Registration
            {
                ExternalId = Guid.NewGuid(),
                OrganisationId = organisationId,
                ApplicationTypeId = 2,
                RegistrationStatusId = 3,
                Materials = new List<RegistrationMaterial>
                {
                    new RegistrationMaterial
                    {
                        Id = 2,
                        Material = new LookupMaterial
                        {
                            Id = 2,
                            MaterialName = "Paper",
                            MaterialCode = "PPR" // Set the required MaterialCode property
                        }
                    }
                }
            }
        };
        await _context.Registrations.AddRangeAsync(registrations);
        await _context.SaveChangesAsync();
        // Act
        var result = await _repository.GetRegistrationsOverviewForOrgIdAsync(organisationId);
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().ContainSingle(r => r.Material == "Metal");
        result.Should().ContainSingle(r => r.Material == "Paper");
    }

}
