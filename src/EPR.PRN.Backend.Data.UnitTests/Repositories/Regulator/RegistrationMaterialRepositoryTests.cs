using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using EPR.PRN.Backend.Data.Repositories.Regulator;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories.Regulator;

[TestClass]
public class RegistrationMaterialRepositoryTests
{
    private EprRegistrationsContext _context;
    private IRegistrationMaterialRepository _repository;

    [TestInitialize]
    public void TestInitialize()
    {
        var options = new DbContextOptionsBuilder<EprRegistrationsContext>()
                        .UseInMemoryDatabase(databaseName: "TestDb")
                        .Options;

        _context = new EprRegistrationsContext(options);
        _repository = new RegistrationMaterialRepository(_context);

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        var address = new LookupAddress
        {
            Id = 1,
            AddressLine1 = "123 Main St",
            AddressLine2 = "Suite 4B",
            Country = "Germany",
            County = "Bavaria",
            PostCode = "12345",
            TownCity = "Munich"
        };

        var taskStatus = new LookupTaskStatus { Id = 1, Name = "Not Started" };
        var task = new LookupRegulatorTask
        {
            Id = 1,
            Name = "SiteAddressAndContactDetails",
            IsMaterialSpecific = false,
            ApplicationTypeId = 1,
            JourneyTypeId = 1
        };
        var materialStatus = new LookupRegistrationMaterialStatus { Id = 1, Name = "Completed" };
        var lookupMaterial = new LookupMaterial { Id = 1, MaterialCode = "PLSTC", MaterialName = "Plastic" };

        var material = new RegistrationMaterial
        {
            Id = 1,
            MaterialId = 1,
            StatusID = 1,
            ReferenceNumber = "REF12345",
            Comments = "Initial comment",
            Status = materialStatus,
            Material = lookupMaterial,
            Tasks = new List<RegulatorApplicationTaskStatus>
        {
            new RegulatorApplicationTaskStatus
            {
                Id = 1,
                TaskId = 1,
                TaskStatusId = 1,
                TaskStatus = taskStatus,
                Task = task
            }
        }
        };

        var registration = new Registration
        {
            Id = 1,
            ApplicationTypeId = 1,
            ExternalId = "TestExternalId",
            ReprocessingSiteAddress = address,
            BusinessAddress = address,
            Tasks = new List<RegulatorRegistrationTaskStatus>
        {
            new RegulatorRegistrationTaskStatus
            {
                Id = 1,
                TaskId = 1,
                TaskStatusId = 1,
                TaskStatus = taskStatus,
                Task = task
            }
        },
            Materials = new List<RegistrationMaterial> { material }
        };

        _context.LookupTasks.Add(task);
        _context.LookupTaskStatuses.Add(taskStatus);
        _context.LookupRegistrationMaterialStatuses.Add(materialStatus);
        _context.LookupMaterials.Add(lookupMaterial);
        _context.LookupAddresses.Add(address);
        _context.Registrations.Add(registration);

        _context.SaveChanges();
    }


    [TestMethod]
    public async Task GetRegistrationById_ShouldReturnRegistration_WhenRegistrationExists()
    {
        var result = await _repository.GetRegistrationById(1);
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
        Assert.IsNotNull(result.Materials);
        Assert.IsNotNull(result.Tasks);
    }

    [TestMethod]
    public async Task GetRegistrationById_ShouldThrowKeyNotFoundException_WhenNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => _repository.GetRegistrationById(999));
    }

    [TestMethod]
    public async Task GetRequiredTasks_ShouldReturnCorrectTasks()
    {
        var result = await _repository.GetRequiredTasks(1, false);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("SiteAddressAndContactDetails", result[0].Name);
    }

    [TestMethod]
    public async Task UpdateRegistrationOutCome_ShouldUpdateFieldsCorrectly()
    {
        var newStatusId = 2;
        var comment = "Updated comment";
        var newReference = "REFUPDATED";

        await _repository.UpdateRegistrationOutCome(1, newStatusId, comment, newReference);
        var updated = await _context.RegistrationMaterials.FindAsync(1);

        Assert.AreEqual(newStatusId, updated.StatusID);
        Assert.AreEqual(comment, updated.Comments);
        Assert.AreEqual(newReference, updated.ReferenceNumber);
        Assert.IsNotNull(updated.StatusUpdatedDate);
        Assert.AreEqual("Test User", updated.StatusUpdatedBy);
    }

    [TestMethod]
    public async Task UpdateRegistrationOutCome_ShouldThrow_WhenMaterialNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() =>
            _repository.UpdateRegistrationOutCome(999, 1, "Test", "REF"));
    }

    [TestMethod]
    public async Task GetRegistrationMaterialById_ShouldReturnMaterial_WhenExists()
    {
        var material = await _repository.GetRegistrationMaterialById(1);
        Assert.IsNotNull(material);
        Assert.AreEqual("REF12345", material.ReferenceNumber);
        Assert.IsNotNull(material.Material);
        Assert.IsNotNull(material.Status);
        Assert.IsNotNull(material.Registration);
    }

    [TestMethod]
    public async Task GetRegistrationMaterialById_ShouldThrow_WhenNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => _repository.GetRegistrationMaterialById(999));
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }
}
