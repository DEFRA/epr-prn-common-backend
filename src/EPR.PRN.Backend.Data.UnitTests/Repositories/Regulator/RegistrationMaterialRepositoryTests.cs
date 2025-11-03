using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
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
    private RegistrationMaterialRepository _repository;

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
        var pendingMaterialStatus = new LookupRegistrationMaterialStatus { Id = 2, Name = "Pending" };
        var readyToSubmitMaterialStatus = new LookupRegistrationMaterialStatus { Id = 3, Name = "ReadyToSubmit" };
        var lookupMaterial = new LookupMaterial { Id = 1, MaterialCode = "PLSTC", MaterialName = "Plastic" };
        var steelMaterial = new LookupMaterial { Id = 5, MaterialCode = "PLSTC", MaterialName = "Steel" };
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
                            ExternalId = Guid.Parse("4c632765-7652-42ae-8527-23f10a971c28"),
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
        _context.LookupRegistrationMaterialStatuses.Add(readyToSubmitMaterialStatus);
        _context.LookupRegistrationMaterialStatuses.Add(pendingMaterialStatus);
        _context.LookupMaterials.Add(lookupMaterial);
        _context.LookupMaterials.Add(steelMaterial);
        _context.LookupAddresses.Add(address);
        _context.Registrations.Add(registration);

        _context.Registrations.Add(Accreditation_registration);

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
        await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() => _repository.GetRegistrationById(Guid.Parse("cd9dcc80-fcf5-4f46-addd-b8a256f735a3")));
    }

    [TestMethod]
    public async Task GetRegistrationByExternalIdAndYear_ShouldReturnRegistration_WhenRegistrationExists()
    {
        var result = await _repository.GetRegistrationByExternalIdAndYear(Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c5d"), 2025);
        using (new AssertionScope())
        {
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.IsNotNull(result.Materials);
            Assert.IsNotNull(result.AccreditationTasks);
            Assert.AreEqual(1, result.AccreditationTasks.Count);
        }
    }

    [TestMethod]
    public async Task GetRegistrationByExternalIdAndNullYear_ShouldReturnRegistration_WhenRegistrationExists()
    {
        var result = await _repository.GetRegistrationByExternalIdAndYear(Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c5d"), null);
        using (new AssertionScope())
        {
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.IsNotNull(result.Materials);
            Assert.IsNotNull(result.AccreditationTasks);
            Assert.AreEqual(2, result.AccreditationTasks.Count);
        }
    }

    [TestMethod]
    public async Task GetRegistrationByExternalIdAndYear_ShouldThrowKeyNotFoundException_WhenNotFound()
    {
        await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() => _repository.GetRegistrationByExternalIdAndYear(Guid.Parse("cd9dcc80-fcf5-4f46-addd-b8a256f735a3"), 2025));
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
        var userId = Guid.NewGuid();

        await _repository.UpdateRegistrationOutCome(Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"), newStatusId, comment, newReference, userId);
        var updated = await _context.RegistrationMaterials.FindAsync(1);

        using (new AssertionScope())
        {
            Assert.AreEqual(newStatusId, updated.StatusId);
            Assert.AreEqual(comment, updated.Comments);
            Assert.AreEqual(newReference, updated.RegistrationReferenceNumber);
            Assert.AreEqual(userId, updated.StatusUpdatedBy);
            Assert.IsNotNull(updated.StatusUpdatedDate);
        }
    }

    [TestMethod]
    public async Task UpdateRegistrationOutCome_ShouldThrow_WhenMaterialNotFound()
    {
        var userId = Guid.NewGuid();
        _ = await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() =>
            _repository.UpdateRegistrationOutCome(Guid.Parse("cd9dcc80-fcf5-4f46-addd-b8a256f735a3"), 1, "Test", "REF", userId));
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
        await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() => _repository.GetRegistrationMaterialById(Guid.Parse("cd9dcc80-fcf5-4f46-addd-b8a256f735a3")));
    }

    [TestMethod]
    public async Task GetAccreditation_FileUploadById_ShouldReturnMaterial_WhenExists()
    {
        var accreditation = await _repository.GetAccreditation_FileUploadById(Guid.Parse("4c632765-7652-42ae-8527-23f10a971c28"));
        using (new AssertionScope())
        {
            Assert.IsNotNull(accreditation);
            Assert.AreEqual("ACC12345", accreditation.ApplicationReferenceNumber);
        }
    }

    [TestMethod]
    public async Task GetAccreditation_FileUploadById_ShouldThrow_WhenNotFound()
    {
        await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() => _repository.GetAccreditation_FileUploadById(Guid.Parse("cd9dcc80-fcf5-4f46-addd-b8a256f735a3")));
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

        await _context.SaveChangesAsync(CancellationToken.None);

        // Act
        await _repository.RegistrationMaterialsMarkAsDulyMade(registrationMaterialId, statusId, determinationDate, dulyMadeDate, userId);

        // Assert
        var dulyMadeEntry = await _context.DulyMade
            .FirstOrDefaultAsync(x => x.RegistrationMaterial.ExternalId == registrationMaterialId);
        var savedDeterminationDate = await _context.DeterminationDate
            .FirstOrDefaultAsync(x => x.RegistrationMaterialId == 1);
        var taskStatusEntry = await _context.RegulatorApplicationTaskStatus
            .FirstOrDefaultAsync(x => x.RegistrationMaterial.ExternalId == registrationMaterialId && x.TaskStatusId == statusId);

        using (new AssertionScope())
        {
            dulyMadeEntry.Should().NotBeNull();
            dulyMadeEntry!.DulyMadeBy.Should().Be(userId);
            dulyMadeEntry!.DulyMadeDate.Should().Be(dulyMadeDate);
            savedDeterminationDate.DeterminateDate.Should().Be(determinationDate);
            taskStatusEntry.Should().NotBeNull();
            taskStatusEntry!.TaskStatusId.Should().Be(statusId);
            taskStatusEntry.RegulatorTaskId.Should().Be(2);
            taskStatusEntry.StatusCreatedBy.Should().Be(userId);
            taskStatusEntry.StatusCreatedDate.Date.Should().Be(DateTime.UtcNow.Date);
        }
    }
    
    [TestMethod]
    public async Task CreateRegistrationMaterialWithExemptionsAsync_ShouldCreateMaterialAndExemptions()
    {
        // Arrange
        var registration = new Registration
        {
            Id = 3,
            ApplicationTypeId = 1,
            ExternalId = Guid.NewGuid()
        };
        var lookupMaterial = new LookupMaterial { Id = 2, MaterialCode = "GLASS", MaterialName = "Glass" };
        var materialStatus = new LookupRegistrationMaterialStatus { Id = 4, Name = "Pending" };
        var registrationMaterial = new RegistrationMaterial
        {
            Id = 3,
            RegistrationId = 3,
            MaterialId = 2,
            StatusId = 2,
            RegistrationReferenceNumber = "REF67890",
            Comments = "New material",
            Status = materialStatus,
            Material = lookupMaterial,
            IsMaterialRegistered = false,
            MaterialExemptionReferences = new List<MaterialExemptionReference>()
        };
        var exemptionReferences = new List<MaterialExemptionReference>
        {
            new MaterialExemptionReference { ReferenceNo = "EXEMPT456", RegistrationMaterialId = 3 },
            new MaterialExemptionReference { ReferenceNo = "EXEMPT789", RegistrationMaterialId = 3 }
        };

        _context.Registrations.Add(registration);
        _context.LookupMaterials.Add(lookupMaterial);
        _context.LookupRegistrationMaterialStatuses.Add(materialStatus);
        _context.RegistrationMaterials.Add(registrationMaterial);
        await _context.SaveChangesAsync(CancellationToken.None);

        // Act
        await _repository.CreateExemptionReferencesAsync(registrationMaterial.ExternalId, exemptionReferences);

        // Assert
        var createdMaterial = await _context.RegistrationMaterials
            .Include(m => m.MaterialExemptionReferences)
            .FirstOrDefaultAsync(m => m.Id == registrationMaterial.Id);
        using (new AssertionScope())
        {
            createdMaterial.Should().NotBeNull();
            createdMaterial!.RegistrationReferenceNumber.Should().Be("REF67890");
            createdMaterial.MaterialExemptionReferences.Should().NotBeNull();
            createdMaterial.MaterialExemptionReferences!.Count.Should().Be(2);
            createdMaterial.MaterialExemptionReferences!.Select(x => x.ReferenceNo).Should().Contain("EXEMPT456");
            createdMaterial.MaterialExemptionReferences!.Select(x => x.ReferenceNo).Should().Contain("EXEMPT789");
        }
    }
    
    [TestMethod]
    public async Task CreateRegistrationMaterialWithExemptionsAsync_ShouldAllowEmptyExemptions()
    {
        // Arrange
        var registration = new Registration
        {
            Id = 4,
            ApplicationTypeId = 1,
            ExternalId = Guid.NewGuid()
        };
        var lookupMaterial = new LookupMaterial { Id = 4, MaterialCode = "PAPER", MaterialName = "Paper" };
        var materialStatus = new LookupRegistrationMaterialStatus { Id = 5, Name = "Pending" };
        var registrationMaterial = new RegistrationMaterial
        {
            Id = 4,
            RegistrationId = 4,
            MaterialId = 4,
            StatusId = 5,
            RegistrationReferenceNumber = "REF33333",
            Comments = "No exemptions",
            Status = materialStatus,
            Material = lookupMaterial,
            IsMaterialRegistered = false,
            MaterialExemptionReferences = new List<MaterialExemptionReference>()
        };

        _context.Registrations.Add(registration);
        _context.LookupMaterials.Add(lookupMaterial);
        _context.LookupRegistrationMaterialStatuses.Add(materialStatus);
        _context.RegistrationMaterials.Add(registrationMaterial);
        await _context.SaveChangesAsync(CancellationToken.None);

        // Act
        await _repository.CreateExemptionReferencesAsync(registrationMaterial.ExternalId, new List<MaterialExemptionReference>());

        // Assert
        var createdMaterial = await _context.RegistrationMaterials
            .Include(m => m.MaterialExemptionReferences)
            .FirstOrDefaultAsync(m => m.Id == registrationMaterial.Id);
        using (new AssertionScope())
        {
            createdMaterial.Should().NotBeNull();
            createdMaterial!.MaterialExemptionReferences.Should().BeEmpty();
        }
    }

    [TestMethod]
    public async Task RegistrationMaterialsMarkAsDulyMade_ShouldUpdateRegulatorApplicationTaskStatus_WhenItExists()
    {
        // Arrange
        var registrationMaterialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var statusId = 6;
        var dulyMadeDate = DateTime.UtcNow.Date;
        var determinationDate = dulyMadeDate.AddDays(84);
        var userId = Guid.NewGuid();

        _context.RegulatorApplicationTaskStatus.Add(new RegulatorApplicationTaskStatus
        {
            Id = 2,
            RegulatorTaskId = 2,
            TaskStatusId = 1,
            TaskStatus = new LookupTaskStatus { Id = 2, Name = "Started" },
            Task = new LookupRegulatorTask
            {
                Id = 2,
                Name = RegulatorTaskNames.CheckRegistrationStatus,
                ApplicationTypeId = 1,
                JourneyTypeId = 1,
                IsMaterialSpecific = true,
            },
            RegistrationMaterialId = 1
        });

        await _context.SaveChangesAsync(CancellationToken.None);
        // Act
        await _repository.RegistrationMaterialsMarkAsDulyMade(registrationMaterialId, statusId, determinationDate, dulyMadeDate, userId);

        // Assert
        var dulyMadeEntry = await _context.DulyMade
            .FirstOrDefaultAsync(x => x.RegistrationMaterial.ExternalId == registrationMaterialId);
        var savedDeterminationDate = await _context.DeterminationDate
            .FirstOrDefaultAsync(x => x.RegistrationMaterialId == 1);
        var taskStatusEntry = await _context.RegulatorApplicationTaskStatus
            .FirstOrDefaultAsync(x => x.RegistrationMaterial.ExternalId == registrationMaterialId && x.TaskStatusId == statusId);

        using (new AssertionScope())
        {
            dulyMadeEntry.Should().NotBeNull();
            dulyMadeEntry!.DulyMadeBy.Should().Be(userId);
            dulyMadeEntry!.DulyMadeDate.Should().Be(dulyMadeDate);
            savedDeterminationDate.DeterminateDate.Should().Be(determinationDate);
            taskStatusEntry.Should().NotBeNull();
            taskStatusEntry!.TaskStatusId.Should().Be(statusId);
            taskStatusEntry.RegulatorTaskId.Should().Be(2);
            taskStatusEntry.StatusUpdatedBy.Should().Be(userId);
        }
    }


    [TestMethod]
    public async Task RegistrationMaterialsMarkAsDulyMade_ShouldThrow_WhenMaterialNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var statusId = 3;
        var dulyMadeDate = DateTime.UtcNow.Date;
        var determinationDate = dulyMadeDate.AddDays(84);
        var userId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() =>
            _repository.RegistrationMaterialsMarkAsDulyMade(nonExistentId, statusId, determinationDate, dulyMadeDate, userId));
    }

    [TestMethod]
    public async Task RegistrationMaterialsMarkAsDulyMade_ShouldThrow_WhenRegistrationNotFound()
    {
        // Arrange: Material exists but its Registration does not  
        var materialId = Guid.NewGuid();
        var newMaterial = new RegistrationMaterial
        {
            ExternalId = materialId,
            RegistrationId = 9999 // No such Registration seeded  
        };
        _context.RegistrationMaterials.Add(newMaterial);
        await _context.SaveChangesAsync(CancellationToken.None);

        // Act & Assert  
        await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() =>
            _repository.RegistrationMaterialsMarkAsDulyMade(materialId, 3, DateTime.UtcNow.AddDays(84), DateTime.UtcNow, Guid.NewGuid()));
    }
  
    [TestMethod]
    public async Task RegistrationMaterialsMarkAsDulyMade_ShouldUpdateDeterminationDate_WhenItAlreadyExists()
    {
        // Arrange
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var userId = Guid.NewGuid();
        var dulyMadeDate = DateTime.UtcNow.Date;
        var determinationDate = dulyMadeDate.AddDays(84);

        _context.DeterminationDate.Add(new DeterminationDate
        {
            Id = 1,
            ExternalId = Guid.NewGuid(),
            RegistrationMaterialId = 1,
            DeterminateDate = DateTime.UtcNow.AddDays(-84)
        });

        _context.LookupTasks.Add(new LookupRegulatorTask
        {
            Id = 2,
            Name = RegulatorTaskNames.CheckRegistrationStatus,
            ApplicationTypeId = 1,
            JourneyTypeId = 1,
            IsMaterialSpecific = true
        });

        await _context.SaveChangesAsync(CancellationToken.None);

        // Act
        await _repository.RegistrationMaterialsMarkAsDulyMade(materialId, 3, determinationDate, dulyMadeDate, userId);

        // Assert
        using (new AssertionScope())
        {

            var updatedDeterminationDate = await _context.DeterminationDate.FirstOrDefaultAsync(x => x.RegistrationMaterialId == 1);
            updatedDeterminationDate!.DeterminateDate.Should().Be(determinationDate);
        }
    }

    [TestMethod]
    public async Task UpdateRegistrationOutCome_ShouldHandleNullCommentAndReference()
    {
        // Arrange
        var id = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");

        // Act
        await _repository.UpdateRegistrationOutCome(id, 2, null, null,Guid.Empty);
        var updated = await _context.RegistrationMaterials.FindAsync(1);

        // Assert
        using (new AssertionScope())
        {
            updated.Should().NotBeNull();
            updated!.StatusId.Should().Be(2);
            updated.Comments.Should().BeNull();
            updated.RegistrationReferenceNumber.Should().BeNull();
        }
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
        await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() => _repository.GetRegistrationMaterial_FileUploadById(Guid.Parse("cd9dcc80-fcf5-4f46-addd-b8a256f735a3")));
    }

    [TestMethod]
    public async Task CreateAsync_NoExistingRegistration_ShouldThrowException()
    {
        await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() => _repository.CreateAsync(Guid.NewGuid(), "Steel"));
    }

    [TestMethod]
    public async Task CreateAsync_ExistingRegistrationMaterial_ShouldThrow()
    {
        // Act
        var result = await _repository.CreateAsync(Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d"),"Plastic");

        // Assert
        result.Registration.ExternalId.Should().Be("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");
    }

    [TestMethod]
    public async Task CreateAsync_ExistingRegistration_ShouldCreate()
    {
        // Act
        var result = await _repository.CreateAsync(Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d"), "Steel");

        // Assert
        var loaded = await _context.RegistrationMaterials.FindAsync(result.Id);
        loaded.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetRegistrationMaterialsByRegistrationId_RegistrationIdDoesNotExists()
    { 
        await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() => _repository.GetRegistrationMaterialsByRegistrationId(Guid.NewGuid()));
    }

    [TestMethod]
    public async Task GetRegistrationMaterialsByRegistrationId_RegistrationIdDoesExist_NoRegistrationMaterials_ReturnEmpty()
    {
        // Arrange
        var id = Guid.NewGuid();
        var registration = new Registration
        {
            Id = 10,
            ExternalId = id
        };

        await _context.Registrations.AddAsync(registration);
        await _context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await _repository.GetRegistrationMaterialsByRegistrationId(id);

        // Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetRegistrationMaterialsByRegistrationId_RegistrationIdDoesExist_RegistrationMaterialsExist_ReturnItems()
    {
        // Arrange
        var id = Guid.NewGuid();
        var registration = new Registration
        {
            Id = 10,
            ExternalId = id
        };

        var registrationMaterial = new RegistrationMaterial
        {
            Registration = registration,
            RegistrationId = registration.Id,
            Id = 55,
            MaterialId = 1,
            PermitTypeId = 1,
            StatusId = 1,
            InstallationPeriodId = 1,
            PPCPeriodId = 1,
            EnvironmentalPermitWasteManagementPeriodId = 1,
            WasteManagementPeriodId = 1,
            MaximumReprocessingPeriodId = 1
        };

        await _context.Registrations.AddAsync(registration);
        await _context.RegistrationMaterials.AddAsync(registrationMaterial);
        await _context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await _repository.GetRegistrationMaterialsByRegistrationId(id);

        // Assert
        result.Should().HaveCount(1);
    }

    [TestMethod]
    public async Task UpdateRegistrationMaterialPermits_WasteExemptions_ShouldUpdate_WhenExists()
    {
        // Arrange
        var registrationMaterialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var exemptions = new List<MaterialExemptionReference>
        {
            new()
            {
                ReferenceNo = "1234",
                RegistrationMaterialId = 1
            },
            new()
            {
                ReferenceNo = "5678",
                RegistrationMaterialId = 1
            }
        };

        var expected = new List<MaterialExemptionReference>();
        expected.AddRange(exemptions);
        expected.Add(new MaterialExemptionReference
        {
            Id = 1,
            ReferenceNo = "EXEMPT123",
            RegistrationMaterialId = 1
        });

        _context.MaterialExemptionReferences.AddRange(exemptions);
        await _context.SaveChangesAsync(CancellationToken.None);

        // Act
        await _repository.UpdateRegistrationMaterialPermits(registrationMaterialId, (int)MaterialPermitType.WasteExemption, null);
        var registrationMaterial = await _context.RegistrationMaterials.FirstOrDefaultAsync(x => x.ExternalId == registrationMaterialId);

        // Assert
        registrationMaterial.PermitTypeId.Should().Be((int)MaterialPermitType.WasteExemption);
        registrationMaterial.EnvironmentalPermitWasteManagementNumber.Should().BeNull();
        registrationMaterial.WasteManagementLicenceNumber.Should().BeNull();
        registrationMaterial.InstallationPermitNumber.Should().BeNull();
        registrationMaterial.PPCPermitNumber.Should().BeNull();

        var loaded = _context.MaterialExemptionReferences.Where(o => o.RegistrationMaterialId == 1).ToList();
        loaded.Should().BeEquivalentTo(expected, o => o.Excluding(x => x.RegistrationMaterial));
    }

    [TestMethod]
    [DataRow(2, "PPC-123")]
    [DataRow(3, "WML-123")]
    [DataRow(4, "IP-123")]
    [DataRow(5, "EP-123")]
    public async Task UpdateRegistrationMaterialPermits_ShouldUpdate_WhenExists(int permitTypeId, string permitNumber)
    {
        // Arrange
        var registrationMaterialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var exemptions = new List<MaterialExemptionReference>
        {
            new()
            {
                ReferenceNo = "1234",
                RegistrationMaterialId = 1
            },
            new()
            {
                ReferenceNo = "5678",
                RegistrationMaterialId = 1
            }
        };

        _context.MaterialExemptionReferences.AddRange(exemptions);
        await _context.SaveChangesAsync(CancellationToken.None);

        // Act
        await _repository.UpdateRegistrationMaterialPermits(registrationMaterialId, permitTypeId, permitNumber);
        var registrationMaterial = await _context.RegistrationMaterials.FirstOrDefaultAsync(x => x.ExternalId == registrationMaterialId);

        // Assert
        registrationMaterial.PermitTypeId.Should().Be(permitTypeId);

        switch ((MaterialPermitType)permitTypeId)
        {
            case MaterialPermitType.PollutionPreventionAndControlPermit:
                registrationMaterial.PPCPermitNumber.Should().Be(permitNumber);
                registrationMaterial.WasteManagementLicenceNumber.Should().BeNull();
                registrationMaterial.InstallationPermitNumber.Should().BeNull();
                registrationMaterial.EnvironmentalPermitWasteManagementNumber.Should().BeNull();
                break;
            case MaterialPermitType.WasteManagementLicence:
                registrationMaterial.WasteManagementLicenceNumber.Should().Be(permitNumber);
                registrationMaterial.PPCPermitNumber.Should().BeNull();
                registrationMaterial.InstallationPermitNumber.Should().BeNull();
                registrationMaterial.EnvironmentalPermitWasteManagementNumber.Should().BeNull();
                break;
            case MaterialPermitType.InstallationPermit:
                registrationMaterial.InstallationPermitNumber.Should().Be(permitNumber);
                registrationMaterial.WasteManagementLicenceNumber.Should().BeNull();
                registrationMaterial.PPCPermitNumber.Should().BeNull();
                registrationMaterial.EnvironmentalPermitWasteManagementNumber.Should().BeNull();
                break;
            case MaterialPermitType.EnvironmentalPermitOrWasteManagementLicence:
                registrationMaterial.EnvironmentalPermitWasteManagementNumber.Should().Be(permitNumber);
                registrationMaterial.WasteManagementLicenceNumber.Should().BeNull();
                registrationMaterial.InstallationPermitNumber.Should().BeNull();
                registrationMaterial.PPCPermitNumber.Should().BeNull();
                break;
        }

        var loaded = await _context.MaterialExemptionReferences.Where(o => o.RegistrationMaterialId == 1).ToListAsync(CancellationToken.None);
        loaded.Should().BeEmpty();
    }

    [TestMethod]
    [DataRow(MaterialPermitType.PollutionPreventionAndControlPermit, 2000, 2)]
    [DataRow(MaterialPermitType.WasteManagementLicence, 3000, 3)]
    [DataRow(MaterialPermitType.InstallationPermit, 4000, 4)]
    [DataRow(MaterialPermitType.EnvironmentalPermitOrWasteManagementLicence, 5000, 5)]
    public async Task UpdateRegistrationMaterialPermitCapacity_ShouldUpdate_WhenExists(MaterialPermitType permitTypeId, double capacityInTonnes, int? periodId)
    {
        // Arrange
        var capacityInTonnesAsDecimal = (decimal)capacityInTonnes;
        var registrationMaterialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");

        // Act
        await _repository.UpdateRegistrationMaterialPermitCapacity(registrationMaterialId, (int)permitTypeId, capacityInTonnesAsDecimal, periodId);
        var registrationMaterial = await _context.RegistrationMaterials.FirstOrDefaultAsync(x => x.ExternalId == registrationMaterialId);

        // Assert
        switch (permitTypeId)
        {
            case MaterialPermitType.PollutionPreventionAndControlPermit:
                registrationMaterial.PPCReprocessingCapacityTonne.Should().Be(capacityInTonnesAsDecimal);
                registrationMaterial.PPCPeriodId.Should().Be(periodId);
                registrationMaterial.WasteManagementReprocessingCapacityTonne.Should().BeNull();
                registrationMaterial.WasteManagementPeriodId.Should().BeNull();
                registrationMaterial.InstallationReprocessingTonne.Should().BeNull();
                registrationMaterial.InstallationPeriodId.Should().BeNull();
                registrationMaterial.EnvironmentalPermitWasteManagementTonne.Should().BeNull();
                registrationMaterial.EnvironmentalPermitWasteManagementPeriodId.Should().BeNull();
                break;
            case MaterialPermitType.WasteManagementLicence:
                registrationMaterial.WasteManagementReprocessingCapacityTonne.Should().Be(capacityInTonnesAsDecimal);
                registrationMaterial.WasteManagementPeriodId.Should().Be(periodId);
                registrationMaterial.PPCReprocessingCapacityTonne.Should().BeNull();
                registrationMaterial.PPCPeriodId.Should().BeNull();
                registrationMaterial.InstallationReprocessingTonne.Should().BeNull();
                registrationMaterial.InstallationPeriodId.Should().BeNull();
                registrationMaterial.EnvironmentalPermitWasteManagementTonne.Should().BeNull();
                registrationMaterial.EnvironmentalPermitWasteManagementPeriodId.Should().BeNull();
                break;
            case MaterialPermitType.InstallationPermit:
                registrationMaterial.InstallationReprocessingTonne.Should().Be(capacityInTonnesAsDecimal);
                registrationMaterial.InstallationPeriodId.Should().Be(periodId);
                registrationMaterial.WasteManagementReprocessingCapacityTonne.Should().BeNull();
                registrationMaterial.WasteManagementPeriodId.Should().BeNull();
                registrationMaterial.PPCReprocessingCapacityTonne.Should().BeNull();
                registrationMaterial.PPCPeriodId.Should().BeNull();
                registrationMaterial.EnvironmentalPermitWasteManagementTonne.Should().BeNull();
                registrationMaterial.EnvironmentalPermitWasteManagementPeriodId.Should().BeNull();
                break;
            case MaterialPermitType.EnvironmentalPermitOrWasteManagementLicence:
                registrationMaterial.EnvironmentalPermitWasteManagementTonne.Should().Be(capacityInTonnesAsDecimal);
                registrationMaterial.EnvironmentalPermitWasteManagementPeriodId.Should().Be(periodId);
                registrationMaterial.WasteManagementReprocessingCapacityTonne.Should().BeNull();
                registrationMaterial.WasteManagementPeriodId.Should().BeNull();
                registrationMaterial.InstallationReprocessingTonne.Should().BeNull();
                registrationMaterial.InstallationPeriodId.Should().BeNull();
                registrationMaterial.PPCReprocessingCapacityTonne.Should().BeNull();
                registrationMaterial.PPCPeriodId.Should().BeNull();
                break;
        }
    }

    [TestMethod]
    public async Task UpdateRegistrationMaterialPermits_ShouldThrow_WhenNotFound()
    {
        var registrationMaterialId = Guid.NewGuid();

        await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() => _repository.UpdateRegistrationMaterialPermits(registrationMaterialId, 2, "TestPermit"));
    }

    [TestMethod]
    public async Task GetMaterialPermitTypes_ShouldReturnExpectedValues()
    {
        // Arrange
        var expectedMaterialPermitTypes = new List<LookupMaterialPermit> {
            new() { Id = 1, Name = PermitTypes.WasteManagementLicence }
        };

        // Act
        var result = await _repository.GetMaterialPermitTypes();

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedMaterialPermitTypes);
        }
    }

    [TestMethod]
    public async Task DeleteRegistrationMaterial_NotFound_ThrowException()
    {
        // Act & Assert
        await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() => _repository.DeleteAsync(Guid.NewGuid()));
    }

    [TestMethod]
    public async Task DeleteRegistrationMaterial_Found_ShouldDelete()
    {
        // Arrange
        var id = Guid.NewGuid();
        var registrationMaterialExternalId = Guid.NewGuid();
        var registrationMaterialExternalId2 = Guid.NewGuid();
        var registration = new Registration
        {
            Id = 10,
            ExternalId = id
        };

        var registrationMaterial = new RegistrationMaterial
        {
            ExternalId = registrationMaterialExternalId,
            Registration = registration,
            RegistrationId = registration.Id,
            Id = 55,
            MaterialId = 1,
            PermitTypeId = 1,
            StatusId = 1,
            InstallationPeriodId = 1,
            PPCPeriodId = 1,
            EnvironmentalPermitWasteManagementPeriodId = 1,
            WasteManagementPeriodId = 1
        };

        // Negative data.
        var registrationMaterial2 = new RegistrationMaterial
        {
            ExternalId = registrationMaterialExternalId2,
            Registration = registration,
            RegistrationId = registration.Id,
            Id = 65,
            MaterialId = 1,
            PermitTypeId = 1,
            StatusId = 1,
            InstallationPeriodId = 1,
            PPCPeriodId = 1,
            EnvironmentalPermitWasteManagementPeriodId = 1,
            WasteManagementPeriodId = 1
        };

        await _context.Registrations.AddAsync(registration);
        await _context.RegistrationMaterials.AddAsync(registrationMaterial);
        await _context.RegistrationMaterials.AddAsync(registrationMaterial2);
        await _context.SaveChangesAsync(CancellationToken.None);

        // Act
        await _repository.DeleteAsync(registrationMaterialExternalId);

        // Assert
        var loaded = _context.RegistrationMaterials.Where(o => o.RegistrationId == 10).ToList();
        loaded.Should().HaveCount(1);
        loaded.First().ExternalId.Should().Be(registrationMaterialExternalId2);
    }

	[TestMethod]
	public async Task UpdateIsMaterialRegisteredAsync_ShouldUpdateMaterialStatus()
	{
		// Arrange
		var materialId = Guid.NewGuid();
		var existingMaterial = new RegistrationMaterial
		{
			Id = 20,
			ExternalId = materialId,
			IsMaterialRegistered = false,
			StatusId = (int)RegistrationMaterialStatus.Started
		};

		await _context.RegistrationMaterials.AddAsync(existingMaterial);
		await _context.SaveChangesAsync(CancellationToken.None);

		var dto = new UpdateIsMaterialRegisteredDto
		{
			RegistrationMaterialId = materialId,
			IsMaterialRegistered = true
		};

		// Act
		await _repository.UpdateIsMaterialRegisteredAsync(new List<UpdateIsMaterialRegisteredDto> { dto });

		// Assert
		var updatedMaterial = await _context.RegistrationMaterials.SingleAsync(m => m.ExternalId == materialId);
		updatedMaterial.IsMaterialRegistered.Should().BeTrue();
		updatedMaterial.StatusId.Should().Be((int)RegistrationMaterialStatus.InProgress);
	}

	[TestMethod]
	public async Task UpdateIsMaterialRegisteredAsync_ShouldThrow_WhenNotFound()
	{
		var dto = new UpdateIsMaterialRegisteredDto
		{
			RegistrationMaterialId = Guid.NewGuid(),
			IsMaterialRegistered = true
		};

		await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() => _repository.UpdateIsMaterialRegisteredAsync(new List<UpdateIsMaterialRegisteredDto> { dto }));
	}

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }
}