using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using EPR.PRN.Backend.Data.Repositories.Regulator;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories.Regulator;

[TestClass]
public class RegistrationAccreditationRepositoryTests
{
    private EprContext _context;
    private IRegulatorRegistrationAccreditationRepository _repository;

    [TestInitialize]
    public void TestInitialize()
    {
        var options = new DbContextOptionsBuilder<EprContext>()
                        .UseInMemoryDatabase(databaseName: "TestDb")
                        .Options;

        _context = new EprContext(options);
        _repository = new RegulatorRegistrationAccreditationRepository(_context);

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        //var address = new Address
        //{
        //    Id = 1,
        //    AddressLine1 = "123 Main St",
        //    AddressLine2 = "Suite 4B",
        //    County = "Bavaria",
        //    PostCode = "12345",
        //    TownCity = "Munich",
        //    GridReference = "123456",
        //    NationId = 1

        //};

        //var taskStatus = new LookupTaskStatus { Id = 1, Name = "Not Started" };
        //var task = new LookupRegulatorTask
        //{
        //    Id = 1,
        //    Name = "SiteAddressAndContactDetails",
        //    IsMaterialSpecific = false,
        //    ApplicationTypeId = 1,
        //    JourneyTypeId = 1
        //};
        //var materialStatus = new LookupRegistrationMaterialStatus { Id = 1, Name = "Completed" };
        //var lookupMaterial = new LookupMaterial { Id = 1, MaterialCode = "PLSTC", MaterialName = "Plastic" };
        //var lookupPeriod = new LookupPeriod { Id = 1, Name = "Per Year" };
        //var lookupMaterialPermit = new LookupMaterialPermit { Id = 1, Name = PermitTypes.WasteManagementLicence };

        //var material = new RegistrationMaterial
        //{
        //    Id = 1,
        //    MaterialId = 1,
        //    StatusId = 1,
        //    RegistrationReferenceNumber = "REF12345",
        //    Comments = "Initial comment",
        //    Status = materialStatus,
        //    Material = lookupMaterial,
        //    IsMaterialRegistered = true,
          
        //    Tasks = new List<RegulatorApplicationTaskStatus>
        //{
        //    new RegulatorApplicationTaskStatus
        //    {
        //        Id = 1,
        //        TaskId = 1,
        //        TaskStatusId = 1,
        //        TaskStatus = taskStatus,
        //        Task = task
        //    }
        //},
        //    PermitType = lookupMaterialPermit,
        //    EnvironmentalPermitWasteManagementPeriod = lookupPeriod,
        //    EnvironmentalPermitWasteManagementTonne = 100,
        //    InstallationPeriod = lookupPeriod,
        //    InstallationReprocessingTonne = 200,
        //    WasteManagementPeriod = lookupPeriod,
        //    WasteManagementReprocessingCapacityTonne = 300,
        //    PPCPeriod = lookupPeriod,
        //    PPCReprocessingCapacityTonne = 400,
        //    MaximumReprocessingCapacityTonne = 500,
        //    MaximumReprocessingPeriod = lookupPeriod,
        //    MaterialExemptionReferences = new List<MaterialExemptionReference> { new MaterialExemptionReference {
        //        Id = 1,
        //        ReferenceNo = "EXEMPT123",
        //        RegistrationMaterialId = 1
        //    }
        //    }
        //};

        //var registration = new Registration
        //{
        //    Id = 1,
        //    ApplicationTypeId = 1,
        //    ExternalId = Guid.NewGuid(),
        //    ReprocessingSiteAddress = address,
        //    BusinessAddress = address,
        //    LegalDocumentAddress = address,
        //    Tasks = new List<RegulatorRegistrationTaskStatus>
        //{
        //    new RegulatorRegistrationTaskStatus
        //    {
        //        Id = 1,
        //        TaskId = 1,
        //        TaskStatusId = 1,
        //        TaskStatus = taskStatus,
        //        Task = task
        //    }
        //},
        //    Materials = new List<RegistrationMaterial> { material }
        //};

        //_context.LookupTasks.Add(task);
        //_context.LookupTaskStatuses.Add(taskStatus);
        //_context.LookupRegistrationMaterialStatuses.Add(materialStatus);
        //_context.LookupMaterials.Add(lookupMaterial);
        //_context.LookupAddresses.Add(address);
        //_context.Registrations.Add(registration);

        //_context.LookupPeriod.Add(lookupPeriod);
        //_context.LookupMaterialPermit.Add(lookupMaterialPermit);

        //_context.SaveChanges();
    }


    //[TestMethod]
    //public async Task RegistrationMaterialsMarkAsDulyMade_ShouldSetDulyMadeCorrectly()
    //{
    //    // Arrange
    //    var dulyMadeDate = DateTime.UtcNow.Date;
    //    var determinationDate = dulyMadeDate.AddDays(84);
    //    var userId = Guid.NewGuid();
    //    // Seed required LookupTask for the CheckAccreditationStatus task
    //    _context.LookupTasks.Add(new LookupRegulatorTask
    //    {
    //        Id = 2,
    //        Name = "CheckAccreditationStatus",
    //        ApplicationTypeId = 1,
    //        JourneyTypeId = 1,
    //        IsMaterialSpecific = true
    //    });

    //    await _context.SaveChangesAsync();

    //    // Act
    //    await _repository.AccreditationMarkAsDulyMade(dulyMadeDate, determinationDate, userId);

    //    // Assert
    //    //var dulyMadeEntry = await _context.DulyMade
    //    //    .FirstOrDefaultAsync(x => x.Id == registrationMaterialId);
    //    //var taskStatusEntry = await _context.RegulatorApplicationTaskStatus
    //    //    .FirstOrDefaultAsync(x => x.RegistrationMaterialId == registrationMaterialId && x.TaskStatusId == statusId);

    //    //using (new AssertionScope())
    //    //{
    //    //    dulyMadeEntry.Should().NotBeNull();
    //    //    dulyMadeEntry!.DulyMadeBy.Should().Be(userId);
    //    //    dulyMadeEntry.DulyMadeDate.Should().Be(dulyMadeDate);
    //    //    dulyMadeEntry.DeterminationDate.Should().Be(determinationDate);

    //    //    taskStatusEntry.Should().NotBeNull();
    //    //    taskStatusEntry.TaskId.Should().Be(2);
    //    //    taskStatusEntry.StatusUpdatedBy.Should().Be(userId);
    //    //    taskStatusEntry.StatusCreatedDate.Date.Should().Be(DateTime.UtcNow.Date);
    //    //}
    //}

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }
}
