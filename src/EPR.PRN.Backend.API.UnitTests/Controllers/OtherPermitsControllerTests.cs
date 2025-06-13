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
}
