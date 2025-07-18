using AutoFixture;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Controllers.Accreditation;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DTO.Accreditiation;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

[TestClass]
public class AccreditationControllerTests
{
    private Mock<IAccreditationService> _serviceMock;
    private Mock<IAccreditationFileUploadService> _fileUploadServiceMock;
    private Mock<IMediator> _mediatorMock;
    private Mock<ILogger<AccreditationController>> _loggerMock;
    private AccreditationController _controller;
    private static readonly IFixture _fixture = new Fixture();

    [TestInitialize]
    public void SetUp()
    {
        _serviceMock = new Mock<IAccreditationService>();
        _fileUploadServiceMock = new Mock<IAccreditationFileUploadService>();
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<AccreditationController>>();
        _controller = new AccreditationController(_serviceMock.Object, _fileUploadServiceMock.Object, _mediatorMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task GetOrCreateAccreditation_ShouldReturnOk()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var organisationId = Guid.NewGuid();
        var materialId = 2;
        var applicationTypeId = 1;
        var accreditation = new AccreditationDto { ExternalId = accreditationId };

        _serviceMock.Setup(s => s.GetOrCreateAccreditation(organisationId, materialId, applicationTypeId))
            .ReturnsAsync(accreditationId);

        // Act
        var result = await _controller.Get(organisationId, materialId, applicationTypeId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(accreditationId);
        _serviceMock.Verify(s => s.GetOrCreateAccreditation(organisationId, materialId, applicationTypeId), Times.Once);
    }

    [TestMethod]
    public async Task Get_ShouldReturnOk_WhenAccreditationExists()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var accreditation = new AccreditationDto { ExternalId = accreditationId };
        _serviceMock.Setup(s => s.GetAccreditationById(accreditationId))
            .ReturnsAsync(accreditation);

        // Act
        var result = await _controller.Get(accreditationId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(accreditation);
        _serviceMock.Verify(s => s.GetAccreditationById(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task Get_ShouldReturnNotFound_WhenAccreditationDoesNotExist()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetAccreditationById(accreditationId))
            .ReturnsAsync((AccreditationDto)null);

        // Act
        var result = await _controller.Get(accreditationId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        _serviceMock.Verify(s => s.GetAccreditationById(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task PostWithNewAccreditation_ShouldReturnOk_WithCreatedAccreditation()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var request = new AccreditationRequestDto { OrganisationId = Guid.NewGuid(), AccreditationYear = 2026 };
        _serviceMock.Setup(s => s.CreateAccreditation(request))
            .ReturnsAsync(accreditationId);

        var accreditation = new AccreditationDto { ExternalId = accreditationId, AccreditationYear = 2026 };
        _serviceMock.Setup(s => s.GetAccreditationById(accreditationId))
            .ReturnsAsync(accreditation);

        // Act
        var result = await _controller.Post(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(accreditation);
        _serviceMock.Verify(s => s.CreateAccreditation(request), Times.Once);
    }

    [TestMethod]
    public async Task PostWithExistingAccreditation_ShouldReturnOk_WithUpdatedAccreditation()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var request = new AccreditationRequestDto { ExternalId = accreditationId, OrganisationId = Guid.NewGuid(), AccreditationYear = 2026 };

        var accreditation = new AccreditationDto { ExternalId = accreditationId, OrganisationId = Guid.NewGuid(), AccreditationYear = 2026 };
        _serviceMock.Setup(s => s.GetAccreditationById(accreditationId))
            .ReturnsAsync(accreditation);

        // Act
        var result = await _controller.Post(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(accreditation);
        _serviceMock.Verify(s => s.UpdateAccreditation(request), Times.Once);
    }

    [TestMethod]
    public async Task GetFileUpload_ShouldReturnBadRequest_WhenExternalIdIsInvalid()
    {
        // Arrange
        var externalId = Guid.NewGuid();

        // Act
        var result = await _controller.GetFileUpload(externalId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public async Task GetFileUpload_ShouldReturnOk_WhenParamsAreValid()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var fileUpload = _fixture.Create<AccreditationFileUploadDto>();

        _fileUploadServiceMock.Setup(s => s.GetByExternalId(externalId))
            .ReturnsAsync(fileUpload);

        // Act
        var result = await _controller.GetFileUpload(externalId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(fileUpload);

        _fileUploadServiceMock.VerifyAll();
    }

    [TestMethod]
    public async Task GetFileUploads_ShouldReturnBadRequest_WhenFileUploadTypeIdIsInvalid()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();

        // Act
        var result = await _controller.GetFileUploads(accreditationId, 0, (int)AccreditationFileUploadStatus.UploadComplete); // Zero is invalid.

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestObjectResult = result as BadRequestObjectResult;
        badRequestObjectResult!.Value.Should().Be("FileUploadTypeId is invalid");
    }

    [TestMethod]
    public async Task GetFileUploads_ShouldReturnBadRequest_WhenFileUploadStatusIdIsInvalid()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();

        // Act
        var result = await _controller.GetFileUploads(accreditationId, (int)AccreditationFileUploadType.SamplingAndInspectionPlan, 0); // Zero is invalid.

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestObjectResult = result as BadRequestObjectResult;
        badRequestObjectResult!.Value.Should().Be("FileUploadStatusId is invalid");
    }

    [TestMethod]
    public async Task GetFileUploads_ShouldReturnOk_WhenParamsAreValid()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var fileUploads = _fixture.CreateMany<AccreditationFileUploadDto>(3).ToList();

        _fileUploadServiceMock.Setup(s => s.GetByAccreditationId(accreditationId, (int)AccreditationFileUploadType.SamplingAndInspectionPlan, (int)AccreditationFileUploadStatus.UploadComplete))
            .ReturnsAsync(fileUploads);

        // Act
        var result = await _controller.GetFileUploads(accreditationId, (int)AccreditationFileUploadType.SamplingAndInspectionPlan, (int)AccreditationFileUploadStatus.UploadComplete);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(fileUploads);

        _fileUploadServiceMock.VerifyAll();
    }

    [TestMethod]
    public async Task UpsertFileUpload_ShouldCallCreateMethod_WhenExternalIdIsNull()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var fileUploadId = Guid.NewGuid();

        var fileUpload = _fixture.Create<AccreditationFileUploadDto>();
        fileUpload.ExternalId = null;

        _fileUploadServiceMock.Setup(s => s.CreateFileUpload(accreditationId, fileUpload))
            .ReturnsAsync(fileUploadId);

        _fileUploadServiceMock.Setup(s => s.GetByExternalId(fileUploadId))
            .ReturnsAsync(fileUpload);

        // Act
        var result = await _controller.UpsertFileUpload(accreditationId, fileUpload);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(fileUpload);

        _fileUploadServiceMock.VerifyAll();
    }

    [TestMethod]
    public async Task UpsertFileUpload_ShouldCallUpdateMethod_WhenExternalIdIsPopulated()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var fileUploadId = Guid.NewGuid();

        var fileUpload = _fixture.Create<AccreditationFileUploadDto>();
        fileUpload.ExternalId = fileUploadId;

        _fileUploadServiceMock.Setup(s => s.UpdateFileUpload(accreditationId, fileUpload));

        _fileUploadServiceMock.Setup(s => s.GetByExternalId(fileUploadId))
            .ReturnsAsync(fileUpload);

        // Act
        var result = await _controller.UpsertFileUpload(accreditationId, fileUpload);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(fileUpload);

        _fileUploadServiceMock.VerifyAll();
    }

    [TestMethod]
    public async Task DeleteFileUpload_ShouldReturnOk_AfterCallingDeleteService()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var fileId = Guid.NewGuid();

        _fileUploadServiceMock.Setup(s => s.DeleteFileUpload(accreditationId, fileId));

        // Act
        var result = await _controller.DeleteFileUpload(accreditationId, fileId);

        // Assert
        result.Should().BeOfType<OkResult>();

        _fileUploadServiceMock.VerifyAll();
    }

    [TestMethod]
    public async Task GetAccreditationsOverviewForOrgId_ValidInputReturnsOkResult()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var query = new GetAccreditationsOverviewByOrgIdQuery { OrganisationId = organisationId };
        var expectedResult = new List<AccreditationOverviewDto>
        {
            new AccreditationOverviewDto
            {
                OrganisationId = organisationId
            },
            new AccreditationOverviewDto
            {
                OrganisationId = organisationId,
            }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAccreditationsOverviewByOrgIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetAccreditationsOverviewForOrgId(organisationId);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(expectedResult);
        }
    }

    [TestMethod]
    public async Task GetAccreditationsOverviewForOrgId_InvalidInputThrows()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var query = new GetAccreditationsOverviewByOrgIdQuery();
        var expectedResult = new List<AccreditationOverviewDto>
        {
            new AccreditationOverviewDto
            {
                OrganisationId = organisationId
            },
            new AccreditationOverviewDto
            {
                OrganisationId = organisationId,
            }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAccreditationsOverviewByOrgIdQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FluentValidation.ValidationException("Invalid organisation ID"));

        // Act
        Func<Task> act = async () => await _controller.GetAccreditationsOverviewForOrgId(organisationId);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .WithMessage("Invalid organisation ID");
    }
}