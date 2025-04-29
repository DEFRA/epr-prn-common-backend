using AutoMapper;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Profiles;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetMaterialWasteLicenceQueryHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _rmRepositoryMock;
    private IMapper _mapper;
    private GetMaterialWasteLicencesQueryHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _rmRepositoryMock = new Mock<IRegistrationMaterialRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RegistrationMaterialProfile>();
        });
        _mapper = config.CreateMapper();

        _handler = new GetMaterialWasteLicencesQueryHandler(_rmRepositoryMock.Object, _mapper);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialExists()
    {
        // Arrange
        int materialId = 1;
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            Id = materialId,
            RegistrationId = 10,
            MaterialId = 2,
            Material = new LookupMaterial { MaterialName = "Plastic" },
            StatusID = 1,
            Status = new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" },
            PermitType = new LookupMaterialPermit { Name = PermitTypes.WasteExemption },
            MaterialExemptionReferences = new List<MaterialExemptionReference>
            {
                new MaterialExemptionReference
                {
                    ReferenceNo = "1"
                },
                new MaterialExemptionReference
                {
                    ReferenceNo = "2"
                }
            },
            MaximumReprocessingCapacityTonne = 123,
            MaximumReprocessingPeriod = new LookupPeriod { Name = "Per Year"},
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationMaterial_WasteLicencesById(materialId))
            .ReturnsAsync(materialEntity);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.MaterialName.Should().Be("Plastic");
        result.PermitType.Should().Be(PermitTypes.WasteExemption);
        result.LicenceNumbers.First().Should().Be("1");
        result.LicenceNumbers.Skip(1).First().Should().Be("2");
        result.MaximumReprocessingCapacityTonne.Should().Be(123);
        result.MaximumReprocessingPeriod.Should().Be("Per Year");

    }
}