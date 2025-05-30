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
            var RegistrationMaterialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");

            var taskStatus = new RegulatorApplicationTaskStatus
            {
                Task = new LookupRegulatorTask { Name = taskName },
                RegistrationMaterial = new RegistrationMaterial { ExternalId = RegistrationMaterialId },
                TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Completed.ToString() }
            };

            _context.RegulatorApplicationTaskStatus.Add(taskStatus);
            _context.SaveChanges();

            // Act
            var result = await _repository.GetTaskStatusAsync(taskName, RegistrationMaterialId);

            // Assert
            result.Should().NotBeNull();
            result!.Task.Name.Should().Be(taskName);
            result.RegistrationMaterial.ExternalId.Should().Be(RegistrationMaterialId);
        }

        [TestMethod]
        public async Task GetTaskStatusAsync_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskName = "NonExistentTask";
            var RegistrationMaterialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");

            // Act
            var result = await _repository.GetTaskStatusAsync(taskName, RegistrationMaterialId);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldAddNewTaskStatus_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskName = "NewTask";
            var RegistrationMaterialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
            var RegistrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");
            var status = RegulatorTaskStatus.Started;
            var comments = "Task started";
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });

            _context.LookupTasks.Add(new LookupRegulatorTask { Id = 1, Name = taskName, IsMaterialSpecific = true, ApplicationTypeId = 1 });

            _context.RegistrationMaterials.Add(new RegistrationMaterial { ExternalId = RegistrationMaterialId, Registration = new Registration { ExternalId = RegistrationId, ApplicationTypeId = 1 } });

            _context.SaveChanges();

            // Act
            await _repository.UpdateStatusAsync(taskName, RegistrationMaterialId, status, comments, user);

            // Assert
            var taskStatus = _context.RegulatorApplicationTaskStatus.FirstOrDefault();

            taskStatus.Should().NotBeNull();
            taskStatus.ExternalId.Should().NotBe(Guid.Empty);
            taskStatus.Task.Name.Should().Be(taskName);
            taskStatus.RegistrationMaterial.ExternalId.Should().Be(RegistrationMaterialId);
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
            var RegistrationMaterialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
            var RegistrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");
            var existingStatus = RegulatorTaskStatus.Queried;
            var status = RegulatorTaskStatus.Completed;
            var comments = "Task completed";
            var existingUser = Guid.NewGuid();
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = status.ToString() });

            _context.RegulatorApplicationTaskStatus.Add(new RegulatorApplicationTaskStatus
            {
                Task = new LookupRegulatorTask { Name = taskName },
                RegistrationMaterial = new RegistrationMaterial { ExternalId = RegistrationMaterialId },
                TaskStatus = new LookupTaskStatus { Name = existingStatus.ToString() },
                Comments = "Task started",
                StatusCreatedBy =  existingUser
            });

            _context.Registrations.Add(new Registration { ExternalId = RegistrationId, ApplicationTypeId = 1 });

            _context.RegistrationMaterials.Add(new RegistrationMaterial { ExternalId = RegistrationMaterialId, Registration = new Registration { ExternalId = RegistrationId } });

            _context.SaveChanges();

            // Act
            await _repository.UpdateStatusAsync(taskName, RegistrationMaterialId, status, comments, user);

            // Assert
            var taskStatus = _context.RegulatorApplicationTaskStatus.FirstOrDefault();

            taskStatus.Should().NotBeNull();
            taskStatus.Task.Name.Should().Be(taskName);
            taskStatus.RegistrationMaterial.ExternalId.Should().Be(RegistrationMaterialId);
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
            var RegistrationMaterialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
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
            var RegistrationMaterialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
            var RegistrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");
            var status = RegulatorTaskStatus.Started;
            var comments = "Task started";
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });

            _context.Registrations.Add(new Registration { ExternalId = RegistrationId, ApplicationTypeId = 1 });

            _context.RegistrationMaterials.Add(new RegistrationMaterial { ExternalId = RegistrationMaterialId, Registration = new Registration { ExternalId = RegistrationId } });
            _context.SaveChanges();

            // Act
            Func<Task> act = async () => await _repository.UpdateStatusAsync(taskName, RegistrationMaterialId, status, comments, user);

            // Assert
            await act.Should().ThrowAsync<RegulatorInvalidOperationException>();
        }
    }
}
