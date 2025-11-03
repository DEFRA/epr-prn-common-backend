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
    public class RegulatorAccreditationTaskStatusRepositoryTests
    {
        private Mock<ILogger<RegulatorAccreditationTaskStatusRepository>> _loggerMock;
        private EprContext _context;
        private RegulatorAccreditationTaskStatusRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EprContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new EprContext(options);
            _loggerMock = new Mock<ILogger<RegulatorAccreditationTaskStatusRepository>>();
            _repository = new RegulatorAccreditationTaskStatusRepository(_context, _loggerMock.Object);
        }

        [TestMethod]
        public async Task GetTaskStatusAsync_ShouldReturnTaskStatus_WhenTaskExists()
        {
            // Arrange
            var taskName = "TestTask";
            var accreditationId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");

            var taskStatus = new RegulatorAccreditationTaskStatus
            {
                Task = new LookupRegulatorTask { Name = taskName },
                Accreditation = new Accreditation { ExternalId = accreditationId, ApplicationReferenceNumber = "A1234" },
                TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Completed.ToString() }
            };

            _context.RegulatorAccreditationTaskStatus.Add(taskStatus);
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
            var status = RegulatorTaskStatus.Started;
            var comments = "Task started";
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });
            _context.LookupTasks.Add(new LookupRegulatorTask { Id = 1, Name = taskName, IsMaterialSpecific = true, ApplicationTypeId = 1 });
            _context.Accreditations.Add(new Accreditation
            {
                ExternalId = accreditationId,
                ApplicationReferenceNumber = "A1234",
                RegistrationMaterial = new RegistrationMaterial
                {
                    ExternalId = registrationMaterialId,
                    Registration = new Registration
                    {
                        ExternalId = registrationId,
                        ApplicationTypeId = 1
                    }
                }
            });

            _context.SaveChanges();

            await _repository.UpdateStatusAsync(taskName, accreditationId, status, comments, user);

            var taskStatus = _context.RegulatorAccreditationTaskStatus.FirstOrDefault();

            taskStatus.Should().NotBeNull();
            taskStatus!.ExternalId.Should().NotBe(Guid.Empty);
            taskStatus.Task.Name.Should().Be(taskName);
            taskStatus.Accreditation.ExternalId.Should().Be(accreditationId);
            taskStatus.TaskStatus.Name.Should().Be(status.ToString());
            taskStatus.StatusCreatedBy.Should().Be(user);
            taskStatus.StatusUpdatedBy.Should().Be(user);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldAddNewTaskStatus_WhenStatusQueried()
        {
            var taskName = "NewTask";
            var accreditationId = Guid.NewGuid();
            var registrationMaterialId = Guid.NewGuid();
            var registrationId = Guid.NewGuid();
            var status = RegulatorTaskStatus.Queried;
            var comments = "Task started";
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 4, Name = "Queried" });
            _context.LookupTasks.Add(new LookupRegulatorTask { Id = 1, Name = taskName, IsMaterialSpecific = true, ApplicationTypeId = 1 });
            _context.Accreditations.Add(new Accreditation
            {
                ExternalId = accreditationId,
                ApplicationReferenceNumber = "A1234",
                RegistrationMaterial = new RegistrationMaterial
                {
                    ExternalId = registrationMaterialId,
                    Registration = new Registration
                    {
                        ExternalId = registrationId,
                        ApplicationTypeId = 1
                    }
                }
            });

            _context.SaveChanges();

            await _repository.UpdateStatusAsync(taskName, accreditationId, status, comments, user);

            var taskStatus = _context.RegulatorAccreditationTaskStatus.FirstOrDefault();

            taskStatus.Should().NotBeNull();
            taskStatus!.ExternalId.Should().NotBe(Guid.Empty);
            taskStatus.Task.Name.Should().Be(taskName);
            taskStatus.Accreditation.ExternalId.Should().Be(accreditationId);
            taskStatus.TaskStatus.Name.Should().Be(status.ToString());
            taskStatus.StatusCreatedBy.Should().Be(user);
            taskStatus.StatusUpdatedBy.Should().Be(user);
        }

        [TestMethod]
        public async Task UpdateStatusAsync_ShouldUpdateExistingTaskStatus_WhenTaskExists()
        {
            var taskName = "ExistingTask";
            var accreditationId = Guid.NewGuid();
            var registrationMaterialId = Guid.NewGuid();
            var registrationId = Guid.NewGuid();
            var status = RegulatorTaskStatus.Completed;
            var comments = "Task completed";
            var existingUser = Guid.NewGuid();
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = status.ToString() });

            _context.RegulatorAccreditationTaskStatus.Add(new RegulatorAccreditationTaskStatus
            {
                Task = new LookupRegulatorTask { Name = taskName },
                TaskStatus = new LookupTaskStatus { Name = RegulatorTaskStatus.Queried.ToString() },
                StatusCreatedBy = existingUser,
                Accreditation = new Accreditation
                {
                    ExternalId = accreditationId,
                    ApplicationReferenceNumber = "A1234",
                    RegistrationMaterial = new RegistrationMaterial
                    {
                        ExternalId = registrationMaterialId,
                        Registration = new Registration
                        {
                            ExternalId = registrationId,
                            ApplicationTypeId = 1
                        }
                    }
                }
            });

            _context.SaveChanges();

            await _repository.UpdateStatusAsync(taskName, accreditationId, status, comments, user);

            var taskStatus = _context.RegulatorAccreditationTaskStatus.FirstOrDefault();

            taskStatus.Should().NotBeNull();
            taskStatus!.Task.Name.Should().Be(taskName);
            taskStatus.Accreditation.ExternalId.Should().Be(accreditationId);
            taskStatus.TaskStatus.Name.Should().Be(status.ToString());
            taskStatus.StatusCreatedBy.Should().Be(existingUser);
            taskStatus.StatusUpdatedBy.Should().Be(user);
        }


        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowRegulatorInvalidOperationException_WhenTaskNotFound()
        {
            var taskName = "NewTask";
            var accreditationId = Guid.NewGuid();
            var registrationMaterialId = Guid.NewGuid();
            var registrationId = Guid.NewGuid();
            var status = RegulatorTaskStatus.Started;
            var comments = "Task started";
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 2, Name = "Started" });
            _context.Accreditations.Add(new Accreditation
            {
                ExternalId = accreditationId,
                ApplicationReferenceNumber = "A1234",
                RegistrationMaterial = new RegistrationMaterial
                {
                    ExternalId = registrationMaterialId,
                    Registration = new Registration
                    {
                        ExternalId = registrationId,
                        ApplicationTypeId = 1
                    }
                }
            });

            _context.SaveChanges();

            Func<Task> act = async () => await _repository.UpdateStatusAsync(taskName, accreditationId, status, comments, user);

            await act.Should().ThrowAsync<RegulatorInvalidOperationException>();
        }


        [TestMethod]
        public async Task UpdateStatusAsync_ShouldThrowInvalidOperationException_WhenLookupTaskStatusNotFound()
        {
            // Arrange
            var taskName = "MissingStatusTask";
            var accreditationId = Guid.NewGuid();
            var registrationMaterialId = Guid.NewGuid();
            var registrationId = Guid.NewGuid();
            var status = RegulatorTaskStatus.Started;
            var user = Guid.NewGuid();

            _context.LookupTasks.Add(new LookupRegulatorTask
            {
                Id = 1,
                Name = taskName,
                IsMaterialSpecific = true,
                ApplicationTypeId = 1
            });

            _context.Accreditations.Add(new Accreditation
            {
                ExternalId = accreditationId,
                ApplicationReferenceNumber = "A1234",
                RegistrationMaterial = new RegistrationMaterial
                {
                    ExternalId = registrationMaterialId,
                    Registration = new Registration
                    {
                        ExternalId = registrationId,
                        ApplicationTypeId = 1
                    }
                }
            });

            _context.SaveChanges();

            // Act
            Func<Task> act = async () => await _repository.UpdateStatusAsync(taskName, accreditationId, status, null, user);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }
        [TestMethod]
        public async Task UpdateStatusAsync_ShouldNotAddNote_WhenStatusIsQueried_AndCommentsAreNull()
        {
            var taskName = "QueriedWithoutNote";
            var accreditationId = Guid.NewGuid();
            var registrationMaterialId = Guid.NewGuid();
            var registrationId = Guid.NewGuid();
            var status = RegulatorTaskStatus.Queried;
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 5, Name = "Queried" });
            _context.LookupTasks.Add(new LookupRegulatorTask
            {
                Id = 2,
                Name = taskName,
                IsMaterialSpecific = true,
                ApplicationTypeId = 1
            });

            _context.Accreditations.Add(new Accreditation
            {
                ExternalId = accreditationId,
                ApplicationReferenceNumber = "A1234",
                RegistrationMaterial = new RegistrationMaterial
                {
                    ExternalId = registrationMaterialId,
                    Registration = new Registration
                    {
                        ExternalId = registrationId,
                        ApplicationTypeId = 1
                    }
                }
            });

            _context.SaveChanges();

            await _repository.UpdateStatusAsync(taskName, accreditationId, status, null, user);

            // Assert that no notes were created
            _context.QueryNote.Should().BeEmpty();
            _context.ApplicationTaskStatusQueryNotes.Should().BeEmpty();
        }
        [TestMethod]
        public async Task UpdateStatusAsync_ShouldLogInformation()
        {
            var taskName = "LogTestTask";
            var accreditationId = Guid.NewGuid();
            var registrationMaterialId = Guid.NewGuid();
            var registrationId = Guid.NewGuid();
            var status = RegulatorTaskStatus.Started;
            var user = Guid.NewGuid();

            _context.LookupTaskStatuses.Add(new LookupTaskStatus { Id = 3, Name = "Started" });
            _context.LookupTasks.Add(new LookupRegulatorTask
            {
                Id = 3,
                Name = taskName,
                IsMaterialSpecific = true,
                ApplicationTypeId = 1
            });

            _context.Accreditations.Add(new Accreditation
            {
                ExternalId = accreditationId,
                ApplicationReferenceNumber = "A1234",
                RegistrationMaterial = new RegistrationMaterial
                {
                    ExternalId = registrationMaterialId,
                    Registration = new Registration
                    {
                        ExternalId = registrationId,
                        ApplicationTypeId = 1
                    }
                }
            });

            _context.SaveChanges();

            await _repository.UpdateStatusAsync(taskName, accreditationId, status, null, user);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Updating status for task")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.AtLeastOnce);
        }
    }
}
