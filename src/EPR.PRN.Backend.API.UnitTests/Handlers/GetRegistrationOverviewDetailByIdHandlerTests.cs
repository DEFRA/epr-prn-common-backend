using AutoMapper;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.API.Profiles.Regulator;
using EPR.PRN.Backend.Data.Interfaces;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetRegistrationOverviewDetailByIdHandlerTests
{
    private Mock<IRegistrationRepository> _repoMock;
    private IMapper _mapper;
    private GetRegistrationOverviewDetailByIdHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _repoMock = new Mock<IRegistrationRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RegistrationMaterialProfile>(); 
        });
        _mapper = config.CreateMapper();

        _handler = new GetRegistrationOverviewDetailByIdHandler(_repoMock.Object, _mapper);
    }

    [TestMethod]
    public async Task Handle_WhenTasksAndMaterialsExist_ShouldReturnMappedDto()
    {
        // Arrange
        Guid registrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");

        var registration = new Registration
        {
            ExternalId = registrationId,
            ApplicationTypeId = 101,
            Tasks = [
                new RegulatorRegistrationTaskStatus
                {
                    Task = new LookupRegulatorTask { Name = "SiteAddressAndContactDetails" },
                    TaskStatus = new LookupTaskStatus { Name = "Completed" }
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
                        new RegulatorApplicationTaskStatus
                        {
                            Task = new LookupRegulatorTask { Name = "BusinessAddress" },
                            TaskStatus = new LookupTaskStatus { Name = "Started" }
                        }
                    ]
                }
            ]
        };

        var requiredTasks = new List<LookupRegulatorTask>
        {
            new() { Name = "SiteAddressAndContactDetails" }
        };
        
        var requiredMaterialTasks = new List<LookupRegulatorTask>
        {
            new() { Name = "BusinessAddress" }
        };
        
        _repoMock.Setup(x => x.GetAsync(registrationId)).ReturnsAsync(registration);
        _repoMock.Setup(x => x.GetRequiredTasks(101, false, 1)).ReturnsAsync(requiredTasks);
        _repoMock.Setup(x => x.GetRequiredTasks(101, true, 1)).ReturnsAsync(requiredMaterialTasks);
        
        var query = new GetRegistrationByIdQuery { Id = registrationId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Tasks.Should().HaveCount(1);
        result.Tasks[0].TaskName.Should().Be("SiteAddressAndContactDetails");
        result.Tasks[0].Status.Should().Be(RegulatorTaskStatus.Completed.ToString());

        result.Materials.Should().HaveCount(1);
        result.Materials[0].Tasks.Should().HaveCount(1);
        result.Materials[0].Tasks[0].TaskName.Should().Be("BusinessAddress");
        result.Materials[0].Tasks[0].Status.Should().Be(RegulatorTaskStatus.Started.ToString());
    }

    [TestMethod]
    public async Task Handle_WhenNoTasksExist_ShouldCreateNotStartedEntriesForRequiredTasks()
    {
        // Arrange
        Guid registrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");

        var registration = new Registration
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
                    Tasks = []
                }
            ]
        };

        var requiredTasks = new List<LookupRegulatorTask>
        {
            new() { Name = "SiteAddressAndContactDetails" }
        };

        var requiredMaterialTasks = new List<LookupRegulatorTask>
        {
            new() { Name = "BusinessAddress" }
        };

        _repoMock.Setup(x => x.GetAsync(registrationId)).ReturnsAsync(registration);
        _repoMock.Setup(x => x.GetRequiredTasks(101, false, 1)).ReturnsAsync(requiredTasks);
        _repoMock.Setup(x => x.GetRequiredTasks(101, true, 1)).ReturnsAsync(requiredMaterialTasks);

        var query = new GetRegistrationByIdQuery { Id = registrationId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Tasks.Should().HaveCount(1);
        result.Tasks[0].TaskName.Should().Be("SiteAddressAndContactDetails");
        result.Tasks[0].Status.Should().Be(RegulatorTaskStatus.NotStarted.ToString());

        result.Materials.Should().HaveCount(1);
        result.Materials[0].Tasks.Should().HaveCount(1);
        result.Materials[0].Tasks[0].TaskName.Should().Be("BusinessAddress");
        result.Materials[0].Tasks[0].Status.Should().Be(RegulatorTaskStatus.NotStarted.ToString());
    }

    [TestMethod]
    public async Task Handle_ShouldReturnEmptyMaterials_WhenNoMaterialsFound()
    {
        // Arrange
        Guid registrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");

        var registration = new Registration
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

        _repoMock.Setup(x => x.GetAsync(registrationId)).ReturnsAsync(registration);
        _repoMock.Setup(x => x.GetRequiredTasks(101, false, 1)).ReturnsAsync(requiredTasks);
        
        var query = new GetRegistrationByIdQuery { Id = registrationId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Materials.Should().BeEmpty();
    }
}
