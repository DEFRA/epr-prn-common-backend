using System.Net;
using AutoFixture;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Controllers.Accreditation;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;


namespace EPR.PRN.Backend.API.UnitTests.Controllers
{
    [TestClass]
    public class AccreditationTaskStatusControllerTests
    {
        private AccreditationTaskStatusController _systemUnderTest;
        private Mock<IMediator> _mockMediator;
        private Mock<ILogger<AccreditationTaskStatusController>> _mockLogger;
        private Mock<IValidator<UpdateAccreditationTaskCommand>> _validatorMock;
        private static readonly AutoFixture.Fixture _fixture = new Fixture();

        [TestInitialize]
        public void TestInitialize()
        {
            _mockMediator = new Mock<IMediator>();
            _mockLogger = new Mock<ILogger<AccreditationTaskStatusController>>();
            _validatorMock = new Mock<IValidator<UpdateAccreditationTaskCommand>>();

            _systemUnderTest = new AccreditationTaskStatusController(
                _mockMediator.Object,
                _validatorMock.Object,
                _mockLogger.Object
            );
        }

        [TestMethod]
        public async Task UpdateAccreditationTaskStatus_ReturnsNoContent_WhenValidCommand()
        {
            // Arrange
            var command = _fixture.Create<UpdateAccreditationTaskCommand>();

            var validationResult = new ValidationResult();
            _validatorMock.Setup(x => x.Validate(It.IsAny<UpdateAccreditationTaskCommand>())).Returns(validationResult);


            _mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _systemUnderTest.UpdateAccreditationTaskStatus(command);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            (result as NoContentResult).StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [TestMethod]
        public async Task UpdateAccreditationTaskStatus_ThrowsValidationException_WhenValidationFails()
        {
            // Arrange
            var validator = new InlineValidator<UpdateAccreditationTaskCommand>();
            validator.RuleFor(x => x.Status).Must(_ => false).WithMessage("Validation failed");

            _systemUnderTest = new AccreditationTaskStatusController(
                _mockMediator.Object,
                validator,
                _mockLogger.Object
            );

            var command = _fixture.Build<UpdateAccreditationTaskCommand>()
                .With(x => x.Status, (TaskStatuses)999)
                .Create();

            // Act & Assert
            await FluentActions.Invoking(() =>
                _systemUnderTest.UpdateAccreditationTaskStatus(command)
            ).Should().ThrowAsync<ValidationException>();
        }

        [TestMethod]
        public async Task UpdateAccreditationTaskStatus_ThrowsException_WhenMediatorThrowsException()
        {
            // Arrange
            var command = _fixture.Create<UpdateAccreditationTaskCommand>();

            var validationResult = new ValidationResult();
            _validatorMock.Setup(x => x.Validate(It.IsAny<UpdateAccreditationTaskCommand>())).Returns(validationResult);

            _mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            Func<Task> act = async () => await _systemUnderTest.UpdateAccreditationTaskStatus(command);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Test exception");
        }
    }
}
