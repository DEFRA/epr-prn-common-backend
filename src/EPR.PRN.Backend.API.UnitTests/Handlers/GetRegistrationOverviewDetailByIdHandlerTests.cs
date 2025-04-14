using AutoMapper;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;
using EPR.PRN.Backend.Data.Profiles;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetRegistrationOverviewDetailByIdHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _repoMock;
    private IMapper _mapper;
    private GetRegistrationOverviewDetailByIdHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _repoMock = new Mock<IRegistrationMaterialRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RegistrationMaterialProfile>(); 
        });
        _mapper = config.CreateMapper();

        _handler = new GetRegistrationOverviewDetailByIdHandler(_repoMock.Object, _mapper);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedOverview_WithTasksAndMaterials()
    {
        // Arrange
        int registrationId = 1;

        var registration = new Registration
        {
            Id = registrationId,
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

        var materials = new List<RegistrationMaterial>
        {
            new()
            {
                Id = 10,
                Registration = registration,
                Material = new LookupMaterial { MaterialName = "Plastic" }
            }
        };

        var requiredMaterialTasks = new List<LookupRegulatorTask>
        {
            new() { Name = "BusinessAddress" }
        };

        var existingMaterialTasks = new List<RegulatorApplicationTaskStatus>
        {
            new()
            {
                Task = new LookupRegulatorTask { Name = "BusinessAddress" },
                TaskStatus = new LookupTaskStatus { Name = "Started" }
            }
        };

        _repoMock.Setup(x => x.GetRegistrationById(registrationId)).ReturnsAsync(registration);
        _repoMock.Setup(x => x.GetRequiredTasks(101, false)).ReturnsAsync(requiredTasks);
        _repoMock.Setup(x => x.GetRegistrationTasks(registrationId)).ReturnsAsync(existingTasks);
        _repoMock.Setup(x => x.GetMaterialsByRegistrationId(registrationId)).ReturnsAsync(materials);
        _repoMock.Setup(x => x.GetRequiredTasks(101, true)).ReturnsAsync(requiredMaterialTasks);
        _repoMock.Setup(x => x.GetMaterialTasks(10)).ReturnsAsync(existingMaterialTasks);

        var query = new GetRegistrationOverviewDetailByIdQuery { Id = registrationId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Tasks.Should().HaveCount(1);
        result.Tasks[0].TaskName.Should().Be("SiteAddressAndContactDetails");
        result.Tasks[0].Status.Should().Be("Completed");

        result.Materials.Should().HaveCount(1);
        result.Materials[0].Tasks.Should().HaveCount(1);
        result.Materials[0].Tasks[0].TaskName.Should().Be("BusinessAddress");
        result.Materials[0].Tasks[0].Status.Should().Be("Started");
    }

    [TestMethod]
    public async Task Handle_ShouldReturnEmptyMaterials_WhenNoMaterialsFound()
    {
        // Arrange
        int registrationId = 1;

        var registration = new Registration
        {
            Id = registrationId,
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

        _repoMock.Setup(x => x.GetRegistrationById(registrationId)).ReturnsAsync(registration);
        _repoMock.Setup(x => x.GetRequiredTasks(101, false)).ReturnsAsync(requiredTasks);
        _repoMock.Setup(x => x.GetRegistrationTasks(registrationId)).ReturnsAsync(existingTasks);
        _repoMock.Setup(x => x.GetMaterialsByRegistrationId(registrationId)).ReturnsAsync(new List<RegistrationMaterial>());

        var query = new GetRegistrationOverviewDetailByIdQuery { Id = registrationId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Materials.Should().BeEmpty();
    }
}
