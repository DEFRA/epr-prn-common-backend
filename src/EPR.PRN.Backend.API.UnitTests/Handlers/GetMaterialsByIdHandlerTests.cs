using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Profiles;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetMaterialsByIdHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _rmRepositoryMock;
    private IMapper _mapper;
    private GetMaterialByIdHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _rmRepositoryMock = new Mock<IRegistrationMaterialRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RegistrationMaterialProfile>();
        });
        _mapper = config.CreateMapper();

        _handler = new GetMaterialByIdHandler(_rmRepositoryMock.Object, _mapper);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialExists()
    {
        // Arrange
        int materialId = 1;
        var query = new GetMaterialDetailByIdQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            Id = materialId,
            RegistrationId = 10,
            MaterialId = 2,
            Material = new LookupMaterial { MaterialName = "Plastic" },
            StatusId = 1,
            Status = new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" }
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationMaterialById(materialId))
            .ReturnsAsync(materialEntity);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(materialEntity.Id);
        result.RegistrationId.Should().Be(materialEntity.RegistrationId);
        result.MaterialName.Should().Be("Plastic");
        result.Status.Should().Be((RegistrationMaterialStatus)1);
    }
}

[TestClass]
public class GetAllMaterialsHandlerTests
{
    private Mock<IMaterialRepository> _mockMaterialRepository;
    private Mock<IMaterialService> _mockMaterialService;
    private GetAllMaterialsQueryHandler _handler;
    private GetAllMaterialsQuery _query;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockMaterialRepository = new Mock<IMaterialRepository>();
        _mockMaterialService = new Mock<IMaterialService>();
        _handler = new GetAllMaterialsQueryHandler(_mockMaterialService.Object);
        _query = new GetAllMaterialsQuery();
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialExists()
    {
        // Arrange
        var materials = new List<MaterialDto>
        {
            new() { Name = "Wood", Code = "W1" },
            new() { Name = "Plastic", Code = "P1" }
        };

        var expectedMaterials = new List<MaterialDto>
        {
            new() { Name = "Wood", Code = "W1" },
            new() { Name = "Plastic", Code = "P1" }
        };

        _mockMaterialService
            .Setup(o => o.GetAllMaterialsAsync(CancellationToken.None))
            .ReturnsAsync(expectedMaterials);

        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedMaterials);
    }
}