using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Commands.ExporterJourney;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Controllers.ExporterJourney;
using EPR.PRN.Backend.API.Controllers.Regulator;
using EPR.PRN.Backend.API.Dto.ExporterJourney;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries.ExporterJourney;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers.ExporterJourney;

[TestClass]
public class OtherPermitsControllerTests
{
    private Mock<IMediator> _mediatorMock;
    private CarrierBrokerDealerPermitsController _controller;
    private Mock<IValidator<UpdateCarrierBrokerDealerPermitsCommand>> _validatorMock;


    [TestInitialize]
    public void TestInitialize()
    {
        _mediatorMock = new Mock<IMediator>();
        _validatorMock = new();

        _controller = new CarrierBrokerDealerPermitsController(_mediatorMock.Object, _validatorMock.Object);
    }

    [TestMethod]
    public async Task GetCarrierBrokerDealerPermits_ReturnsNotFound_WhenResourceDoesNotExist()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<CarrierBrokerDealerPermitsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetCarrierBrokerDealerPermitsResultDto)null);

        // Act
        var result = await _controller.GetCarrierBrokerDealerPermits(Guid.Empty);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public async Task GetCarrierBrokerDealerPermits_ReturnsOk_WhenResourceExists()
    {
        // Arrange
        var expectedDto = new GetCarrierBrokerDealerPermitsResultDto
        {
            WasteCarrierBrokerDealerRegistration = "test 1",
            CarrierBrokerDealerPermitId = Guid.NewGuid(),
            RegistrationId = Guid.NewGuid(),
            WasteLicenseOrPermitNumber = "test 1",
            PpcNumber = "test 2",
            WasteExemptionReference = new List<string> { "test 3", "test 4" }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<CarrierBrokerDealerPermitsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _controller.GetCarrierBrokerDealerPermits(expectedDto.RegistrationId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().BeEquivalentTo(expectedDto);
    }

    [TestMethod]
    public async Task CreateCarrierBrokerDealerPermits_ReturnsCreated_WhenResourceCreated()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCarrierBrokerDealerPermitsCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var dto = new CreateCarrierBrokerDealerPermitsDto() { WasteCarrierBrokerDealerRegistration = "test 1" };
        var result = await _controller.CreateCarrierBrokerDealerPermits(Guid.Empty, Guid.Empty, dto);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [TestMethod]
    public async Task CreateCarrierBrokerDealerPermits_ReturnsOk_WhenResourceExists()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCarrierBrokerDealerPermitsCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var carrierDealerBrokerPermitsDto = new CreateCarrierBrokerDealerPermitsDto() { WasteCarrierBrokerDealerRegistration = "test 1" };
        var result = await _controller.CreateCarrierBrokerDealerPermits(Guid.Empty, Guid.Empty, carrierDealerBrokerPermitsDto);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [TestMethod]
    public async Task CreateCarrierBrokerDealerPermits_ReturnsNotFound_WhenRegistrationDoesNotExist()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCarrierBrokerDealerPermitsCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new KeyNotFoundException());

        // Act
        var dto = new CreateCarrierBrokerDealerPermitsDto() { WasteCarrierBrokerDealerRegistration = "test 1" };
        var result = await _controller.CreateCarrierBrokerDealerPermits(Guid.Empty, Guid.Empty, dto);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public async Task UpdateCarrierBrokerDealerPermits_ReturnsNotFound_WhenCarrierBrokerDealerPermitDoesNotExist()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateCarrierBrokerDealerPermitsCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new KeyNotFoundException());

        var validationResult = new ValidationResult();
        _validatorMock.Setup(x => x.Validate(It.IsAny<UpdateCarrierBrokerDealerPermitsCommand>())).Returns(validationResult);

        // Act
        var result = await _controller.UpdateCarrierBrokerDealerPermits(Guid.Empty, Guid.Empty, new UpdateCarrierBrokerDealerPermitsDto());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public async Task UpdateCarrierBrokerDealerPermits_ReturnsOk_WhenCarrierBrokerDealerPermitUpdated()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateCarrierBrokerDealerPermitsCommand>(), It.IsAny<CancellationToken>()));

        var validationResult = new ValidationResult();
        _validatorMock.Setup(x => x.Validate(It.IsAny<UpdateCarrierBrokerDealerPermitsCommand>())).Returns(validationResult);

        // Act
        var result = await _controller.UpdateCarrierBrokerDealerPermits(Guid.Empty, Guid.Empty, new UpdateCarrierBrokerDealerPermitsDto());

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [TestMethod]
    public async Task UpdateCarrierBrokerDealerPermits_ReturnsBadRequest_WhenCarrierBrokerDealerPermitDtoIsInvalidUpdated()
    {
        // Arrange
        var validator = new InlineValidator<UpdateCarrierBrokerDealerPermitsCommand>();
        validator.RuleFor(x => x.Dto.PpcNumber).Must(_ => false).WithMessage("Validation failed");
        var userId = Guid.NewGuid();

        var _systemUnderTest = new CarrierBrokerDealerPermitsController(_mediatorMock.Object, validator);

        var registrationId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var requestDto = new UpdateCarrierBrokerDealerPermitsDto
        {
            PpcNumber = "xxxxxxxxxxxxxxxxxxxxxxxx", 
            WasteCarrierBrokerDealerRegistration = "abc", 
            WasteLicenseOrPermitNumber = "abc", 
            WasteExemptionReference = new List<string>()
        };

        // Act & Assert
        await FluentActions.Invoking(() =>
            _systemUnderTest.UpdateCarrierBrokerDealerPermits(registrationId, userId, requestDto)
        ).Should().ThrowAsync<ValidationException>();
    }

}
