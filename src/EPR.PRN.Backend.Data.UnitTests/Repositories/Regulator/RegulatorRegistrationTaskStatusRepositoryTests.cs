using EPR.PRN.Backend.API.Common.Enums;
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

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });

            _context.LookupTasks.Add(new LookupRegulatorTask { Id = 1, Name = taskName, IsMaterialSpecific = false, ApplicationTypeId = 1 });

            _context.Registrations.Add(new Registration { Id = registrationId, ExternalId="", ApplicationTypeId = 1 });

            _context.SaveChanges();

            // Act
            await _repository.UpdateStatusAsync(taskName, registrationId, status, comments);

            // Assert
            _context.RegulatorRegistrationTaskStatus.First().Should().NotBeNull();
            _context.RegulatorRegistrationTaskStatus.First().Task.Name.Should().Be(taskName);
            _context.RegulatorRegistrationTaskStatus.First().RegistrationId.Should().Be(registrationId);
            _context.RegulatorRegistrationTaskStatus.First().TaskStatus.Name.Should().Be(status.ToString());
            _context.RegulatorRegistrationTaskStatus.First().Comments.Should().Be(comments);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldUpdateExistingTaskStatus_WhenTaskExists()
        {
            // Arrange
            var taskName = "ExistingTask";
            var registrationId = 1;
            var status = StatusTypes.Completed;
            var comments = "Task completed";

            _context.RegulatorRegistrationTaskStatus.Add(new RegulatorRegistrationTaskStatus
            {
                Task = new LookupRegulatorTask { Name = taskName },
                RegistrationId = registrationId,
                TaskStatus = new LookupTaskStatus { Name = StatusTypes.Completed.ToString() },
                Comments = "Task started"
            });

            _context.Registrations.Add(new Registration { Id = registrationId, ExternalId = "", ApplicationTypeId = 1 });

            _context.SaveChanges();

            // Act
            await _repository.UpdateStatusAsync(taskName, registrationId, status, comments);

            // Assert
            _context.RegulatorRegistrationTaskStatus.First().Should().NotBeNull();
            _context.RegulatorRegistrationTaskStatus.First().Task.Name.Should().Be(taskName);
            _context.RegulatorRegistrationTaskStatus.First().RegistrationId.Should().Be(registrationId);
            _context.RegulatorRegistrationTaskStatus.First().TaskStatus.Name.Should().Be(status.ToString());
            _context.RegulatorRegistrationTaskStatus.First().Comments.Should().Be(comments);
        }
    }
}
