using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Profiles;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetMaterialSamplingPlanQueryHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _rmRepositoryMock;
    private IMapper _mapper;
    private GetMaterialSamplingPlanQueryHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _rmRepositoryMock = new Mock<IRegistrationMaterialRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RegistrationMaterialProfile>();
        });
        _mapper = config.CreateMapper();

        _handler = new GetMaterialSamplingPlanQueryHandler(_rmRepositoryMock.Object, _mapper);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialExists()
    {
        // Arrange
        int materialId = 1;
        DateTime dateUploaded = DateTime.UtcNow;
        var query = new GetMaterialSamplingPlanQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            Id = materialId,
            RegistrationId = 10,
            MaterialId = 2,
            Material = new LookupMaterial { MaterialName = "Plastic" },
            StatusID = 1,
            Status = new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" },
            FileUploads = new List<FileUpload>{
                new FileUpload
                {
                    
                    Filename = "Filename",
                    FileUploadType = new LookupFileUploadType{ Name = "Upload" },
                    FileUploadStatus = new LookupFileUploadStatus{ Name = "Uploaded"},
                    DateUploaded = dateUploaded,
                    UpdatedBy = "Test User",
                    Comments = "Test comment",
                    FileId = "FileId"
                }
            }
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationMaterial_FileUploadById(materialId))
            .ReturnsAsync(materialEntity);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.MaterialName.Should().Be("Plastic");
            result.Files.Should().NotBeNull();
            result.Files.First().Filename.Should().Be("Filename");
            result.Files.First().FileId.Should().Be("FileId");
            result.Files.First().DateUploaded.Should().Be(dateUploaded);
            result.Files.First().UpdatedBy.Should().Be("Test User");
            result.Files.First().FileUploadType.Should().Be("Upload");
            result.Files.First().FileUploadStatus.Should().Be("Uploaded");
            result.Files.First().Comments.Should().Be("Test comment");
        }
    }
}