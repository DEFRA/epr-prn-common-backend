using AutoMapper;
using EPR.PRN.Backend.API.Handlers.ExporterJourney;
using EPR.PRN.Backend.API.Profiles.Regulator;
using EPR.PRN.Backend.API.Queries.ExporterJourney;
using EPR.PRN.Backend.Data.DataModels.ExporterJourney;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers.ExporterJourney;

[TestClass]
public class GetOtherPermitsHandlerTests
{
    private Mock<ICarrierBrokerDealerPermitRepository> _carrierBrokerDealerPermitRepositoryMock;
    private IMapper _mapper;
    private GetCarrierBrokerDealerPermitsHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _carrierBrokerDealerPermitRepositoryMock = new Mock<ICarrierBrokerDealerPermitRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<OtherPermitsProfile>();
        });
        _mapper = config.CreateMapper();

        _handler = new GetCarrierBrokerDealerPermitsHandler(_carrierBrokerDealerPermitRepositoryMock.Object, _mapper);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnNull_WhenPermitDoesNotExists()
    {
        // Arrange
        var registration = new Registration
        {
            Id = 1,
            ExternalId = Guid.NewGuid()
        };

        _carrierBrokerDealerPermitRepositoryMock
            .Setup(r => r.GetByRegistrationId(registration.ExternalId, CancellationToken.None))
            .ReturnsAsync((CarrierBrokerDealerPermit)null);

        var query = new CarrierBrokerDealerPermitsQuery { RegistrationId = registration.ExternalId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenPermitExists()
    {
        // Arrange
        var registration = new Registration
        {
            Id = 1,
            ExternalId = Guid.NewGuid()
        };

        var carrierBrokerDealerPermit = new CarrierBrokerDealerPermit
        {
            Id = 1,
            ExternalId = Guid.NewGuid(),
            RegistrationId = registration.Id,
            Registration = registration,
            WasteManagementorEnvironmentPermitNumber = "test 1",
            InstallationPermitorPPCNumber = "test 2",
            WasteExemptionReference = "test 3,test 4,test 5"
        };

        _carrierBrokerDealerPermitRepositoryMock
            .Setup(r => r.GetByRegistrationId(registration.ExternalId, CancellationToken.None))
            .ReturnsAsync(carrierBrokerDealerPermit);

        var query = new CarrierBrokerDealerPermitsQuery { RegistrationId = registration.ExternalId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CarrierBrokerDealerPermitId.Should().Be(carrierBrokerDealerPermit.ExternalId);
        result.RegistrationId.Should().Be(registration.ExternalId);
        result.WasteLicenseOrPermitNumber.Should().Be(carrierBrokerDealerPermit.WasteManagementorEnvironmentPermitNumber);
        result.WasteExemptionReference.Should().BeEquivalentTo(new List<string> { "test 3", "test 4", "test 5" });
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDtoWithEmptyWasteExemptionReferenceList_WhenWasteExemptionReferenceIsNull()
    {
        // Arrange
        var registration = new Registration
        {
            Id = 1,
            ExternalId = Guid.NewGuid()
        };

        var carrierBrokerDealerPermit = new CarrierBrokerDealerPermit
        {
            Id = 1,
            ExternalId = Guid.NewGuid(),
            RegistrationId = registration.Id,
            Registration = registration,
            WasteManagementorEnvironmentPermitNumber = "test 1",
            InstallationPermitorPPCNumber = "test 2",
            WasteExemptionReference = null
        };

        _carrierBrokerDealerPermitRepositoryMock
            .Setup(r => r.GetByRegistrationId(registration.ExternalId, CancellationToken.None))
            .ReturnsAsync(carrierBrokerDealerPermit);

        var query = new CarrierBrokerDealerPermitsQuery { RegistrationId = registration.ExternalId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CarrierBrokerDealerPermitId.Should().Be(carrierBrokerDealerPermit.ExternalId);
        result.RegistrationId.Should().Be(registration.ExternalId);
        result.WasteLicenseOrPermitNumber.Should().Be(carrierBrokerDealerPermit.WasteManagementorEnvironmentPermitNumber);
        result.WasteExemptionReference.Should().BeEmpty();
    }
}
