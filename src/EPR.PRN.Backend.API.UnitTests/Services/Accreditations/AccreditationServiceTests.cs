using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.Interfaces.Accreditations;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using AutoMapper;
using EPR.PRN.Backend.API.Profiles;

namespace EPR.PRN.Backend.API.UnitTests.Services.Accreditations;

[TestClass]
public class AccreditationServiceTests
{
    private Mock<IAccreditationRepository> _repositoryMock;
    private Mock<ILogger<AccreditationService>> _loggerMock;
    private Mock<IConfiguration> _configurationMock;
    private AccreditationService _service;

    [TestInitialize]
    public void Setup()
    {
        _repositoryMock = new Mock<IAccreditationRepository>();
        _loggerMock = new Mock<ILogger<AccreditationService>>();
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c["LogPrefix"]).Returns("[EPR.PRN.Backend]");

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AccreditationProfile>();
        });
        var mapper = config.CreateMapper();

        _service = new AccreditationService(
            _repositoryMock.Object,
            mapper,
            _loggerMock.Object,
            _configurationMock.Object
        );
    }

    [TestMethod]
    public async Task GetOrCreateAccreditation_Should_Create_WhenAccreditation_NotExists()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var materialId = 2;
        var applicationTypeId = 1;
        var accreditationStatusId = 1;
        AccreditationEntity accreditation = null!;

        _repositoryMock
            .Setup(r => r.GetAccreditationDetails(organisationId,materialId,applicationTypeId))
            .ReturnsAsync(accreditation);

        // Act
        await _service.GetOrCreateAccreditation(
            organisationId,
            materialId,
            applicationTypeId);

        // Assert
        _repositoryMock.Verify(r => r.Create(It.Is<AccreditationEntity>(x =>
            x.OrganisationId == organisationId &&
            x.RegistrationMaterialId == materialId &&
            x.ApplicationTypeId == applicationTypeId &&
            x.AccreditationStatusId == accreditationStatusId
            )), Times.Once);

    }

    [TestMethod]
    public async Task GetOrCreateAccreditation_Should_Get_WhenAccreditation_Exists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var organisationId = Guid.NewGuid();
        var materialId = 2;
        var applicationTypeId = 1;
        var accreditationStatusId = 1;

        var accreditation = new AccreditationEntity
        {
            ExternalId = id,
            OrganisationId = organisationId,
            RegistrationMaterialId = materialId,
            ApplicationTypeId = applicationTypeId,
            AccreditationStatusId = accreditationStatusId
        };

        _repositoryMock
            .Setup(r => r.GetAccreditationDetails(organisationId, materialId, applicationTypeId))
            .ReturnsAsync(accreditation);

        // Act
        var result = await _service.GetOrCreateAccreditation(
            organisationId,
            materialId,
            applicationTypeId);

        // Assert
        _repositoryMock.Verify(r => r.GetAccreditationDetails(organisationId, materialId, applicationTypeId), Times.Once);
        result.Should().Be(id);

        _repositoryMock.Verify(r => r.Create(It.IsAny<AccreditationEntity>()), Times.Never);
    }

    [TestMethod]
    public async Task GetAccreditationById_ReturnsMappedDto_WhenEntityExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = new AccreditationEntity
        {
            ExternalId = id,
            OrganisationId = Guid.NewGuid(),
            RegistrationMaterialId = 1,
            ApplicationTypeId = 2,
            AccreditationStatusId = 3
        };
        var dto = new AccreditationDto
        {
            ExternalId = id,
            OrganisationId = entity.OrganisationId,
            RegistrationMaterialId = entity.RegistrationMaterialId,
            ApplicationTypeId = entity.ApplicationTypeId,
            AccreditationStatusId = entity.AccreditationStatusId
        };

        _repositoryMock.Setup(r => r.GetById(id)).ReturnsAsync(entity);

        // Act
        var result = await _service.GetAccreditationById(id);

        // Assert
        result.Should().NotBeNull();
        result.ExternalId.Should().Be(id);
        _repositoryMock.Verify(r => r.GetById(id), Times.Once);
        result.Should().BeEquivalentTo(dto);
    }

    [TestMethod]
    public async Task GetAccreditationById_ReturnsNull_WhenEntityIsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetById(id)).ReturnsAsync((AccreditationEntity)null);

        // Act
        var result = await _service.GetAccreditationById(id);

        // Assert
        result.Should().BeNull();
        _repositoryMock.Verify(r => r.GetById(id), Times.Once);
    }

    [TestMethod]
    public async Task CreateAccreditation_CallsRepositoryWithCorrectParameters()
    {
        // Arrange
        var requestDto = new AccreditationRequestDto
        {
            OrganisationId = Guid.NewGuid(),
            RegistrationMaterialId = 1,
            ApplicationTypeId = 2,
            AccreditationStatusId = 3,
            PrnTonnageAndAuthoritiesConfirmed = false
        };
        
        // Act
        await _service.CreateAccreditation(requestDto);

        // Assert
        _repositoryMock.Verify(r => r.Create(It.Is<AccreditationEntity>(x => 
            x.OrganisationId == requestDto.OrganisationId &&
            x.RegistrationMaterialId == requestDto.RegistrationMaterialId &&
            x.ApplicationTypeId == requestDto.ApplicationTypeId &&
            x.AccreditationStatusId == requestDto.AccreditationStatusId
            )), Times.Once);
    }

    [TestMethod]
    public async Task UpdateAccreditation_CallsRepositoryWithCorrectParameters()
    {
        // Arrange
        var requestDto = new AccreditationRequestDto
        {
            ExternalId = Guid.NewGuid(),
            OrganisationId = Guid.NewGuid(),
            RegistrationMaterialId = 1,
            ApplicationTypeId = 2,
            AccreditationStatusId = 3
        };
        
        // Act
        await _service.UpdateAccreditation(requestDto);

        // Assert
        _repositoryMock.Verify(r => r.Update(It.Is<AccreditationEntity>(x =>
            x.ExternalId == requestDto.ExternalId &&
            x.OrganisationId == requestDto.OrganisationId &&
            x.RegistrationMaterialId == requestDto.RegistrationMaterialId &&
            x.ApplicationTypeId == requestDto.ApplicationTypeId &&
            x.AccreditationStatusId == requestDto.AccreditationStatusId
            )), Times.Once);
    }
}
