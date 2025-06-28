using EPR.PRN.Backend.API.Commands.ExporterJourney;
using EPR.PRN.Backend.API.Handlers.ExporterJourney;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers.ExporterJourney;

[TestClass]
public class CreateCarrierBrokerDealerPermitsHandlerTests
{
    private Mock<ICarrierBrokerDealerPermitRepository> _carrierBrokerDealerPermitRepositoryMock;
    private Mock<IRegistrationRepository> _registrationRepository;
    private CreateCarrierBrokerDealerPermitsHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _carrierBrokerDealerPermitRepositoryMock = new Mock<ICarrierBrokerDealerPermitRepository>();
        _registrationRepository = new Mock<IRegistrationRepository>();

        _handler = new CreateCarrierBrokerDealerPermitsHandler(_carrierBrokerDealerPermitRepositoryMock.Object, _registrationRepository.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldThrowException_WhenRegistrationNotFound()
    {
        // Arrange
        _registrationRepository.Setup(x => x.GetRegistrationByExternalId(Guid.Empty, CancellationToken.None))
            .ReturnsAsync((Registration)null);

        var command = new CreateCarrierBrokerDealerPermitsCommand()
        {
            WasteCarrierBrokerDealerRegistration = "Test 1"
        };

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
            CarrierBrokerDealerPermit = new CarrierBrokerDealerPermits()
        };

        _registrationRepository.Setup(x => x.GetRegistrationByExternalId(registration.ExternalId, CancellationToken.None))
            .ReturnsAsync(registration);

        var command = new CreateCarrierBrokerDealerPermitsCommand { RegistrationId = registration.ExternalId, WasteCarrierBrokerDealerRegistration = "test 1" };

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
        CarrierBrokerDealerPermits createdCarrierBrokerDealerPermit = null;

        _registrationRepository.Setup(x => x.GetRegistrationByExternalId(registration.ExternalId, CancellationToken.None))
            .ReturnsAsync(registration);

        _carrierBrokerDealerPermitRepositoryMock.Setup(x => x.Add(It.IsAny<CarrierBrokerDealerPermits>(), CancellationToken.None))
            .Callback<CarrierBrokerDealerPermits, CancellationToken>((x, y) => createdCarrierBrokerDealerPermit = x);

        var command = new CreateCarrierBrokerDealerPermitsCommand
        {
            UserId = Guid.NewGuid(),
            RegistrationId = registration.ExternalId,
            WasteCarrierBrokerDealerRegistration = "Test 1",
            RegisteredWasteCarrierBrokerDealerFlag = true

        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        createdCarrierBrokerDealerPermit.Should().NotBeNull();
        createdCarrierBrokerDealerPermit.WasteCarrierBrokerDealerRegistration.Should().Be("Test 1");
        createdCarrierBrokerDealerPermit.CreatedBy.Should().Be(command.UserId);
        createdCarrierBrokerDealerPermit.RegisteredWasteCarrierBrokerDealerFlag.Should().BeTrue();
    }
}