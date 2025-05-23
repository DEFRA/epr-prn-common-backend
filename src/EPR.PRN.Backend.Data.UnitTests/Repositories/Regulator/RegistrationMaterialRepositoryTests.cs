using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using EPR.PRN.Backend.Data.Repositories.Regulator;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories.Regulator;

[TestClass]
public class RegistrationMaterialRepositoryTests
{
    private EprContext _context;
    private IRegistrationMaterialRepository _repository;

    [TestInitialize]
    public void TestInitialize()
    {
        var options = new DbContextOptionsBuilder<EprContext>()
                        .UseInMemoryDatabase(databaseName: "TestDb")
                        .Options;

        _context = new EprContext(options);
        _repository = new RegistrationMaterialRepository(_context);

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        var address = new Address
        {
            Id = 1,
            AddressLine1 = "123 Main St",
            AddressLine2 = "Suite 4B",
            County = "Bavaria",
            PostCode = "12345",
            TownCity = "Munich",
            GridReference = "123456",
            NationId = 1

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
        var lookupPeriod = new LookupPeriod { Id = 1, Name = "Per Year" };
        var lookupMaterialPermit = new LookupMaterialPermit { Id = 1, Name = PermitTypes.WasteManagementLicence };

        var material = new RegistrationMaterial
        {
            Id = 1,
            ExternalId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"),
            MaterialId = 1,
            StatusId = 1,
            RegistrationReferenceNumber = "REF12345",
            Comments = "Initial comment",
            Status = materialStatus,
            Material = lookupMaterial,
            IsMaterialRegistered = true,
          
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
        },
            PermitType = lookupMaterialPermit,
            EnvironmentalPermitWasteManagementPeriod = lookupPeriod,
            EnvironmentalPermitWasteManagementTonne = 100,
            InstallationPeriod = lookupPeriod,
            InstallationReprocessingTonne = 200,
            WasteManagementPeriod = lookupPeriod,
            WasteManagementReprocessingCapacityTonne = 300,
            PPCPeriod = lookupPeriod,
            PPCReprocessingCapacityTonne = 400,
            MaximumReprocessingCapacityTonne = 500,
            MaximumReprocessingPeriod = lookupPeriod,
            MaterialExemptionReferences = new List<MaterialExemptionReference> { new MaterialExemptionReference {
                Id = 1,
                ReferenceNo = "EXEMPT123",
                RegistrationMaterialId = 1
            }
            }
        };

        var registration = new Registration
        {
            Id = 1,
            ApplicationTypeId = 1,
            ExternalId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d"),
            ReprocessingSiteAddress = address,
            BusinessAddress = address,
            LegalDocumentAddress = address,
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

        _context.LookupPeriod.Add(lookupPeriod);
        _context.LookupMaterialPermit.Add(lookupMaterialPermit);

        _context.SaveChanges();
    }


    [TestMethod]
    public async Task GetRegistrationById_ShouldReturnRegistration_WhenRegistrationExists()
    {
        var result = await _repository.GetRegistrationById(Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d"));
        using (new AssertionScope())
        {
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.IsNotNull(result.Materials);
            Assert.IsNotNull(result.Tasks);
        }
    }

    [TestMethod]
    public async Task GetRegistrationById_ShouldThrowKeyNotFoundException_WhenNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => _repository.GetRegistrationById(Guid.Parse("cd9dcc80-fcf5-4f46-addd-b8a256f735a3")));
    }

    [TestMethod]
    public async Task GetRequiredTasks_ShouldReturnCorrectTasks()
    {
        var result = await _repository.GetRequiredTasks(1, false, 1);
        using (new AssertionScope())
        {
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("SiteAddressAndContactDetails", result[0].Name);
        }
    }

    [TestMethod]
    public async Task UpdateRegistrationOutCome_ShouldUpdateFieldsCorrectly()
    {
        var newStatusId = 2;
        var comment = "Updated comment";
        var newReference = "REFUPDATED";

        await _repository.UpdateRegistrationOutCome(Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"), newStatusId, comment, newReference);
        var updated = await _context.RegistrationMaterials.FindAsync(1);

        using (new AssertionScope())
        {
            Assert.AreEqual(newStatusId, updated.StatusId);
            Assert.AreEqual(comment, updated.Comments);
            Assert.AreEqual(newReference, updated.RegistrationReferenceNumber);
            Assert.IsNotNull(updated.StatusUpdatedDate);
        }
    }

    [TestMethod]
    public async Task UpdateRegistrationOutCome_ShouldThrow_WhenMaterialNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() =>
            _repository.UpdateRegistrationOutCome(Guid.Parse("cd9dcc80-fcf5-4f46-addd-b8a256f735a3"), 1, "Test", "REF"));
    }

    [TestMethod]
    public async Task GetRegistrationMaterialById_ShouldReturnMaterial_WhenExists()
    {
        var material = await _repository.GetRegistrationMaterialById(Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"));
        using (new AssertionScope())
        {
            Assert.IsNotNull(material);
            Assert.AreEqual("REF12345", material.RegistrationReferenceNumber);
            Assert.IsNotNull(material.Material);
            Assert.IsNotNull(material.Status);
            Assert.IsNotNull(material.Registration);
        }
    }

    [TestMethod]
    public async Task GetRegistrationMaterialById_ShouldThrow_WhenNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => _repository.GetRegistrationMaterialById(Guid.Parse("cd9dcc80-fcf5-4f46-addd-b8a256f735a3")));
    }

    [TestMethod]
    public async Task GetRegistrationMaterial_WasteLicencesById_ShouldReturnMaterial_WhenExists()
    {
        var material = await _repository.GetRegistrationMaterial_WasteLicencesById(Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"));
        using (new AssertionScope())
        {
            Assert.IsNotNull(material);
            Assert.AreEqual("REF12345", material.RegistrationReferenceNumber);
            Assert.IsNotNull(material.Material);
        }
    }

    [TestMethod]
    public async Task GetRegistrationMaterial_WasteLicencesById_ShouldThrow_WhenNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => _repository.GetRegistrationMaterial_WasteLicencesById(Guid.Parse("cd9dcc80-fcf5-4f46-addd-b8a256f735a3")));
    }

    [TestMethod]
    public async Task GetRegistrationMaterial_RegistrationReprocessingIOById_ShouldReturnMaterial_WhenExists()
    {
        var material = await _repository.GetRegistrationMaterial_RegistrationReprocessingIOById(Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"));
        using (new AssertionScope())
        {
            Assert.IsNotNull(material);
            Assert.AreEqual("REF12345", material.RegistrationReferenceNumber);
            Assert.IsNotNull(material.Material);
        }
    }

    [TestMethod]
    public async Task GetRegistrationMaterial_RegistrationReprocessingIOById_ShouldThrow_WhenNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => _repository.GetRegistrationMaterial_RegistrationReprocessingIOById(Guid.Parse("cd9dcc80-fcf5-4f46-addd-b8a256f735a3")));
    }

    [TestMethod]
    public async Task GetRegistrationMaterial_FileUploadById_ShouldReturnMaterial_WhenExists()
    {
        var material = await _repository.GetRegistrationMaterial_FileUploadById(Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"));
        using (new AssertionScope())
        {
            Assert.IsNotNull(material);
            Assert.AreEqual("REF12345", material.RegistrationReferenceNumber);
        }
    }

    [TestMethod]
    public async Task GetRegistrationMaterial_FileUploadById_ShouldThrow_WhenNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => _repository.GetRegistrationMaterial_FileUploadById(Guid.Parse("cd9dcc80-fcf5-4f46-addd-b8a256f735a3")));
    }
    [TestMethod]
    public async Task RegistrationMaterialsMarkAsDulyMade_ShouldSetDulyMadeCorrectly()
    {
        // Arrange
        var registrationMaterialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var statusId = 3;
        var dulyMadeDate = DateTime.UtcNow.Date;
        var determinationDate = dulyMadeDate.AddDays(84);
        var userId = Guid.NewGuid();
        // Seed required LookupTask for the CheckRegistrationStatus task
        _context.LookupTasks.Add(new LookupRegulatorTask
        {
            Id = 2,
            Name = "CheckRegistrationStatus",
            ApplicationTypeId = 1,
            JourneyTypeId = 1,
            IsMaterialSpecific = true
        });

        await _context.SaveChangesAsync();

        // Act
        await _repository.RegistrationMaterialsMarkAsDulyMade(registrationMaterialId, statusId, determinationDate, dulyMadeDate, userId);

        // Assert
        var dulyMadeEntry = await _context.DulyMade
            .FirstOrDefaultAsync(x => x.RegistrationMaterial.ExternalId == registrationMaterialId);
        var taskStatusEntry = await _context.RegulatorApplicationTaskStatus
            .FirstOrDefaultAsync(x => x.RegistrationMaterial.ExternalId == registrationMaterialId && x.TaskStatusId == statusId);

        using (new AssertionScope())
        {
            dulyMadeEntry.Should().NotBeNull();
            dulyMadeEntry!.DulyMadeBy.Should().Be(userId);
            dulyMadeEntry.DulyMadeDate.Should().Be(dulyMadeDate);
            dulyMadeEntry.DeterminationDate.Should().Be(determinationDate);
            dulyMadeEntry.TaskStatusId.Should().Be(statusId);

            taskStatusEntry.Should().NotBeNull();
            taskStatusEntry!.TaskStatusId.Should().Be(statusId);
            taskStatusEntry.TaskId.Should().Be(2);
            taskStatusEntry.StatusUpdatedBy.Should().Be(userId);
            taskStatusEntry.StatusCreatedDate.Date.Should().Be(DateTime.UtcNow.Date);
        }
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }
}
