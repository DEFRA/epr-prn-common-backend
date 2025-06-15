using AutoMapper;
using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.API.Profiles.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.API.UnitTests.Handlers
{
    [TestClass]
    public class GetAccreditationSamplingPlanQueryHandlerTests
    {
        private Mock<IRegistrationMaterialRepository> _rmRepositoryMock;
        private IMapper _mapper;
        private GetAccreditationSamplingPlanQueryHandler _handler;

        [TestInitialize]
        public void TestInitialize()
        {
            _rmRepositoryMock = new Mock<IRegistrationMaterialRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RegistrationMaterialProfile>();
            });
            _mapper = config.CreateMapper();

            _handler = new GetAccreditationSamplingPlanQueryHandler(_rmRepositoryMock.Object, _mapper);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnMappedDto_WhenMaterialExists()
        {
            // Arrange
            var accreditationId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a8");
            var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
            DateTime dateUploaded = DateTime.UtcNow;
            var query = new GetAccreditationSamplingPlanQuery { Id = accreditationId };
            var updatedBy = "Test user";
            var fileId = Guid.NewGuid();

            var materialEntity = new RegistrationMaterial
            {
                ExternalId = materialId,
                RegistrationId = 10,
                MaterialId = 2,
                Material = new LookupMaterial { MaterialName = "Plastic" },
                StatusId = 1,
                Status = new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" },
                FileUploads = new List<RegistrationFileUpload>{
                new RegistrationFileUpload
                {

                    Filename = "Filename",
                    FileUploadType = new LookupFileUploadType{ Name = "Upload" },
                    FileUploadStatus = new LookupFileUploadStatus{ Name = "Uploaded"},
                    DateUploaded = dateUploaded,
                    UpdatedBy = updatedBy,
                    Comments = "Test comment",
                    FileId = fileId
                }
            }
            };

            var accreditationEntity = new Accreditation
            {
                ApplicationReferenceNumber = "APP12345",
                ExternalId = accreditationId,
                RegistrationMaterial = materialEntity,
                FileUploads = new List<AccreditationFileUpload>
                {
                    new AccreditationFileUpload
                    {
                        Filename = "Filename",
                        FileId = fileId,
                        DateUploaded = dateUploaded,
                        UpdatedBy = updatedBy,
                        FileUploadType = new LookupFileUploadType { Name = "Upload" },
                        FileUploadStatus = new LookupFileUploadStatus { Name = "Uploaded" },
                    }
                }
            };

            _rmRepositoryMock
                .Setup(r => r.GetAccreditation_FileUploadById(accreditationId))
                .ReturnsAsync(accreditationEntity);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.MaterialName.Should().Be("Plastic");
                result.Files.Should().NotBeNull();
                result.Files.First().Filename.Should().Be("Filename");
                result.Files.First().FileId.Should().Be(fileId.ToString());
                result.Files.First().DateUploaded.Should().Be(dateUploaded);
                result.Files.First().UpdatedBy.Should().Be(updatedBy.ToString());
                result.Files.First().FileUploadType.Should().Be("Upload");
                result.Files.First().FileUploadStatus.Should().Be("Uploaded");
            }
        }
    }
}