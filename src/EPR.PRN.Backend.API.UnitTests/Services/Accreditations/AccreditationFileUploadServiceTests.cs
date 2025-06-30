using AutoFixture;
using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Profiles;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.Interfaces.Accreditations;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Services.Accreditations;

[TestClass]
public class AccreditationFileUploadServiceTests
{
    private Mock<IAccreditationFileUploadRepository> _repositoryMock;
    private Mock<ILogger<AccreditationFileUploadService>> _loggerMock;
    private Mock<IConfiguration> _configurationMock;
    private AccreditationFileUploadService _service;
    private static readonly IFixture _fixture = new Fixture();

    [TestInitialize]
    public void Setup()
    {
        _repositoryMock = new Mock<IAccreditationFileUploadRepository>();
        _loggerMock = new Mock<ILogger<AccreditationFileUploadService>>();
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c["LogPrefix"]).Returns("[EPR.PRN.Backend]");

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AccreditationProfile>();
        });
        var mapper = config.CreateMapper();

        _service = new AccreditationFileUploadService(
            _repositoryMock.Object,
            _loggerMock.Object,
            _configurationMock.Object
        );
    }

    [TestMethod]
    public async Task GetByExternalId_ReturnsMappedDto_WhenEntityExists()
    {
        // Arrange
        var fileUploadId = Guid.NewGuid();
        int fileUploadTypeId = (int)AccreditationFileUploadType.SamplingAndInspectionPlan;
        int fileUploadStatusId = (int)AccreditationFileUploadStatus.UploadComplete;

        var entity = new AccreditationFileUpload
        {
            ExternalId = Guid.NewGuid(),
            FileId = Guid.NewGuid(),
            FileName = "file1.txt",
            FileUploadTypeId = fileUploadTypeId,
            FileUploadStatusId = fileUploadStatusId,
            UploadedBy = "A N Other",
            UploadedOn = DateTime.UtcNow,
            OverseasSiteId = 100
        };

        var expectedDto = new AccreditationFileUploadDto
        {
            ExternalId = entity.ExternalId,
            FileId = entity.FileId,
            Filename = entity.FileName,
            FileUploadTypeId = entity.FileUploadTypeId,
            FileUploadStatusId = entity.FileUploadStatusId,
            UploadedBy = entity.UploadedBy,
            UploadedOn = entity.UploadedOn,
            OverseasSiteId = entity.OverseasSiteId
        };

        _repositoryMock.Setup(r => r.GetByExternalId(fileUploadId)).ReturnsAsync(entity);

        // Act
        var result = await _service.GetByExternalId(fileUploadId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedDto);
        _repositoryMock.VerifyAll();
    }

    [TestMethod]
    public async Task GetByAccreditationId_ReturnsMappedDtos_WhenEntitiesExist()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        int fileUploadTypeId = (int)AccreditationFileUploadType.SamplingAndInspectionPlan;
        int fileUploadStatusId = (int)AccreditationFileUploadStatus.UploadComplete;

        var entities = new List<AccreditationFileUpload>
        {
            new AccreditationFileUpload
            {
                ExternalId = Guid.NewGuid(),
                FileId = Guid.NewGuid(),
                FileName = "file1.txt",
                FileUploadTypeId = fileUploadTypeId,
                FileUploadStatusId = fileUploadStatusId,
                UploadedBy = "A N Other",
                UploadedOn = DateTime.UtcNow,
                OverseasSiteId = 100
            },
            new AccreditationFileUpload
            {
                ExternalId = Guid.NewGuid(),
                FileId = Guid.NewGuid(),
                FileName = "file2.txt",
                FileUploadTypeId = fileUploadTypeId,
                FileUploadStatusId = fileUploadStatusId,
                UploadedBy = "Joe Bloggs",
                UploadedOn = DateTime.UtcNow,
                OverseasSiteId = 101
            }
        };

        var expectedDtos = new List<AccreditationFileUploadDto>
        {
            new AccreditationFileUploadDto
            {
                ExternalId = entities[0].ExternalId,
                FileId = entities[0].FileId,
                Filename = entities[0].FileName,
                FileUploadTypeId = entities[0].FileUploadTypeId,
                FileUploadStatusId = entities[0].FileUploadStatusId,
                UploadedBy = entities[0].UploadedBy,
                UploadedOn = entities[0].UploadedOn,
                OverseasSiteId = entities[0].OverseasSiteId
            },
            new AccreditationFileUploadDto
            {
                ExternalId = entities[1].ExternalId,
                FileId = entities[1].FileId,
                Filename = entities[1].FileName,
                FileUploadTypeId = entities[1].FileUploadTypeId,
                FileUploadStatusId = entities[1].FileUploadStatusId,
                UploadedBy = entities[1].UploadedBy,
                UploadedOn = entities[1].UploadedOn,
                OverseasSiteId = entities[1].OverseasSiteId
            }
        };

        _repositoryMock.Setup(r => r.GetByAccreditationId(accreditationId, fileUploadTypeId, fileUploadStatusId)).ReturnsAsync(entities);

        // Act
        var result = await _service.GetByAccreditationId(accreditationId, fileUploadTypeId, fileUploadStatusId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedDtos);
        _repositoryMock.VerifyAll();
    }

    [TestMethod]
    public async Task CreateFileUpload_PassesCorrectlyMappedEntityToRepository()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var fileUploadId = Guid.NewGuid();
        int fileUploadTypeId = (int)AccreditationFileUploadType.SamplingAndInspectionPlan;
        int fileUploadStatusId = (int)AccreditationFileUploadStatus.UploadComplete;

        var dto = new AccreditationFileUploadDto
        {
            ExternalId = null,
            FileId = Guid.NewGuid(),
            Filename = "file1.txt",
            FileUploadTypeId = fileUploadTypeId,
            FileUploadStatusId = fileUploadStatusId,
            UploadedBy = "A N Other",
            UploadedOn = DateTime.UtcNow,
            OverseasSiteId = 100
        };

        _repositoryMock.Setup(r => r.Create(accreditationId, It.Is<AccreditationFileUpload>(x =>
                x.ExternalId == Guid.Empty &&
                x.FileId == dto.FileId &&
                x.FileName == dto.Filename &&
                x.FileUploadTypeId == dto.FileUploadTypeId &&
                x.FileUploadStatusId == dto.FileUploadStatusId &&
                x.UploadedBy == dto.UploadedBy &&
                x.UploadedOn == dto.UploadedOn &&
                x.OverseasSiteId == dto.OverseasSiteId
            ))).ReturnsAsync(fileUploadId);

        // Act
        var result = await _service.CreateFileUpload(accreditationId, dto);

        // Assert
        result.Should().Be(fileUploadId);
        _repositoryMock.VerifyAll();
    }

    [TestMethod]
    public async Task UpdateFileUpload_PassesCorrectlyMappedEntityToRepository()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var fileUploadId = Guid.NewGuid();
        int fileUploadTypeId = (int)AccreditationFileUploadType.SamplingAndInspectionPlan;
        int fileUploadStatusId = (int)AccreditationFileUploadStatus.UploadComplete;

        var dto = new AccreditationFileUploadDto
        {
            ExternalId = fileUploadId,
            FileId = Guid.NewGuid(),
            Filename = "file1.txt",
            FileUploadTypeId = fileUploadTypeId,
            FileUploadStatusId = fileUploadStatusId,
            UploadedBy = "A N Other",
            UploadedOn = DateTime.UtcNow,
            OverseasSiteId = 100
        };

        _repositoryMock.Setup(r => r.Update(accreditationId, It.Is<AccreditationFileUpload>(x =>
                x.ExternalId == fileUploadId &&
                x.FileId == dto.FileId &&
                x.FileName == dto.Filename &&
                x.FileUploadTypeId == dto.FileUploadTypeId &&
                x.FileUploadStatusId == dto.FileUploadStatusId &&
                x.UploadedBy == dto.UploadedBy &&
                x.UploadedOn == dto.UploadedOn &&
                x.OverseasSiteId == dto.OverseasSiteId
            )));

        // Act
        await _service.UpdateFileUpload(accreditationId, dto);

        // Assert
        _repositoryMock.VerifyAll();
    }

    [TestMethod]
    public async Task DeleteFileUpload_PassesCorrectParamsToRepository()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var fileId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.Delete(accreditationId, fileId));

        // Act
        await _service.DeleteFileUpload(accreditationId, fileId);

        // Assert
        _repositoryMock.VerifyAll();
    }
}

