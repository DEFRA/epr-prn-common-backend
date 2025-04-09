using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.API.Tests.Handlers
{
    [TestClass]
    public class UpdateRegulatorRegistrationTaskHandlerTests
    {
        private Mock<IRegulatorRegistrationTaskStatusRepository> _repositoryMock;
        private Mock<ILogger<UpdateRegulatorRegistrationTaskHandler>> _loggerMock;
        private UpdateRegulatorRegistrationTaskHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _repositoryMock = new Mock<IRegulatorRegistrationTaskStatusRepository>();
            _loggerMock = new Mock<ILogger<UpdateRegulatorRegistrationTaskHandler>>();
            _handler = new UpdateRegulatorRegistrationTaskHandler(_repositoryMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public async Task Handle_TaskStatusAlreadyComplete_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorRegistrationTaskCommand { Id = 1, Status = StatusTypes.Complete };
            var taskStatus = new RegulatorRegistrationTaskStatus { TaskStatusId = (int)StatusTypes.Complete };
            _repositoryMock.Setup(r => r.GetTaskStatusByIdAsync(command.Id)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Cannot set task status to {StatusTypes.Complete} as it is already {StatusTypes.Complete}: {command.Id}");
        }

        [TestMethod]
        public async Task Handle_TaskStatusAlreadyQueried_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorRegistrationTaskCommand { Id = 1, Status = StatusTypes.Queried };
            var taskStatus = new RegulatorRegistrationTaskStatus { TaskStatusId = (int)StatusTypes.Queried };
            _repositoryMock.Setup(r => r.GetTaskStatusByIdAsync(command.Id)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Cannot set task status to {StatusTypes.Queried} as it is already {StatusTypes.Queried}: {command.Id}");
        }

        [TestMethod]
        public async Task Handle_TaskStatusCompleteToQueried_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorRegistrationTaskCommand { Id = 1, Status = StatusTypes.Queried };
            var taskStatus = new RegulatorRegistrationTaskStatus { TaskStatusId = (int)StatusTypes.Complete };
            _repositoryMock.Setup(r => r.GetTaskStatusByIdAsync(command.Id)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Cannot set task status to {StatusTypes.Queried} as it is {StatusTypes.Complete}: {command.Id}");
        }

        [TestMethod]
        public async Task Handle_InvalidStatusType_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorRegistrationTaskCommand { Id = 1, Status = (StatusTypes)999 };
            var taskStatus = new RegulatorRegistrationTaskStatus { TaskStatusId = 1 };
            _repositoryMock.Setup(r => r.GetTaskStatusByIdAsync(command.Id)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Invalid status type: {command.Status}");
        }

        [TestMethod]
        public async Task Handle_ValidStatusUpdate_ShouldUpdateStatus()
        {
            // Arrange
            var command = new UpdateRegulatorRegistrationTaskCommand { Id = 1, Status = StatusTypes.Complete, Comment = "Completed" };
            var taskStatus = new RegulatorRegistrationTaskStatus { TaskStatusId = 1 };
            _repositoryMock.Setup(r => r.GetTaskStatusByIdAsync(command.Id)).ReturnsAsync(taskStatus);
            _repositoryMock.Setup(r => r.UpdateStatusAsync(command.Id, command.Status, command.Comment)).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            _repositoryMock.Verify(r => r.UpdateStatusAsync(command.Id, command.Status, command.Comment), Times.Once);
        }
    }
}
