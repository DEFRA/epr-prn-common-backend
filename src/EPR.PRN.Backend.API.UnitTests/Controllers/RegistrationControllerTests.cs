using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Services.Interfaces;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

[TestClass]
public class RegistrationControllerTests
{
    private Mock<IMediator> _mediatorMock;
    private Mock<IValidationService> _validationServiceMock;
    private Mock<ILogger<RegistrationController>> _loggerMock;
    private RegistrationController _controller;

    [TestInitialize]
    public void TestInitialize()
    {
        _mediatorMock = new Mock<IMediator>();
        _validationServiceMock = new Mock<IValidationService>();
        _loggerMock = new Mock<ILogger<RegistrationController>>();

        _controller = new RegistrationController(_mediatorMock.Object, _validationServiceMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task UpdateRegistrationSiteAddress_ValidCommand_ReturnsNoContent()
    {
        // Arrange
        int registrationId = 1;
        var command = new UpdateRegistrationSiteAddressCommand();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateRegistrationSiteAddressCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.UpdateSiteAddress(registrationId, command);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<NoContentResult>();
            command.Id.Should().Be(registrationId);
        }
    }
}
