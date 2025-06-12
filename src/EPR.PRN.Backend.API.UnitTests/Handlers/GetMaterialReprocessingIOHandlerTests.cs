using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.API.Profiles.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetMaterialReprocessingIOHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _rmRepositoryMock;
    private IMapper _mapper;
    private GetMaterialReprocessingIOQueryHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _rmRepositoryMock = new Mock<IRegistrationMaterialRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RegistrationMaterialProfile>();
        });
        _mapper = config.CreateMapper();

        _handler = new GetMaterialReprocessingIOQueryHandler(_rmRepositoryMock.Object, _mapper);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialExists()
    {
        // Arrange
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var query = new GetMaterialReprocessingIOQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            ExternalId = materialId,
            RegistrationId = 10,
            MaterialId = 2,
            Material = new LookupMaterial { MaterialName = "Plastic" },
            StatusId = 1,
            Status = new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" },
            RegistrationReprocessingIO = new List<RegistrationReprocessingIO> {
                new RegistrationReprocessingIO
                {
                    ContaminantsTonne = 1,
                    NonUKPackagingWasteTonne = 2,
                    NotPackingWasteTonne = 3,
                    ProcessLossTonne = 4,
                    ReprocessingPackagingWasteLastYearFlag = true,
                    SenttoOtherSiteTonne = 5,
                    UKPackagingWasteTonne = 6,
                    TotalInputs = 7,
                    TotalOutputs = 8,
                    PlantEquipmentUsed = "shredder",
                    TypeOfSupplier = "Shed"
                }
            }
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationMaterialById(materialId))
            .ReturnsAsync(materialEntity);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.MaterialName.Should().Be("Plastic");
            result.ContaminantsTonne.Should().Be(1);
            result.NonUKPackagingWasteTonne.Should().Be(2);
            result.NotPackingWasteTonne.Should().Be(3);
            result.ProcessLossTonne.Should().Be(4);
            result.ReprocessingPackagingWasteLastYearFlag.Should().Be(true);
            result.SenttoOtherSiteTonne.Should().Be(5);
            result.UKPackagingWasteTonne.Should().Be(6);
            result.TotalInputs.Should().Be(7);
            result.TotalOutputs.Should().Be(8);
            result.PlantEquipmentUsed.Should().Be("shredder");
            result.SourcesOfPackagingWaste.Should().Be("Shed");
        }
    }
}
