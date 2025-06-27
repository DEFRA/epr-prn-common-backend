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
    public async Task GetAsync_ShouldReturnNull_WhenNoMatchFound()
    {
        // Arrange
        var registrationId = Guid.NewGuid();

        // Act
        var result = await _repository.GetAsync(registrationId);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetAsync_ShouldReturnEntity_WhenMatchFound()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var existing = new Registration
        {
            Id = 1,
            ExternalId = registrationId
        };

        await _context.Registrations.AddAsync(existing);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync(registrationId);

        // Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetTaskStatusAsync_ShouldReturnNull_WhenNoMatchFound()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        
        // Act
        var result = await _repository.GetTaskStatusAsync("NonExistentTask", registrationId);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetTaskStatusAsync_ShouldReturnStatus_WhenMatchExists()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var registration = new Registration { Id = 1, ExternalId = registrationId };

        var task = new LookupRegulatorTask { Name = "TestTask", ApplicationTypeId = 1 };
        var taskStatusEntity = new LookupTaskStatus { Name = "InProgress" };
        var status = new ApplicantRegistrationTaskStatus
        {
            RegistrationId = 1,
            Task = task,
            TaskStatus = taskStatusEntity
        };

        await _context.Registrations.AddAsync(registration);
        await _context.LookupTasks.AddAsync(task);
        await _context.LookupTaskStatuses.AddAsync(taskStatusEntity);
        await _context.RegistrationTaskStatus.AddAsync(status);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetTaskStatusAsync("TestTask", registrationId);

        // Assert
        result.Should().NotBeNull();
        result!.Task.Name.Should().Be("TestTask");
        result.TaskStatus.Name.Should().Be("InProgress");
    }

    [TestMethod]
    public async Task UpdateRegistrationTaskStatusAsync_ShouldAddNewStatus_WhenNotExists()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var registration = new Registration { Id = 1, ApplicationTypeId = 1, ExternalId = registrationId};
        var task = new LookupRegulatorTask { Name = "NewTask", ApplicationTypeId = 1, IsMaterialSpecific = false };
        var status = new LookupTaskStatus { Name = TaskStatuses.Completed.ToString() };

        await _context.Registrations.AddAsync(registration);
        await _context.LookupTasks.AddAsync(task);
        await _context.LookupTaskStatuses.AddAsync(status);
        await _context.SaveChangesAsync();

        // Act
        await _repository.UpdateRegistrationTaskStatusAsync("NewTask", registrationId, TaskStatuses.Completed);

        // Assert
        var result = await _repository.GetTaskStatusAsync("NewTask", registrationId);
        result.Should().NotBeNull();
        result!.TaskStatus.Name.Should().Be(TaskStatuses.Completed.ToString());
    }

    [TestMethod]
    public async Task UpdateRegistrationTaskStatusAsync_ShouldUpdateExistingStatus()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var registration = new Registration { Id = 2, ApplicationTypeId = 1, ExternalId = registrationId};
        var task = new LookupRegulatorTask { Name = "ExistingTask", ApplicationTypeId = 1, IsMaterialSpecific = false };
        var oldStatus = new LookupTaskStatus { Name = TaskStatuses.Started.ToString() };
        var newStatus = new LookupTaskStatus { Name = TaskStatuses.Completed.ToString() };

        await _context.Registrations.AddAsync(registration);
        await _context.LookupTasks.AddAsync(task);
        await _context.LookupTaskStatuses.AddRangeAsync(oldStatus, newStatus);

        var taskStatus = new ApplicantRegistrationTaskStatus
        {
            RegistrationId = 2,
            Task = task,
            TaskStatus = oldStatus
        };

        await _context.RegistrationTaskStatus.AddAsync(taskStatus);
        await _context.SaveChangesAsync();

        // Act
        await _repository.UpdateRegistrationTaskStatusAsync("ExistingTask", registrationId, TaskStatuses.Completed);

        // Assert
        var result = await _repository.GetTaskStatusAsync("ExistingTask", registrationId);
        result!.TaskStatus.Name.Should().Be(TaskStatuses.Completed.ToString());
    }

    [TestMethod]
    public async Task UpdateRegistrationTaskStatusAsync_ShouldThrow_WhenRegistrationNotFound()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var status = new LookupTaskStatus { Name = TaskStatuses.Completed.ToString() };
        await _context.LookupTaskStatuses.AddAsync(status);
        await _context.SaveChangesAsync();

        // Act
        Func<Task> act = async () => await _repository.UpdateRegistrationTaskStatusAsync("AnyTask", registrationId, TaskStatuses.Completed);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [TestMethod]
    public async Task UpdateRegistrationTaskStatusAsync_ShouldThrow_WhenTaskNotFound()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var registration = new Registration { Id = 3, ApplicationTypeId = 10, ExternalId = registrationId};
        var status = new LookupTaskStatus { Name = TaskStatuses.Completed.ToString() };

        await _context.Registrations.AddAsync(registration);
        await _context.LookupTaskStatuses.AddAsync(status);
        await _context.SaveChangesAsync();

        // Act
        Func<Task> act = async () => await _repository.UpdateRegistrationTaskStatusAsync("MissingTask", registrationId, TaskStatuses.Completed);

        // Assert
        await act.Should().ThrowAsync<RegulatorInvalidOperationException>()
            .WithMessage("No Valid Task Exists: MissingTask");
    }

    [TestMethod]
    public async Task UpdateApplicantRegistrationTaskStatusAsync_ShouldAddNewStatus_WhenNotExists()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var registration = new Registration { Id = 1, ApplicationTypeId = 1, ExternalId = registrationId };
        var task = new LookupRegulatorTask { Name = "NewTask", ApplicationTypeId = 1, IsMaterialSpecific = false };
        var status = new LookupTaskStatus { Name = TaskStatuses.Completed.ToString() };

        await _context.Registrations.AddAsync(registration);
        await _context.LookupTasks.AddAsync(task);
        await _context.LookupTaskStatuses.AddAsync(status);
        await _context.SaveChangesAsync();

        // Act
        await _repository.UpdateRegistrationTaskStatusAsync("NewTask", registrationId, TaskStatuses.Completed);

        // Assert
        var result = await _repository.GetTaskStatusAsync("NewTask", registrationId);
        result.Should().NotBeNull();
        result!.TaskStatus.Name.Should().Be(TaskStatuses.Completed.ToString());
    }

    [TestMethod]
    public async Task UpdateApplicantRegistrationTaskStatusAsync_ShouldUpdateExistingStatus()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var registration = new Registration { Id = 2, ApplicationTypeId = 1, ExternalId = registrationId };
        var task = new LookupRegulatorTask { Name = "ExistingTask", ApplicationTypeId = 1, IsMaterialSpecific = false };
        var oldStatus = new LookupTaskStatus { Name = TaskStatuses.Started.ToString() };
        var newStatus = new LookupTaskStatus { Name = TaskStatuses.Completed.ToString() };

        await _context.Registrations.AddAsync(registration);
        await _context.LookupTasks.AddAsync(task);
        await _context.LookupTaskStatuses.AddRangeAsync(oldStatus, newStatus);

        var taskStatus = new ApplicantRegistrationTaskStatus
        {
            RegistrationId = 2,
            Task = task,
            TaskStatus = oldStatus
        };

        await _context.RegistrationTaskStatus.AddAsync(taskStatus);
        await _context.SaveChangesAsync();

        // Act
        await _repository.UpdateRegistrationTaskStatusAsync("ExistingTask", registrationId, TaskStatuses.Completed);

        // Assert
        var result = await _repository.GetTaskStatusAsync("ExistingTask", registrationId);
        result!.TaskStatus.Name.Should().Be(TaskStatuses.Completed.ToString());
    }

    [TestMethod]
    public async Task UpdateApplicantRegistrationTaskStatusAsync_ShouldThrow_WhenRegistrationNotFound()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var status = new LookupTaskStatus { Name = TaskStatuses.Completed.ToString() };
        await _context.LookupTaskStatuses.AddAsync(status);
        await _context.SaveChangesAsync();

        // Act
        Func<Task> act = async () => await _repository.UpdateRegistrationTaskStatusAsync("AnyTask", registrationId, TaskStatuses.Completed);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [TestMethod]
    public async Task UpdateApplicantRegistrationTaskStatusAsync_ShouldThrow_WhenTaskNotFound()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var registration = new Registration { Id = 3, ApplicationTypeId = 10, ExternalId = registrationId };
        var status = new LookupTaskStatus { Name = TaskStatuses.Completed.ToString() };

        await _context.Registrations.AddAsync(registration);
        await _context.LookupTaskStatuses.AddAsync(status);
        await _context.SaveChangesAsync();

        // Act
        Func<Task> act = async () => await _repository.UpdateRegistrationTaskStatusAsync("MissingTask", registrationId, TaskStatuses.Completed);

        // Assert
        await act.Should().ThrowAsync<RegulatorInvalidOperationException>()
            .WithMessage("No Valid Task Exists: MissingTask");
    }

    [TestMethod]
    public async Task UpdateSiteAddressAsync_ShouldUpdateRegistrationWithNewAddresses_WhenAddressesHaveNoId()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var registration = new Registration { Id = 1, ExternalId = registrationId };
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
        await _repository.UpdateSiteAddressAsync(registrationId, reprocessingAddress);

        // Assert
        var updatedRegistration = await _context.Registrations.FirstAsync(r => r.Id == registration.Id);
        updatedRegistration.ReprocessingSiteAddressId.Should().NotBeNull();

        var reprocAddress = await _context.LookupAddresses.FindAsync(updatedRegistration.ReprocessingSiteAddressId);

        reprocAddress!.AddressLine1.Should().Be("123 Test St");
    }

    [TestMethod]
    public async Task UpdateSiteAddressAsync_ShouldThrowException_WhenRegistrationNotFound()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var reprocessingAddress = new AddressDto();

        // Act
        Func<Task> act = async () => await _repository.UpdateSiteAddressAsync(registrationId, reprocessingAddress);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Registration not found.");
    }

    [TestMethod]
    public async Task UpdateSiteAddressAsync_ShouldReuseAddressIds_WhenAddressDtosContainIds()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var registration = new Registration { Id = 2, ExternalId = registrationId };
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
        await _repository.UpdateSiteAddressAsync(registrationId, reprocessingAddress);

        // Assert
        var updatedRegistration = await _context.Registrations.FindAsync(registration.Id);
        updatedRegistration!.ReprocessingSiteAddressId.Should().Be(101);
    }

    [TestMethod]
    public async Task UpdateAsync_NoRegistration_ShouldThrowException()
    {
        // Arrange
        var registrationId = Guid.NewGuid();

        // Negative data, purposely using a different ExternalId than the one in the above variable as we don't want to match on this.
        // We want to have one non-matching item in the database as negative data,
        var registration = new Registration { Id = 2, ExternalId = Guid.NewGuid() };

        _context.Registrations.Add(registration);

        await _context.SaveChangesAsync();

        var businessAddress = new AddressDto { Id = 101, AddressLine1 = "Address line 1" };
        var reprocessingAddress = new AddressDto { Id = 101, AddressLine1 = "Address line 1" };
        var legalAddress = new AddressDto { Id = 101, AddressLine1 = "Address line 1" };

        // Act
        Func<Task> act = async () => await _repository.UpdateAsync(registrationId, businessAddress, reprocessingAddress, legalAddress);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [TestMethod]
    public async Task UpdateAsync_NoExistingAddresses_ShouldUpdateAccordingly()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var registration = new Registration { Id = 2, ExternalId = registrationId};

        _context.Registrations.Add(registration);

        await _context.SaveChangesAsync();

        var businessAddress = new AddressDto { Id = 101, AddressLine1 = "Address line 1" };
        var reprocessingAddress = new AddressDto { Id = 101, AddressLine1 = "Address line 1" };
        var legalAddress = new AddressDto { Id = 101, AddressLine1 = "Address line 1" };

        // Act
        await _repository.UpdateAsync(registrationId, businessAddress, reprocessingAddress, legalAddress);

        // Assert
        var updatedRegistration = await _context.Registrations.FindAsync(registration.Id);
        updatedRegistration!.BusinessAddressId.Should().Be(1);
        updatedRegistration!.ReprocessingSiteAddressId.Should().Be(2);
        updatedRegistration!.LegalDocumentAddressId.Should().Be(3);
    }

    [TestMethod]
    public async Task UpdateAsync_WithExistingABusinessAddress_ShouldUpdateAccordingly()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var registration = new Registration { Id = 2, ExternalId = registrationId };
        var address = new Address
        {
            Id = 101,
            AddressLine1 = "Address line 1",
            PostCode = "postcode",
            TownCity = "town"
        };

        registration.BusinessAddressId = address.Id;
        registration.LegalDocumentAddressId = address.Id;
        registration.ReprocessingSiteAddressId = address.Id;

        _context.Registrations.Add(registration);
        _context.LookupAddresses.Add(address);

        await _context.SaveChangesAsync();

        var businessAddress = new AddressDto { Id = 101, AddressLine1 = "new Address line 1" };
        var reprocessingAddress = new AddressDto { Id = 101, AddressLine1 = "new Address line 1" };
        var legalAddress = new AddressDto { Id = 101, AddressLine1 = "new Address line 1" };

        // Act
        await _repository.UpdateAsync(registrationId, businessAddress, reprocessingAddress, legalAddress);

        // Assert
        var updatedRegistration = await _context.Registrations.FindAsync(registration.Id);
        updatedRegistration!.BusinessAddressId.Should().Be(101);
        updatedRegistration!.ReprocessingSiteAddressId.Should().Be(101);
        updatedRegistration!.LegalDocumentAddressId.Should().Be(101);
    }

    [TestMethod]
    public async Task GetByOrganisationAsync_Exists_ReturnRegistration()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var registration = new Registration { Id = 2, ApplicationTypeId = 1, ExternalId = Guid.NewGuid(), OrganisationId = organisationId };

        _context.Registrations.Add(registration);

        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByOrganisationAsync(1, organisationId);

        // Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetByOrganisationAsync_DoesNotExist_ReturnNull()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        // Negative data
        var registration = new Registration { Id = 2, ApplicationTypeId = 1, ExternalId = Guid.NewGuid(), OrganisationId = organisationId };

        _context.Registrations.Add(registration);

        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByOrganisationAsync(2, organisationId);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task CreateRegistrationAsync_ShouldCreateNewRecord()
    {
        // Arrange
        var organisationId = Guid.NewGuid();

        // Act
        var result = await _repository.CreateRegistrationAsync(1, organisationId, new AddressDto
        {
            AddressLine1 = "address line 1",
            AddressLine2 = "address line 2",
            PostCode = "cv1 2tt",
            TownCity = "town",
            Country = "country",
            County = "county",
            GridReference = "grid ref",
            NationId = 1
        });

        // Assert
        var loaded = await _context.Registrations.FindAsync(result.Id);
        result.Id.Should().Be(1);
        loaded.Should().BeEquivalentTo(new Registration
        {
            Id = result.Id,
            RegistrationStatusId = 1,
            ReprocessingSiteAddressId = 1,
            ExternalId = loaded!.ExternalId,
            ApplicationTypeId = 1,
            OrganisationId = organisationId,
            ReprocessingSiteAddress = new Address
            {
                Id = 1,
                AddressLine1 = "address line 1",
                AddressLine2 = "address line 2",
                PostCode = "cv1 2tt",
                TownCity = "town",
                County = "county",
                GridReference = "grid ref",
                NationId = 1
            },
            CreatedBy = loaded.CreatedBy,
            CreatedDate = loaded.CreatedDate,
            UpdatedBy = loaded.UpdatedBy
        });
    }
}