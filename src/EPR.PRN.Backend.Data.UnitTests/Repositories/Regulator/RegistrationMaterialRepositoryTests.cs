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
    public void Setup()
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
            ReferenceNumber = "REF12345"
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
            JourneyTypeId =1

        };
        _context.LookupTasks.Add(lookupTask);

        _context.SaveChanges();
    }



    [TestMethod]
    public async Task GetRegistrationById_ShouldReturnRegistration_WhenRegistrationExists()
    {
        // Act
        var result = await _repository.GetRegistrationById(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
    }

    [TestMethod]
    public async Task GetRegistrationById_ShouldThrowKeyNotFoundException_WhenRegistrationDoesNotExist()
    {
        // Act & Assert
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => _repository.GetRegistrationById(99));
    }

    [TestMethod]
    public async Task GetMaterialsByRegistrationId_ShouldReturnMaterials_WhenMaterialsExist()
    {
        // Act
        var result = await _repository.GetMaterialsByRegistrationId(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(1, result[0].Id);
    }

    [TestMethod]
    public async Task GetMaterialsByRegistrationId_ShouldReturnEmptyList_WhenNoMaterialsExist()
    {
        // Act
        var result = await _repository.GetMaterialsByRegistrationId(2); 

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public async Task GetRequiredTasks_ShouldReturnTasks_WhenTasksExist()
    {
        // Act
        var result = await _repository.GetRequiredTasks(1, false);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("SiteAddressAndContactDetails", result[0].Name);
    }

    [TestMethod]
    public async Task GetRegistrationMaterialById_ShouldReturnMaterial_WhenMaterialExists()
    {
        // Act
        var result = await _repository.GetRegistrationMaterialById(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
    }

    [TestMethod]
    public async Task GetRegistrationMaterialById_ShouldThrowKeyNotFoundException_WhenMaterialDoesNotExist()
    {
        // Act & Assert
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => _repository.GetRegistrationMaterialById(99));
    }

    [TestMethod]
    public async Task UpdateRegistrationOutCome_ShouldUpdateMaterial_WhenValidDataProvided()
    {
        // Arrange
        var statusId = 2;
        var comment = "Updated comment";
        var registrationReferenceNumber = "REF98765";

        // Act
        await _repository.UpdateRegistrationOutCome(1, statusId, comment, registrationReferenceNumber);
        var updatedMaterial = await _context.RegistrationMaterials.FirstOrDefaultAsync(m => m.Id == 1);

        // Assert
        Assert.IsNotNull(updatedMaterial);
        Assert.AreEqual(statusId, updatedMaterial.StatusID);
        Assert.AreEqual(comment, updatedMaterial.Comments);
        Assert.AreEqual(registrationReferenceNumber, updatedMaterial.ReferenceNumber);
    }

    [TestMethod]
    public async Task GetRegistrationReferenceDataId_ShouldReturnCorrectData_WhenValidDataExists()
    {
        // Act
        var result = await _repository.GetRegistrationReferenceDataId(1, 1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("R", result.OrganisationType); 
        Assert.AreEqual("UNK", result.CountryCode);
        Assert.AreEqual("PLSTC", result.MaterialCode);
    }


    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();  
    }
}
