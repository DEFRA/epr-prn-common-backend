using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Testing.Platform.Extensions;
using Moq;
using Moq.EntityFrameworkCore;
using System.Xml.XPath;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories;

[TestClass]
public class OverseasAddressTests
{
    private MaterialRepository _materialRepository;
    private EprContext _context;
    private Mock<ILogger<MaterialRepository>> _mockLogger;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EprContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        _context = new EprContext(options);
        _mockLogger = new Mock<ILogger<MaterialRepository>>();
        _materialRepository = new MaterialRepository(_context, _mockLogger.Object);

        SeedDatabase();

        var dbContextOptions = new DbContextOptionsBuilder<EprContext>().Options;

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
        var taskStatusCompleted = new LookupTaskStatus { Id = 2, Name = "Completed" };
        var task = new LookupRegulatorTask
        {
            Id = 1,
            Name = ApplicantRegistrationTaskNames.SiteAddressAndContactDetails,
            IsMaterialSpecific = false,
            ApplicationTypeId = 1,
            JourneyTypeId = 1
        };
        var overseasTask = new LookupApplicantRegistrationTask
        {
            Id = 2,
            Name = ApplicantRegistrationTaskNames.OverseasReprocessorSiteDetails,
            IsMaterialSpecific = true,
            ApplicationTypeId = 2,
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
            ApplicationTypeId = 2,
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
        _context.LookupApplicantRegistrationTasks.Add(overseasTask);
        _context.LookupTasks.Add(task);
        _context.LookupTaskStatuses.AddRange(taskStatus, taskStatusCompleted);
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
    public async Task SaveOverseasReprocessingSites_Should_Update_Existing_And_Add_New()
    {
        //Arrange
        var registrationExternalId = new Guid("6021d372-ab62-4e87-92e8-7544908453e6");
        var registrationMaterialId = 5;
        var registrationId = 1;

        _context.RegistrationMaterials.Add(new RegistrationMaterial
        {
            Id = registrationMaterialId,
            ExternalId = registrationExternalId,
            RegistrationId = registrationId,
            MaterialId = 1,
            StatusId = 1,
            IsMaterialRegistered = false,
        });
        await _context.SaveChangesAsync();

        _context.LookupCountries.Add(new LookupCountry { Id = 1, Name = "CountryA" });
        await _context.SaveChangesAsync();

        var existingExternalId = new Guid("6497f0b0-d55d-4462-a6e2-f32733bec6ea");

        var userId = new Guid("ff027f06-04a1-4392-bb37-bc16cb3e7d9f");
        var wasteCodeId = new Guid("5cf7d749-cdda-4bdd-8b6e-38e70259e560");

        var existingAddress = new OverseasAddress
        {
            ExternalId = existingExternalId,
            RegistrationId = registrationId,
            AddressLine1 = "Old Line 1",
            OrganisationName = "Old Org",
            AddressLine2 = "Old Address Line 2",
            CityOrTown = "Old Town",
            StateProvince = "Old State",
            PostCode = "12345",
            SiteCoordinates = "51.5074, -0.1278",
            OverseasAddressContacts = new List<OverseasAddressContact>
            {
                new OverseasAddressContact { CreatedBy = userId, FullName = "Old Contact", Email = "old@email.com", PhoneNumber = "111" }
            },
            OverseasAddressWasteCodes = new List<OverseasAddressWasteCode>
            {
                new OverseasAddressWasteCode { ExternalId = wasteCodeId, CodeName = "OldCode" }
            }
        };
        _context.OverseasAddress.Add(existingAddress);
        await _context.SaveChangesAsync();

        var updateDto = new UpdateOverseasAddressDto
        {
            RegistrationMaterialId = registrationExternalId,
            OverseasAddresses = new List<OverseasAddressDto>
            {
                new OverseasAddressDto
                {
                    ExternalId = existingExternalId,
                    AddressLine1 = "New Line 1",
                    OrganisationName = "New Org",
                    OverseasAddressContacts = new List<OverseasAddressContactDto>
                    {
                        new OverseasAddressContactDto { CreatedBy = userId, FullName = "New Contact", Email = "new@email.com", PhoneNumber = "222" }
                    },
                    OverseasAddressWasteCodes = new List<OverseasAddressWasteCodeDto>
                    {
                        new OverseasAddressWasteCodeDto { ExternalId = wasteCodeId, CodeName = "NewCode" }
                    },
                    CountryName = "CountryA"
                },
                new OverseasAddressDto
                {
                    ExternalId = Guid.NewGuid(),
                    OrganisationName = "New Org2",
                    AddressLine1 = "New Address Line 2",
                    OverseasAddressContacts = new List<OverseasAddressContactDto>()
                    {
                        new() { CreatedBy = userId,
                            FullName = "New Contact2",
                            Email = "test",
                            PhoneNumber = "04343"
                        }
                        },

                    OverseasAddressWasteCodes = new List<OverseasAddressWasteCodeDto>()
                    {
                        new() { ExternalId = Guid.NewGuid(), CodeName = "NewCode2" }
                    },
                    CountryName = "CountryA",
                    AddressLine2 = "New Address Line 3",
                    CityOrTown = "New Town",
                    StateProvince = "New State",
                    PostCode = "54321",
                    SiteCoordinates = "52.5074, -0.1278",
                }
            }
        };

        // Act
        await _materialRepository.SaveOverseasReprocessingSites(updateDto);

        // Assert
        var allAddresses = await _context.OverseasAddress
            .Include(a => a.OverseasAddressContacts)
            .Include(a => a.OverseasAddressWasteCodes)
            .ToListAsync();
        var allTasks = await _context.RegistrationTaskStatus
            .ToListAsync();
        // Old address updated
        var updated = allAddresses.First(a => a.ExternalId == existingExternalId);
        updated.AddressLine1.Should().Be("New Line 1");
        updated.OverseasAddressContacts.Should().ContainSingle(c => c.CreatedBy == userId && c.FullName == "New Contact" && c.Email == "new@email.com");
        updated.OverseasAddressWasteCodes.Should().ContainSingle(wc => wc.CodeName == "NewCode");

        // New address added
        allAddresses.Should().Contain(a => a.OrganisationName == "New Org" && a.AddressLine1 == "New Line 1");

        // No addresses deleted incorrectly
        allAddresses.Count.Should().Be(2);

        // Check completed task is created
        allTasks.Count.Should().Be(1);
        allTasks.FirstOrDefault()!.RegistrationMaterialId.Should().Be(registrationMaterialId);
        allTasks.FirstOrDefault()!.TaskStatus.Name.Should().Be(TaskStatuses.Completed.ToString());
        allTasks.FirstOrDefault()!.Task.Name.Should().Be(ApplicantRegistrationTaskNames.OverseasReprocessorSiteDetails);
    }

    [TestMethod]
    public async Task SaveOverseasReprocessingSites_Should_Delete_Removed_Addresses()
    {
        // Arrange
        var externalId = new Guid("6021d372-ab62-4e87-92e8-7544908453e6");
        var registrationMaterialId = 202;
        var registrationMaterialExternalId = new Guid("f32b8fd6-9c20-461b-9788-003e82490d8f");
        var registrationId = 1;
        _context.RegistrationMaterials.Add(new RegistrationMaterial
        {
            Id = registrationMaterialId,
            ExternalId = registrationMaterialExternalId,
            RegistrationId = registrationId,
            MaterialId = 1,
            StatusId = 1,
            IsMaterialRegistered = false
        });
        await _context.SaveChangesAsync();

        var existingToKeep = new OverseasAddress
        {
            ExternalId = externalId,
            RegistrationId = registrationId,
            AddressLine1 = "Keep This",
            OrganisationName = "Old Org",
            AddressLine2 = "Old Address Line 2",
            CityOrTown = "Old Town",
            StateProvince = "Old State",
            PostCode = "12345",
            SiteCoordinates = "51.5074, -0.1278",
        };
        var existingToDelete = new OverseasAddress
        {
            ExternalId = Guid.NewGuid(),
            RegistrationId = registrationId,
            AddressLine1 = "Delete This",
            OrganisationName = "Old Org",
            AddressLine2 = "Old Address Line 2",
            CityOrTown = "Old Town",
            StateProvince = "Old State",
            PostCode = "12345",
            SiteCoordinates = "51.5074, -0.1278",
        };
        var existingToKeepDifferentRegistrationId = new OverseasAddress
        {
            ExternalId = externalId,
            RegistrationId = 10,
            AddressLine1 = "Keep This",
            OrganisationName = "Old Org",
            AddressLine2 = "Old Address Line 2",
            CityOrTown = "Old Town",
            StateProvince = "Old State",
            PostCode = "12345",
            SiteCoordinates = "51.5074, -0.1278",

        };

        _context.OverseasAddress.AddRange(existingToKeep, existingToDelete, existingToKeepDifferentRegistrationId);
        await _context.SaveChangesAsync();

        var updateDto = new UpdateOverseasAddressDto
        {
            RegistrationMaterialId = registrationMaterialExternalId,
            OverseasAddresses =
            [
                new OverseasAddressDto
                {
                    ExternalId = externalId,
                    AddressLine1 = "Keep This",
                    OrganisationName = "Old Org",
                    AddressLine2 = "Old Address Line 2",
                    CityOrTown = "Old Town",
                    StateProvince = "Old State",
                    PostCode = "12345",
                    SiteCoordinates = "51.5074, -0.1278",
                    OverseasAddressContacts = new List<OverseasAddressContactDto>(),
                    OverseasAddressWasteCodes = new List<OverseasAddressWasteCodeDto>(),
                }
            ]
        };

        // Act
        await _materialRepository.SaveOverseasReprocessingSites(updateDto);

        // Assert
        var allAddresses = await _context.OverseasAddress.ToListAsync();
        allAddresses.Should().Contain(a => a.ExternalId == existingToKeep.ExternalId);
        allAddresses.Should().NotContain(a => a.ExternalId == existingToDelete.ExternalId);
    }

    [TestMethod]
    public async Task SaveOverseasReprocessingSites_ShouldThrow_WhenRegistrationMaterialNotFound()
    {
        // Arrange
        var registrationMaterialExternalId = Guid.NewGuid();
        var updateDto = new UpdateOverseasAddressDto
        {
            RegistrationMaterialId = registrationMaterialExternalId,
            OverseasAddresses =
           [
               new OverseasAddressDto
                {
                    ExternalId = Guid.NewGuid(),
                    AddressLine1 = "Keep This",
                    OrganisationName = "Old Org",
                    AddressLine2 = "Old Address Line 2",
                    CityOrTown = "Old Town",
                    StateProvince = "Old State",
                    PostCode = "12345",
                    SiteCoordinates = "51.5074, -0.1278",
                    OverseasAddressContacts = new List<OverseasAddressContactDto>(),
                    OverseasAddressWasteCodes = new List<OverseasAddressWasteCodeDto>(),
                }
           ]
        };

        // Act & Assert
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
        {
            await _materialRepository.SaveOverseasReprocessingSites(updateDto);
        });
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }
}