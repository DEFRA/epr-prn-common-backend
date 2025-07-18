using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.DTO.Registration;
using EPR.PRN.Backend.Data.Interfaces;
using FluentAssertions;
using Moq;
namespace EPR.PRN.Backend.API.UnitTests.Handlers;
[TestClass]
public class GetRegistrationsOverviewByOrgIdHandlerTests
{
    private Mock<IRegistrationRepository> _mockRegistrationRepository;
    private GetRegistrationsOverviewByOrgIdHandler _handler;
    private GetRegistrationsOverviewByOrgIdQuery _query;
    private readonly Guid _organisationId = Guid.NewGuid();
    [TestInitialize]
    public void TestInitialize()
    {
        _mockRegistrationRepository = new Mock<IRegistrationRepository>();
        _handler = new GetRegistrationsOverviewByOrgIdHandler(_mockRegistrationRepository.Object);
        _query = new GetRegistrationsOverviewByOrgIdQuery
        {
            OrganisationId = _organisationId
        };
    }
    [TestMethod]
    public async Task Handle_ShouldReturnOverviewDtos_WhenRegistrationsExist()
    {
        // Arrange
        var expectedOverviewDtos = new List<RegistrationOverviewDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                RegistrationMaterialId = 1,
                MaterialId = 1,
                Material = "Plastic",
                MaterialCode = "PLS",
                ApplicationTypeId = 1,
                RegistrationStatus = 2,
                AccreditationStatus = 1,
                AccreditationYear = 2023,
                ReprocessingSiteId = 1,
                ReprocessingSiteAddress = new AddressDto
                {
                    Id = 1,
                    AddressLine1 = "123 Test St",
                    AddressLine2 = "Test Area",
                    TownCity = "Testville",
                    County = "Test County",
                    PostCode = "TST 123",
                    NationId = 1,
                    GridReference = "GB1234567890"
                },
                RegistrationYear = 2023
            }
        };
        _mockRegistrationRepository
            .Setup(repo => repo.GetRegistrationsOverviewForOrgIdAsync(_organisationId))
            .ReturnsAsync(expectedOverviewDtos);
        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedOverviewDtos);
    }
    [TestMethod]
    public async Task Handle_ShouldReturnEmptyList_WhenNoRegistrationsExist()
    {
        // Arrange
        _mockRegistrationRepository
            .Setup(repo => repo.GetRegistrationsOverviewForOrgIdAsync(_organisationId))
            .ReturnsAsync(new List<RegistrationOverviewDto>());
        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
    [TestMethod]
    public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
    {
        // Arrange
        _mockRegistrationRepository
            .Setup(repo => repo.GetRegistrationsOverviewForOrgIdAsync(_organisationId))
            .ThrowsAsync(new Exception("Unexpected error"));
        // Act
        Func<Task> act = async () => await _handler.Handle(_query, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Unexpected error");
    }
}
