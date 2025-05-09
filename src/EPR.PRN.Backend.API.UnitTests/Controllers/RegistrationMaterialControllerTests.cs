using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;

using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EPR.PRN.Backend.API.Dto;

using Moq;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.API.Handlers;

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
        using (new AssertionScope())
        {
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(expectedDto);
        }
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
        using (new AssertionScope())
        {
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(expectedDto);
        }
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
        using (new AssertionScope())
        {
            result.Should().BeOfType<NoContentResult>();
            command.Id.Should().Be(materialId);
        }
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

    [TestMethod]
    public async Task GetWasteLicences_ReturnsOk_WithExpectedResult()
    {
        // Arrange
        int materialId = 2;
        var expectedDto = new RegistrationMaterialWasteLicencesDto() { PermitType = "", LicenceNumbers = [], MaterialName = "", MaximumReprocessingCapacityTonne = 1,  MaximumReprocessingPeriod = "", RegistrationMaterialId = 1  };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetMaterialWasteLicencesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _controller.GetWasteLicences(materialId);

        // Assert
        using (new AssertionScope())
        {
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(expectedDto);
        }
    }

    [TestMethod]
    public async Task GetSamplingPlan_ReturnsOk_WithExpectedResult()
    {
        // Arrange
        int materialId = 2;
        var expectedDto = new RegistrationMaterialSamplingPlanDto() { MaterialName = "" };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetMaterialSamplingPlanQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _controller.GetSamplingPlan(materialId);

        // Assert
        using (new AssertionScope())
        {
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(expectedDto);
        }
    }

    [TestMethod]
    public async Task GetreprocessingIO_ReturnsOk_WithExpectedResult()
    {
        // Arrange
        int materialId = 2;
        var expectedDto = new RegistrationMaterialReprocessingIODto() { MaterialName = "", SourcesOfPackagingWaste = "", PlantEquipmentUsed = "" };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetMaterialReprocessingIOQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _controller.GetReprocessingIO(materialId);

        // Assert
        using (new AssertionScope())
        {
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(expectedDto);
        }
    }

    [TestMethod]
    public async Task GetSiteAddressByRegistrationId_ReturnsOk_WithExpectedResult()
    {
        // Arrange
        int registrationId = 2;

       
        var expectedDto = new RegistrationSiteAddressDto();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetRegistrationSiteAddressByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _controller.GetRegistrationSiteAddressById(registrationId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(expectedDto);
    }
    [TestMethod]
    public async Task GetMaterialsAuthorisedOnSiteId_ReturnsOk_WithExpectedResult()
    {
        // Arrange
        int registrationId = 2;


        var expectedDto = new MaterialsAuthorisedOnSiteDto();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetMaterialsAuthorisedOnSiteByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _controller.GetAuthorisedMaterial(registrationId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(expectedDto);
    }



}
