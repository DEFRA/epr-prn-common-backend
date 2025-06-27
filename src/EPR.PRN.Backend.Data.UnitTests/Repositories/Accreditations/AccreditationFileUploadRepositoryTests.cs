using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.Repositories.Accreditations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories.Accreditations;

[TestClass]
public class AccreditationFileUploadRepositoryTests
{
    private DbContextOptions<EprAccreditationContext> _dbContextOptions;
    private EprAccreditationContext _dbContext;
    private AccreditationFileUploadRepository _repository;

    private readonly Guid _accreditationId = Guid.NewGuid();

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EprAccreditationContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase" + Guid.NewGuid().ToString())
            .Options;
        _dbContext = new EprAccreditationContext(options);
        _repository = new AccreditationFileUploadRepository(_dbContext);

        
        var accreditation = new AccreditationEntity { Id = 1, ExternalId = _accreditationId };
        _dbContext.Accreditations.Add(accreditation);

        var fileUploads = new List<AccreditationFileUpload>
        {
            new AccreditationFileUpload
            {
                ExternalId = Guid.NewGuid(),
                AccreditationId = 1,
                FileUploadTypeId = (int)AccreditationFileUploadType.SamplingAndInspectionPlan,
                FileUploadStatusId = (int)AccreditationFileUploadStatus.UploadComplete,
                FileName = "complete.txt",
                UploadedBy = "A N Other",
            },
            new AccreditationFileUpload
            {
                ExternalId = Guid.NewGuid(),
                AccreditationId = 1,
                FileUploadTypeId = (int)AccreditationFileUploadType.SamplingAndInspectionPlan,
                FileUploadStatusId = (int)AccreditationFileUploadStatus.UploadFailed,
                FileName = "failed.txt",
                UploadedBy = "A N Other",
            },
            new AccreditationFileUpload
            {
                ExternalId = Guid.NewGuid(),
                AccreditationId = 1,
                FileUploadTypeId = (int)AccreditationFileUploadType.SamplingAndInspectionPlan,
                FileUploadStatusId = (int)AccreditationFileUploadStatus.FileDeleted,
                FileName = "deleted.txt",
                UploadedBy = "A N Other",
            },
            new AccreditationFileUpload
            {
                ExternalId = Guid.NewGuid(),
                AccreditationId = 1,
                FileUploadTypeId = (int)AccreditationFileUploadType.OverseasSiteEvidence,
                FileUploadStatusId = (int)AccreditationFileUploadStatus.UploadComplete,
                FileName = "overseas.txt",
                UploadedBy = "A N Other",
            },
            new AccreditationFileUpload
            {
                ExternalId = Guid.NewGuid(),
                AccreditationId = 2,
                FileUploadTypeId = (int)AccreditationFileUploadType.SamplingAndInspectionPlan,
                FileUploadStatusId = (int)AccreditationFileUploadStatus.UploadComplete,
                FileName = "file for acc 2.txt",
                UploadedBy = "A N Other",
            },
        };

        _dbContext.AccreditationFileUploads.AddRange(fileUploads);
        _dbContext.SaveChangesAsync();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [TestMethod]
    public async Task GetByExternalId_ReturnsEntity_WhenItExists()
    {
        // Arrange
        var fileUploadId = Guid.NewGuid();
        var entityToReturn = await _dbContext.AccreditationFileUploads.FirstAsync();
        entityToReturn.ExternalId = fileUploadId;
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetByExternalId(fileUploadId);

        // Assert
        result.Should().NotBeNull();
        result.ExternalId.Should().Be(fileUploadId);
        result.FileName.Should().Be(entityToReturn.FileName);
    }

    [TestMethod]
    public async Task GetByExternalId_ThrowsException_WhenNotFound()
    {
        // Arrange
        var fileUploadId = Guid.NewGuid();

        // Act
        Func<Task> act = async () => await _repository.GetByExternalId(fileUploadId);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Accreditation file upload not found");
    }

    [TestMethod]
    public async Task GetByAccreditationId_ReturnsEntitiesWithMatchingStatus_UploadComplete()
    {
        // Arrange

        // Act
        var result = await _repository.GetByAccreditationId(_accreditationId, 
            (int)AccreditationFileUploadType.SamplingAndInspectionPlan, (int)AccreditationFileUploadStatus.UploadComplete);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(1);
        result[0].FileName.Should().Be("complete.txt");
    }

    [TestMethod]
    public async Task GetByAccreditationId_ReturnsEntitiesWithMatchingStatus_UploadFailed()
    {
        // Arrange

        // Act
        var result = await _repository.GetByAccreditationId(_accreditationId,
            (int)AccreditationFileUploadType.SamplingAndInspectionPlan, (int)AccreditationFileUploadStatus.UploadFailed);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(1);
        result[0].FileName.Should().Be("failed.txt");
    }

    [TestMethod]
    public async Task GetByAccreditationId_ReturnsEntitiesWithMatchingStatus_FileDeleted()
    {
        // Arrange

        // Act
        var result = await _repository.GetByAccreditationId(_accreditationId,
            (int)AccreditationFileUploadType.SamplingAndInspectionPlan, (int)AccreditationFileUploadStatus.FileDeleted);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(1);
        result[0].FileName.Should().Be("deleted.txt");
    }

    [TestMethod]
    public async Task GetByAccreditationId_ReturnsEntitiesWithMatchingType_OverseasSiteEvidence()
    {
        // Arrange

        // Act
        var result = await _repository.GetByAccreditationId(_accreditationId,
            (int)AccreditationFileUploadType.OverseasSiteEvidence, (int)AccreditationFileUploadStatus.UploadComplete);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(1);
        result[0].FileName.Should().Be("overseas.txt");
    }

    [TestMethod]
    public async Task GetByAccreditationId_ThrowsException_WhenAccreditationNotFound()
    {
        // Arrange

        // Act
        Func<Task> act = async () => await _repository.GetByAccreditationId(Guid.NewGuid(),
            (int)AccreditationFileUploadType.SamplingAndInspectionPlan, (int)AccreditationFileUploadStatus.UploadComplete);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Accreditation not found");
    }

    [TestMethod]
    public async Task Create_AddsNewEntityToDbContext()
    {
        // Arrange
        var newEntity = new AccreditationFileUpload
        {
            ExternalId = Guid.Empty,
            AccreditationId = 1,
            FileUploadTypeId = (int)AccreditationFileUploadType.SamplingAndInspectionPlan,
            FileUploadStatusId = (int)AccreditationFileUploadStatus.UploadComplete,
            FileName = "new file.txt",
            UploadedBy = "A N Other",
        };

        // Act
        var result = await _repository.Create(_accreditationId, newEntity);

        // Assert
        result.Should().NotBeEmpty();
        _dbContext.AccreditationFileUploads.Count().Should().Be(6);
        var entityInContext = _dbContext.AccreditationFileUploads.Last();
        entityInContext.ExternalId.Should().NotBeEmpty();
        entityInContext.FileName.Should().Be(newEntity.FileName);
    }

    [TestMethod]
    public async Task Create_ThrowsException_WhenAccreditationNotFound()
    {
        // Arrange
        
        // Act
        Func<Task> act = async () => await _repository.Create(Guid.NewGuid(), new AccreditationFileUpload());

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Accreditation not found");
    }

    [TestMethod]
    public async Task Update_ModifiesExistingEntityInDbContext()
    {
        // Arrange
        var fileUploadId = Guid.NewGuid();
        var entityToUpdate = await _dbContext.AccreditationFileUploads.FirstAsync();
        entityToUpdate.ExternalId = fileUploadId;
        await _dbContext.SaveChangesAsync();

        var updatedEntity = new AccreditationFileUpload
        {
            ExternalId = fileUploadId,
            AccreditationId = 1,
            FileUploadTypeId = (int)AccreditationFileUploadType.OverseasSiteEvidence,
            FileUploadStatusId = (int)AccreditationFileUploadStatus.FileDeleted,
            FileId = Guid.NewGuid(),
            FileName = "updated name.txt",
            UploadedBy = "A Different Person",
            UploadedOn = DateTime.UtcNow,
            OverseasSiteId = 200
        };

        // Act
        await _repository.Update(_accreditationId, updatedEntity);

        // Assert
        _dbContext.AccreditationFileUploads.Count().Should().Be(5);
        var entityInContext = _dbContext.AccreditationFileUploads.First();
        entityInContext.ExternalId.Should().Be(updatedEntity.ExternalId);
        entityInContext.AccreditationId.Should().Be(updatedEntity.AccreditationId);
        entityInContext.FileUploadTypeId.Should().Be(updatedEntity.FileUploadTypeId);
        entityInContext.FileUploadStatusId.Should().Be(updatedEntity.FileUploadStatusId);
        entityInContext.FileId.Should().Be(updatedEntity.FileId);
        entityInContext.FileName.Should().Be(updatedEntity.FileName);
        entityInContext.UploadedBy.Should().Be(updatedEntity.UploadedBy);
        entityInContext.UploadedOn.Should().Be(updatedEntity.UploadedOn);
        entityInContext.OverseasSiteId.Should().Be(updatedEntity.OverseasSiteId);
    }

    [TestMethod]
    public async Task Update_ThrowsException_WhenFileUploadNotFound()
    {
        // Arrange
        
        // Act
        Func<Task> act = async () => await _repository.Update(_accreditationId, new AccreditationFileUpload() { ExternalId = Guid.NewGuid() });

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Accreditation file upload not found");
    }

    [TestMethod]
    public async Task Delete_UpdatesExistingEntityInDbContext()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var entityToUpdate = await _dbContext.AccreditationFileUploads.FirstAsync();
        entityToUpdate.FileId = fileId;
        await _dbContext.SaveChangesAsync();

        // Act
        await _repository.Delete(_accreditationId, fileId);

        // Assert
        _dbContext.AccreditationFileUploads.Count().Should().Be(5);
        var entityInContext = _dbContext.AccreditationFileUploads.First();
        entityInContext.FileId.Should().Be(fileId);
        entityInContext.FileUploadStatusId.Should().Be((int)AccreditationFileUploadStatus.FileDeleted);
    }

    [TestMethod]
    public async Task Delete_ThrowsException_WhenFileUploadNotFound()
    {
        // Arrange

        // Act
        Func<Task> act = async () => await _repository.Delete(_accreditationId, Guid.NewGuid());

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Accreditation file upload not found");
    }
}
