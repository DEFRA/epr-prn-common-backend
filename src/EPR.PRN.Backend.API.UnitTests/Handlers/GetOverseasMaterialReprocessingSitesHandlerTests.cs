using AutoMapper;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetOverseasMaterialReprocessingSitesHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _mockRepository;
    private Mock<IMapper> _mockMapper;
    private GetOverseasMaterialReprocessingSitesHandler _handler;
    private GetOverseasMaterialReprocessingSitesQuery _query;
    private readonly Guid _registrationMaterialId = Guid.NewGuid();

    [TestInitialize]
    public void TestInitialize()
    {
        _mockRepository = new Mock<IRegistrationMaterialRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetOverseasMaterialReprocessingSitesHandler(_mockRepository.Object, _mockMapper.Object);
        _query = new GetOverseasMaterialReprocessingSitesQuery
        {
            RegistrationMaterialId = _registrationMaterialId
        };
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenSitesExist()
    {
        // Arrange
        var siteId = 123;
        var overseasSites = new List<RegistrationMaterial>
        {
            new RegistrationMaterial
            {
                OverseasMaterialReprocessingSites = new List<OverseasMaterialReprocessingSite>
                {
                    new OverseasMaterialReprocessingSite
                    {
                        Id = siteId
                    }
                }
            }
        };

        var expectedDtos = new List<OverseasMaterialReprocessingSiteDto>
        {
            new OverseasMaterialReprocessingSiteDto
            {
                OverseasAddress = new OverseasAddressBaseDto
                {
                    AddressLine1 = "Address line 1",
                    AddressLine2 = "Address line 2",
                    CityorTown = "lisbon",
                    Country = "portugal",
                    OrganisationName = "bb ltd",
                    PostCode = "123-456-789",
                    StateProvince = "samplestate"
                }
            }
        };

        _mockRepository.Setup(r => r.GetOverseasMaterialReprocessingSites(_registrationMaterialId))
            .ReturnsAsync(overseasSites);

        _mockMapper.Setup(m => m.Map<IList<OverseasMaterialReprocessingSiteDto>>(It.IsAny<object>()))
            .Returns(expectedDtos);

        // Act
        var result = await _handler.Handle(_query, default);

        // Assert
        result.Should().BeEquivalentTo(expectedDtos);
        _mockRepository.Verify(r => r.GetOverseasMaterialReprocessingSites(_registrationMaterialId), Times.Once);
        _mockMapper.Verify(m => m.Map<IList<OverseasMaterialReprocessingSiteDto>>(overseasSites), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnEmptyList_WhenNoSitesExist()
    {
        // Arrange
        var overseasSites = new List<RegistrationMaterial>();
        var expectedDtos = new List<OverseasMaterialReprocessingSiteDto>();

        _mockRepository.Setup(r => r.GetOverseasMaterialReprocessingSites(_registrationMaterialId))
            .ReturnsAsync(overseasSites);

        _mockMapper.Setup(m => m.Map<IList<OverseasMaterialReprocessingSiteDto>>(overseasSites))
            .Returns(expectedDtos);

        // Act
        var result = await _handler.Handle(_query, default);

        // Assert
        result.Should().BeEmpty();
    }
}