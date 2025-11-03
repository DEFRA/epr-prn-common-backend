using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Controllers.Regulator;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers.Regulator;

[TestClass]
public class RegistrationMaterialControllerTests
{
    private Mock<IMediator> _mediatorMock;
    private Mock<IValidator<RegistrationMaterialsOutcomeCommand>> _validatorMock;
    private Mock<IValidator<RegistrationMaterialsMarkAsDulyMadeCommand>> _validatorMockdual;
    private Mock<ILogger<RegistrationMaterialController>> _loggerMock;
    private RegistrationMaterialController _controller;

    [TestInitialize]
    public void TestInitialize()
    {
        _mediatorMock = new Mock<IMediator>();
        _validatorMock = new Mock<IValidator<RegistrationMaterialsOutcomeCommand>>();
        _validatorMockdual = new Mock<IValidator<RegistrationMaterialsMarkAsDulyMadeCommand>>();
        _loggerMock = new Mock<ILogger<RegistrationMaterialController>>();
        _controller = new RegistrationMaterialController(_mediatorMock.Object, _validatorMock.Object, _validatorMockdual.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task GetRegistrationOverviewDetailById_ReturnsOk_WithExpectedResult()
    {
        // Arrange
        var registrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");
        var expectedDto = new RegistrationOverviewDto
        {
            OrganisationId = Guid.NewGuid(),
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
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
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
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var command = new RegistrationMaterialsOutcomeCommand
        {
            RegistrationReferenceNumber = "R25ER2475638626AL" // Ensure the required property is set
        };


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

        var registrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");
        var command = new RegistrationMaterialsOutcomeCommand
        {
            RegistrationReferenceNumber = "R26ER2375628626PL" // Ensure the required property is set
        };

        var controller = new RegistrationMaterialController(
            _mediatorMock.Object,
            validator,
            _validatorMockdual.Object,
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
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var expectedDto = new RegistrationMaterialWasteLicencesDto
        {
            PermitType = "",
            LicenceNumbers = Array.Empty<string>(),
            MaterialName = "",
            MaximumReprocessingCapacityTonne = 1,
            MaximumReprocessingPeriod = "",
            RegistrationMaterialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"),
            RegistrationId = Guid.NewGuid() // Fix: Set the required RegistrationId property  
        };

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
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
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
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
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
        var registrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");


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
        var registrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");


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

    [TestMethod]
    public async Task GetMaterialPaymentFeeById_ReturnsOk_WithExpectedResult()
    {
        // Arrange
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var expectedDto = new MaterialPaymentFeeDto();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetMaterialPaymentFeeByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _controller.GetRegistrationMaterialpaymentFeesById(materialId);

        // Assert
        using (new AssertionScope())
        {
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(expectedDto);
        }
    }

    [TestMethod]
    public async Task MarkAsDulyMode_ShouldThrowValidationException_WhenValidationFails()
    {
        // Arrange
        var validator = new InlineValidator<RegistrationMaterialsMarkAsDulyMadeCommand>();
        validator.RuleFor(x => x.DeterminationDate).Must((model, determinationDate) =>
                determinationDate >= model.DulyMadeDate.AddDays(7 * 12))
            .WithMessage("DeterminationDate must be at least 12 weeks after DulyMadeDate.");

        var registrationMaterialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var command = new RegistrationMaterialsMarkAsDulyMadeCommand
        {
            DulyMadeDate = new DateTime(2025, 5, 12),
            DeterminationDate = new DateTime(2025, 7, 12),
            DulyMadeBy = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")
        };

        var controller = new RegistrationMaterialController(
            _mediatorMock.Object,
            _validatorMock.Object,
            validator,
            _loggerMock.Object
        );

        // Act & Assert
        await FluentActions.Invoking(() =>
            controller.RegistrationMaterialsMarkAsDulyMade(registrationMaterialId, command)
        ).Should().ThrowAsync<ValidationException>();
    }
    [TestMethod]
    public async Task GetRegistrationAccreditationReferenceById_ReturnsOk_WithExpectedResult()
    {
        // Arrange
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var expectedDto = new RegistrationAccreditationReferenceDto();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetRegistrationAccreditationReferenceByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _controller.GetRegistrationAccreditationReference(materialId);

        // Assert
        using (new AssertionScope())
        {
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(expectedDto);
        }
    }
    [TestMethod]
    public async Task GetRegistrationAccreditationReference_ReturnsOk_WithExpectedResult()
    {
        // Arrange
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var expectedDto = new RegistrationAccreditationReferenceDto();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetRegistrationAccreditationReferenceByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _controller.GetRegistrationAccreditationReference(materialId);

        // Assert
        using (new AssertionScope())
        {
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(expectedDto);
        }
    }
    [TestMethod]
    public async Task RegistrationMaterialsMarkAsDulyMade_ValidCommand_ReturnsNoContent()
    {
        // Arrange
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var command = new RegistrationMaterialsMarkAsDulyMadeCommand
        {
            DulyMadeDate = DateTime.UtcNow.AddMonths(-1),
            DeterminationDate = DateTime.UtcNow
        };

        _validatorMockdual
            .Setup(v => v.ValidateAsync(command, default))
            .ReturnsAsync(new ValidationResult());

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<RegistrationMaterialsMarkAsDulyMadeCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.RegistrationMaterialsMarkAsDulyMade(materialId, command);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<NoContentResult>();
            command.RegistrationMaterialId.Should().Be(materialId);
        }
    }
    [TestMethod]
    public async Task RegistrationMaterialsMarkAsDulyMade_ShouldThrowValidationException_WhenValidationFails()
    {
        // Arrange
        var validator = new InlineValidator<RegistrationMaterialsMarkAsDulyMadeCommand>();
        validator.RuleFor(x => x.DulyMadeDate).Must(_ => false).WithMessage("Validation failed");

        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var command = new RegistrationMaterialsMarkAsDulyMadeCommand
        {
            DulyMadeDate = DateTime.UtcNow,
            DeterminationDate = DateTime.UtcNow.AddMonths(1)
        };

        var controller = new RegistrationMaterialController(
            _mediatorMock.Object,
            _validatorMock.Object,
            validator,
            _loggerMock.Object
        );

        // Act & Assert
        await FluentActions.Invoking(() =>
            controller.RegistrationMaterialsMarkAsDulyMade(materialId, command)
        ).Should().ThrowAsync<ValidationException>();
    }
}
