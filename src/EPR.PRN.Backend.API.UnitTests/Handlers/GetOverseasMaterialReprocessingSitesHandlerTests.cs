using AutoMapper;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;

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
    public async Task Handle_ShouldFilterParentSitesAndMapCorrectly()
    {
        // Arrange
        var parentExternalId = Guid.NewGuid();
        var interimExternalId = Guid.NewGuid();

        var interimAddress = new OverseasAddress
        {
            ExternalId = interimExternalId,
            IsInterimSite = true,
            OrganisationName = "BB Ltd",
            AddressLine1 = "Addresss line 1",
            CityOrTown = "testcity"
        };

        var childConnection = new InterimOverseasConnections
        {
            OverseasAddress = interimAddress,
            ParentOverseasAddress = new OverseasAddress
            {
                OrganisationName = "test org",
                AddressLine1 = "address line 1",
                CityOrTown = "testcity"
            }
        };

        var parentAddress = new OverseasAddress
        {
            ExternalId = parentExternalId,
            IsInterimSite = false,
            ChildInterimConnections = new List<InterimOverseasConnections>
            {
                childConnection
            },
            OrganisationName = "BB Ltd",
            AddressLine1 = "Addresss line 1",
            CityOrTown = "testcity"
        };

        var reprocessingSites = new List<OverseasMaterialReprocessingSite>
        {
            new OverseasMaterialReprocessingSite
            {
                OverseasAddress = parentAddress
            },
            new OverseasMaterialReprocessingSite
            {
                OverseasAddress = new OverseasAddress
                {
                    IsInterimSite = true,
                    OrganisationName = "Interim Site",
                    AddressLine1 = "Address line 1",
                    CityOrTown = "Interim City"
                }
            }
        };

        var parentDto = new OverseasMaterialReprocessingSiteDto
        {
            OverseasAddressId = parentExternalId,
            InterimSiteAddresses = new List<InterimSiteAddressDto>(),
            OverseasAddress = new OverseasAddressDto
            {
                OrganisationName = "test",
                AddressLine1 = "test",
                CityOrTown = "test",
                CountryName = "TEST"
            }
        };

        var interimDto = new InterimSiteAddressDto
        {
            AddressLine1 = "Address line 1",
            AddressLine2 = "Address line 2",
            CityOrTown = "TestCity",
            CountryName = "TestCountry",
            OrganisationName = "BB Ltd",
            PostCode = "9999999",
            StateProvince = "teststate"
        };

        _mockRepository
            .Setup(x => x.GetOverseasMaterialReprocessingSites(_registrationMaterialId))
            .ReturnsAsync(reprocessingSites);

        _mockMapper
            .Setup(x => x.Map<IList<OverseasMaterialReprocessingSiteDto>>(It.IsAny<List<OverseasMaterialReprocessingSite>>()))
            .Returns(new List<OverseasMaterialReprocessingSiteDto> { parentDto });

        _mockMapper
            .Setup(x => x.Map<InterimSiteAddressDto>(interimAddress))
            .Returns(interimDto);

        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result.First().InterimSiteAddresses.Should().ContainSingle()
            .Which.Should().Be(interimDto);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnEmptyList_WhenNoParentSitesExist()
    {
        // Arrange
        var sites = new List<OverseasMaterialReprocessingSite>
        {
            new OverseasMaterialReprocessingSite
            {
                OverseasAddress = new OverseasAddress
                {
                    IsInterimSite = true,
                    OrganisationName = "Interim org",
                    AddressLine1 = "Address line 1",
                    CityOrTown = "testinterimcity"
                }
            }
        };

        _mockRepository
            .Setup(x => x.GetOverseasMaterialReprocessingSites(_registrationMaterialId))
            .ReturnsAsync(sites);

        _mockMapper
            .Setup(x => x.Map<IList<OverseasMaterialReprocessingSiteDto>>(It.IsAny<List<OverseasMaterialReprocessingSite>>()))
            .Returns(new List<OverseasMaterialReprocessingSiteDto>());

        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task Handle_ShouldHandleEmptyChildInterimConnections()
    {
        // Arrange
        var parentExternalId = Guid.NewGuid();

        var parentAddress = new OverseasAddress
        {
            ExternalId = parentExternalId,
            IsInterimSite = false,
            ChildInterimConnections = new List<InterimOverseasConnections>(),
            OrganisationName = "Test Org",
            AddressLine1 = "Address line 1",
            CityOrTown = "TestCity"
        };

        var reprocessingSites = new List<OverseasMaterialReprocessingSite>
        {
            new OverseasMaterialReprocessingSite
            {
                OverseasAddress = parentAddress
            }
        };

        var parentDto = new OverseasMaterialReprocessingSiteDto
        {
            OverseasAddressId = parentExternalId,
            InterimSiteAddresses = new List<InterimSiteAddressDto>(),
            OverseasAddress = new OverseasAddressDto
            {
                OrganisationName = "Test Org",
                AddressLine1 = "Address line 1",
                CityOrTown = "Test City",
                CountryName = "TestCountry"
            }
        };

        _mockRepository
            .Setup(x => x.GetOverseasMaterialReprocessingSites(_registrationMaterialId))
            .ReturnsAsync(reprocessingSites);

        _mockMapper
            .Setup(x => x.Map<IList<OverseasMaterialReprocessingSiteDto>>(It.IsAny<List<OverseasMaterialReprocessingSite>>()))
            .Returns(new List<OverseasMaterialReprocessingSiteDto> { parentDto });

        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result.First().InterimSiteAddresses.Should().BeEmpty();
    }

    [TestMethod]
    public async Task Handle_ShouldSkip_WhenParentIsNull()
    {
        // Arrange
        var unmatchedDtoId = Guid.NewGuid();

        var dtoWithNoMatchingParent = new OverseasMaterialReprocessingSiteDto
        {
            OverseasAddressId = unmatchedDtoId,
            InterimSiteAddresses = new List<InterimSiteAddressDto>(),
            OverseasAddress = new OverseasAddressDto
            {
                OrganisationName = "test org",
                AddressLine1 = "Address line 1",
                CityOrTown = "testcity",
                CountryName = "testcountry"
            }
        };

        var existingParent = new OverseasMaterialReprocessingSite
        {
            OverseasAddress = new OverseasAddress
            {
                ExternalId = Guid.NewGuid(), // does not match dtoWithNoMatchingParent.OverseasAddressId
                IsInterimSite = false,
                ChildInterimConnections = new List<InterimOverseasConnections>(),
                OrganisationName = "test org",
                AddressLine1 = "address line 1",
                CityOrTown = "test city"
            }
        };

        _mockRepository
            .Setup(x => x.GetOverseasMaterialReprocessingSites(_registrationMaterialId))
            .ReturnsAsync(new List<OverseasMaterialReprocessingSite> { existingParent });

        _mockMapper
            .Setup(x => x.Map<IList<OverseasMaterialReprocessingSiteDto>>(It.IsAny<List<OverseasMaterialReprocessingSite>>()))
            .Returns(new List<OverseasMaterialReprocessingSiteDto> { dtoWithNoMatchingParent });

        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result.First().InterimSiteAddresses.Should().BeEmpty();
    }

}
