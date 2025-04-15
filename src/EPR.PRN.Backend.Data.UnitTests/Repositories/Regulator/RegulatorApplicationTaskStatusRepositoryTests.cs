using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Repositories.Regulator;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.Data.Tests.Repositories.Regulator
{
    [TestClass]
    public class RegulatorApplicationTaskStatusRepositoryTests
    {
        private Mock<EprRegistrationsContext> _contextMock;
        private Mock<ILogger<RegulatorApplicationTaskStatusRepository>> _loggerMock;
        private RegulatorApplicationTaskStatusRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _contextMock = new Mock<EprRegistrationsContext>();
            _loggerMock = new Mock<ILogger<RegulatorApplicationTaskStatusRepository>>();
            _repository = new RegulatorApplicationTaskStatusRepository(_contextMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenContextIsNull()
        {
            // Act
            Action act = () => new RegulatorApplicationTaskStatusRepository(null, _loggerMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'context')");
        }

        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            // Act
            Action act = () => new RegulatorApplicationTaskStatusRepository(_contextMock.Object, null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'logger')");
        }

        [TestMethod]
        public async Task GetTaskStatusByIdAsync_ShouldReturnTaskStatus_WhenTaskStatusExists()
        {
            // Arrange
            var taskStatus = new RegulatorApplicationTaskStatus { Id = 1, TaskStatusId = (int)StatusTypes.Completed };
                .ReturnsAsync(taskStatus);

            // Act
            var result = await _repository.GetTaskStatusByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(taskStatus);
        }

        [TestMethod]
        public async Task GetTaskStatusByIdAsync_ShouldReturnNull_WhenTaskStatusDoesNotExist()
        {
            // Arrange
            _contextMock.Setup(c => c.RegulatorApplicationTaskStatus.FindAsync(1))
                .ReturnsAsync((RegulatorApplicationTaskStatus)null);

            // Act
            Func<Task> act = async () => await _repository.GetTaskStatusByIdAsync(1);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Task status not found: 1");
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldUpdateTaskStatus_WhenTaskStatusExists()
        {
            // Arrange
            var taskStatus = new RegulatorApplicationTaskStatus { Id = 1, TaskStatusId = (int)StatusTypes.Queried };
            var registrationMaterialId = 1;
                .ReturnsAsync(taskStatus);
            _contextMock.Setup(c => c.SaveChangesAsync(default))
                .ReturnsAsync(1);

            // Act
            await _repository.UpdateStatusAsync(1, StatusTypes.Completed, "Updated comments");

            // Assert
            taskStatus.TaskStatusId.Should().Be((int)StatusTypes.Completed);
            taskStatus.Comments.Should().Be("Updated comments");
            _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowKeyNotFoundException_WhenTaskStatusDoesNotExist()
        {
            // Arrange
            _contextMock.Setup(c => c.RegulatorApplicationTaskStatus.FindAsync(1))
                .ReturnsAsync((RegulatorApplicationTaskStatus)null);

            // Act
            Func<Task> act = async () => await _repository.UpdateStatusAsync(1, StatusTypes.Completed, "Updated comments");

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Regulator application task status not found: 1");
        }
    }
}
