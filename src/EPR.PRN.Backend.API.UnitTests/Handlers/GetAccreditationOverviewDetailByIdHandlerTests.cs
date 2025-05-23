using AutoMapper;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Profiles;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetAccreditationOverviewDetailByIdHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _repoMock;
    private IMapper _mapper;
    private GetAccreditationOverviewDetailByIdHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _repoMock = new Mock<IRegistrationMaterialRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RegistrationMaterialProfile>(); 
        });
        _mapper = config.CreateMapper();

        _handler = new GetAccreditationOverviewDetailByIdHandler(_repoMock.Object, _mapper);
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
                            Tasks = [
                                new RegulatorAccreditationTaskStatus
                                {
                                    Task = new LookupRegulatorTask { Name = "BusinessAddress" },
                                    TaskStatus = new LookupTaskStatus { Name = "Started" }
                                }
                            ]
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
        
        _repoMock.Setup(x => x.GetRegistrationByExternalIdAndYear(registrationId, year)).ReturnsAsync(registration);
        _repoMock.Setup(x => x.GetRequiredTasks(101, false, 2)).ReturnsAsync(requiredTasks);
        _repoMock.Setup(x => x.GetRequiredTasks(101, true, 2)).ReturnsAsync(requiredMaterialTasks);
        
        var query = new GetAccreditationOverviewDetailByIdQuery { Id = registrationId, Year = year };

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

    //[TestMethod]
    //public async Task Handle_WhenNoTasksExist_ShouldCreateNotStartedEntriesForRequiredTasks()
    //{
    //    // Arrange
    //    const int registrationId = 1;

    //    var registration = new Registration
    //    {
    //        Id = registrationId,
    //        ApplicationTypeId = 101,
    //        Tasks = [],
    //        Materials =
    //        [
    //            new RegistrationMaterial
    //            {
    //                Id = 10,
    //                Material = new LookupMaterial { MaterialName = "Plastic" },
    //                IsMaterialRegistered = true,
    //                Tasks = []
    //            }
    //        ]
    //    };

    //    var requiredTasks = new List<LookupRegulatorTask>
    //    {
    //        new() { Name = "SiteAddressAndContactDetails" }
    //    };

    //    var requiredMaterialTasks = new List<LookupRegulatorTask>
    //    {
    //        new() { Name = "BusinessAddress" }
    //    };

    //    _repoMock.Setup(x => x.GetRegistrationById(registrationId)).ReturnsAsync(registration);
    //    _repoMock.Setup(x => x.GetRequiredTasks(101, false, 1)).ReturnsAsync(requiredTasks);
    //    _repoMock.Setup(x => x.GetRequiredTasks(101, true, 1)).ReturnsAsync(requiredMaterialTasks);

    //    var query = new GetRegistrationOverviewDetailByIdQuery { Id = registrationId };

    //    // Act
    //    var result = await _handler.Handle(query, CancellationToken.None);

    //    // Assert
    //    result.Should().NotBeNull();
    //    result.Tasks.Should().HaveCount(1);
    //    result.Tasks[0].TaskName.Should().Be("SiteAddressAndContactDetails");
    //    result.Tasks[0].Status.Should().Be(RegulatorTaskStatus.NotStarted.ToString());

    //    result.Materials.Should().HaveCount(1);
    //    result.Materials[0].Tasks.Should().HaveCount(1);
    //    result.Materials[0].Tasks[0].TaskName.Should().Be("BusinessAddress");
    //    result.Materials[0].Tasks[0].Status.Should().Be(RegulatorTaskStatus.NotStarted.ToString());
    //}

    //[TestMethod]
    //public async Task Handle_ShouldReturnEmptyMaterials_WhenNoMaterialsFound()
    //{
    //    // Arrange
    //    int registrationId = 1;

    //    var registration = new Registration
    //    {
    //        Id = registrationId,
    //        ApplicationTypeId = 101
    //    };

    //    var requiredTasks = new List<LookupRegulatorTask>
    //    {
    //        new() { Name = "SiteAddressAndContactDetails" }
    //    };

    //    var existingTasks = new List<RegulatorRegistrationTaskStatus>
    //    {
    //        new()
    //        {
    //            Task = new LookupRegulatorTask { Name = "SiteAddressAndContactDetails" },
    //            TaskStatus = new LookupTaskStatus { Name = "Completed" }
    //        }
    //    };

    //    _repoMock.Setup(x => x.GetRegistrationById(registrationId)).ReturnsAsync(registration);
    //    _repoMock.Setup(x => x.GetRequiredTasks(101, false, 1)).ReturnsAsync(requiredTasks);
        
    //    var query = new GetRegistrationOverviewDetailByIdQuery { Id = registrationId };

    //    // Act
    //    var result = await _handler.Handle(query, CancellationToken.None);

    //    // Assert
    //    result.Should().NotBeNull();
    //    result.Materials.Should().BeEmpty();
    //}
}
