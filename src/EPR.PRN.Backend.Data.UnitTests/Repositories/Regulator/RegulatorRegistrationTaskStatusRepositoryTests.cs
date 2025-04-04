using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using EPR.PRN.Backend.Data.Repositories.Regulator;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EPR.PRN.Backend.Data.Tests.Repositories.Regulator
{
    [TestClass]
    public class RegulatorRegistrationTaskStatusRepositoryTests
    {
        private Mock<EprRegistrationsContext> _contextMock;
        private Mock<ILogger<RegulatorRegistrationTaskStatusRepository>> _loggerMock;
        private RegulatorRegistrationTaskStatusRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _contextMock = new Mock<EprRegistrationsContext>();
            _loggerMock = new Mock<ILogger<RegulatorRegistrationTaskStatusRepository>>();
            _repository = new RegulatorRegistrationTaskStatusRepository(_contextMock.Object, _loggerMock.Object);
        }


        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenContextIsNull()
        {
            // Act
            Action act = () => new RegulatorRegistrationTaskStatusRepository(null, _loggerMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'context')");
        }

        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            // Act
            Action act = () => new RegulatorRegistrationTaskStatusRepository(_contextMock.Object, null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'logger')");
        }

        [TestMethod]
        public async Task GetTaskStatusByIdAsync_ShouldReturnTaskStatus_WhenTaskStatusExists()
        {
            // Arrange
            var taskStatus = new RegulatorRegistrationTaskStatus { Id = 1, TaskStatusId = (int)StatusTypes.Complete };
            _contextMock.Setup(c => c.RegulatorRegistrationTaskStatus.FindAsync(1))
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
            _contextMock.Setup(c => c.RegulatorRegistrationTaskStatus.FindAsync(1))
                .ReturnsAsync((RegulatorRegistrationTaskStatus)null);

            // Act
            var result = await _repository.GetTaskStatusByIdAsync(1);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldUpdateTaskStatus_WhenTaskStatusExists()
        {
            // Arrange
            var taskStatus = new RegulatorRegistrationTaskStatus { Id = 1, TaskStatusId = (int)StatusTypes.Queried };
            _contextMock.Setup(c => c.RegulatorRegistrationTaskStatus.FindAsync(1))
                .ReturnsAsync(taskStatus);
            _contextMock.Setup(c => c.SaveChangesAsync(default))
                .ReturnsAsync(1);

            // Act
            await _repository.UpdateStatusAsync(1, StatusTypes.Complete, "Updated comments");

            // Assert
            taskStatus.TaskStatusId.Should().Be((int)StatusTypes.Complete);
            taskStatus.Comments.Should().Be("Updated comments");
            _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowKeyNotFoundException_WhenTaskStatusDoesNotExist()
        {
            // Arrange
            _contextMock.Setup(c => c.RegulatorRegistrationTaskStatus.FindAsync(1))
                .ReturnsAsync((RegulatorRegistrationTaskStatus)null);

            // Act
            Func<Task> act = async () => await _repository.UpdateStatusAsync(1, StatusTypes.Complete, "Updated comments");

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Regulator Registration task status not found: 1");
        }
    }
}
