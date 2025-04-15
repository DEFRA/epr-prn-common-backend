﻿using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Dto.Regulator;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Queries;

using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

[TestClass]
public class RegistrationMaterialControllerTests
{
    private Mock<IMediator> _mediatorMock;
    private Mock<IValidator<RegistrationMaterialsOutcomeCommand>> _validatorMock;
    private Mock<ILogger<RegistrationMaterialController>> _loggerMock;
    private RegistrationMaterialController _controller;

    [TestInitialize]
    public void TestInitialize()
    {
        _mediatorMock = new Mock<IMediator>();
        _validatorMock = new Mock<IValidator<RegistrationMaterialsOutcomeCommand>>();
        _loggerMock = new Mock<ILogger<RegistrationMaterialController>>();

        _controller = new RegistrationMaterialController(_mediatorMock.Object, _validatorMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task GetRegistrationOverviewDetailById_ReturnsOk_WithExpectedResult()
    {
        // Arrange
        int registrationId = 1;
        var expectedDto = new RegistrationOverviewDto
        {
            OrganisationName = "Test Organisation",
            Regulator = "Test Regulator"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetRegistrationOverviewDetailByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _controller.GetRegistrationOverviewDetailById(registrationId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(expectedDto);
    }

    [TestMethod]
    public async Task GetMaterialDetailById_ReturnsOk_WithExpectedResult()
    {
        // Arrange
        int materialId = 2;
        var expectedDto = new RegistrationMaterialDetailsDto();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetMaterialDetailByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _controller.GetMaterialDetailById(materialId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(expectedDto);
    }

    [TestMethod]
    public async Task UpdateRegistrationOutcome_ValidCommand_ReturnsNoContent()
    {
        // Arrange
        int materialId = 3;
        var command = new RegistrationMaterialsOutcomeCommand();

        _validatorMock
            .Setup(v => v.ValidateAsync(command, default))
            .ReturnsAsync(new ValidationResult());

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<RegistrationMaterialsOutcomeCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.UpdateRegistrationOutcome(materialId, command);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        command.Id.Should().Be(materialId);
    }

    [TestMethod]
    public async Task UpdateRegistrationOutcome_ShouldThrowValidationException_WhenValidationFails()
    {
        // Arrange
        var validator = new InlineValidator<RegistrationMaterialsOutcomeCommand>();
        validator.RuleFor(x => x.Status).Must(_ => false).WithMessage("Validation failed");

        var registrationId = 10;
        var command = new RegistrationMaterialsOutcomeCommand
        {
            Status = (RegistrationMaterialStatus)999
        };

        var controller = new RegistrationMaterialController(
            _mediatorMock.Object,
            validator,
            _loggerMock.Object
        );

        // Act & Assert
        await FluentActions.Invoking(() =>
            controller.UpdateRegistrationOutcome(registrationId, command)
        ).Should().ThrowAsync<ValidationException>();
    }
}
