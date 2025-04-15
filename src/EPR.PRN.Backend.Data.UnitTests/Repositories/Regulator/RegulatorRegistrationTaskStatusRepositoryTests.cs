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
            var status = StatusTypes.Started;
            var comments = "Task started";
            var userName = "userName";

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });

            _context.LookupTasks.Add(new LookupRegulatorTask { Id = 1, Name = taskName, IsMaterialSpecific = false, ApplicationTypeId = 1 });

            _context.Registrations.Add(new Registration { Id = registrationId, ExternalId="", ApplicationTypeId = 1 });

            _context.SaveChanges();

            // Act
            await _repository.UpdateStatusAsync(taskName, registrationId, status, comments, userName);

            // Assert
            var taskStatus = _context.RegulatorRegistrationTaskStatus.FirstOrDefault();

            taskStatus.Should().NotBeNull();
            taskStatus.Task.Name.Should().Be(taskName);
            taskStatus.RegistrationId.Should().Be(registrationId);
            taskStatus.TaskStatus.Name.Should().Be(status.ToString());
            taskStatus.Comments.Should().Be(comments);
            taskStatus.StatusCreatedBy.Should().Be(userName);
            taskStatus.StatusUpdatedBy.Should().Be(userName);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldUpdateExistingTaskStatus_WhenTaskExists()
        {
            // Arrange
            var taskName = "ExistingTask";
            var registrationId = 1;
            var existingStatus = StatusTypes.Queried;
            var status = StatusTypes.Completed;
            var comments = "Task completed";
            var existingUserName = "Existing userName";
            var userName = "userName";

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = status.ToString() });

            _context.RegulatorRegistrationTaskStatus.Add(new RegulatorRegistrationTaskStatus
            {
                Task = new LookupRegulatorTask { Name = taskName },
                RegistrationId = registrationId,
                TaskStatus = new LookupTaskStatus { Name = existingStatus.ToString() },
                Comments = "Task started",
                StatusCreatedBy = existingUserName,
                StatusUpdatedBy = existingUserName
            });

            _context.Registrations.Add(new Registration { Id = registrationId, ExternalId = "", ApplicationTypeId = 1 });

            _context.SaveChanges();

            // Act
            await _repository.UpdateStatusAsync(taskName, registrationId, status, comments, userName);

            // Assert
            var taskStatus = _context.RegulatorRegistrationTaskStatus.FirstOrDefault();

            taskStatus.Should().NotBeNull();
            taskStatus.Task.Name.Should().Be(taskName);
            taskStatus.RegistrationId.Should().Be(registrationId);
            taskStatus.TaskStatus.Name.Should().Be(status.ToString());
            taskStatus.Comments.Should().Be(comments);
            taskStatus.StatusCreatedBy.Should().Be(existingUserName);
            taskStatus.StatusUpdatedBy.Should().Be(userName);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowKeyNotFoundException_WhenRegistrationMaterialNotFound()
        {
            // Arrange
            var taskName = "NewTask";
            var RegistrationId = 1;
            var status = StatusTypes.Started;
            var comments = "Task started";
            var userName = "userName";

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });

            _context.LookupTasks.Add(new LookupRegulatorTask { Id = 1, Name = taskName, IsMaterialSpecific = true, ApplicationTypeId = 1 });

            _context.SaveChanges();

            // Act
            Func<Task> act = async () => await _repository.UpdateStatusAsync(taskName, RegistrationId, status, comments, userName);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowRegulatorInvalidOperationException_WhenTaskNotFound()
        {
            // Arrange
            var taskName = "NewTask";
            var RegistrationId = 1;
            var status = StatusTypes.Started;
            var comments = "Task started";
            var userName = "userName";

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });

            _context.Registrations.Add(new Registration { Id = RegistrationId, ExternalId = "", ApplicationTypeId = 1 });

            _context.SaveChanges();

            // Act
            Func<Task> act = async () => await _repository.UpdateStatusAsync(taskName, RegistrationId, status, comments, userName);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>();
        }
    }
}
