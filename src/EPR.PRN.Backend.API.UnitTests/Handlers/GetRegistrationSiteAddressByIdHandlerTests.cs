using AutoMapper;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Profiles;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetRegistrationSiteAddressByIdHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _rmRepositoryMock;
    private IMapper _mapper;
    private GetRegistrationSiteAddressByIdHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _rmRepositoryMock = new Mock<IRegistrationMaterialRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RegistrationMaterialProfile>(); 
        });
        _mapper = config.CreateMapper();

        _handler = new GetRegistrationSiteAddressByIdHandler(_rmRepositoryMock.Object, _mapper);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenReprocessingSiteAddressExists()
    {
        // Arrange
        int registrationId = 55;
        var query = new GetRegistrationSiteAddressByIdQuery { Id = registrationId };

        var registration = new Registration
        {
            Id = registrationId,
            ReprocessingSiteAddressId = 99,
            ReprocessingSiteAddress = new Address
            {
                AddressLine1 = "42 Recycle Road",
                TownCity = "EcoTown",
                PostCode = "ET1 9XY"
            }
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationById(registrationId))
            .ReturnsAsync(registration);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.SiteAddress.Should().Contain("Recycle Road"); 
    }

    [TestMethod]
    public async Task Handle_ShouldReturnEmptyDto_WhenReprocessingSiteAddressIsNull()
    {
        // Arrange
        int registrationId = 56;
        var query = new GetRegistrationSiteAddressByIdQuery { Id = registrationId };

        var registration = new Registration
        {
            Id = registrationId,
            ReprocessingSiteAddressId = null
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationById(registrationId))
            .ReturnsAsync(registration);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.SiteAddress.Should().BeNullOrEmpty();
    }
}
