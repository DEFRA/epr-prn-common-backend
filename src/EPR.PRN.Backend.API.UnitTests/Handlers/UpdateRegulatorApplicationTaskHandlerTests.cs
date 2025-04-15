using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using EPR.PRN.Backend.API.Helpers;
using FluentAssertions;
using MediatR;
using Moq;
using EPR.PRN.Backend.API.Commands;

namespace EPR.PRN.Backend.API.Tests.Handlers
{
    [TestClass]
    public class UpdateRegulatorApplicationTaskHandlerTests
    {
        private Mock<IRegulatorApplicationTaskStatusRepository> _repositoryMock;
        private UpdateRegulatorApplicationTaskHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _repositoryMock = new Mock<IRegulatorApplicationTaskStatusRepository>();
            _handler = new UpdateRegulatorApplicationTaskHandler(_repositoryMock.Object);
        }

        [TestMethod]
        public async Task Handle_TaskStatusAlreadyComplete_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorApplicationTaskCommand { TaskName = "Test Task", registrationMaterialId = 1, Status = StatusTypes.Completed, UserName = "UserName" };
            var taskStatus = new RegulatorApplicationTaskStatus { TaskStatus = new LookupTaskStatus { Name = StatusTypes.Completed.ToString() } };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Cannot set task status to {StatusTypes.Completed} as it is already {StatusTypes.Completed}: {command.TaskName}:{command.TypeId}");
        }
        [TestMethod]
        public async Task Handle_TaskStatusUnrecognised_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorApplicationTaskCommand { TaskName = "Test Task", registrationMaterialId = 1, Status = StatusTypes.Queried, UserName = "UserName" };
            var taskStatus = new RegulatorApplicationTaskStatus { TaskStatus = new LookupTaskStatus { Name = StatusTypes.NotStarted.ToString() } };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Cannot set task status to {StatusTypes.Queried} as it is NotStarted: {command.TaskName}:{command.TypeId}");
        }

        [TestMethod]
        public async Task Handle_TaskStatusAlreadyQueried_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorApplicationTaskCommand { TaskName = "Test Task", registrationMaterialId = 1, Status = StatusTypes.Queried, UserName = "UserName" };
            var taskStatus = new RegulatorApplicationTaskStatus { TaskStatus = new LookupTaskStatus { Name = StatusTypes.Queried.ToString() } };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Cannot set task status to {StatusTypes.Queried} as it is already {StatusTypes.Queried}: {command.TaskName}:{command.TypeId}");
        }

        [TestMethod]
        public async Task Handle_TaskStatusCompleteToQueried_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorApplicationTaskCommand { TaskName = "Test Task", registrationMaterialId = 1, Status = StatusTypes.Queried, UserName = "UserName" };
            var taskStatus = new RegulatorApplicationTaskStatus { TaskStatus = new LookupTaskStatus { Name = StatusTypes.Completed.ToString() } };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Cannot set task status to {StatusTypes.Queried} as it is {StatusTypes.Completed}: {command.TaskName}:{command.TypeId}");
        }

        [TestMethod]
        public async Task Handle_InvalidStatusType_ShouldThrowRegulatorInvalidOperationException()
        {
            // Arrange
            var command = new UpdateRegulatorApplicationTaskCommand { TaskName = "Test Task", registrationMaterialId = 1, Status = (StatusTypes)999, UserName = "UserName" };
            var taskStatus = new RegulatorApplicationTaskStatus { TaskStatus = new LookupTaskStatus { Name = StatusTypes.Completed.ToString() } };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>().WithMessage($"Invalid status type: {command.Status}");
        }

        [TestMethod]
        public async Task Handle_ValidStatusUpdate_ShouldUpdateStatus()
        {
            // Arrange
            var command = new UpdateRegulatorApplicationTaskCommand { TaskName = "Test Task", registrationMaterialId = 1, Status = StatusTypes.Completed, Comment = "Completed", UserName = "UserName" };
            var taskStatus = new RegulatorApplicationTaskStatus { TaskStatus = new LookupTaskStatus { Name = StatusTypes.Queried.ToString() } };
            _repositoryMock.Setup(r => r.GetTaskStatusAsync(command.TaskName, command.TypeId)).ReturnsAsync(taskStatus);
            _repositoryMock.Setup(r => r.UpdateStatusAsync(command.TaskName, command.TypeId, command.Status, command.Comment, command.UserName)).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            _repositoryMock.Verify(r => r.UpdateStatusAsync(command.TaskName, command.TypeId, command.Status, command.Comment, command.UserName), Times.Once);
        }
    }
}
