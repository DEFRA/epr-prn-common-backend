using System.Collections.Generic;
using AutoMapper;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Profiles;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Accreditation;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Services.Accreditations;

[TestClass]
public class AccreditationPrnIssueAuthServiceTests
{
    private Mock<IAccreditationPrnIssueAuthRepository> _repositoryMock;
    private Mock<ILogger<AccreditationPrnIssueAuthService>> _loggerMock;
    private Mock<IConfiguration> _configurationMock;
    private AccreditationPrnIssueAuthService _service;

    [TestInitialize]
    public void Setup()
    {
        _repositoryMock = new Mock<IAccreditationPrnIssueAuthRepository>();
        _loggerMock = new Mock<ILogger<AccreditationPrnIssueAuthService>>();
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c["LogPrefix"]).Returns("[EPR.PRN.Backend]");

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AccreditationProfile>();
        });
        var mapper = config.CreateMapper();

        _service = new AccreditationPrnIssueAuthService(
            _repositoryMock.Object,
            mapper,
            _loggerMock.Object,
            _configurationMock.Object
        );
    }

    [TestMethod]
    public async Task GetByAccreditationId_ReturnsMappedDtos_WhenEntitiesExist()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var entities = new List<AccreditationPrnIssueAuth>
        {
            new AccreditationPrnIssueAuth { ExternalId = Guid.NewGuid(), AccreditationExternalId = accreditationId, PersonExternalId = Guid.NewGuid() },
            new AccreditationPrnIssueAuth { ExternalId = Guid.NewGuid(), AccreditationExternalId = accreditationId, PersonExternalId = Guid.NewGuid() }
        };
        var dtos = new List<AccreditationPrnIssueAuthDto>
        {
            new AccreditationPrnIssueAuthDto { ExternalId = entities[0].ExternalId, AccreditationExternalId = accreditationId, PersonExternalId = entities[0].PersonExternalId },
            new AccreditationPrnIssueAuthDto { ExternalId = entities[1].ExternalId, AccreditationExternalId = accreditationId, PersonExternalId = entities[1].PersonExternalId }
        };

        _repositoryMock.Setup(r => r.GetByAccreditationId(accreditationId)).ReturnsAsync(entities);

        // Act
        var result = await _service.GetByAccreditationId(accreditationId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(dtos);
        _repositoryMock.Verify(r => r.GetByAccreditationId(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task GetByAccreditationId_ReturnsEmptyList_WhenNoEntities()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var entities = new List<AccreditationPrnIssueAuth>();

        _repositoryMock.Setup(r => r.GetByAccreditationId(accreditationId)).ReturnsAsync(entities);

        // Act
        var result = await _service.GetByAccreditationId(accreditationId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _repositoryMock.Verify(r => r.GetByAccreditationId(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task ReplaceAllByAccreditationId_CallsRepositoryWithCorrectParameters()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var request = new List<AccreditationPrnIssueAuthRequestDto>
        {
            new AccreditationPrnIssueAuthRequestDto { PersonExternalId = Guid.NewGuid() },
            new AccreditationPrnIssueAuthRequestDto { PersonExternalId = Guid.NewGuid() }
        };
        
        // Act
        await _service.ReplaceAllByAccreditationId(accreditationId, request);

        // Assert
        _repositoryMock.Verify(r => r.ReplaceAllByAccreditationId(accreditationId, It.Is<List<AccreditationPrnIssueAuth>>(x =>
            x[0].PersonExternalId == request[0].PersonExternalId &&
            x[1].PersonExternalId == request[1].PersonExternalId)), Times.Once);
    }
}

