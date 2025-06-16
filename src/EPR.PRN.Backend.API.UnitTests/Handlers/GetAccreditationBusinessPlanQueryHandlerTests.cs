using AutoMapper;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.API.Profiles.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers
{
    [TestClass]
    public class GetAccreditationBusinessPlanQueryHandlerTests
    {
        private Mock<IRegulatorAccreditationRepository> _raRepositoryMock;
        private IMapper _mapper;
        private GetAccreditationBusinessPlanQueryHandler _handler;

        [TestInitialize]
        public void TestInitialize()
        {
            _raRepositoryMock = new Mock<IRegulatorAccreditationRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RegistrationMaterialProfile>();
            });
            _mapper = config.CreateMapper();

            _handler = new GetAccreditationBusinessPlanQueryHandler(_raRepositoryMock.Object, _mapper);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnMappedDto_WhenAccreditationExists()
        {
            // Arrange
            var accreditationId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a8");
            var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
            DateTime dateUploaded = DateTime.UtcNow;
            var query = new GetRegistrationAccreditationBusinessPlanByIdQuery { Id = accreditationId };
            var updatedBy = "Test user";

            var materialEntity = new RegistrationMaterial
            {
                ExternalId = materialId,
                RegistrationId = 10,
                MaterialId = 2,
                Material = new LookupMaterial { MaterialName = "Plastic" },
                StatusId = 1,
                Status = new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" },
            };

            var accreditationEntity = new Accreditation
            {
                ApplicationReferenceNumber = "APP12345",
                ExternalId = accreditationId,
                RegistrationMaterial = materialEntity,
                AccreditationYear = 2025,
                AccreditationStatusId = 1,
                PRNTonnage = 1000,
                InfrastructurePercentage = 25.0m,
                BusinessCollectionsPercentage = 15.0m,
                RecycledWastePercentage = 20.0m,
                NewMarketsPercentage = 10.0m,
                CommunicationsPercentage = 5.0m,
                NewUsersRecycledPackagingWastePercentage = 10.0m,
                NotCoveredOtherCategoriesPercentage = 5.0m,
                TotalPercentage = 100.0m,
                InfrastructureNotes = "Infrastructure notes",
                BusinessCollectionsNotes = "Business collections notes",
                RecycledWasteNotes = "Recycled waste notes",
                NewMarketsNotes = "New markets notes",
                CommunicationsNotes = "Communications notes",
                NewUsersRecycledPackagingWasteNotes = "New users recycled packaging waste notes",
                NotCoveredOtherCategoriesNotes = "Not covered other categories notes",
                Tasks = new List<RegulatorAccreditationTaskStatus>
                {

                }
            };

            _raRepositoryMock
                .Setup(r => r.GetAccreditationPaymentFeesById(accreditationId))
                .ReturnsAsync(accreditationEntity);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.MaterialName.Should().Be("Plastic");
                result.AccreditationId.Should().Be(accreditationId);
                result.InfrastructurePercentage.Should().Be(25.0m);
                result.InfrastructureNotes.Should().Be("Infrastructure notes");
                result.RecycledWastePercentage.Should().Be(20.0m);
                result.RecycledWasteNotes.Should().Be("Recycled waste notes");
                result.BusinessCollectionsPercentage.Should().Be(15.0m);
                result.BusinessCollectionsNotes.Should().Be("Business collections notes");
                result.CommunicationsPercentage.Should().Be(5.0m);
                result.CommunicationsNotes.Should().Be("Communications notes");
                result.NewMarketsPercentage.Should().Be(10.0m);
                result.NewMarketsNotes.Should().Be("New markets notes");
                result.NewUsersRecycledPackagingWastePercentage.Should().Be(10.0m);
                result.NewUsersRecycledPackagingWasteNotes.Should().Be("New users recycled packaging waste notes");
                result.NotCoveredOtherCategoriesPercentage.Should().Be(5.0m);
                result.NotCoveredOtherCategoriesNotes.Should().Be("Not covered other categories notes");
                result.TaskStatus.Should().Be(RegulatorTaskStatus.NotStarted); 
            }
        }

        [TestMethod]
        public async Task Handle_ShouldReturnMappedDto_WhenAccreditationExists_TasksNull()
        {
            // Arrange
            var accreditationId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a8");
            var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
            DateTime dateUploaded = DateTime.UtcNow;
            var query = new GetRegistrationAccreditationBusinessPlanByIdQuery { Id = accreditationId };
            var updatedBy = "Test user";

            var materialEntity = new RegistrationMaterial
            {
                ExternalId = materialId,
                RegistrationId = 10,
                MaterialId = 2,
                Material = new LookupMaterial { MaterialName = "Plastic" },
                StatusId = 1,
                Status = new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" },
            };

            var accreditationEntity = new Accreditation
            {
                ApplicationReferenceNumber = "APP12345",
                ExternalId = accreditationId,
                RegistrationMaterial = materialEntity,
                AccreditationYear = 2025,
                AccreditationStatusId = 1,
                PRNTonnage = 1000,
                InfrastructurePercentage = 25.0m,
                BusinessCollectionsPercentage = 15.0m,
                RecycledWastePercentage = 20.0m,
                NewMarketsPercentage = 10.0m,
                CommunicationsPercentage = 5.0m,
                NewUsersRecycledPackagingWastePercentage = 10.0m,
                NotCoveredOtherCategoriesPercentage = 5.0m,
                TotalPercentage = 100.0m,
                InfrastructureNotes = "Infrastructure notes",
                BusinessCollectionsNotes = "Business collections notes",
                RecycledWasteNotes = "Recycled waste notes",
                NewMarketsNotes = "New markets notes",
                CommunicationsNotes = "Communications notes",
                NewUsersRecycledPackagingWasteNotes = "New users recycled packaging waste notes",
                NotCoveredOtherCategoriesNotes = "Not covered other categories notes"
            };

            _raRepositoryMock
                .Setup(r => r.GetAccreditationPaymentFeesById(accreditationId))
                .ReturnsAsync(accreditationEntity);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.MaterialName.Should().Be("Plastic");
                result.AccreditationId.Should().Be(accreditationId);
                result.InfrastructurePercentage.Should().Be(25.0m);
                result.InfrastructureNotes.Should().Be("Infrastructure notes");
                result.RecycledWastePercentage.Should().Be(20.0m);
                result.RecycledWasteNotes.Should().Be("Recycled waste notes");
                result.BusinessCollectionsPercentage.Should().Be(15.0m);
                result.BusinessCollectionsNotes.Should().Be("Business collections notes");
                result.CommunicationsPercentage.Should().Be(5.0m);
                result.CommunicationsNotes.Should().Be("Communications notes");
                result.NewMarketsPercentage.Should().Be(10.0m);
                result.NewMarketsNotes.Should().Be("New markets notes");
                result.NewUsersRecycledPackagingWastePercentage.Should().Be(10.0m);
                result.NewUsersRecycledPackagingWasteNotes.Should().Be("New users recycled packaging waste notes");
                result.NotCoveredOtherCategoriesPercentage.Should().Be(5.0m);
                result.NotCoveredOtherCategoriesNotes.Should().Be("Not covered other categories notes");
                result.TaskStatus.Should().Be(RegulatorTaskStatus.NotStarted);
            }
        }

        [TestMethod]
        public async Task Handle_ShouldReturnCorrectTaskStatusinDto_WhenTaskExists()
        {
            // Arrange
            var accreditationId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a8");
            var query = new GetRegistrationAccreditationBusinessPlanByIdQuery { Id = accreditationId };
            var accreditationEntity = new Accreditation
            {
                ApplicationReferenceNumber = "APP12345",
                ExternalId = accreditationId,
                AccreditationYear = 2025,
                AccreditationStatusId = 1,
                PRNTonnage = 1000,
                InfrastructurePercentage = 25.0m,
                BusinessCollectionsPercentage = 15.0m,
                RecycledWastePercentage = 20.0m,
                NewMarketsPercentage = 10.0m,
                CommunicationsPercentage = 5.0m,
                NewUsersRecycledPackagingWastePercentage = 10.0m,
                NotCoveredOtherCategoriesPercentage = 5.0m,
                TotalPercentage = 100.0m,
                InfrastructureNotes = "Infrastructure notes",
                BusinessCollectionsNotes = "Business collections notes",
                RecycledWasteNotes = "Recycled waste notes",
                NewMarketsNotes = "New markets notes",
                CommunicationsNotes = "Communications notes",
                NewUsersRecycledPackagingWasteNotes = "New users recycled packaging waste notes",
                NotCoveredOtherCategoriesNotes = "Not covered other categories notes",
                Tasks = new List<RegulatorAccreditationTaskStatus>
                {
                    new RegulatorAccreditationTaskStatus
                    {
                        RegulatorTaskId = 1,
                        Task = new LookupRegulatorTask
                        {
                            Id = 1,
                            Name = RegulatorTaskNames.BusinessPlan
                        },
                        TaskStatusId =  (int)RegulatorTaskStatus.Completed,
                        TaskStatus = new LookupTaskStatus
                        {
                            Id = (int)RegulatorTaskStatus.Completed,
                            Name = "Completed"
                        }
                    }
                }
            };
            _raRepositoryMock
                .Setup(r => r.GetAccreditationPaymentFeesById(accreditationId))
                .ReturnsAsync(accreditationEntity);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.TaskStatus.Should().Be(RegulatorTaskStatus.Completed);
            }
        }
    }
}
