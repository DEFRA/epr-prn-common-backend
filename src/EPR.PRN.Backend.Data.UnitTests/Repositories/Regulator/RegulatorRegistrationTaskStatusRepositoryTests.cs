using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Repositories.Regulator;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories.Regulator
{
    [TestClass]
    public class RegulatorRegistrationTaskStatusRepositoryTests
    {
        private Mock<ILogger<RegulatorRegistrationTaskStatusRepository>> _loggerMock;
        private EprContext _context;
        private RegulatorRegistrationTaskStatusRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EprContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new EprContext(options);
            _loggerMock = new Mock<ILogger<RegulatorRegistrationTaskStatusRepository>>();
            _repository = new RegulatorRegistrationTaskStatusRepository(_context, _loggerMock.Object);
        }

        [TestMethod]
        public async Task GetTaskStatusAsync_ShouldReturnTaskStatus_WhenTaskExists()
        {
            // Arrange
            var taskName = "TestTask";
            var registrationId = 1;

            var taskStatus = new RegulatorRegistrationTaskStatus
            {
                Task = new LookupRegulatorTask { Name = taskName },
                RegistrationId = registrationId
            };

            _context.RegulatorRegistrationTaskStatus.Add(taskStatus);
            _context.SaveChanges();

            // Act
            var result = await _repository.GetTaskStatusAsync(taskName, registrationId);

            // Assert
            result.Should().NotBeNull();
            result!.Task.Name.Should().Be(taskName);
            result.RegistrationId.Should().Be(registrationId);
        }

        [TestMethod]
        public async Task GetTaskStatusAsync_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskName = "NonExistentTask";
            var registrationId = 1;

            // Act
            var result = await _repository.GetTaskStatusAsync(taskName, registrationId);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldAddNewTaskStatus_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskName = "NewTask";
            var registrationId = 1;
            var status = RegulatorTaskStatus.Started;
            var comments = "Task started";
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });

            _context.LookupTasks.Add(new LookupRegulatorTask { Id = 1, Name = taskName, IsMaterialSpecific = false, ApplicationTypeId = 1 });

            _context.Registrations.Add(new Registration { Id = registrationId, ExternalId=Guid.NewGuid(), ApplicationTypeId = 1 });

            _context.SaveChanges();

            // Act
            await _repository.UpdateStatusAsync(taskName, registrationId, status, comments, user);

            // Assert
            var taskStatus = _context.RegulatorRegistrationTaskStatus.FirstOrDefault();

            taskStatus.Should().NotBeNull();
            taskStatus.Task.Name.Should().Be(taskName);
            taskStatus.RegistrationId.Should().Be(registrationId);
            taskStatus.TaskStatus.Name.Should().Be(status.ToString());
            taskStatus.Comments.Should().Be(comments);
            taskStatus.StatusCreatedBy.Should().Be(user);
            taskStatus.StatusUpdatedBy.Should().Be(user);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldUpdateExistingTaskStatus_WhenTaskExists()
        {
            // Arrange
            var taskName = "ExistingTask";
            var registrationId = 1;
            var existingStatus = RegulatorTaskStatus.Queried;
            var status = RegulatorTaskStatus.Completed;
            var comments = "Task completed";
            var existingUser = Guid.NewGuid();
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = status.ToString() });

            _context.RegulatorRegistrationTaskStatus.Add(new RegulatorRegistrationTaskStatus
            {
                Task = new LookupRegulatorTask { Name = taskName },
                RegistrationId = registrationId,
                TaskStatus = new LookupTaskStatus { Name = existingStatus.ToString() },
                Comments = "Task started",
                StatusCreatedBy = existingUser,
                StatusUpdatedBy = existingUser
            });

            _context.Registrations.Add(new Registration { Id = registrationId, ExternalId = Guid.NewGuid(), ApplicationTypeId = 1 });

            _context.SaveChanges();

            // Act
            await _repository.UpdateStatusAsync(taskName, registrationId, status, comments, user);

            // Assert
            var taskStatus = _context.RegulatorRegistrationTaskStatus.FirstOrDefault();

            taskStatus.Should().NotBeNull();
            taskStatus.Task.Name.Should().Be(taskName);
            taskStatus.RegistrationId.Should().Be(registrationId);
            taskStatus.TaskStatus.Name.Should().Be(status.ToString());
            taskStatus.Comments.Should().Be(comments);
            taskStatus.StatusCreatedBy.Should().Be(existingUser);
            taskStatus.StatusUpdatedBy.Should().Be(user);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowKeyNotFoundException_WhenRegistrationMaterialNotFound()
        {
            // Arrange
            var taskName = "NewTask";
            var RegistrationId = 1;
            var status = RegulatorTaskStatus.Started;
            var comments = "Task started";
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });

            _context.LookupTasks.Add(new LookupRegulatorTask { Id = 1, Name = taskName, IsMaterialSpecific = true, ApplicationTypeId = 1 });

            _context.SaveChanges();

            // Act
            Func<Task> act = async () => await _repository.UpdateStatusAsync(taskName, RegistrationId, status, comments, user);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowRegulatorInvalidOperationException_WhenTaskNotFound()
        {
            // Arrange
            var taskName = "NewTask";
            var RegistrationId = 1;
            var status = RegulatorTaskStatus.Started;
            var comments = "Task started";
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });

            _context.Registrations.Add(new Registration { Id = RegistrationId, ExternalId = Guid.NewGuid(), ApplicationTypeId = 1 });

            _context.SaveChanges();

            // Act
            Func<Task> act = async () => await _repository.UpdateStatusAsync(taskName, RegistrationId, status, comments, user);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>();
        }
    }
}
