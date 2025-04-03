using System;
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
        private Mock<ILogger<RegulatorRegistrationTaskStatusRepository>> _loggerMock;
        private EprRegistrationsContext _context;
        private RegulatorRegistrationTaskStatusRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EprRegistrationsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new EprRegistrationsContext(options);
            _loggerMock = new Mock<ILogger<RegulatorRegistrationTaskStatusRepository>>();
            _repository = new RegulatorRegistrationTaskStatusRepository(_context, _loggerMock.Object);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldUpdateStatus_WhenTaskExists()
        {
            // Arrange
            var taskStatus = new RegulatorRegistrationTaskStatus
            {
                Id = 1,
                TaskStatusId = (int)StatusTypes.Queried
            };
            _context.RegulatorRegistrationTaskStatus.Add(taskStatus);
            await _context.SaveChangesAsync();

            // Act
            await _repository.UpdateStatusAsync(1, StatusTypes.Complete, "Completed");

            // Assert
            var updatedTaskStatus = await _context.RegulatorRegistrationTaskStatus.FindAsync(1);
            updatedTaskStatus.TaskStatusId.Should().Be((int)StatusTypes.Complete);
            updatedTaskStatus.Comments.Should().Be("Completed");
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowKeyNotFoundException_WhenTaskDoesNotExist()
        {
            // Act
            Func<Task> act = async () => await _repository.UpdateStatusAsync(99, StatusTypes.Complete, "Completed");

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Regulator Registration task status not found: 99");
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowInvalidOperationException_WhenStatusIsAlreadyComplete()
        {
            // Arrange
            var taskStatus = new RegulatorRegistrationTaskStatus
            {
                Id = 1,
                TaskStatusId = (int)StatusTypes.Complete
            };
            _context.RegulatorRegistrationTaskStatus.Add(taskStatus);
            await _context.SaveChangesAsync();

            // Act
            Func<Task> act = async () => await _repository.UpdateStatusAsync(1, StatusTypes.Complete, "Completed");

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Cannot set task status to Complete as it is already Complete: 1");
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowInvalidOperationException_WhenStatusIsAlreadyQueried()
        {
            // Arrange
            var taskStatus = new RegulatorRegistrationTaskStatus
            {
                Id = 1,
                TaskStatusId = (int)StatusTypes.Queried
            };
            _context.RegulatorRegistrationTaskStatus.Add(taskStatus);
            await _context.SaveChangesAsync();

            // Act
            Func<Task> act = async () => await _repository.UpdateStatusAsync(1, StatusTypes.Queried, "Queried");

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Cannot set task status to Queried as it is already Queried: 1");
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowInvalidOperationException_WhenSettingQueriedStatusToComplete()
        {
            // Arrange
            var taskStatus = new RegulatorRegistrationTaskStatus
            {
                Id = 1,
                TaskStatusId = (int)StatusTypes.Complete
            };
            _context.RegulatorRegistrationTaskStatus.Add(taskStatus);
            await _context.SaveChangesAsync();

            // Act
            Func<Task> act = async () => await _repository.UpdateStatusAsync(1, StatusTypes.Queried, "Queried");

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Cannot set task status to Queried as it is Complete: 1");
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowInvalidOperationException_WhenStatusIsInvalid()
        {           
            // Arrange
            var taskStatus = new RegulatorRegistrationTaskStatus
            {
                Id = 1,
                TaskStatusId = (int)StatusTypes.Complete
            };
            _context.RegulatorRegistrationTaskStatus.Add(taskStatus);
            await _context.SaveChangesAsync();

            // Act
            Func<Task> act = async () => await _repository.UpdateStatusAsync(1, (StatusTypes)999, "Invalid");

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Invalid status type: 999");
        }
    }
}
