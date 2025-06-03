using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
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

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

[TestClass]
public class RegulatorAccreditationControllerTests
{
    private Mock<IMediator> _mediatorMock;
    private Mock<IValidator<RegulatorAccreditationMarkAsDulyMadeCommand>> _validatorMock;
    private Mock<ILogger<RegulatorAccreditationController>> _loggerMock;
    private RegulatorAccreditationController _controller;

    [TestInitialize]
    public void TestInitialize()
    {
        _mediatorMock = new Mock<IMediator>();
        _validatorMock = new Mock<IValidator<RegulatorAccreditationMarkAsDulyMadeCommand>>();
        _loggerMock = new Mock<ILogger<RegulatorAccreditationController>>();
        _controller = new RegulatorAccreditationController(_mediatorMock.Object, _validatorMock.Object,  _loggerMock.Object);
    }

    [TestMethod]
    public async Task GetRegistrationAccreditationPaymentFeeDetailsById_ReturnsOk_WhenResultFound()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var dto = new AccreditationPaymentFeeDetailsDto
        {
            AccreditationId = accreditationId,
            OrganisationName = "Test Org",
            SiteAddress = "123 Test St, Testville, TS1 1AA",
            ApplicationReferenceNumber = "APP-001",
            MaterialName = "Plastic",
            SubmittedDate = DateTime.UtcNow,
            FeeAmount = 100.0m,
            ApplicationType = ApplicationOrganisationType.Reprocessor,
            Regulator = "EA"
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetRegistrationAccreditationPaymentFeesByIdQuery>(q => q.Id == accreditationId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        // Act
        var result = await _controller.GetRegistrationAccreditationPaymentFeeDetailsById(accreditationId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(dto);
    }

    [TestMethod]
    public async Task RegistrationAccreditationMarkAsDulyMade_ValidCommand_ReturnsNoContent()
    {
        // Arrange
        var accreditationId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var command = new RegulatorAccreditationMarkAsDulyMadeCommand
        {
            DulyMadeDate = DateTime.UtcNow.AddMonths(-1),
            DeterminationDate = DateTime.UtcNow
        };

        _validatorMock
            .Setup(v => v.ValidateAsync(command, default))
            .ReturnsAsync(new ValidationResult());

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<RegulatorAccreditationMarkAsDulyMadeCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.RegulatorAccreditationMarkAsDulyMade(accreditationId, command);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<NoContentResult>();
            command.Id.Should().Be(accreditationId);
        }
    }
    [TestMethod]
    public async Task RegistrationMaterialsMarkAsDulyMade_ShouldThrowValidationException_WhenValidationFails()
    {
        // Arrange
        var validator = new InlineValidator<RegulatorAccreditationMarkAsDulyMadeCommand>();
        validator.RuleFor(x => x.DulyMadeDate).Must(_ => false).WithMessage("Validation failed");

        var accreditationId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var command = new RegulatorAccreditationMarkAsDulyMadeCommand
        {
            DulyMadeDate = DateTime.UtcNow,
            DeterminationDate = DateTime.UtcNow.AddMonths(1)
        };

        var controller = new RegulatorAccreditationController(
            _mediatorMock.Object,
            validator,
            _loggerMock.Object
        );

        // Act & Assert
        await FluentActions.Invoking(() =>
            controller.RegulatorAccreditationMarkAsDulyMade(accreditationId, command)
        ).Should().ThrowAsync<ValidationException>();
    }

}
