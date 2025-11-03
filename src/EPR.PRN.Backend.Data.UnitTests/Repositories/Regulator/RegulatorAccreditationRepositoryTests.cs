using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using EPR.PRN.Backend.Data.Repositories.Regulator;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories.Regulator;

[TestClass]
public class RegulatorAccreditationRepositoryTests
{
    private EprContext _context;
    private RegulatorAccreditationRepository _repository;

    [TestInitialize]
    public void TestInitialize()
    {
        var options = new DbContextOptionsBuilder<EprContext>()
                        .UseInMemoryDatabase(databaseName: "TestDb")
                        .Options;

        _context = new EprContext(options);
        _repository = new RegulatorAccreditationRepository(_context);

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
        await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() => _repository.GetAccreditationPaymentFeesById(Guid.Parse("cd9dcc80-fcf5-4f46-addd-b8a256f735a3")));
    }


    [TestMethod]
    public async Task AccreditationMarkAsDulyMade_Throws_WhenAccreditationNotFound()
    {
        // Act & Assert
        await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() =>
            _repository.AccreditationMarkAsDulyMade(Guid.NewGuid(), 1, DateTime.UtcNow, DateTime.UtcNow, Guid.NewGuid()));
    }


    [TestMethod]
    public async Task AccreditationMarkAsDulyMade_ShouldSetDulyMadeCorrectly()
    {
        // Arrange
        var accreditationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");
        var registrationMaterialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var statusId = 5;
        var dulyMadeDate = DateTime.UtcNow.Date;
        var determinationDate = dulyMadeDate.AddDays(84);
        var userId = Guid.NewGuid();

        _context.LookupTasks.Add(new LookupRegulatorTask
        {
            Id = 27,
            Name = "DulyMade",
            ApplicationTypeId = 1,
            JourneyTypeId = 1,
            IsMaterialSpecific = true
        });

        await _context.SaveChangesAsync(CancellationToken.None);

        // Act
        await _repository.AccreditationMarkAsDulyMade(accreditationId, statusId, dulyMadeDate, determinationDate, userId);

        // Assert
        var accreditation = await _context.Accreditations.FirstAsync(x => x.ExternalId == accreditationId);
        var dulyMadeEntry = await _context.AccreditationDulyMade
            .FirstOrDefaultAsync(x => x.AccreditationId == accreditation.Id);
        var determinationEntry = await _context.AccreditationDeterminationDate
            .FirstOrDefaultAsync(x => x.AccreditationId == accreditation.Id);
        var taskStatusEntry = await _context.RegulatorAccreditationTaskStatus
            .FirstOrDefaultAsync(x => x.AccreditationId == accreditation.Id && x.TaskStatusId == statusId);

        using (new AssertionScope())
        {
            dulyMadeEntry.Should().NotBeNull();
            dulyMadeEntry!.DulyMadeBy.Should().Be(userId);
            dulyMadeEntry.DulyMadeDate.Should().Be(dulyMadeDate);

            determinationEntry.Should().NotBeNull();
            determinationEntry!.DeterminationDate.Should().Be(determinationDate);

            taskStatusEntry.Should().NotBeNull();
            taskStatusEntry!.TaskStatusId.Should().Be(statusId);
            taskStatusEntry.StatusUpdatedBy.Should().Be(userId);
            taskStatusEntry.StatusCreatedDate.Date.Should().Be(DateTime.UtcNow.Date);
        }
    }

    [TestMethod]
    public async Task AccreditationMarkAsDulyMade_CreatesAccreditationDeterminationDateRecord()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var registrationMaterialId = Guid.NewGuid();
        var statusId = 2;
        var dulyMadeDate = DateTime.UtcNow;
        var determinationDate = DateTime.UtcNow.AddDays(10);
        var userId = Guid.NewGuid();

        var registration = new Registration
        {
            Id = 10,
            ApplicationTypeId = 1
        };

        var material = new RegistrationMaterial
        {
            Id = 10,
            ExternalId = registrationMaterialId,
            RegistrationId = registration.Id,
            Registration = registration
        };

        var accreditation = new Accreditation
        {
            Id = 10,
            ExternalId = accreditationId,
            RegistrationMaterialId = material.Id,
            RegistrationMaterial = material,
            ApplicationReferenceNumber = "ACC78910"
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
        await _context.SaveChangesAsync(CancellationToken.None);

        // Act
        await _repository.AccreditationMarkAsDulyMade(accreditationId, statusId, dulyMadeDate, determinationDate, userId);

        // Assert
        var determination = await _context.AccreditationDeterminationDate.FirstOrDefaultAsync(x => x.AccreditationId == accreditation.Id);

        using (new AssertionScope())
        {
            determination.Should().NotBeNull();
            determination!.AccreditationId.Should().Be(accreditation.Id);
            determination.DeterminationDate.Date.Should().Be(determinationDate.Date);
        }
    }

    [TestMethod]
    public async Task AccreditationMarkAsDulyMade_UpdatesAccreditationDeterminationDateRecord()
    {
        // Arrange
        var accreditationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");
        var newDeterminationDate = DateTime.UtcNow.AddDays(30);
        var statusId = 5;
        var dulyMadeDate = DateTime.UtcNow.Date;
        var userId = Guid.NewGuid();

        // Insert an initial determination date to simulate existing record
        var accreditation = await _context.Accreditations
            .Include(x => x.RegistrationMaterial)
            .FirstAsync(x => x.ExternalId == accreditationId);

        var existingDetermination = new AccreditationDeterminationDate
        {
            AccreditationId = accreditation.Id,
            DeterminationDate = DateTime.UtcNow.AddDays(-10),
            ExternalId = Guid.NewGuid()
        };

        _context.AccreditationDeterminationDate.Add(existingDetermination);

        _context.LookupTasks.Add(new LookupRegulatorTask
        {
            Id = 27,
            Name = "DulyMade",
            ApplicationTypeId = 1,
            JourneyTypeId = 1,
            IsMaterialSpecific = true
        });

        await _context.SaveChangesAsync(CancellationToken.None);

        // Act
        await _repository.AccreditationMarkAsDulyMade(accreditationId, statusId, dulyMadeDate, newDeterminationDate, userId);

        // Assert
        var updatedDetermination = await _context.AccreditationDeterminationDate
            .FirstOrDefaultAsync(x => x.AccreditationId == accreditation.Id);

        using (new AssertionScope())
        {
            updatedDetermination.Should().NotBeNull();
            updatedDetermination!.DeterminationDate.Date.Should().Be(newDeterminationDate.Date);
        }
    }

    [TestMethod]
    public async Task AccreditationMarkAsDulyMade_DoesNotDuplicateAccreditationDeterminationDateRecord()
    {
        // Arrange
        var accreditationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");
        var newDeterminationDate = DateTime.UtcNow.AddDays(45);
        var statusId = 5;
        var dulyMadeDate = DateTime.UtcNow.Date;
        var userId = Guid.NewGuid();

        var accreditation = await _context.Accreditations
            .FirstAsync(x => x.ExternalId == accreditationId);

        var existingDetermination = new AccreditationDeterminationDate
        {
            AccreditationId = accreditation.Id,
            DeterminationDate = DateTime.UtcNow.AddDays(-20),
            ExternalId = Guid.NewGuid()
        };

        _context.AccreditationDeterminationDate.Add(existingDetermination);

        _context.LookupTasks.Add(new LookupRegulatorTask
        {
            Id = 28,
            Name = "DulyMade",
            ApplicationTypeId = 1,
            JourneyTypeId = 1,
            IsMaterialSpecific = true
        });

        await _context.SaveChangesAsync(CancellationToken.None);

        // Act
        await _repository.AccreditationMarkAsDulyMade(accreditationId, statusId, dulyMadeDate, newDeterminationDate, userId);

        // Assert
        var determinationRecords = await _context.AccreditationDeterminationDate
            .Where(x => x.AccreditationId == accreditation.Id)
            .ToListAsync(CancellationToken.None);

        using (new AssertionScope())
        {
            determinationRecords.Count.Should().Be(1);
            determinationRecords[0].DeterminationDate.Date.Should().Be(newDeterminationDate.Date);
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

    [TestMethod]
    public async Task GetAccreditationById_ReturnsEntity_WhenItExists()
    {
        // Arrange
        var id = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d"); // seeded in SeedDatabase()

        // Act
        var result = await _repository.GetAccreditationById(id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(id, result.ExternalId);
    }

    [TestMethod]
    public async Task GetAccreditationById_ThrowsKeyNotFoundException_WhenNotFound()
    {
        // Arrange
        var missingId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() =>
            _repository.GetAccreditationById(missingId));
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }
}
