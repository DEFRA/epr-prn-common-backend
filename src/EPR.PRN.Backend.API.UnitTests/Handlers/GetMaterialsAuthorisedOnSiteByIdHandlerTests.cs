using AutoMapper;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.API.Profiles.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetMaterialsAuthorisedOnSiteByIdHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _rmRepositoryMock;
    private IMapper _mapper;
    private GetMaterialsAuthorisedOnSiteByIdHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _rmRepositoryMock = new Mock<IRegistrationMaterialRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RegistrationMaterialProfile>(); 
        });
        _mapper = config.CreateMapper();

        _handler = new GetMaterialsAuthorisedOnSiteByIdHandler(_rmRepositoryMock.Object, _mapper);
    }

    [TestMethod]
    public async Task Handle_ShouldMapRegistrationAndMaterialsCorrectly()
    {
        // Arrange
        var registrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d");
        var query = new GetMaterialsAuthorisedOnSiteByIdQuery { Id = registrationId };

        var registration = new Registration
        {
            ExternalId = registrationId,
            OrganisationId = Guid.NewGuid(),
            ReprocessingSiteAddress = new Address
            {
                AddressLine1 = "Unit 7",
                TownCity = "Greenwich",
                PostCode = "GR1 2AB"
            },
            Materials = new List<RegistrationMaterial>
            {
                new()
                {
                    Id = 1,
                    MaterialId = 1,
                    Material = new LookupMaterial { MaterialName = "Plastic" },
                    IsMaterialRegistered = false,
                    ReasonforNotreg = "Low quality"
                },
                new()
                {
                    Id = 2,
                    MaterialId = 2,
                    Material = new LookupMaterial { MaterialName = "Steel" },
                    IsMaterialRegistered = true
                }
            }
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationById(registrationId))
            .ReturnsAsync(registration);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.RegistrationId.Should().Be(registrationId);
        result.SiteAddress.Should().Contain("Unit 7");
        result.MaterialsAuthorisation.Should().HaveCount(2);

        var paper = result.MaterialsAuthorisation.First(m => m.MaterialName == "Plastic");
        paper.IsMaterialRegistered.Should().BeFalse();
        paper.Reason.Should().Be("Low quality");

        var metal = result.MaterialsAuthorisation.First(m => m.MaterialName == "Steel");
        metal.IsMaterialRegistered.Should().BeTrue();
        metal.Reason.Should().BeNullOrEmpty();
    }
    [TestMethod]
    public async Task Handle_ShouldFilterTasksCorrectly_WhenTasksExist()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var query = new GetMaterialsAuthorisedOnSiteByIdQuery { Id = registrationId };
        var applicationTypeId = 1;

        var validTask = new RegulatorRegistrationTaskStatus
        {
            RegulatorTaskId = 2,
            TaskStatusId = (int)RegulatorTaskStatus.Queried,
            Task = new LookupRegulatorTask
            {
                Name = RegulatorTaskNames.MaterialsAuthorisedOnSite,
                ApplicationTypeId = applicationTypeId,
                IsMaterialSpecific = false
            }
        };
        var invalidTask1 = new RegulatorRegistrationTaskStatus
        {
            RegulatorTaskId = 2,
            TaskStatusId = (int)RegulatorTaskStatus.Queried,
            Task = new LookupRegulatorTask
            {
                Name = "OtherTaskName",
                ApplicationTypeId = applicationTypeId,
                IsMaterialSpecific = false
            }
        };

        var invalidTask2 = new RegulatorRegistrationTaskStatus
        {
            RegulatorTaskId = 2,
            TaskStatusId = (int)RegulatorTaskStatus.Queried,
            Task = new LookupRegulatorTask
            {
                Name = RegulatorTaskNames.MaterialsAuthorisedOnSite,
                ApplicationTypeId = applicationTypeId,
                IsMaterialSpecific = true // filtered by IsMaterialSpecific = false
            }
        };

        var registration = new Registration
        {
            ExternalId = registrationId,
            ApplicationTypeId = applicationTypeId,
            Tasks = new List<RegulatorRegistrationTaskStatus> { validTask, invalidTask1, invalidTask2 }, // Fixed syntax
            Materials = new List<RegistrationMaterial>(),
            ReprocessingSiteAddress = new Address()
        };

        _rmRepositoryMock.Setup(r => r.GetRegistrationById(registrationId))
            .ReturnsAsync(registration);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        registration.Tasks.Should().HaveCount(1);
        registration.Tasks.First().Should().BeEquivalentTo(validTask);
    }

    [TestMethod]
    public async Task Handle_ShouldInitializeTasks_WhenFilteredTasksBecomeNull()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var query = new GetMaterialsAuthorisedOnSiteByIdQuery { Id = registrationId };
        var applicationTypeId = 1;

        // All tasks will be filtered out
        var tasks = new List<RegulatorRegistrationTaskStatus>
    {
        new RegulatorRegistrationTaskStatus
        {
            Task = new LookupRegulatorTask
            {
                Name = "NonMatching",
                ApplicationTypeId = applicationTypeId,
                IsMaterialSpecific = true
            }
        }
    };

        var registration = new Registration
        {
            ExternalId = registrationId,
            ApplicationTypeId = applicationTypeId,
            Tasks = tasks,
            Materials = new List<RegistrationMaterial>(),
            ReprocessingSiteAddress = new Address()
        };

        _rmRepositoryMock.Setup(r => r.GetRegistrationById(registrationId))
            .ReturnsAsync(registration);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        registration.Tasks.Should().NotBeNull();
        registration.Tasks.Should().BeEmpty(); // List is filtered to empty
    }

}
