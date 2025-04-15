using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using EPR.PRN.Backend.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories;

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
        var registration = new Registration
        {
            Id = 1,
            ApplicationTypeId = 1,
            ExternalId = "TestExternalId"
        };
        _context.Registrations.Add(registration);

        var material = new RegistrationMaterial
        {
            Id = 1,
            RegistrationId = 1,
            MaterialId = 1,
            StatusID = 1,
            ReferenceNumber = "REF12345",
            Comments = "Initial comment"
        };
        _context.RegistrationMaterials.Add(material);

        var lookupMaterial = new LookupMaterial
        {
            Id = 1,
            MaterialCode = "PLSTC",
            MaterialName = "Plastic"
        };
        _context.LookupMaterials.Add(lookupMaterial);

        var status = new LookupRegistrationMaterialStatus
        {
            Id = 1,
            Name = "Completed"
        };
        _context.LookupRegistrationMaterialStatuses.Add(status);

        var lookupTask = new LookupRegulatorTask
        {
            Id = 1,
            Name = "SiteAddressAndContactDetails",
            IsMaterialSpecific = false,
            ApplicationTypeId = 1,
            JourneyTypeId = 1
        };
        _context.LookupTasks.Add(lookupTask);

        var taskStatus = new LookupTaskStatus
        {
            Id = 1,
            Name = "Not Started"
        };
        _context.LookupTaskStatuses.Add(taskStatus);

        var regTaskStatus = new RegulatorRegistrationTaskStatus
        {
            Id = 1,
            RegistrationId = 1,
            TaskId = 1,
            TaskStatusId = 1
        };
        _context.RegulatorRegistrationTaskStatus.Add(regTaskStatus);

        var appTaskStatus = new RegulatorApplicationTaskStatus
        {
            Id = 1,
            RegistrationMaterialId = 1,
            TaskId = 1,
            TaskStatusId = 1
        };
        _context.RegulatorApplicationTaskStatus.Add(appTaskStatus);

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
        _context.LookupAddresses.Add(address);

        _context.SaveChanges();
    }

    [TestMethod]
    public async Task GetRegistrationById_ShouldReturnRegistration_WhenRegistrationExists()
    {
        var result = await _repository.GetRegistrationById(1);
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
    }

    [TestMethod]
    public async Task GetRegistrationById_ShouldThrowKeyNotFoundException_WhenNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => _repository.GetRegistrationById(999));
    }

    [TestMethod]
    public async Task GetMaterialsByRegistrationId_ShouldReturnMaterials()
    {
        var result = await _repository.GetMaterialsByRegistrationId(1);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("Plastic", result[0].Material.MaterialName);
    }

    [TestMethod]
    public async Task GetMaterialsByRegistrationId_ShouldReturnEmptyList_WhenNoMatch()
    {
        var result = await _repository.GetMaterialsByRegistrationId(999);
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public async Task GetRequiredTasks_ShouldReturnCorrectTasks()
    {
        var result = await _repository.GetRequiredTasks(1, false);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("SiteAddressAndContactDetails", result[0].Name);
    }

    [TestMethod]
    public async Task GetRegistrationMaterialById_ShouldReturnCorrectMaterial()
    {
        var result = await _repository.GetRegistrationMaterialById(1);
        Assert.AreEqual("REF12345", result.ReferenceNumber);
        Assert.AreEqual("Plastic", result.Material.MaterialName);
    }

    [TestMethod]
    public async Task GetRegistrationMaterialById_ShouldThrow_WhenNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => _repository.GetRegistrationMaterialById(999));
    }

    [TestMethod]
    public async Task UpdateRegistrationOutCome_ShouldUpdateFieldsCorrectly()
    {
        var newStatusId = 2;
        var comment = "Updated";
        var newReference = "REF99999";

        await _repository.UpdateRegistrationOutCome(1, newStatusId, comment, newReference);
        var updated = await _context.RegistrationMaterials.FindAsync(1);

        Assert.AreEqual(newStatusId, updated.StatusID);
        Assert.AreEqual(comment, updated.Comments);
        Assert.AreEqual(newReference, updated.ReferenceNumber);
    }

    [TestMethod]
    public async Task GetRegistrationTasks_ShouldReturnCorrectTasks()
    {
        var result = await _repository.GetRegistrationTasks(1);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(1, result[0].TaskId);
    }

    [TestMethod]
    public async Task GetMaterialTasks_ShouldReturnCorrectTasks()
    {
        var result = await _repository.GetMaterialTasks(1);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(1, result[0].TaskId);
    }

    [TestMethod]
    public async Task GetAddressById_ShouldReturnCorrectAddress()
    {
        var result = await _repository.GetAddressById(1);
        Assert.IsNotNull(result);
        Assert.AreEqual("123 Main St", result.AddressLine1);
    }

    [TestMethod]
    public async Task GetMaterialById_ShouldReturnCorrectMaterial()
    {
        var result = await _repository.GetMaterialById(1);
        Assert.IsNotNull(result);
        Assert.AreEqual("PLSTC", result.MaterialCode);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }
}
