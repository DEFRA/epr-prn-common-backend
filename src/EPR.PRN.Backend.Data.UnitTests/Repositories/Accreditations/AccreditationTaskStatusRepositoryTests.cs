using System;
using System.Threading.Tasks;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Repositories.Accreditations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories.Accreditations
{
    [TestClass]
    public class AccreditationTaskStatusRepositoryTests
    {
        private Mock<ILogger<AccreditationTaskStatusRepository>> _loggerMock;
        private EprContext _context;
        private AccreditationTaskStatusRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EprContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new EprContext(options);
            _loggerMock = new Mock<ILogger<AccreditationTaskStatusRepository>>();
            _repository = new AccreditationTaskStatusRepository(_context, _loggerMock.Object);
        }

        [TestMethod]
        public async Task GetTaskStatusAsync_ShouldReturnTaskStatus_WhenTaskExists()
        {
            // Arrange
            var taskName = "TestTask";
            var accreditationId = Guid.NewGuid();

            var taskStatus = new AccreditationTaskStatus
            {
                Task = new LookupApplicantRegistrationTask { Name = taskName },
                Accreditation = new Accreditation { ExternalId = accreditationId, ApplicationReferenceNumber = "A1234" },
                TaskStatus = new LookupTaskStatus { Name = TaskStatuses.Completed.ToString() }
            };

            _context.AccreditationTaskStatus.Add(taskStatus);
            _context.SaveChanges();

            // Act
            var result = await _repository.GetTaskStatusAsync(taskName, accreditationId);

            // Assert
            result.Should().NotBeNull();
            result!.Task.Name.Should().Be(taskName);
            result.Accreditation.ExternalId.Should().Be(accreditationId);
        }

        [TestMethod]
        public async Task GetTaskStatusAsync_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            var taskName = "NonExistentTask";
            var accreditationId = Guid.NewGuid();

            var result = await _repository.GetTaskStatusAsync(taskName, accreditationId);

            result.Should().BeNull();
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldAddNewTaskStatus_WhenTaskDoesNotExist()
        {
            var taskName = "NewTask";
            var accreditationId = Guid.NewGuid();
            var registrationMaterialId = Guid.NewGuid();
            var registrationId = Guid.NewGuid();
            var status = TaskStatuses.Started;

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });
            _context.LookupJourneyTypes.Add(new LookupJourneyType { Id = 1, Name = "Accreditation" });
            _context.LookupApplicantRegistrationTasks.Add(new LookupApplicantRegistrationTask
            {
                Id = 1,
                Name = taskName,
                IsMaterialSpecific = true,
                ApplicationTypeId = 1,
                JourneyTypeId = 1
            });
            _context.Accreditations.Add(new Accreditation
            {
                ExternalId = accreditationId,
                ApplicationReferenceNumber = "A1234",
                RegistrationMaterial = new RegistrationMaterial
                {
                    Registration = new Registration
                    {
                        ApplicationTypeId = 1
                    }
                }
            });

            _context.SaveChanges();

            await _repository.UpdateStatusAsync(taskName, accreditationId, status);

            var taskStatus = _context.AccreditationTaskStatus.FirstOrDefault();

            taskStatus.Should().NotBeNull();
            taskStatus!.ExternalId.Should().NotBe(Guid.Empty);
            taskStatus.Task.Name.Should().Be(taskName);
            taskStatus.Accreditation.ExternalId.Should().Be(accreditationId);
            taskStatus.TaskStatus.Name.Should().Be(status.ToString());
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldUpdateExistingTaskStatus_WhenTaskExists()
        {
            var taskName = "ExistingTask";
            var accreditationId = Guid.NewGuid();
            var status = TaskStatuses.Completed;

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = status.ToString() });

            _context.AccreditationTaskStatus.Add(new AccreditationTaskStatus
            {
                Task = new LookupApplicantRegistrationTask { Name = taskName },
                TaskStatus = new LookupTaskStatus { Name = TaskStatuses.Queried.ToString() },
                Accreditation = new Accreditation
                {
                    ExternalId = accreditationId,
                    ApplicationReferenceNumber = "A1234",
                    RegistrationMaterial = new RegistrationMaterial
                    {
                        Registration = new Registration
                        {
                            ApplicationTypeId = 1
                        }
                    }
                }
            });

            _context.SaveChanges();

            await _repository.UpdateStatusAsync(taskName, accreditationId, status);

            var taskStatus = _context.AccreditationTaskStatus.FirstOrDefault();

            taskStatus.Should().NotBeNull();
            taskStatus!.Task.Name.Should().Be(taskName);
            taskStatus.Accreditation.ExternalId.Should().Be(accreditationId);
            taskStatus.TaskStatus.Name.Should().Be(status.ToString());
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowKeyNotFoundException_WhenAccreditationNotFound()
        {
            var taskName = "NewTask";
            var accreditationId = Guid.NewGuid();
            var status = TaskStatuses.Started;

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });
            _context.SaveChanges();

            Func<Task> act = async () => await _repository.UpdateStatusAsync(taskName, accreditationId, status);

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowKeyNotFoundException_WhenRegistrationNotFound()
        {
            var taskName = "NewTask";
            var accreditationId = Guid.NewGuid();
            var status = TaskStatuses.Started;

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });
            _context.Accreditations.Add(new Accreditation
            {
                ExternalId = accreditationId,
                ApplicationReferenceNumber = "A1234",
                RegistrationMaterial = null // No registration
            });
            _context.SaveChanges();

            Func<Task> act = async () => await _repository.UpdateStatusAsync(taskName, accreditationId, status);

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowInvalidOperationException_WhenJourneyTypeNotFound()
        {
            var taskName = "NewTask";
            var accreditationId = Guid.NewGuid();
            var status = TaskStatuses.Started;

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });
            _context.Accreditations.Add(new Accreditation
            {
                ExternalId = accreditationId,
                ApplicationReferenceNumber = "A1234",
                RegistrationMaterial = new RegistrationMaterial
                {
                    Registration = new Registration
                    {
                        ApplicationTypeId = 1
                    }
                }
            });
            _context.SaveChanges();

            Func<Task> act = async () => await _repository.UpdateStatusAsync(taskName, accreditationId, status);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowInvalidOperationException_WhenTaskNotFound()
        {
            var taskName = "NewTask";
            var accreditationId = Guid.NewGuid();
            var status = TaskStatuses.Started;

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });
            _context.LookupJourneyTypes.Add(new LookupJourneyType { Id = 1, Name = "Accreditation" });
            _context.Accreditations.Add(new Accreditation
            {
                ExternalId = accreditationId,
                ApplicationReferenceNumber = "A1234",
                RegistrationMaterial = new RegistrationMaterial
                {
                    Registration = new Registration
                    {
                        ApplicationTypeId = 1
                    }
                }
            });
            _context.SaveChanges();

            Func<Task> act = async () => await _repository.UpdateStatusAsync(taskName, accreditationId, status);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldLogInformation()
        {
            var taskName = "LogTestTask";
            var accreditationId = Guid.NewGuid();
            var status = TaskStatuses.Started;

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 3, Name = "Started" });
            _context.LookupJourneyTypes.Add(new LookupJourneyType { Id = 3, Name = "Accreditation" });
            _context.LookupApplicantRegistrationTasks.Add(new LookupApplicantRegistrationTask
            {
                Id = 3,
                Name = taskName,
                IsMaterialSpecific = true,
                ApplicationTypeId = 1,
                JourneyTypeId = 3
            });

            _context.Accreditations.Add(new Accreditation
            {
                ExternalId = accreditationId,
                ApplicationReferenceNumber = "A1234",
                RegistrationMaterial = new RegistrationMaterial
                {
                    Registration = new Registration
                    {
                        ApplicationTypeId = 1
                    }
                }
            });

            _context.SaveChanges();

            await _repository.UpdateStatusAsync(taskName, accreditationId, status);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Updating status for task")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.AtLeastOnce);
        }
    }
}
