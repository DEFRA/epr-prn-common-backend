using System.Xml.XPath;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Testing.Platform.Extensions;
using Moq;
using Moq.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories;

[TestClass]
public class MaterialRepositoryTests
{
	private MaterialRepository _materialRepository;
    private MaterialRepository _materialRepositoryFull;
    private Mock<EprContext> _mockEprContext;
    private Mock<ILogger<MaterialRepository>> _mockLogger;
    private EprContext _context;
    private readonly List<Material> _materials =
		[
			new Material { Id = 1, MaterialCode = "PL", MaterialName = MaterialType.Plastic.ToString() },
			new Material { Id = 2, MaterialCode = "WD", MaterialName = MaterialType.Wood.ToString() },
			new Material { Id = 3, MaterialCode = "AL", MaterialName = MaterialType.Aluminium.ToString() },
			new Material { Id = 4, MaterialCode = "ST", MaterialName = MaterialType.Steel.ToString() },
			new Material { Id = 5, MaterialCode = "PC", MaterialName = MaterialType.Paper.ToString() },
			new Material { Id = 6, MaterialCode = "GL", MaterialName = MaterialType.Glass.ToString() },
			new Material { Id = 7, MaterialCode = "GR", MaterialName = MaterialType.GlassRemelt.ToString() },
			new Material { Id = 8, MaterialCode = "FC", MaterialName = MaterialType.FibreComposite.ToString() }
		];

	[TestInitialize]
	public void Setup()
	{
		var dbContextOptions = new DbContextOptionsBuilder<EprContext>().Options;
		_mockEprContext = new Mock<EprContext>(dbContextOptions);
		_mockEprContext.Setup(context => context.Material).ReturnsDbSet(_materials);
        _mockLogger = new Mock<ILogger<MaterialRepository>>();
        _materialRepository = new MaterialRepository(_mockEprContext.Object, _mockLogger.Object);

        var options = new DbContextOptionsBuilder<EprContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new EprContext(options);
        _materialRepositoryFull = new MaterialRepository(_context, _mockLogger.Object);
    }

	[TestMethod]
	public async Task GetAllMaterials_ShouldReturnAllMaterials()
	{
		// Act
		var result = await _materialRepository.GetAllMaterials();

		// Assert
		result.Should().NotBeNull(); // Check that result is not null
		result.Should().HaveCount(8); // Check that 7 materials are returned
		result.Should().Contain(material => material.MaterialCode == "PL" && material.MaterialName == MaterialType.Plastic.ToString());
		result.Should().Contain(material => material.MaterialCode == "WD" && material.MaterialName == MaterialType.Wood.ToString());
		result.Should().Contain(material => material.MaterialCode == "AL" && material.MaterialName == MaterialType.Aluminium.ToString());
		result.Should().Contain(material => material.MaterialCode == "ST" && material.MaterialName == MaterialType.Steel.ToString());
		result.Should().Contain(material => material.MaterialCode == "PC" && material.MaterialName == MaterialType.Paper.ToString());
		result.Should().Contain(material => material.MaterialCode == "GL" && material.MaterialName == MaterialType.Glass.ToString());
		result.Should().Contain(material => material.MaterialCode == "GR" && material.MaterialName == MaterialType.GlassRemelt.ToString());
		result.Should().Contain(material => material.MaterialCode == "FC" && material.MaterialName == MaterialType.FibreComposite.ToString());
	}

    [TestMethod]
    public async Task UpsertRegistrationMaterialContact_ShouldCreateNewContact_WhenNoneExists()
    {
        // Arrange
        var registrationMaterialId = 5;
        var registrationMaterialExternalId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var registrationMaterials = new List<RegistrationMaterial>
        {
            new() { Id = registrationMaterialId, ExternalId = registrationMaterialExternalId, RegistrationMaterialContact = null }
        };

        _mockEprContext.Setup(c => c.RegistrationMaterials)
            .ReturnsDbSet(registrationMaterials);

        _mockEprContext.Setup(c => c.RegistrationMaterialContacts)
            .ReturnsDbSet(new List<RegistrationMaterialContact>());

        // Act
        var result = await _materialRepository.UpsertRegistrationMaterialContact(registrationMaterialExternalId, userId);

        // Assert
        result.Should().NotBeNull();
        result.RegistrationMaterialId.Should().Be(registrationMaterialId);
        
        _mockEprContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [TestMethod]
    public async Task UpsertRegistrationMaterialContact_ShouldUpdateExistingContact_WhenExists()
    {
        var registrationMaterialId = 5;
        var registrationMaterialExternalId = Guid.NewGuid();
        var registrationMaterialContactId = 100;
        var userId = Guid.NewGuid();

        var registrationMaterialContact = new RegistrationMaterialContact
        {
            Id = registrationMaterialContactId,
            ExternalId = Guid.NewGuid(),
            RegistrationMaterialId = registrationMaterialId,
            UserId = Guid.NewGuid()
        };

        var registrationMaterials = new List<RegistrationMaterial>
        {
            new() { Id = registrationMaterialId, ExternalId = registrationMaterialExternalId, RegistrationMaterialContact = registrationMaterialContact }
        };

        _mockEprContext.Setup(c => c.RegistrationMaterials)
            .ReturnsDbSet(registrationMaterials);

        _mockEprContext.Setup(c => c.RegistrationMaterialContacts)
            .ReturnsDbSet(new List<RegistrationMaterialContact> { registrationMaterialContact});

        // Act
        var result = await _materialRepository.UpsertRegistrationMaterialContact(registrationMaterialExternalId, userId);

        // Assert
        result.Should().NotBeNull();
        result.RegistrationMaterialId.Should().Be(registrationMaterialId);
        result.UserId.Should().Be(userId);

        _mockEprContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [TestMethod]
    public async Task UpsertRegistrationMaterialContact_ShouldThrow_WhenRegistrationMaterialNotFound()
    {
        // Arrange
        var registrationMaterialExternalId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        _mockEprContext.Setup(c => c.RegistrationMaterials)
            .ReturnsDbSet(new List<RegistrationMaterial>());
        
        // Act & Assert
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
        {
            await _materialRepository.UpsertRegistrationMaterialContact(registrationMaterialExternalId, userId);
        });
    }

    [TestMethod]
    public async Task UpdateApplicantRegistrationTaskStatusAsync_ShouldAddNewStatus_WhenNotExists()
    {
        // Arrange
        var registrationMaterialId = Guid.NewGuid();
        var registrationMaterial = new RegistrationMaterial { Id = 1, ExternalId = registrationMaterialId, RegistrationId = 1 };
        var registration = new Registration { Id = 1, ApplicationTypeId = 1, ExternalId = Guid.NewGuid() };
        var task = new LookupApplicantRegistrationTask { Name = "NewTask", ApplicationTypeId = 1, IsMaterialSpecific = true };
        var status = new LookupTaskStatus { Name = nameof(TaskStatuses.Completed) };

        await _context.Registrations.AddAsync(registration);
        await _context.RegistrationMaterials.AddAsync(registrationMaterial);
        await _context.LookupApplicantRegistrationTasks.AddAsync(task);
        await _context.LookupTaskStatuses.AddAsync(status);
        await _context.SaveChangesAsync();

        // Act
        await _materialRepositoryFull.UpdateApplicantRegistrationTaskStatusAsync("NewTask", registrationMaterialId, TaskStatuses.Completed);

        // Assert
        var result = await _materialRepositoryFull.GetTaskStatusAsync("NewTask", registrationMaterialId);
        result.Should().NotBeNull();
        result!.TaskStatus.Name.Should().Be(TaskStatuses.Completed.ToString());
    }

    [TestMethod]
    public async Task UpdateApplicantRegistrationTaskStatusAsync_ShouldUpdateExistingStatus()
    {
        // Arrange
        var registrationMaterialId = Guid.NewGuid();
        var registrationMaterial = new RegistrationMaterial { Id = 1, ExternalId = registrationMaterialId, RegistrationId = 1 };
        var registration = new Registration { Id = 1, ApplicationTypeId = 1, ExternalId = Guid.NewGuid() };
        var task = new LookupApplicantRegistrationTask { Name = "ExistingTask", ApplicationTypeId = 1, IsMaterialSpecific = false };
        var oldStatus = new LookupTaskStatus { Name = TaskStatuses.Started.ToString() };
        var newStatus = new LookupTaskStatus { Name = TaskStatuses.Completed.ToString() };

        await _context.Registrations.AddAsync(registration);
        await _context.RegistrationMaterials.AddAsync(registrationMaterial);
        await _context.LookupApplicantRegistrationTasks.AddAsync(task);
        await _context.LookupTaskStatuses.AddRangeAsync(oldStatus, newStatus);

        var taskStatus = new ApplicantRegistrationTaskStatus
        {
            RegistrationId = 2,
            Task = task,
            TaskStatus = oldStatus,
            RegistrationMaterialId = registrationMaterial.Id,
        };

        await _context.RegistrationTaskStatus.AddAsync(taskStatus);
        await _context.SaveChangesAsync();

        // Act
        await _materialRepositoryFull.UpdateApplicantRegistrationTaskStatusAsync("ExistingTask", registrationMaterialId, TaskStatuses.Completed);

        // Assert
        var result = await _materialRepositoryFull.GetTaskStatusAsync("ExistingTask", registrationMaterialId);
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
        Func<Task> act = async () => await _materialRepositoryFull.UpdateApplicantRegistrationTaskStatusAsync("AnyTask", registrationId, TaskStatuses.Completed);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [TestMethod]
    public async Task UpdateApplicantRegistrationTaskStatusAsync_ShouldThrow_WhenTaskNotFound()
    {
        // Arrange
        var registrationMaterialId = Guid.NewGuid();
        var registrationMaterial = new RegistrationMaterial { Id = 1, ExternalId = registrationMaterialId, RegistrationId = 1 };
        var registration = new Registration { Id = 1, ApplicationTypeId = 1, ExternalId = Guid.NewGuid() };
        var status = new LookupTaskStatus { Name = TaskStatuses.Completed.ToString() };

        await _context.Registrations.AddAsync(registration);
        await _context.RegistrationMaterials.AddAsync(registrationMaterial);
        await _context.LookupTaskStatuses.AddAsync(status);
        await _context.SaveChangesAsync();

        // Act
        Func<Task> act = async () => await _materialRepositoryFull.UpdateApplicantRegistrationTaskStatusAsync("MissingTask", registrationMaterialId, TaskStatuses.Completed);

        // Assert
        await act.Should().ThrowAsync<RegulatorInvalidOperationException>()
            .WithMessage("No Valid Task Exists: MissingTask");
    }
}