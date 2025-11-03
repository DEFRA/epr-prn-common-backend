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
public class GetRegistrationWasteCarrierDetailsByIdHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _rmRepositoryMock;
    private IMapper _mapper;
    private GetRegistrationWasteCarrierDetailsByIdHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _rmRepositoryMock = new Mock<IRegistrationMaterialRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RegistrationMaterialProfile>();
        });
        _mapper = config.CreateMapper();

        _handler = new GetRegistrationWasteCarrierDetailsByIdHandler(_rmRepositoryMock.Object, _mapper);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto()
    {
        // Arrange
        var registrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");
        var query = new GetRegistrationWasteCarrierDetailsByIdQuery { Id = registrationId };

        var registration = new Registration
        {
            ExternalId = registrationId,
            ReprocessingSiteAddressId = 99,
            ReprocessingSiteAddress = new Address
            {
                AddressLine1 = "42 Recycle Road",
                TownCity = "EcoTown",
                PostCode = "ET1 9XY"
            },
            CarrierBrokerDealerPermit = new CarrierBrokerDealerPermits
            {
                WasteCarrierBrokerDealerRegistration = "Test123"
            }
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationById(registrationId))
            .ReturnsAsync(registration);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.RegistrationId.Should().Be(registrationId);
        result.SiteAddress.Should().Contain("Recycle Road");
        result.WasteCarrierBrokerDealerNumber.Should().Be(registration.CarrierBrokerDealerPermit.WasteCarrierBrokerDealerRegistration);
    }

    [TestMethod]
    public async Task Handle_ShouldMapQueryNotesCorrectly_WhenValidQueryNotesExist()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var createdBy = Guid.NewGuid();
        var createdDate = new DateTime(2024, 1, 1);
        var query = new GetRegistrationWasteCarrierDetailsByIdQuery() { Id = registrationId };

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
            ApplicationTypeId = 1,
            ReprocessingSiteAddress = new Address
            {
                AddressLine1 = "123 Mapper Street",
                TownCity = "Noteville",
                PostCode = "NT1 2AB",
                NationId = 1,
                GridReference = "GR123"
            },
            OrganisationId = Guid.NewGuid(),
            Tasks =
            [
                new RegulatorRegistrationTaskStatus()
                {
                    ExternalId = Guid.NewGuid(),
                    TaskStatusId = (int)RegulatorTaskStatus.Queried,
                    Task = new LookupRegulatorTask
                    {
                        Name = RegulatorTaskNames.WasteCarrierBrokerDealerNumber,
                        ApplicationTypeId = 1,
                        IsMaterialSpecific = false
                    },
                    RegistrationTaskStatusQueryNotes =
                    [
                        new() { QueryNote = queryNote },
                        new() { QueryNote = queryNote }
                    ]
                }
            ]
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
}
