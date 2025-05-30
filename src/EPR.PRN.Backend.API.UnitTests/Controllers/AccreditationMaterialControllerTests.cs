using EPR.PRN.Backend.API.Controllers.Regulator;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;

using FluentAssertions;
using FluentAssertions.Execution;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

[TestClass]
public class AccreditationMaterialControllerTests
{
    private Mock<IMediator> _mediatorMock;
    private Mock<ILogger<AccreditationController>> _loggerMock;
    private AccreditationController _controller;

    [TestInitialize]
    public void TestInitialize()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<AccreditationController>>();
        _controller = new AccreditationController(_mediatorMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task GetAccreditationOverviewDetailById_ReturnsOk_WithExpectedResult()
    {
        // Arrange
        Guid registrationId = Guid.NewGuid();
        int year = 1;
        var expectedDto = new RegistrationOverviewDto
        {
            OrganisationId = Guid.NewGuid(),
            Regulator = "Test Regulator"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetRegistrationOverviewDetailWithAccreditationsByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _controller.GetRegistrationByIdWithAccreditationsAsync(registrationId, year);

        // Assert
        using (new AssertionScope())
        {
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(expectedDto);
        }
    }
}