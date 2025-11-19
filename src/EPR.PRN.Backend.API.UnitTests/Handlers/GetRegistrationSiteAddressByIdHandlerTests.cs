using AutoMapper;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.API.Profiles.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetRegistrationSiteAddressByIdHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _rmRepositoryMock;
    private IMapper _mapper;
    private GetRegistrationSiteAddressByIdHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _rmRepositoryMock = new Mock<IRegistrationMaterialRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RegistrationMaterialProfile>(); 
        });
        _mapper = config.CreateMapper();

        _handler = new GetRegistrationSiteAddressByIdHandler(_rmRepositoryMock.Object, _mapper);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenReprocessingSiteAddressExists()
    {
        // Arrange
        var registrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");
        var query = new GetRegistrationSiteAddressByIdQuery { Id = registrationId };

        var registration = new Registration
        {
            ExternalId = registrationId,
            ReprocessingSiteAddressId = 99,
            ReprocessingSiteAddress = new Address
            {
                AddressLine1 = "42 Recycle Road",
                TownCity = "EcoTown",
                PostCode = "ET1 9XY"
            }
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationById(registrationId))
            .ReturnsAsync(registration);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.SiteAddress.Should().Contain("Recycle Road"); 
    }

    [TestMethod]
    public async Task Handle_ShouldReturnEmptyDto_WhenReprocessingSiteAddressIsNull()
    {
        // Arrange
        var registrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");
        var query = new GetRegistrationSiteAddressByIdQuery { Id = registrationId };

        var registration = new Registration
        {
            ExternalId = registrationId,
            ReprocessingSiteAddressId = null
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationById(registrationId))
            .ReturnsAsync(registration);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.SiteAddress.Should().BeNullOrEmpty();
    }
    
    [TestMethod]
    public async Task Handle_ShouldMapQueryNotesCorrectly_WhenValidQueryNotesExist()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var createdBy = Guid.NewGuid();
        var createdDate = new DateTime(2024, 1, 1);


        var query = new GetRegistrationSiteAddressByIdQuery { Id = registrationId };

        var queryNote = new Note
        {
            Notes = "Note A",
            CreatedBy = createdBy,
            CreatedDate = createdDate
        };

        var registration = new Registration
        {
            ExternalId = registrationId,
            ReprocessingSiteAddressId = 123,
            ApplicationTypeId=1,
            ReprocessingSiteAddress = new Address
            {
                AddressLine1 = "123 Mapper Street",
                TownCity = "Noteville",
                PostCode = "NT1 2AB",
                NationId = 1,
                GridReference = "GR123"
            },
            OrganisationId = Guid.NewGuid(),
            Tasks = new List<RegulatorRegistrationTaskStatus>
        {
            new RegulatorRegistrationTaskStatus
            {
                ExternalId = Guid.NewGuid(),
                TaskStatusId = (int)RegulatorTaskStatus.Queried,
                Task = new LookupRegulatorTask
                {
                    Name = RegulatorTaskNames.SiteAddressAndContactDetails,
                    ApplicationTypeId = 1,
                    IsMaterialSpecific = false
                },
                RegistrationTaskStatusQueryNotes = new List<RegistrationTaskStatusQueryNote>
                {
                    new() { QueryNote = queryNote },
                    new() { QueryNote = queryNote } // Intentional duplicate
                }
            }
        }
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationById(registrationId))
            .ReturnsAsync(registration);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.QueryNotes.Should().ContainSingle(q =>
            q.Notes == "Note A" &&
            q.CreatedBy == createdBy &&
            q.CreatedDate == createdDate);

        result.QueryNotes.Should().HaveCount(1); // Deduplication assertion
    }


    [TestMethod]
    public async Task Handle_ShouldNotFail_WhenTasksIsEmptyAndAddressExists()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var query = new GetRegistrationSiteAddressByIdQuery { Id = registrationId };

        var registration = new Registration
        {
            ExternalId = registrationId,
            ReprocessingSiteAddressId = 10,
            ReprocessingSiteAddress = new Address
            {
                AddressLine1 = "Empty Tasks Street",
                TownCity = "Quietville",
                PostCode = "QT1 9ZZ"
            },
            Tasks = new List<RegulatorRegistrationTaskStatus>() // empty list
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationById(registrationId))
            .ReturnsAsync(registration);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.SiteAddress.Should().Contain("Empty Tasks Street");
        registration.Tasks.Should().BeEmpty();
    }
    
    [TestMethod]
    public async Task Handle_ShouldReturnEmptyDto_WhenRegistrationIsNull()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var query = new GetRegistrationSiteAddressByIdQuery { Id = registrationId };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationById(registrationId))
            .ReturnsAsync((Registration)null!);

        // Act & Assert
        await Assert.ThrowsExactlyAsync<NullReferenceException>(async () =>
        {
            await _handler.Handle(query, CancellationToken.None);
        });
    }

}
