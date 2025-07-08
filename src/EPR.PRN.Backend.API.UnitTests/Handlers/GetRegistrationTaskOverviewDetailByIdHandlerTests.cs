using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetRegistrationTaskOverviewDetailByIdHandlerTests : HandlerTestsBase<IRegistrationRepository>
{
    private GetRegistrationTaskOverviewDetailByIdHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _handler = new GetRegistrationTaskOverviewDetailByIdHandler(MockRepository.Object, Mapper.CreateMapper());
    }

    [TestMethod]
    public async Task Handle_NoMaterialsExist_EnsureTasksReturned()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var organisationId = Guid.NewGuid();
        var registration = new Registration
        {
            Id = 1,
            ApplicationTypeId = 1,
            OrganisationId = organisationId,
            ExternalId = registrationId,
            ApplicantRegistrationTasksStatus =
            [
                new()
                {
                    Id = 1,
                    Task = new LookupApplicantRegistrationTask
                    {
                        Name = "task",
                        Id = 1,
                        ApplicationTypeId = 1,
                        IsMaterialSpecific = false,
                        JourneyTypeId = 1
                    }
                }
            ],
            Materials = []
        };
        
        // Expectations
        MockRepository.Setup(o => o.GetTasksForRegistrationAndMaterialsAsync(registrationId)).ReturnsAsync(registration);
        MockRepository.Setup(o => o.GetRequiredTasks(1, false, 1)).ReturnsAsync([
            new()
            {
                Id = 1,
                Name = "task 1",
                ApplicationTypeId = 1,
                IsMaterialSpecific = false,
                JourneyTypeId = 1
            }
        ]);

        MockRepository.Setup(o => o.GetRequiredTasks(1, true, 1)).ReturnsAsync([
            new()
            {
                Id = 2,
                Name = "task 2",
                ApplicationTypeId = 1,
                IsMaterialSpecific = true,
                JourneyTypeId = 1
            }
        ]);

        // Act
        var result = await _handler.Handle(new GetRegistrationTaskOverviewByIdQuery
        {
            Id = registrationId
        }, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(new ApplicantRegistrationTasksOverviewDto
        {
            OrganisationId = organisationId,
            Id = registrationId,
            Materials = [],
            Tasks =
            [
                new()
                {
                    Id = Guid.Empty,
                    TaskName = "task",
                    Status = null
                },
                new()
                {
                    TaskName = "task 1",
                    Status = "CannotStartYet"
                },
                new()
                {
                    TaskName = "task 2",
                    Status = "CannotStartYet"
                }
            ]
        });
    }

    [TestMethod]
    public async Task Handle_MaterialsExist_NoTasksReturned()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var organisationId = Guid.NewGuid();
        var registration = new Registration
        {
            Id = 1,
            ApplicationTypeId = 1,
            OrganisationId = organisationId,
            ExternalId = registrationId,
            ApplicantRegistrationTasksStatus =
            [
                new()
                {
                    Id = 1,
                    Task = new LookupApplicantRegistrationTask
                    {
                        Name = "task",
                        Id = 1,
                        ApplicationTypeId = 1,
                        IsMaterialSpecific = false,
                        JourneyTypeId = 1
                    }
                }
            ],
            Materials =
            [
                new()
                {
                    Id = 1,
                    Material = new LookupMaterial
                    {
                        Id = 1,
                        MaterialCode = "STL",
                        MaterialName = "steel"
                    },
                    IsMaterialRegistered = true,
                    Tasks = new List<RegulatorApplicationTaskStatus>
                    {
                        new ()
                        {
                            Id = 1, 
                            RegistrationMaterialId = 1,
                            TaskStatus = new LookupTaskStatus{Id = 1, Name = nameof(TaskStatuses.CannotStartYet)},
                            Task = new LookupRegulatorTask
                            {
                                Id = 1,
                                Name = "task",
                                ApplicationTypeId = 1,
                                IsMaterialSpecific = true,
                                JourneyTypeId = 1
                            }
                        }
                    }
                }
            ]
        };

        // Expectations
        MockRepository.Setup(o => o.GetTasksForRegistrationAndMaterialsAsync(registrationId)).ReturnsAsync(registration);
        MockRepository.Setup(o => o.GetRequiredTasks(1, false, 1)).ReturnsAsync([
            new()
            {
                Id = 1,
                Name = "task 1",
                ApplicationTypeId = 1,
                IsMaterialSpecific = false,
                JourneyTypeId = 1
            }
        ]);

        MockRepository.Setup(o => o.GetRequiredTasks(1, true, 1)).ReturnsAsync([
            new()
            {
                Id = 2,
                Name = "task 2",
                ApplicationTypeId = 1,
                IsMaterialSpecific = true,
                JourneyTypeId = 1
            }
        ]);

        // Act
        var result = await _handler.Handle(new GetRegistrationTaskOverviewByIdQuery
        {
            Id = registrationId
        }, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(new ApplicantRegistrationTasksOverviewDto
        {
            OrganisationId = organisationId,
            Id = registrationId,
            Materials = [],
            Tasks =
            [
                new()
                {
                    Id = Guid.Empty,
                    TaskName = "task",
                    Status = null
                },
                new()
                {
                    TaskName = "task 1",
                    Status = "CannotStartYet"
                },
                new()
                {
                    TaskName = "task 2",
                    Status = "CannotStartYet"
                }
            ]
        }, options => options.Excluding(o => o.Materials));
    }
}