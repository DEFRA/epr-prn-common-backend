using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories;

[TestClass]
public class InterimSitesRepositoryTests
{
    private EprContext _context;
    private MaterialRepository _repository;
    private Guid _regMaterialExternalId;
    private int _regId;
    private int _regMaterialId;
    private Guid _userId;

    [TestInitialize]
    public void Init()
    {
        var options = new DbContextOptionsBuilder<EprContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _context = new EprContext(options);
        _repository = new MaterialRepository(_context, new LoggerFactory().CreateLogger<MaterialRepository>());

        _userId = Guid.NewGuid();
        _regMaterialExternalId = Guid.NewGuid();

        // Seed Registration and RegistrationMaterial
        var registration = new Registration { Id = 1, ExternalId = Guid.NewGuid() };
        var registrationMaterial = new RegistrationMaterial
        {
            Id = 1,
            Registration = registration,
            RegistrationId = registration.Id,
            ExternalId = _regMaterialExternalId
        };

        _context.RegistrationMaterials.Add(registrationMaterial);

        _context.LookupApplicantRegistrationTasks.Add(new LookupApplicantRegistrationTask
        {
            Id = 1,
            Name = "InterimSites",
            IsMaterialSpecific = true,
            ApplicationTypeId = registration.ApplicationTypeId
        });

        _regMaterialId = registrationMaterial.Id;
        _regId = registration.Id;

        
        _context.LookupCountries.Add(new LookupCountry { Id = 1, Name = "TestCountry" });

        
        _context.LookupTaskStatuses.Add(new LookupTaskStatus
        {
            Id = 5,
            Name = "Completed"
        });

        // Seed ApplicantRegistrationTaskStatus for status update method to work
        _context.RegistrationTaskStatus.Add(new ApplicantRegistrationTaskStatus
        {
            RegistrationMaterialId = _regMaterialId,
            TaskStatusId = 5
        });

        _context.SaveChanges();
    }


    [TestMethod]
    public async Task Should_Add_New_Interim_Site_And_Connection()
    {
        var parentExternalId = Guid.NewGuid();
        _context.OverseasAddress.Add(new OverseasAddress
        {
            Id = 99,
            ExternalId = parentExternalId,
            IsInterimSite = false,
            RegistrationId = _regId,
            OrganisationName = "TestOrg",
            AddressLine1 = "Addressline1",
            CityOrTown = "Testcity"
        });

        var dto = new SaveInterimSitesRequestDto
        {
            RegistrationMaterialId = _regMaterialExternalId,
            UserId = _userId,
            OverseasMaterialReprocessingSites =
            [
                new OverseasMaterialReprocessingSiteDto
                {
                    OverseasAddressId = parentExternalId,
                    InterimSiteAddresses =
                    [
                        new InterimSiteAddressDto
                        {
                            OrganisationName = "TestOrg",
                            AddressLine1 = "Line1",
                            CityOrTown = "City",
                            StateProvince = "State",
                            PostCode = "Post",
                            CountryName = "TestCountry",
                            ParentExternalId = parentExternalId,
                            InterimAddressContact = new List<OverseasAddressContactDto>
                            {
                                new()
                                {
                                    FullName = "Test Person",
                                    Email = "test@email.com",
                                    PhoneNumber = "123"
                                }
                            },
                            AddressLine2 = "Addresss line 2"
                        }
                    ],
                    OverseasAddress = new OverseasAddressDto
                    {
                        OrganisationName = "Testorg",
                        AddressLine1 = "Address line 1",
                        CityOrTown = "Testcity",
                        CountryName = "Testcountry"
                    }
                }
            ]
        };

        await _repository.SaveInterimSitesAsync(dto);

        _context.OverseasAddress.Should().ContainSingle(x => x.IsInterimSite == true);
        _context.InterimOverseasConnections.Should().ContainSingle();
    }

    [TestMethod]
    public async Task Should_Update_Existing_Interim_Site()
    {
        var existingExternalId = Guid.NewGuid();
        _context.OverseasAddress.Add(new OverseasAddress
        {
            Id = 1,
            ExternalId = existingExternalId,
            RegistrationId = _regId,
            IsInterimSite = true,
            OrganisationName = "Old Name",
            OverseasAddressContacts =
            [
                new OverseasAddressContact
                {
                    FullName = "Old Person",
                    CreatedBy = _userId,
                    Email = "bb@gmail.com",
                    PhoneNumber = ""
                }
            ],
            AddressLine1 = "Adddress line 1",
            CityOrTown = "testcity"
        });
        _context.SaveChanges();

        var dto = new SaveInterimSitesRequestDto
        {
            RegistrationMaterialId = _regMaterialExternalId,
            UserId = _userId,
            OverseasMaterialReprocessingSites =
            [
                new OverseasMaterialReprocessingSiteDto
                {
                    OverseasAddressId = Guid.NewGuid(),
                    InterimSiteAddresses =
                    [
                        new InterimSiteAddressDto
                        {
                            ExternalId = existingExternalId,
                            OrganisationName = "Updated Name",
                            AddressLine1 = "New Line",
                            CityOrTown = "City",
                            CountryName = "TestCountry",
                            InterimAddressContact = new List<OverseasAddressContactDto>(),
                            AddressLine2 = "Address line 2",
                            PostCode = "987-654-321"
                        }
                    ],
                    OverseasAddress = new OverseasAddressDto
                    {
                        OrganisationName = "Testorg",
                        AddressLine1 = "Address line 1",
                        CityOrTown = "Testcity",
                        CountryName = "Testcountry"
                    }
                }
            ]
        };

        await _repository.SaveInterimSitesAsync(dto);

        var updated = _context.OverseasAddress.First(x => x.ExternalId == existingExternalId);
        updated.OrganisationName.Should().Be("Updated Name");
    }

    [TestMethod]
    public async Task Should_Delete_Obsolete_Site_When_Missing_In_Request()
    {
        var existingExternalId = Guid.NewGuid();
        _context.OverseasAddress.Add(new OverseasAddress
        {
            Id = 2,
            ExternalId = existingExternalId,
            RegistrationId = _regId,
            IsInterimSite = true,
            OrganisationName = "test org",
            AddressLine1 = "Address line 1",
            CityOrTown = "test city"
        });
        _context.SaveChanges();

        var dto = new SaveInterimSitesRequestDto
        {
            RegistrationMaterialId = _regMaterialExternalId,
            UserId = _userId,
            OverseasMaterialReprocessingSites = []
        };

        await _repository.SaveInterimSitesAsync(dto);

        _context.OverseasAddress.Should().BeEmpty();
    }

    [TestMethod]
    public async Task Should_Skip_When_InterimSiteAddresses_Null()
    {
        var dto = new SaveInterimSitesRequestDto
        {
            RegistrationMaterialId = _regMaterialExternalId,
            UserId = _userId,
            OverseasMaterialReprocessingSites =
            [
                new OverseasMaterialReprocessingSiteDto
                {
                    OverseasAddressId = Guid.NewGuid(),
                    InterimSiteAddresses = null,
                    OverseasAddress = new OverseasAddressDto
                    {
                        OrganisationName = "Testorg",
                        AddressLine1 = "Address line 1",
                        CityOrTown = "Testcity",
                        CountryName = "Testcountry"
                    }
                }
            ]
        };

        Func<Task> act = async () => await _repository.SaveInterimSitesAsync(dto);

        await act.Should().NotThrowAsync();
    }

    [TestMethod]
    public async Task Should_Skip_Connection_If_Parent_Not_Found()
    {
        var dto = new SaveInterimSitesRequestDto
        {
            RegistrationMaterialId = _regMaterialExternalId,
            UserId = _userId,
            OverseasMaterialReprocessingSites =
            [
                new OverseasMaterialReprocessingSiteDto
                {
                    OverseasAddressId = Guid.NewGuid(),
                    InterimSiteAddresses =
                    [
                        new InterimSiteAddressDto
                        {
                            OrganisationName = "TestOrg",
                            AddressLine1 = "Line1",
                            CityOrTown = "City",
                            CountryName = "TestCountry",
                            ParentExternalId = Guid.NewGuid(),
                            InterimAddressContact =
                            [
                            ],
                            AddressLine2 = "Address line 2",
                            PostCode = "987-654-321"
                        }
                    ],
                    OverseasAddress = new OverseasAddressDto
                    {
                        OrganisationName = "Testorg",
                        AddressLine1 = "Address line 1",
                        CityOrTown = "Testcity",
                        CountryName = "Testcountry"
                    }
                }
            ]
        };

        await _repository.SaveInterimSitesAsync(dto);

        _context.InterimOverseasConnections.Should().BeEmpty();
    }

    [TestMethod]
    public async Task Should_Throw_When_RegistrationMaterial_NotFound()
    {
        var dto = new SaveInterimSitesRequestDto
        {
            RegistrationMaterialId = Guid.NewGuid(),
            UserId = _userId,
            OverseasMaterialReprocessingSites = []
        };

        Func<Task> act = async () => await _repository.SaveInterimSitesAsync(dto);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [TestMethod]
    public async Task Should_Handle_Empty_Input_And_Still_Update_Status()
    {
        var dto = new SaveInterimSitesRequestDto
        {
            RegistrationMaterialId = _regMaterialExternalId,
            UserId = _userId,
            OverseasMaterialReprocessingSites = []
        };

        Func<Task> act = async () => await _repository.SaveInterimSitesAsync(dto);

        await act.Should().NotThrowAsync();
    }

    [TestMethod]
    public async Task GetOverseasMaterialReprocessingSites_ShouldReturnSitesWithRelatedEntities()
    {
        // Arrange
        var regMaterialExternalId = Guid.NewGuid();

        var registration = new Registration
        {
            Id = 10,
            ExternalId = Guid.NewGuid()
        };

        var registrationMaterial = new RegistrationMaterial
        {
            Id = 10,
            ExternalId = regMaterialExternalId,
            Registration = registration,
            RegistrationId = registration.Id
        };

        var childInterim = new OverseasAddress
        {
            OrganisationName = "Child Org",
            AddressLine1 = "Child Address",
            CityOrTown = "Child City",
            Country = new LookupCountry { Name = "TestCountry" },
            OverseasAddressContacts = new List<OverseasAddressContact>
        {
            new() { FullName = "Child Contact", Email = "child@test.com", PhoneNumber = "11111" }
        }
        };

        var parentAddress = new OverseasAddress
        {
            Id = 20,
            OrganisationName = "Parent Org",
            AddressLine1 = "Parent Address",
            CityOrTown = "Parent City",
            Country = new LookupCountry { Name = "TestCountry" },
            OverseasAddressContacts = new List<OverseasAddressContact>
        {
            new() { FullName = "Parent Contact", Email = "parent@test.com", PhoneNumber = "22222" }
        },
            OverseasAddressWasteCodes = new List<OverseasAddressWasteCode>
        {
            new() { CodeName = "OEC123" }
        },
            ChildInterimConnections = new List<InterimOverseasConnections>
        {
            new() { OverseasAddress = childInterim }
        }
        };

        var site = new OverseasMaterialReprocessingSite
        {
            OverseasAddress = parentAddress,
            OverseasAddressId = parentAddress.Id,
            RegistrationMaterial = registrationMaterial,
            RegistrationMaterialId = registrationMaterial.Id
        };

        _context.Registrations.Add(registration);
        _context.RegistrationMaterials.Add(registrationMaterial);
        _context.OverseasAddress.AddRange(parentAddress, childInterim);
        _context.OverseasMaterialReprocessingSite.Add(site);
        _context.SaveChanges();

        // Act
        var result = await _repository.GetOverseasMaterialReprocessingSites(regMaterialExternalId);

        // Assert
        result.Should().HaveCount(1);
        var fetchedSite = result.First();
        fetchedSite.OverseasAddress.Should().NotBeNull();
        fetchedSite.OverseasAddress.Country.Should().NotBeNull();
        fetchedSite.OverseasAddress.OverseasAddressWasteCodes.Should().NotBeEmpty();
        fetchedSite.OverseasAddress.OverseasAddressContacts.Should().NotBeEmpty();
        fetchedSite.OverseasAddress.ChildInterimConnections.Should().NotBeEmpty();
        fetchedSite.OverseasAddress.ChildInterimConnections.First().OverseasAddress.Should().NotBeNull();
        fetchedSite.OverseasAddress.ChildInterimConnections.First().OverseasAddress.OverseasAddressContacts.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task GetOverseasMaterialReprocessingSites_ShouldReturnEmptyList_WhenNoSitesExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _repository.GetOverseasMaterialReprocessingSites(nonExistentId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

}
