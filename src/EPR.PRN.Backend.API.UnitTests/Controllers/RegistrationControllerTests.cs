using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DTO.Registration;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
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
        var registrationId = Guid.NewGuid();
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
            command.RegistrationId.Should().Be(registrationId);
        }
    }


    [TestMethod]
    public async Task GetRegistrationsOverviewForOrgId_NoRegistrations_ReturnsOkWithEmptyList()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var query = new GetRegistrationsOverviewByOrgIdQuery { OrganisationId = organisationId };
        var expectedResult = new List<RegistrationOverviewDto>();
        _validationServiceMock
            .Setup(v => v.ValidateAndThrowAsync(query, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mediatorMock
            .Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);
        // Act
        var result = await _controller.GetRegistrationsOverviewForOrgId(organisationId);
        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(expectedResult);
        }
    }

    [TestMethod]
    public async Task GetRegistrationsOverviewForOrgId_ValidOrganisationId_ReturnsOkResult()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var query = new GetRegistrationsOverviewByOrgIdQuery { OrganisationId = organisationId };
        var expectedResult = new List<RegistrationOverviewDto>
        {
            new RegistrationOverviewDto { Id = Guid.NewGuid() },
            new RegistrationOverviewDto { Id = Guid.NewGuid() }
        };
        _validationServiceMock
            .Setup(v => v.ValidateAndThrowAsync(It.IsAny<GetRegistrationsOverviewByOrgIdQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetRegistrationsOverviewByOrgIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);
        // Act
        var result = await _controller.GetRegistrationsOverviewForOrgId(organisationId);
        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(expectedResult);
        }
    }

    [TestMethod]
    public async Task GetRegistrationsOverviewForOrgId_InvalidOrganisationId_ThrowsValidationException()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var query = new GetRegistrationsOverviewByOrgIdQuery { OrganisationId = organisationId };
        _validationServiceMock
            .Setup(v => v.ValidateAndThrowAsync(It.IsAny<GetRegistrationsOverviewByOrgIdQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FluentValidation.ValidationException("Invalid organisation ID"));
        // Act
        Func<Task> act = async () => await _controller.GetRegistrationsOverviewForOrgId(organisationId);
        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .WithMessage("Invalid organisation ID");
    }


    [TestMethod]
    public async Task GetRegistrationsOverviewForOrgId_UnexpectedError_ThrowsException()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var query = new GetRegistrationsOverviewByOrgIdQuery { OrganisationId = organisationId };
        _validationServiceMock
            .Setup(v => v.ValidateAndThrowAsync(It.IsAny<GetRegistrationsOverviewByOrgIdQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetRegistrationsOverviewByOrgIdQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Unexpected error"));
        // Act
        Func<Task> act = async () => await _controller.GetRegistrationsOverviewForOrgId(organisationId);
        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Unexpected error");
    }

    [TestMethod]
    public async Task UpdateRegistrationTaskStatus_ValidCommand_ReturnsNoContent()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var command = new UpdateRegistrationTaskStatusCommand();

        _validationServiceMock
            .Setup(v => v.ValidateAndThrowAsync(It.IsAny<UpdateRegistrationTaskStatusCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateRegistrationTaskStatusCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.UpdateRegistrationTaskStatus(registrationId, command);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<NoContentResult>();
            command.RegistrationId.Should().Be(registrationId);
        }
    }

    [TestMethod]
    public async Task UpdateApplicationRegistrationTaskStatus_ValidCommand_ReturnsNoContent()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var command = new UpdateApplicationRegistrationTaskStatusCommand();

        _validationServiceMock
            .Setup(v => v.ValidateAndThrowAsync(It.IsAny<UpdateApplicationRegistrationTaskStatusCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateApplicationRegistrationTaskStatusCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.UpdateApplicationRegistrationTaskStatus(registrationId, command);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<NoContentResult>();
            command.RegistrationId.Should().Be(registrationId);
        }
    }


}
