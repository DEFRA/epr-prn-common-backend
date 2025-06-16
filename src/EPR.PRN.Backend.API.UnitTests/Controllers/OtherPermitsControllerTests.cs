using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Controllers.Regulator;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

[TestClass]
public class OtherPermitsControllerTests
{
    private Mock<IMediator> _mediatorMock;
    private OtherPermitsController _controller;

    [TestInitialize]
    public void TestInitialize()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new OtherPermitsController(_mediatorMock.Object);
    }

    [TestMethod]
    public async Task GetOtherPermits_ReturnsNotFound_WhenResourceDoesNotExist()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetOtherPermitsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetOtherPermitsResultDto)null);

        // Act
        var result = await _controller.GetOtherPermits(Guid.Empty);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public async Task GetOtherPermits_ReturnsOk_WhenResourceExists()
    {
        // Arrange
        var expectedDto = new GetOtherPermitsResultDto
        {
            Id = Guid.NewGuid(),
            RegistrationId = Guid.NewGuid(),
            WasteLicenseOrPermitNumber = "test 1",
            PpcNumber = "test 2",
            WasteExemptionReference = new List<string> { "test 3", "test 4" }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetOtherPermitsQuery>(), It.IsAny<CancellationToken>()))
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
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateOtherPermitsCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.CreateOtherPermits(Guid.Empty, Guid.Empty, new CreateOtherPermitsDto());

        // Assert
        result.Should().BeOfType<CreatedResult>();
    }

    [TestMethod]
    public async Task CreateOtherPermits_ReturnsOk_WhenResourceExists()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateOtherPermitsCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.CreateOtherPermits(Guid.Empty, Guid.Empty, new CreateOtherPermitsDto());

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [TestMethod]
    public async Task CreateOtherPermits_ReturnsNotFound_WhenRegistrationDoesNotExist()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateOtherPermitsCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _controller.CreateOtherPermits(Guid.Empty, Guid.Empty, new CreateOtherPermitsDto());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public async Task UpdateOtherPermits_ReturnsNotFound_WhenCarrierBrokerDealerPermitDoesNotExist()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateOtherPermitsCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _controller.UpdateOtherPermits(Guid.Empty, Guid.Empty, new UpdateOtherPermitsDto());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public async Task UpdateOtherPermits_ReturnsOk_WhenCarrierBrokerDealerPermitUpdated()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateOtherPermitsCommand>(), It.IsAny<CancellationToken>()));

        // Act
        var result = await _controller.UpdateOtherPermits(Guid.Empty, Guid.Empty, new UpdateOtherPermitsDto());

        // Assert
        result.Should().BeOfType<OkResult>();
    }
}
