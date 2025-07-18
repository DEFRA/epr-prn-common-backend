using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class UpsertInterimSiteHandlerTests
{
    private Mock<IMaterialRepository> _mockRepository;
    private UpsertInterimSiteHandler _handler;
    private UpsertInterimSiteCommand _command;
    private readonly Guid _registrationMaterialId = Guid.NewGuid();

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IMaterialRepository>();
        _handler = new UpsertInterimSiteHandler(_mockRepository.Object);

        _command = new UpsertInterimSiteCommand
        {
            InterimSitesRequestDto = new SaveInterimSitesRequestDto
            {
                RegistrationMaterialId = _registrationMaterialId,
                OverseasMaterialReprocessingSites = new List<OverseasMaterialReprocessingSiteDto>
                {
                    new OverseasMaterialReprocessingSiteDto
                    {
                        OverseasAddressId = Guid.NewGuid(),
                        InterimSiteAddresses = new List<InterimSiteAddressDto>
                        {
                            new InterimSiteAddressDto
                            {
                                OrganisationName = "Interim A",
                                AddressLine1 = "Address line 1",
                                AddressLine2 = "Address line 2",
                                CityOrTown = "Test City",
                                CountryName = "TestCountry",
                                PostCode = "987654"
                                
                            },
                            new InterimSiteAddressDto
                            {
                                OrganisationName = "Interim B",
                                AddressLine1 = "Address line 1",
                                AddressLine2 = "Address line 2",
                                CityOrTown = "Test City",
                                CountryName = "TestCountry",
                                PostCode = "987654"
                            }
                        },
                        OverseasAddress = null
                    }
                }
            }
        };
    }

    [TestMethod]
    public async Task Handle_ShouldAssignParentExternalIdAndCallRepository()
    {
        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        var parentId = _command.InterimSitesRequestDto!.OverseasMaterialReprocessingSites.First().OverseasAddressId;

        foreach (var interim in _command.InterimSitesRequestDto.OverseasMaterialReprocessingSites.SelectMany(p => p.InterimSiteAddresses))
        {
            interim.ParentExternalId.Should().Be(parentId);
        }

        _mockRepository.Verify(r => r.SaveInterimSitesAsync(_command.InterimSitesRequestDto), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldNotFail_WhenNoInterimSites()
    {
        // Arrange
        _command.InterimSitesRequestDto!.OverseasMaterialReprocessingSites = new List<OverseasMaterialReprocessingSiteDto>
        {
            new OverseasMaterialReprocessingSiteDto
            {
                OverseasAddressId = Guid.NewGuid(),
                InterimSiteAddresses = null,
                OverseasAddress = new OverseasAddressDto
                {
                    OrganisationName = "Parent Org",
                    AddressLine1 = "Address line 1",
                    AddressLine2 = "Address line 2",
                    CityOrTown = "Test City",
                    CountryName = "TestCountry",
                    PostCode = "987654"
                }
            }
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        _mockRepository.Verify(r => r.SaveInterimSitesAsync(_command.InterimSitesRequestDto), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldNotFail_WhenNoParentSites()
    {
        // Arrange
        _command.InterimSitesRequestDto!.OverseasMaterialReprocessingSites = [];

        // Act
        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        _mockRepository.Verify(r => r.SaveInterimSitesAsync(_command.InterimSitesRequestDto), Times.Once);
    }
}
