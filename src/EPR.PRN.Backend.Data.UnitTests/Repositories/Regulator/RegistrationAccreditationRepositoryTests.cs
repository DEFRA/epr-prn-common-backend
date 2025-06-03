using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
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

    [TestMethod]
    public async Task GetAccreditationPaymentFeesById_ReturnsExpectedAccreditation()
    {
        var result = await _repository.GetAccreditationPaymentFeesById(Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d"));

        // Assert
        using (new AssertionScope())
        {
            Assert.IsNotNull(result);
            Assert.AreEqual(Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d"), result.ExternalId);
            Assert.IsNotNull(result.RegistrationMaterial);
        }
    }

    [TestMethod]
    public async Task GetRegistrationById_ShouldThrowKeyNotFoundException_WhenNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => _repository.GetAccreditationPaymentFeesById(Guid.Parse("cd9dcc80-fcf5-4f46-addd-b8a256f735a3")));
    }


    [TestMethod]
    public async Task AccreditationMarkAsDulyMade_Throws_WhenAccreditationNotFound()
    {
        // Act & Assert
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() =>
            _repository.AccreditationMarkAsDulyMade(Guid.NewGuid(), 1, DateTime.UtcNow, DateTime.UtcNow, Guid.NewGuid()));
    }


    [TestMethod]
    public async Task AccreditationMarkAsDulyMade_ShouldSetDulyMadeCorrectly()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var registrationMaterialId = Guid.NewGuid();
        var statusId = 5;
        var dulyMadeDate = DateTime.UtcNow.Date;
        var determinationDate = dulyMadeDate.AddDays(84);
        var userId = Guid.NewGuid();

        var registration = new Registration
        {
            Id = 99,
            ApplicationTypeId = 1
        };

        var material = new RegistrationMaterial
        {
            Id = 99,
            ExternalId = registrationMaterialId,
            RegistrationId = registration.Id,
            Registration = registration
        };

        var accreditation = new Accreditation
        {
            Id = 99,
            ExternalId = accreditationId,
            RegistrationMaterialId = material.Id,
            RegistrationMaterial = material,
            ApplicationReferenceNumber = "ACC12345",
        };

        var lookupTask = new LookupRegulatorTask
        {
            Id = 27,
            Name = "DulyMade",
            ApplicationTypeId = registration.ApplicationTypeId,
            JourneyTypeId = 1,
            IsMaterialSpecific = true
        };

        _context.Registrations.Add(registration);
        _context.RegistrationMaterials.Add(material);
        _context.Accreditations.Add(accreditation);
        _context.LookupTasks.Add(lookupTask);

        await _context.SaveChangesAsync();

        // Act
        await _repository.AccreditationMarkAsDulyMade(accreditationId, statusId, dulyMadeDate, determinationDate, userId);

        // Assert 
        var dulyMadeEntry = await _context.AccreditationDulyMade
            .FirstOrDefaultAsync(x => x.ExternalId == registrationMaterialId);
        var taskStatusEntry = await _context.RegulatorAccreditationTaskStatus
            .FirstOrDefaultAsync(x => x.ExternalId == registrationMaterialId && x.TaskStatusId == statusId);


        using (new AssertionScope())
        {
            dulyMadeEntry.Should().NotBeNull();
            dulyMadeEntry!.DulyMadeBy.Should().Be(userId);
            dulyMadeEntry.DulyMadeDate.Should().Be(dulyMadeDate);
            dulyMadeEntry.DeterminationDate.Should().Be(determinationDate);
            dulyMadeEntry.TaskStatusId.Should().Be(statusId);

            taskStatusEntry.Should().NotBeNull();
            taskStatusEntry!.TaskStatusId.Should().Be(statusId);
            taskStatusEntry.RegulatorTaskId.Should().Be(lookupTask.Id);
            taskStatusEntry.StatusUpdatedBy.Should().Be(userId);
            taskStatusEntry.StatusCreatedDate.Date.Should().Be(DateTime.UtcNow.Date);
        }
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
                RegulatorTaskId = 1,
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
                RegulatorTaskId = 1,
                TaskStatusId = 1,
                TaskStatus = taskStatus,
                Task = task
            }
        },
            Materials = new List<RegistrationMaterial> { material }
        };

        var Accreditation_registration = new Registration
        {
            Id = 2,
            ApplicationTypeId = 1,
            ExternalId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c5d"),
            ReprocessingSiteAddress = address,
            BusinessAddress = address,
            LegalDocumentAddress = address,
            AccreditationTasks = new List<RegulatorAccreditationRegistrationTaskStatus>
            {
                new RegulatorAccreditationRegistrationTaskStatus
                {
                    Id = 2,
                    RegulatorTaskId = 1,
                    TaskStatusId = 1,
                    TaskStatus = taskStatus,
                    Task = task,
                    AccreditationYear = 2025,

                },
                new RegulatorAccreditationRegistrationTaskStatus
                {
                    Id = 3,
                    RegulatorTaskId = 1,
                    TaskStatusId = 1,
                    TaskStatus = taskStatus,
                    Task = task,
                    AccreditationYear = 2026,
                }
            },
            Materials = new List<RegistrationMaterial> {
                new RegistrationMaterial {
                    Id = 2,
                    ExternalId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408754a9"),
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
                            Id = 3,
                            RegulatorTaskId = 1,
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
                                Id = 2,
                                ReferenceNo = "EXEMPT123",
                                RegistrationMaterialId = 2
                            }
                        },
                    Accreditations = new List<Accreditation>{
                        new Accreditation
                        {
                            Id = 1,
                            ExternalId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d"),
                            RegistrationMaterialId = 2,
                            AccreditationStatusId = 1,
                            ApplicationReferenceNumber = "ACC12345",
                            AccreditationYear = 2025,
                            AccreditationStatus = new LookupAccreditationStatus
                            {
                                Id = 1,
                                Name = "Pending"
                            },
                            Tasks = new List<RegulatorAccreditationTaskStatus>
                            {
                                new RegulatorAccreditationTaskStatus
                                {
                                    Id = 1,
                                    RegulatorTaskId = 1,
                                    TaskStatusId = 1,
                                    TaskStatus = taskStatus,
                                    Task = task
                                }
                            }
                        },
                                                new Accreditation
                        {
                            Id = 2,
                            RegistrationMaterialId = 2,
                            AccreditationStatusId = 1,
                            ApplicationReferenceNumber = "ACC12345",
                            AccreditationYear = 2026,
                            AccreditationStatus = new LookupAccreditationStatus
                            {
                                Id = 2,
                                Name = "Granted"
                            },
                            Tasks = new List<RegulatorAccreditationTaskStatus>
                            {
                                new RegulatorAccreditationTaskStatus
                                {
                                    Id = 2,
                                    RegulatorTaskId = 1,
                                    TaskStatusId = 1,
                                    TaskStatus = taskStatus,
                                    Task = task
                                }
                            }
                        }
                    }
                }
            }
        };

        _context.LookupTasks.Add(task);
        _context.LookupTaskStatuses.Add(taskStatus);
        _context.LookupRegistrationMaterialStatuses.Add(materialStatus);
        _context.LookupMaterials.Add(lookupMaterial);
        _context.LookupAddresses.Add(address);
        _context.Registrations.Add(registration);

        _context.Registrations.Add(Accreditation_registration);

        _context.LookupPeriod.Add(lookupPeriod);
        _context.LookupMaterialPermit.Add(lookupMaterialPermit);

        _context.SaveChanges();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }
}
