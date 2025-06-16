using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Commands.ExporterJourney;
using EPR.PRN.Backend.API.Dto.ExporterJourney;
using EPR.PRN.Backend.API.Handlers.ExporterJourney;
using EPR.PRN.Backend.Data.DataModels.ExporterJourney;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers.ExporterJourney;

[TestClass]
public class CreateOtherPermitsHandlerTests
{
    private Mock<ICarrierBrokerDealerPermitRepository> _carrierBrokerDealerPermitRepositoryMock;
    private Mock<IRegistrationRepository> _registrationRepository;
    private CreateOtherPermitsHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _carrierBrokerDealerPermitRepositoryMock = new Mock<ICarrierBrokerDealerPermitRepository>();
        _registrationRepository = new Mock<IRegistrationRepository>();

        _handler = new CreateOtherPermitsHandler(_carrierBrokerDealerPermitRepositoryMock.Object, _registrationRepository.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldThrowException_WhenRegistrationNotFound()
    {
        // Arrange
        _registrationRepository.Setup(x => x.GetRegistrationByExternalId(Guid.Empty, CancellationToken.None))
            .ReturnsAsync((Registration)null);

        var command = new CreateOtherPermitsCommand();

        // Act, Assert
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task Handle_ShouldReturnFalse_WhenCarrierBrokerDealerPermitExists()
    {
        // Arrange
        var registration = new Registration
        {
            ExternalId = Guid.NewGuid(),
            CarrierBrokerDealerPermit = new CarrierBrokerDealerPermit()
        };

        _registrationRepository.Setup(x => x.GetRegistrationByExternalId(registration.ExternalId, CancellationToken.None))
            .ReturnsAsync(registration);

        var command = new CreateOtherPermitsCommand { RegistrationId = registration.ExternalId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task Handle_ShouldReturnTrue_WhenCarrierBrokerDealerPermitDoesNotExist()
    {
        // Arrange
        var registration = new Registration { ExternalId = Guid.NewGuid() };
        CarrierBrokerDealerPermit createdCarrierBrokerDealerPermit = null;

        _registrationRepository.Setup(x => x.GetRegistrationByExternalId(registration.ExternalId, CancellationToken.None))
            .ReturnsAsync(registration);

        _carrierBrokerDealerPermitRepositoryMock.Setup(x => x.Add(It.IsAny<CarrierBrokerDealerPermit>(), CancellationToken.None))
            .Callback<CarrierBrokerDealerPermit, CancellationToken>((x, y) => createdCarrierBrokerDealerPermit = x);

        var command = new CreateOtherPermitsCommand
        {
            UserId = Guid.NewGuid(),
            RegistrationId = registration.ExternalId,
            Dto = new CreateOtherPermitsDto
            {
                WasteLicenseOrPermitNumber = "test 1",
                PpcNumber = "test 2",
                WasteExemptionReference = new List<string> { "test 3", "test 4" }
            }
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        createdCarrierBrokerDealerPermit.Should().NotBeNull();
        createdCarrierBrokerDealerPermit.WasteManagementorEnvironmentPermitNumber.Should().Be(command.Dto.WasteLicenseOrPermitNumber);
        createdCarrierBrokerDealerPermit.InstallationPermitorPPCNumber.Should().Be(command.Dto.PpcNumber);
        createdCarrierBrokerDealerPermit.WasteExemptionReference.Should().Be("test 3,test 4");
        createdCarrierBrokerDealerPermit.CreatedBy.Should().Be(command.UserId);
    }
}
