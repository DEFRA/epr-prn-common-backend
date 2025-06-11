using AutoMapper;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Profiles.Regulator;
using EPR.PRN.Backend.API.Handlers.Regulator;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetAccreditationOverviewDetailByIdHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _repoMock;
    private IMapper _mapper;
    private GetRegistrationOverviewDetailWithAccreditationsByIdHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _repoMock = new Mock<IRegistrationMaterialRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RegistrationMaterialProfile>(); 
        });
        _mapper = config.CreateMapper();

        _handler = new GetRegistrationOverviewDetailWithAccreditationsByIdHandler(_repoMock.Object, _mapper);
    }

    [TestMethod]
    public async Task Handle_WhenTasksAndMaterialsExist_ShouldReturnMappedDto()
    {
        // Arrange
        Guid registrationId = Guid.NewGuid();
        int year = 2025;

        var registration = new Registration
        {
            Id = 1,
            ExternalId = registrationId,
            ApplicationTypeId = 101,
            AccreditationTasks = [
                new RegulatorAccreditationRegistrationTaskStatus
                {
                    Task = new LookupRegulatorTask { Name = "SiteAddressAndContactDetails" },
                    TaskStatus = new LookupTaskStatus { Name = "Completed" },
                    AccreditationYear = year,
                }
            ],
            Materials =
            [
                new RegistrationMaterial
                {
                    Id = 10,
                    Material = new LookupMaterial { MaterialName = "Plastic" },
                    IsMaterialRegistered = true,
                    Tasks =
                    [
                    ],
                    Accreditations =
                    [
                        new Accreditation
                        {
                            Id = 1,
                            AccreditationYear = year,
                            ApplicationReferenceNumber = "APP-12345",
                            Tasks = [

                            ]
                        }
                    ]
                }
            ]
        };

        var task = new RegulatorAccreditationTaskStatus
        {
            AccreditationId = 1,
            Accreditation = registration.Materials[0].Accreditations[0],
            Task = new LookupRegulatorTask { Name = "BusinessAddress" },
            TaskStatus = new LookupTaskStatus { Name = "Started" }
        };

        registration.Materials[0].Accreditations[0].Tasks.Add(task);

        var requiredTasks = new List<LookupRegulatorTask>
        {
            new() { Name = "SiteAddressAndContactDetails" }
        };
        
        var requiredMaterialTasks = new List<LookupRegulatorTask>
        {
            new() { Name = "BusinessAddress" }
        };
        
        _repoMock.Setup(x => x.GetRegistrationByExternalIdAndYear(registrationId, year)).ReturnsAsync(registration);
        _repoMock.Setup(x => x.GetRequiredTasks(101, false, 2)).ReturnsAsync(requiredTasks);
        _repoMock.Setup(x => x.GetRequiredTasks(101, true, 2)).ReturnsAsync(requiredMaterialTasks);
        
        var query = new GetRegistrationOverviewDetailWithAccreditationsByIdQuery { Id = registrationId, Year = year };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Tasks.Should().HaveCount(1);
        result.Tasks[0].TaskName.Should().Be("SiteAddressAndContactDetails");
        result.Tasks[0].Status.Should().Be(RegulatorTaskStatus.Completed.ToString());

        result.Materials.Should().HaveCount(1);
        result.Materials[0].Tasks.Should().HaveCount(0);

        result.Materials[0].Accreditations.Should().HaveCount(1);
        result.Materials[0].Accreditations[0].AccreditationYear.Should().Be(year);
        result.Materials[0].Accreditations[0].Tasks.Should().HaveCount(1);
        result.Materials[0].Accreditations[0].Tasks[0].TaskName.Should().Be("BusinessAddress");
        result.Materials[0].Accreditations[0].Tasks[0].Status.Should().Be(RegulatorTaskStatus.Started.ToString());
    }

    [TestMethod]
    public async Task Handle_WhenNoTasksExist_ShouldCreateNotStartedEntriesForRequiredTasks()
    {
        // Arrange
        Guid registrationId = Guid.NewGuid();
        int year = 2025;

        var accreditation_registration = new Registration
        {
            ExternalId = registrationId,
            ApplicationTypeId = 101,
            Tasks = [],
            Materials =
            [
                new RegistrationMaterial
                {
                    Id = 10,
                    Material = new LookupMaterial { MaterialName = "Plastic" },
                    IsMaterialRegistered = true,
                    Tasks = [],
                    Accreditations = new List<Accreditation>{ 
                        new Accreditation{ 
                        AccreditationYear = year,
                        ApplicationReferenceNumber = "APP-12345",
                        }
                    }
                }
            ]
        };

        var requiredTasks = new List<LookupRegulatorTask>
        {
            new() { Name = "AssignOfficer", JourneyTypeId = 2 }
        };

        var requiredMaterialTasks = new List<LookupRegulatorTask>
        {
        };

        _repoMock.Setup(x => x.GetRegistrationByExternalIdAndYear(registrationId, year)).ReturnsAsync(accreditation_registration);
        _repoMock.Setup(x => x.GetRequiredTasks(101, false, 2)).ReturnsAsync(requiredTasks);
        _repoMock.Setup(x => x.GetRequiredTasks(101, true, 2)).ReturnsAsync(requiredMaterialTasks);

        var query = new GetRegistrationOverviewDetailWithAccreditationsByIdQuery { Id = registrationId, Year = year };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Tasks.Should().HaveCount(1);
        result.Tasks[0].TaskName.Should().Be("AssignOfficer");
        result.Tasks[0].Status.Should().Be(RegulatorTaskStatus.NotStarted.ToString());

        result.Materials.Should().HaveCount(1);
        result.Materials[0].Tasks.Should().HaveCount(0);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnEmptyMaterials_WhenNoMaterialsFound()
    {
        // Arrange
        Guid registrationId = Guid.NewGuid();
        int year = 2025;

        var accreditation_registration = new Registration
        {
            ExternalId = registrationId,
            ApplicationTypeId = 101
        };

        var requiredTasks = new List<LookupRegulatorTask>
        {
            new() { Name = "SiteAddressAndContactDetails" }
        };

        var existingTasks = new List<RegulatorRegistrationTaskStatus>
        {
            new()
            {
                Task = new LookupRegulatorTask { Name = "SiteAddressAndContactDetails" },
                TaskStatus = new LookupTaskStatus { Name = "Completed" }
            }
        };

        _repoMock.Setup(x => x.GetRegistrationByExternalIdAndYear(registrationId, year)).ReturnsAsync(accreditation_registration);
        _repoMock.Setup(x => x.GetRequiredTasks(101, false, 1)).ReturnsAsync(requiredTasks);

        var query = new GetRegistrationOverviewDetailWithAccreditationsByIdQuery { Id = registrationId, Year = year };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Materials.Should().BeEmpty();
    }
}
