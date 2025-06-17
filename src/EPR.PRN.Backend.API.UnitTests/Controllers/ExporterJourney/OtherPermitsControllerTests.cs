using EPR.PRN.Backend.API.Commands.ExporterJourney;
using EPR.PRN.Backend.API.Controllers.ExporterJourney;
using EPR.PRN.Backend.API.Dto.ExporterJourney;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries.ExporterJourney;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers.ExporterJourney;

[TestClass]
public class OtherPermitsControllerTests
{
    private Mock<IMediator> _mediatorMock;
    private CarrierBrokerDealerPermitsController _controller;

    [TestInitialize]
    public void TestInitialize()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new CarrierBrokerDealerPermitsController(_mediatorMock.Object);
    }

    [TestMethod]
    public async Task GetOtherPermits_ReturnsNotFound_WhenResourceDoesNotExist()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<CarrierBrokerDealerPermitsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetCarrierBrokerDealerPermitsResultDto)null);

        // Act
        var result = await _controller.GetOtherPermits(Guid.Empty);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public async Task GetOtherPermits_ReturnsOk_WhenResourceExists()
    {
        // Arrange
        var expectedDto = new GetCarrierBrokerDealerPermitsResultDto
        {
            Id = Guid.NewGuid(),
            RegistrationId = Guid.NewGuid(),
            WasteLicenseOrPermitNumber = "test 1",
            PpcNumber = "test 2",
            WasteExemptionReference = new List<string> { "test 3", "test 4" }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<CarrierBrokerDealerPermitsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _controller.GetOtherPermits(expectedDto.RegistrationId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().BeEquivalentTo(expectedDto);
    }

    [TestMethod]
    public async Task CreateOtherPermits_ReturnsCreated_WhenResourceCreated()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCarrierBrokerDealerPermitsCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.CreateOtherPermits(Guid.Empty, Guid.Empty, new CreateCarrierBrokerDealerPermitsDto());

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [TestMethod]
    public async Task CreateOtherPermits_ReturnsOk_WhenResourceExists()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCarrierBrokerDealerPermitsCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.CreateOtherPermits(Guid.Empty, Guid.Empty, new CreateCarrierBrokerDealerPermitsDto());

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [TestMethod]
    public async Task CreateOtherPermits_ReturnsNotFound_WhenRegistrationDoesNotExist()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCarrierBrokerDealerPermitsCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _controller.CreateOtherPermits(Guid.Empty, Guid.Empty, new CreateCarrierBrokerDealerPermitsDto());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public async Task UpdateOtherPermits_ReturnsNotFound_WhenCarrierBrokerDealerPermitDoesNotExist()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateCarrierBrokerDealerPermitsCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _controller.UpdateOtherPermits(Guid.Empty, Guid.Empty, new UpdateCarrierBrokerDealerPermitsDto());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public async Task UpdateOtherPermits_ReturnsOk_WhenCarrierBrokerDealerPermitUpdated()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateCarrierBrokerDealerPermitsCommand>(), It.IsAny<CancellationToken>()));

        // Act
        var result = await _controller.UpdateOtherPermits(Guid.Empty, Guid.Empty, new UpdateCarrierBrokerDealerPermitsDto());

        // Assert
        result.Should().BeOfType<OkResult>();
    }
}
