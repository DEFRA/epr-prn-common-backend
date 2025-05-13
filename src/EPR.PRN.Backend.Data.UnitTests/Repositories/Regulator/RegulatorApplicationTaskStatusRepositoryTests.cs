using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Repositories.Regulator;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories.Regulator
{
    [TestClass]
    public class RegulatorApplicationTaskStatusRepositoryTests
    {
        private Mock<ILogger<RegulatorApplicationTaskStatusRepository>> _loggerMock;
        private EprContext _context;
        private RegulatorApplicationTaskStatusRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EprContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new EprContext(options);
            _loggerMock = new Mock<ILogger<RegulatorApplicationTaskStatusRepository>>();
            _repository = new RegulatorApplicationTaskStatusRepository(_context, _loggerMock.Object);
        }

        [TestMethod]
        public async Task GetTaskStatusAsync_ShouldReturnTaskStatus_WhenTaskExists()
        {
            // Arrange
            var taskName = "TestTask";
            var RegistrationMaterialId = 1;

            var taskStatus = new RegulatorApplicationTaskStatus
            {
                Task = new LookupRegulatorTask { Name = taskName },
                RegistrationMaterialId = RegistrationMaterialId
            };

            _context.RegulatorApplicationTaskStatus.Add(taskStatus);
            _context.SaveChanges();

            // Act
            var result = await _repository.GetTaskStatusAsync(taskName, RegistrationMaterialId);

            // Assert
            result.Should().NotBeNull();
            result!.Task.Name.Should().Be(taskName);
            result.RegistrationMaterialId.Should().Be(RegistrationMaterialId);
        }

        [TestMethod]
        public async Task GetTaskStatusAsync_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskName = "NonExistentTask";
            var ApplicationId = 1;

            // Act
            var result = await _repository.GetTaskStatusAsync(taskName, ApplicationId);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldAddNewTaskStatus_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskName = "NewTask";
            var RegistrationMaterialId = 1;
            var RegistrationId = 1;
            var status = RegulatorTaskStatus.Started;
            var comments = "Task started";
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });

            _context.LookupTasks.Add(new LookupRegulatorTask { Id = 1, Name = taskName, IsMaterialSpecific = true, ApplicationTypeId = 1 });

            _context.Registrations.Add(new Registration{ Id = RegistrationId, ExternalId = Guid.NewGuid(), ApplicationTypeId = 1 });

            _context.RegistrationMaterials.Add(new RegistrationMaterial { Id = RegistrationMaterialId, RegistrationId = RegistrationId });

            _context.SaveChanges();

            // Act
            await _repository.UpdateStatusAsync(taskName, RegistrationMaterialId, status, comments, user);

            // Assert
            var taskStatus = _context.RegulatorApplicationTaskStatus.FirstOrDefault();

            taskStatus.Should().NotBeNull();
            taskStatus.Task.Name.Should().Be(taskName);
            taskStatus.RegistrationMaterialId.Should().Be(RegistrationMaterialId);
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
            var RegistrationMaterialId = 1;
            var RegistrationId = 1;
            var existingStatus = RegulatorTaskStatus.Queried;
            var status = RegulatorTaskStatus.Completed;
            var comments = "Task completed";
            var existingUser = Guid.NewGuid();
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = status.ToString() });

            _context.RegulatorApplicationTaskStatus.Add(new RegulatorApplicationTaskStatus
            {
                Task = new LookupRegulatorTask { Name = taskName },
                RegistrationMaterialId = RegistrationMaterialId,
                TaskStatus = new LookupTaskStatus { Name = existingStatus.ToString() },
                Comments = "Task started",
                StatusCreatedBy =  existingUser
            });

            _context.Registrations.Add(new Registration { Id = RegistrationId, ExternalId = Guid.NewGuid(), ApplicationTypeId = 1 });

            _context.RegistrationMaterials.Add(new RegistrationMaterial { Id = RegistrationMaterialId, RegistrationId = RegistrationId });

            _context.SaveChanges();

            // Act
            await _repository.UpdateStatusAsync(taskName, RegistrationMaterialId, status, comments, user);

            // Assert
            var taskStatus = _context.RegulatorApplicationTaskStatus.FirstOrDefault();

            taskStatus.Should().NotBeNull();
            taskStatus.Task.Name.Should().Be(taskName);
            taskStatus.RegistrationMaterialId.Should().Be(RegistrationMaterialId);
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
            var RegistrationMaterialId = 1;
            var status = RegulatorTaskStatus.Started;
            var comments = "Task started";
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });

            _context.LookupTasks.Add(new LookupRegulatorTask { Id = 1, Name = taskName, IsMaterialSpecific = true, ApplicationTypeId = 1 });

            _context.SaveChanges();

            // Act
            Func<Task> act = async () => await _repository.UpdateStatusAsync(taskName, RegistrationMaterialId, status, comments, user);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowRegulatorInvalidOperationException_WhenTaskNotFound()
        {
            // Arrange
            var taskName = "NewTask";
            var RegistrationMaterialId = 1;
            var RegistrationId = 1;
            var status = RegulatorTaskStatus.Started;
            var comments = "Task started";
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });

            _context.Registrations.Add(new Registration { Id = RegistrationId, ExternalId = Guid.NewGuid(), ApplicationTypeId = 1 });

            _context.RegistrationMaterials.Add(new RegistrationMaterial { Id = RegistrationMaterialId, RegistrationId = RegistrationId });
            _context.SaveChanges();

            // Act
            Func<Task> act = async () => await _repository.UpdateStatusAsync(taskName, RegistrationMaterialId, status, comments, user);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>();
        }
    }
}
