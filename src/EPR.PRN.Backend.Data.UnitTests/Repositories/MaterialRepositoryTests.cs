using System.Xml.XPath;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Testing.Platform.Extensions;
using Moq;
using Moq.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories;

[TestClass]
public class MaterialRepositoryTests
{
	private MaterialRepository _materialRepositoryMockContext;
    private MaterialRepository _materialRepository;
    private Mock<EprContext> _mockEprContext;
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
        var options = new DbContextOptionsBuilder<EprContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        _context = new EprContext(options);
        _materialRepository = new MaterialRepository(_context);

        SeedDatabase();

        var dbContextOptions = new DbContextOptionsBuilder<EprContext>().Options;
		_mockEprContext = new Mock<EprContext>(dbContextOptions);
		_mockEprContext.Setup(context => context.Material).ReturnsDbSet(_materials);
		_materialRepositoryMockContext = new MaterialRepository(_mockEprContext.Object);
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
	public async Task GetAllMaterials_ShouldReturnAllMaterials()
	{
		// Act
		var result = await _materialRepositoryMockContext.GetAllMaterials();

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
        var result = await _materialRepositoryMockContext.UpsertRegistrationMaterialContact(registrationMaterialExternalId, userId);

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
        var result = await _materialRepositoryMockContext.UpsertRegistrationMaterialContact(registrationMaterialExternalId, userId);

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
            await _materialRepositoryMockContext.UpsertRegistrationMaterialContact(registrationMaterialExternalId, userId);
        });
    }

    [TestMethod]
    public async Task UpsertRegistrationReprocessingDetails_ShouldThrow_WhenRegistrationMaterialNotFound()
    {
        // Arrange
        var registrationMaterialExternalId = Guid.NewGuid();
        var registrationReprocessingIO = new RegistrationReprocessingIO { TypeOfSuppliers = "Supplier Green", TotalInputs = 10 };
        _mockEprContext.Setup(c => c.RegistrationMaterials)
            .ReturnsDbSet([]);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
        {
            await _materialRepositoryMockContext.UpsertRegistrationReprocessingDetailsAsync(registrationMaterialExternalId, registrationReprocessingIO);
        });
    }

    [TestMethod]
    public async Task UpsertRegistrationReprocessingDetails_ShouldCreateNewReprocessingIO_WhenNoneExists()
    {
        // Arrange
        var registrationMaterialId = 5;
        var registrationMaterialExternalId = Guid.NewGuid();

        var registrationMaterials = new List<RegistrationMaterial>
    {
        new()
        {
            Id = registrationMaterialId,
            ExternalId = registrationMaterialExternalId,
            RegistrationReprocessingIO = null
        }
    };

        var newIO = new RegistrationReprocessingIO
        {
            TypeOfSuppliers = "Supplier A",
            ReprocessingPackagingWasteLastYearFlag = true,
            TotalInputs = 10,
            TotalOutputs = 9,
            UKPackagingWasteTonne = 5,
            NonUKPackagingWasteTonne = 3,
            NotPackingWasteTonne = 0,
            ContaminantsTonne = 1,
            SenttoOtherSiteTonne = 0.5m,
            ProcessLossTonne = 0.5m,
            PlantEquipmentUsed = "Machine X"
        };

        _mockEprContext.Setup(c => c.RegistrationMaterials)
            .ReturnsDbSet(registrationMaterials);
        _mockEprContext.Setup(c => c.RegistrationReprocessingIO)
            .ReturnsDbSet(new List<RegistrationReprocessingIO>());

        // Act
        await _materialRepositoryMockContext.UpsertRegistrationReprocessingDetailsAsync(registrationMaterialExternalId, newIO);

        // Assert
        _mockEprContext.Verify(c => c.RegistrationReprocessingIO.AddAsync(It.Is<RegistrationReprocessingIO>(
            io => io.RegistrationMaterialId == registrationMaterialId &&
                  io.TypeOfSuppliers == "Supplier A" &&
                  io.ExternalId != Guid.Empty
        ), default), Times.Once);

        _mockEprContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [TestMethod]
    public async Task UpsertRegistrationReprocessingDetails_ShouldUpdateExistingReprocessingIO_WhenExists()
    {
        // Arrange
        var registrationMaterialId = 5;
        var registrationMaterialExternalId = Guid.NewGuid();
        var existingIO = new RegistrationReprocessingIO
        {
            Id = 100,
            ExternalId = Guid.NewGuid(),
            RegistrationMaterialId = registrationMaterialId,
            TypeOfSuppliers = "Old Supplier",
            TotalInputs = 1,
            TotalOutputs = 1
        };

        var updatedIO = new RegistrationReprocessingIO
        {
            TypeOfSuppliers = "Updated Supplier",
            ReprocessingPackagingWasteLastYearFlag = true,
            TotalInputs = 20,
            TotalOutputs = 18,
            UKPackagingWasteTonne = 10,
            NonUKPackagingWasteTonne = 5,
            NotPackingWasteTonne = 2,
            ContaminantsTonne = 1,
            SenttoOtherSiteTonne = 0.5m,
            ProcessLossTonne = 0.5m,
            PlantEquipmentUsed = "Updated Machine"
        };

        var registrationMaterials = new List<RegistrationMaterial>
    {
        new()
        {
            Id = registrationMaterialId,
            ExternalId = registrationMaterialExternalId,
            RegistrationReprocessingIO = new List<RegistrationReprocessingIO> { existingIO }
        }
    };

        _mockEprContext.Setup(c => c.RegistrationMaterials)
            .ReturnsDbSet(registrationMaterials);

        _mockEprContext.Setup(c => c.RegistrationReprocessingIO)
            .ReturnsDbSet(new List<RegistrationReprocessingIO> { existingIO });

        // Act
        await _materialRepositoryMockContext.UpsertRegistrationReprocessingDetailsAsync(registrationMaterialExternalId, updatedIO);

        // Assert
        existingIO.TypeOfSuppliers.Should().Be("Updated Supplier");
        existingIO.TotalInputs.Should().Be(20);
        existingIO.TotalOutputs.Should().Be(18);
        existingIO.PlantEquipmentUsed.Should().Be("Updated Machine");

        _mockEprContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [TestMethod]
    public async Task UpsertRegistrationReprocessingDetails_ShouldCreateNewReprocessingIO_WithRawMaterialOrProducts_WhenNoneExists()
    {
        // Arrange
        var registrationMaterialId = 5;
        var registrationMaterialExternalId = Guid.NewGuid();

        var registrationMaterials = new List<RegistrationMaterial>
    {
        new()
        {
            Id = registrationMaterialId,
            ExternalId = registrationMaterialExternalId,
            RegistrationReprocessingIO = null
        }
    };

        var newRawMaterialItems = new List<RegistrationReprocessingIORawMaterialOrProducts>
    {
        new() { TonneValue = 10 },
        new() { RawMaterialOrProductName = "Product Name" }
    };

        var newIO = new RegistrationReprocessingIO
        {
            TypeOfSuppliers = "Supplier A",
            ReprocessingPackagingWasteLastYearFlag = true,
            TotalInputs = 10,
            TotalOutputs = 9,
            UKPackagingWasteTonne = 5,
            NonUKPackagingWasteTonne = 3,
            NotPackingWasteTonne = 0,
            ContaminantsTonne = 1,
            SenttoOtherSiteTonne = 0.5m,
            ProcessLossTonne = 0.5m,
            PlantEquipmentUsed = "Machine X",
            RegistrationReprocessingIORawMaterialOrProducts = newRawMaterialItems
        };

        _mockEprContext.Setup(c => c.RegistrationMaterials)
            .ReturnsDbSet(registrationMaterials);
        _mockEprContext.Setup(c => c.RegistrationReprocessingIO)
            .ReturnsDbSet(new List<RegistrationReprocessingIO>());
        _mockEprContext.Setup(c => c.RegistrationReprocessingIORawMaterialOrProducts)
            .ReturnsDbSet(new List<RegistrationReprocessingIORawMaterialOrProducts>());

        // Act
        await _materialRepository.UpsertRegistrationReprocessingDetailsAsync(registrationMaterialExternalId, newIO);

        // Assert
        _mockEprContext.Verify(c => c.RegistrationReprocessingIO.AddAsync(
            It.Is<RegistrationReprocessingIO>(io =>
                io.RegistrationMaterialId == registrationMaterialId &&
                io.RegistrationReprocessingIORawMaterialOrProducts.Count == 2 &&
                io.RegistrationReprocessingIORawMaterialOrProducts.All(item => item.ExternalId != Guid.Empty)
            ),
            default), Times.Once);

        _mockEprContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [TestMethod]
    public async Task UpsertRegistrationReprocessingDetails_ShouldUpdateExistingReprocessingIO_AndReplaceRawMaterialOrProducts()
    {
        // Arrange
        var registrationMaterialId = 5;
        var registrationMaterialExternalId = Guid.NewGuid();

        var existingRawMaterials = new List<RegistrationReprocessingIORawMaterialOrProducts>
    {
        new() { Id = 1, RawMaterialOrProductName = "OldItem1" },
        new() { Id = 2, RawMaterialOrProductName = "OldItem2" }
    };

        var existingIO = new RegistrationReprocessingIO
        {
            Id = 100,
            ExternalId = Guid.NewGuid(),
            RegistrationMaterialId = registrationMaterialId,
            TypeOfSuppliers = "Old Supplier",
            TotalInputs = 1,
            TotalOutputs = 1,
            RegistrationReprocessingIORawMaterialOrProducts = existingRawMaterials
        };

        var newRawMaterials = new List<RegistrationReprocessingIORawMaterialOrProducts>
    {
        new() { RawMaterialOrProductName = "NewItem1" },
        new() { RawMaterialOrProductName = "NewItem2" }
    };

        var updatedIO = new RegistrationReprocessingIO
        {
            TypeOfSuppliers = "Updated Supplier",
            ReprocessingPackagingWasteLastYearFlag = true,
            TotalInputs = 20,
            TotalOutputs = 18,
            UKPackagingWasteTonne = 10,
            NonUKPackagingWasteTonne = 5,
            NotPackingWasteTonne = 2,
            ContaminantsTonne = 1,
            SenttoOtherSiteTonne = 0.5m,
            ProcessLossTonne = 0.5m,
            PlantEquipmentUsed = "Updated Machine",
            RegistrationReprocessingIORawMaterialOrProducts = newRawMaterials
        };

        var registrationMaterials = new List<RegistrationMaterial>
    {
        new()
        {
            Id = registrationMaterialId,
            ExternalId = registrationMaterialExternalId,
            RegistrationReprocessingIO = new List<RegistrationReprocessingIO> { existingIO }
        }
    };

        _mockEprContext.Setup(c => c.RegistrationMaterials)
            .ReturnsDbSet(registrationMaterials);
        _mockEprContext.Setup(c => c.RegistrationReprocessingIO)
            .ReturnsDbSet(new List<RegistrationReprocessingIO> { existingIO });
        _mockEprContext.Setup(c => c.RegistrationReprocessingIORawMaterialOrProducts)
            .ReturnsDbSet(existingRawMaterials);

        // Act
        await _materialRepository.UpsertRegistrationReprocessingDetailsAsync(registrationMaterialExternalId, updatedIO);

        // Assert
        _mockEprContext.Verify(c => c.RegistrationReprocessingIORawMaterialOrProducts.RemoveRange(existingRawMaterials), Times.Once);
        _mockEprContext.Verify(c => c.RegistrationReprocessingIORawMaterialOrProducts.Add(
            It.Is<RegistrationReprocessingIORawMaterialOrProducts>(x =>
                x.RawMaterialOrProductName == "NewItem1" || x.RawMaterialOrProductName == "NewItem2"
            )
        ), Times.Exactly(2));

        _mockEprContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

        existingIO.TypeOfSuppliers.Should().Be("Updated Supplier");
        existingIO.TotalInputs.Should().Be(20);
        existingIO.TotalOutputs.Should().Be(18);
        existingIO.PlantEquipmentUsed.Should().Be("Updated Machine");
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
            IsMaterialRegistered = false
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
                    OverseasAddressContacts = new List<OverseasAddressContactDto>(),
                    OverseasAddressWasteCodes = new List<OverseasAddressWasteCodeDto>(),
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
        await _materialRepository.SaveOverseasSitesTransaction(updateDto);

        // Assert
        var allAddresses = await _context.OverseasAddress
            .Include(a => a.OverseasAddressContacts)
            .Include(a => a.OverseasAddressWasteCodes)
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

        _context.OverseasAddress.AddRange(existingToKeep, existingToDelete);
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
        await _materialRepository.SaveOverseasSitesTransaction(updateDto);

        // Assert
        var allAddresses = await _context.OverseasAddress.ToListAsync();
        allAddresses.Should().ContainSingle(a => a.ExternalId == existingToKeep.ExternalId);
        allAddresses.Should().NotContain(a => a.ExternalId == existingToDelete.ExternalId);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }
}