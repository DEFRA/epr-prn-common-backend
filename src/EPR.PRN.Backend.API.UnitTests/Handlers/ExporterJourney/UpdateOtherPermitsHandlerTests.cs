using EPR.PRN.Backend.API.Commands.ExporterJourney;
using EPR.PRN.Backend.API.Dto.ExporterJourney;
using EPR.PRN.Backend.API.Handlers.ExporterJourney;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers.ExporterJourney;

[TestClass]
public class UpdateOtherPermitsHandlerTests
{
    private Mock<ICarrierBrokerDealerPermitRepository> _carrierBrokerDealerPermitRepositoryMock;
    private UpdateCarrierBrokerDealerPermitsHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _carrierBrokerDealerPermitRepositoryMock = new Mock<ICarrierBrokerDealerPermitRepository>();

        _handler = new UpdateCarrierBrokerDealerPermitsHandler(_carrierBrokerDealerPermitRepositoryMock.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldThrowException_WhenCarrierBrokerDealerPermitNotFound()
    {
        // Arrange
        _carrierBrokerDealerPermitRepositoryMock.Setup(x => x.GetByRegistrationId(Guid.Empty, CancellationToken.None))
            .ReturnsAsync((CarrierBrokerDealerPermits)null);

        var carrierDealerBrokerPermitsDto = new UpdateCarrierBrokerDealerPermitsDto() { WasteCarrierBrokerDealerRegistration = "test 1" };

        var command = new UpdateCarrierBrokerDealerPermitsCommand()
        {
            Dto = carrierDealerBrokerPermitsDto
        };

        // Act, Assert
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task Handle_ShouldUpdate_WhenCarrierBrokerDealerPermitFound()
    {
        // Arrange
        var registration = new Registration
        {
            ExternalId = Guid.NewGuid(),
            CarrierBrokerDealerPermit = new CarrierBrokerDealerPermits()
        };

        CarrierBrokerDealerPermits updatedCarrierBrokerDealerPermit = null;

        _carrierBrokerDealerPermitRepositoryMock.Setup(x => x.GetByRegistrationId(registration.ExternalId, CancellationToken.None))
            .ReturnsAsync(registration.CarrierBrokerDealerPermit);

        _carrierBrokerDealerPermitRepositoryMock.Setup(x => x.Update(registration.CarrierBrokerDealerPermit, CancellationToken.None))
            .Callback<CarrierBrokerDealerPermits, CancellationToken>((x, y) => updatedCarrierBrokerDealerPermit = x);

        var command = new UpdateCarrierBrokerDealerPermitsCommand
        {
            UserId = Guid.NewGuid(),
            RegistrationId = registration.ExternalId,
            Dto = new UpdateCarrierBrokerDealerPermitsDto
            {
                WasteLicenseOrPermitNumber = "test 1",
                PpcNumber = "test 2",
                WasteExemptionReference = new List<string> { "test 3", "test 4" },
            }
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        updatedCarrierBrokerDealerPermit.Should().NotBeNull();
        updatedCarrierBrokerDealerPermit.WasteManagementEnvironmentPermitNumber.Should().Be(command.Dto.WasteLicenseOrPermitNumber);
        updatedCarrierBrokerDealerPermit.InstallationPermitOrPPCNumber.Should().Be(command.Dto.PpcNumber);
        updatedCarrierBrokerDealerPermit.WasteExemptionReference.Should().Be("test 3,test 4");
        updatedCarrierBrokerDealerPermit.UpdatedBy.Should().Be(command.UserId);
    }

    [TestMethod]
    public async Task Handle_ShouldUpdate_WhenRegistrationIsSupplied()
    {
        // Arrange
        var registration = new Registration
        {
            ExternalId = Guid.NewGuid(),
            CarrierBrokerDealerPermit = new CarrierBrokerDealerPermits()
        };

        CarrierBrokerDealerPermits updatedCarrierBrokerDealerPermit = null;

        _carrierBrokerDealerPermitRepositoryMock.Setup(x => x.GetByRegistrationId(registration.ExternalId, CancellationToken.None))
            .ReturnsAsync(registration.CarrierBrokerDealerPermit);

        _carrierBrokerDealerPermitRepositoryMock.Setup(x => x.Update(registration.CarrierBrokerDealerPermit, CancellationToken.None))
            .Callback<CarrierBrokerDealerPermits, CancellationToken>((x, y) => updatedCarrierBrokerDealerPermit = x);

        var command = new UpdateCarrierBrokerDealerPermitsCommand
        {
            UserId = Guid.NewGuid(),
            RegistrationId = registration.ExternalId,
            Dto = new UpdateCarrierBrokerDealerPermitsDto
            {
                WasteCarrierBrokerDealerRegistration = "123123", 
                WasteLicenseOrPermitNumber = "test 1",
                PpcNumber = "test 2",
                WasteExemptionReference = new List<string> { "test 3", "test 4" },
            }
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        updatedCarrierBrokerDealerPermit.Should().NotBeNull();
        updatedCarrierBrokerDealerPermit.WasteCarrierBrokerDealerRegistration.Should().Be(command.Dto.WasteCarrierBrokerDealerRegistration);
        updatedCarrierBrokerDealerPermit.WasteManagementEnvironmentPermitNumber.Should().Be(command.Dto.WasteLicenseOrPermitNumber);
        updatedCarrierBrokerDealerPermit.InstallationPermitOrPPCNumber.Should().Be(command.Dto.PpcNumber);
        updatedCarrierBrokerDealerPermit.WasteExemptionReference.Should().Be("test 3,test 4");
        updatedCarrierBrokerDealerPermit.UpdatedBy.Should().Be(command.UserId);
    }
}
