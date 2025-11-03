using AutoMapper;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Profiles;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Data.Interfaces.Accreditation;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Services.Accreditations;

[TestClass]
public class AccreditationServiceTests
{
    private Mock<IAccreditationRepository> _repositoryMock;
    private Mock<IRegistrationRepository> _registrationRepository;
    private Mock<IRegistrationMaterialRepository> _registrationMaterialRepositoryMock;
    private Mock<ILogger<AccreditationService>> _loggerMock;
    private Mock<IConfiguration> _configurationMock;
    private AccreditationService _service;

    [TestInitialize]
    public void Setup()
    {
        _repositoryMock = new Mock<IAccreditationRepository>();
        _registrationRepository = new Mock<IRegistrationRepository>();
        _registrationMaterialRepositoryMock = new Mock<IRegistrationMaterialRepository>();
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
            _registrationRepository.Object,
            _registrationMaterialRepositoryMock.Object,
            mapper,
            _loggerMock.Object,
            _configurationMock.Object
        );
    }

    [TestMethod]
    [DataRow(1, 1)]
    [DataRow(1, 2)]
    [DataRow(1, 3)]
    [DataRow(2, 4)]
    [DataRow(2, 5)]
    [DataRow(2, 6)]
    public async Task GetOrCreateAccreditation_Should_Create_WhenAccreditation_NotExists(int applicationTypeId, int materialId)
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var accreditationStatusId = 1;
        Accreditation accreditation = null!;



        _repositoryMock
            .Setup(r => r.GetAccreditationDetails(organisationId, materialId, applicationTypeId))
            .ReturnsAsync(accreditation);
        // Mock _registrationRepository and _registrationMaterialRepositoryMock to return a material and registration
        Guid regMaterialId = Guid.NewGuid();
        Guid registrationId = Guid.NewGuid();

        _registrationRepository.Setup(Setup => Setup.GetRegistrationByExternalId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Registration
            {
                Id = 1,
                ExternalId = registrationId,
                OrganisationId = organisationId,

            });
        _registrationRepository.Setup(Setup => Setup.CreateRegistrationAsync(It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<AddressDto>()))
            .ReturnsAsync(new Registration
            {
                Id = 1,
                ExternalId = registrationId,
                OrganisationId = organisationId,
            });

        _registrationMaterialRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(new RegistrationMaterial { Id = materialId, RegistrationId = 1 });




        // Act
        await _service.GetOrCreateAccreditation(
            organisationId,
            materialId,
            applicationTypeId);

        // Assert
        _repositoryMock.Verify(r => r.Create(It.Is<Accreditation>(x =>
            //x.OrganisationId == organisationId &&
            x.RegistrationMaterialId == materialId &&
            //x.ApplicationTypeId == applicationTypeId &&
            x.AccreditationStatusId == accreditationStatusId
            )), Times.Once);

    }


    [TestMethod]
    public async Task GetOrCreateAccreditation_Should_Throw_WhenRegistrationNotFound()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var materialId = 2;
        var applicationTypeId = 1;

        _repositoryMock
            .Setup(r => r.GetAccreditationDetails(organisationId, materialId, applicationTypeId))
            .ReturnsAsync((Accreditation)null);

        // Simulate registration not found
        _registrationRepository
            .Setup(r => r.GetRegistrationByExternalId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Registration)null);

        _registrationRepository
            .Setup(r => r.CreateRegistrationAsync(It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<AddressDto>()))
            .ReturnsAsync((Registration)null);

        // Act & Assert
        Func<Task> act = async () => await _service.GetOrCreateAccreditation(
            organisationId,
            materialId,
            applicationTypeId);

        await act.Should().ThrowAsync<Exception>(); // Replace Exception with your specific type if needed
    }

    //[TestMethod]
    //public async Task GetOrCreateAccreditation_Should_Get_WhenAccreditation_Exists() // no longer needed
    //{
    //    // Arrange
    //    var id = Guid.NewGuid();
    //    var organisationId = Guid.NewGuid();
    //    var materialId = 2;
    //    var applicationTypeId = 1;
    //    var accreditationStatusId = 1;

    //    var accreditation = new Accreditation
    //    {
    //        ExternalId = id,
    //        //OrganisationId = organisationId,
    //        RegistrationMaterialId = materialId,
    //        //ApplicationTypeId = applicationTypeId,
    //        AccreditationStatusId = accreditationStatusId,
    //        ApplicationReferenceNumber = "APP-123456",
    //    };

    //    //_repositoryMock
    //    //    .Setup(r => r.GetAccreditationDetails(organisationId, materialId, applicationTypeId))
    //    //    .ReturnsAsync(accreditation);

    //    _registrationRepository.Setup(Setup => Setup.CreateRegistrationAsync(It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<AddressDto>()))
    //        .ReturnsAsync(new Registration
    //        {
    //            Id = 1,
    //            ExternalId = Guid.NewGuid(),
    //            OrganisationId = organisationId,
    //        });

    //    _registrationMaterialRepositoryMock
    //        .Setup(r => r.CreateAsync(It.IsAny<Guid>(), It.IsAny<string>()))
    //        .ReturnsAsync(new RegistrationMaterial { Id = materialId, RegistrationId = 1 });



    //    // Act
    //    var result = await _service.GetOrCreateAccreditation(
    //        organisationId,
    //        materialId,
    //        applicationTypeId);

    //    // Assert
    //    _repositoryMock.Verify(r => r.GetAccreditationDetails(organisationId, materialId, applicationTypeId), Times.Once);
    //    result.Should().Be(id);

    //    _repositoryMock.Verify(r => r.Create(It.IsAny<Accreditation>()), Times.Never);
    //}

    [TestMethod]
    public async Task GetAccreditationById_ReturnsMappedDto_WhenEntityExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = new Accreditation
        {
            Id = 1,
            ExternalId = id,
            //OrganisationId = Guid.NewGuid(),
            RegistrationMaterialId = 1,
            //ApplicationTypeId = 2,
            AccreditationStatusId = 3,
            ApplicationReferenceNumber = "APP-123456",
            AccreditationYear = 2026,
            PRNTonnage = 1000,
        };
        var dto = new AccreditationDto
        {
            ExternalId = id,
            OrganisationId = Guid.NewGuid(),// entity.OrganisationId,
            RegistrationMaterialId = entity.RegistrationMaterialId,
            ApplicationTypeId = 2,//entity.ApplicationTypeId.GetValueOrDefault(),
            AccreditationStatusId = entity.AccreditationStatusId
        };

        _repositoryMock.Setup(r => r.GetById(id)).ReturnsAsync(entity);

        // Act
        var result = await _service.GetAccreditationById(id);


        // Assert
        result.Should().NotBeNull();
        result.ExternalId.Should().Be(id);
        _repositoryMock.Verify(r => r.GetById(id), Times.Once);
        //result.Should().BeEquivalentTo(dto);
        // check entity fields against dto fields
        result.AccreferenceNumber.Should().Be(entity.ApplicationReferenceNumber);
        result.AccreditationYear.Should().Be(entity.AccreditationYear);
        result.PrnTonnage.Should().Be(entity.PRNTonnage);
        //result.OrganisationId.Should().Be(entity.OrganisationId);
        result.RegistrationMaterialId.Should().Be(entity.RegistrationMaterialId);
        //result.ApplicationTypeId.Should().Be(entity.ApplicationTypeId);
        result.AccreditationStatusId.Should().Be(entity.AccreditationStatusId);



    }

    [TestMethod]
    public async Task GetAccreditationById_ReturnsNull_WhenEntityIsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetById(id)).ReturnsAsync((Accreditation)null);

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
        _repositoryMock.Verify(r => r.Create(It.Is<Accreditation>(x =>
            //x.OrganisationId == requestDto.OrganisationId &&
            x.RegistrationMaterialId == requestDto.RegistrationMaterialId &&
            //x.ApplicationTypeId == requestDto.ApplicationTypeId &&
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
        _repositoryMock.Verify(r => r.Update(It.Is<Accreditation>(x =>
            x.ExternalId == requestDto.ExternalId &&
            //x.OrganisationId == requestDto.OrganisationId &&
            x.RegistrationMaterialId == requestDto.RegistrationMaterialId &&
            //x.ApplicationTypeId == requestDto.ApplicationTypeId &&
            x.AccreditationStatusId == requestDto.AccreditationStatusId
            )), Times.Once);
    }
}
