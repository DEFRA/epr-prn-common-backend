using AutoMapper;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Profiles.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetAllRegistrationMaterialsQueryHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _mockRegistrationMaterialRepository;
    private GetAllRegistrationMaterialsQueryHandler _handler;
    private GetAllRegistrationMaterialsQuery _query;
    private readonly Guid _registrationId = Guid.NewGuid();

    [TestInitialize]
    public void TestInitialize()
    {
        _mockRegistrationMaterialRepository = new Mock<IRegistrationMaterialRepository>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RegistrationMaterialProfile>();
        });
        var mapper = config.CreateMapper();
        _handler = new GetAllRegistrationMaterialsQueryHandler(_mockRegistrationMaterialRepository.Object, mapper);
        _query = new GetAllRegistrationMaterialsQuery()
        {
            RegistrationId = _registrationId
        };
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialExists()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var expectedMaterials = new List<ApplicantRegistrationMaterialDto>
        {
            new()
            {
                Id = externalId,
                RegistrationId = _registrationId,
                PPCReprocessingCapacityTonne = 10,
                EnvironmentalPermitWasteManagementTonne = 20,
                InstallationReprocessingTonne = 30,
                WasteManagementReprocessingCapacityTonne = 40,
                MaximumReprocessingCapacityTonne = 100,
                EnvironmentalPermitWasteManagementNumber = "env",
                PPCPermitNumber = "ppc",
                WasteManagementLicenceNumber = "waste",
                InstallationPermitNumber = "install",
                PermitType = new PermitTypeLookupDto
                {
                    Id = 1,
                    Name = "permitType"
                },
                IsMaterialRegistered = true,
                MaterialLookup = new MaterialLookupDto
                {
                    Id = 1,
                    Name = "plastic"
                },
                StatusLookup = new MaterialStatusLookupDto
                {
                    Id = 1,
                    Status = "ready"
                },
                ExemptionReferences =
                [
                    new()
                    {
                        ReferenceNumber = "exemption"
                    }
                ],
                PPCPeriodId = 1,
                InstallationPeriodId = 1,
                WasteManagementPeriodId = 1,
                EnvironmentalPeriodId = 1,
            }
        };

        _mockRegistrationMaterialRepository.Setup(o => o.GetRegistrationMaterialsByRegistrationId(_registrationId))
            .ReturnsAsync(
                new List<RegistrationMaterial>
                {
                    new()
                    {
                        Id = 1,
                        ExternalId = externalId,
                        Registration = new Registration
                        {
                            Id = 1,
                            ExternalId = _registrationId
                        },
                        EnvironmentalPermitWasteManagementNumber = "env",
                        PPCPermitNumber = "ppc",
                        WasteManagementLicenceNumber = "waste",
                        InstallationPermitNumber = "install",
                        PPCReprocessingCapacityTonne = 10,
                        EnvironmentalPermitWasteManagementTonne = 20,
                        InstallationReprocessingTonne = 30,
                        WasteManagementReprocessingCapacityTonne = 40,
                        MaximumReprocessingCapacityTonne = 100,
                        PermitType = new LookupMaterialPermit
                        {
                            Name = "permitType",
                            Id = 1
                        },
                        Material = new LookupMaterial
                        {
                            Id = 1,
                            MaterialCode = "code",
                            MaterialName = "plastic"
                        },
                        Status = new LookupRegistrationMaterialStatus
                        {
                            Name = "ready",
                            Id = 1
                        },
                        MaterialExemptionReferences =
                        [
                            new()
                            {
                                ReferenceNo = "exemption"
                            }
                        ],
                        IsMaterialRegistered = true,
                        PPCPeriodId = 1,
                        InstallationPeriodId = 1,
                        WasteManagementPeriodId = 1,
                        EnvironmentalPermitWasteManagementPeriodId = 1
                    }
                });

        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedMaterials);
    }
}