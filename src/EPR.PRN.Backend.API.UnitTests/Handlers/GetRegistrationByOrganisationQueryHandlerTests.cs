using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetRegistrationByOrganisationQueryHandlerTests
{
    private Mock<IRegistrationRepository> _mockRegistrationRepository;
    private GetRegistrationByOrganisationQueryHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockRegistrationRepository = new Mock<IRegistrationRepository>();
        _handler = new GetRegistrationByOrganisationQueryHandler(_mockRegistrationRepository.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenRegistrationExists()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var externalId = Guid.NewGuid();
        var query = new GetRegistrationByOrganisationQuery { ApplicationTypeId = 1, OrganisationId = organisationId };
        var expectedDto = new RegistrationDto
        {
            Id = 1,
            ApplicationTypeId = 1,
            ExternalId = externalId,
            OrganisationId = organisationId,
            BusinessAddress = new AddressDto
            {
                AddressLine1 = "address line 1",
                AddressLine2 = "address line 2",
                County = "County",
                PostCode = "Postcode",
                TownCity = "Town",
                GridReference = "TQ12345678",
                NationId = 1,
                Id = 1
            },
            ReprocessingSiteAddress = new AddressDto
            {
                AddressLine1 = "address line 1",
                AddressLine2 = "address line 2",
                County = "County",
                PostCode = "Postcode",
                TownCity = "Town",
                GridReference = "TQ12345678",
                NationId = 1,
                Id = 1
            },
            LegalDocumentAddress = new AddressDto
            {
                AddressLine1 = "address line 1",
                AddressLine2 = "address line 2",
                County = "County",
                PostCode = "Postcode",
                TownCity = "Town",
                GridReference = "TQ12345678",
                NationId = 1,
                Id = 1
            },
            RegistrationStatusId = 1,
            Tasks = new List<RegistrationTaskDto>
            {
                new()
                {
                    Id = externalId,
                    Status = "SiteDetails",
                    TaskName = "SiteDetails"
                }
            }
        };
    
        var address = new Address
        {
            AddressLine1 = "address line 1",
            AddressLine2 = "address line 2",
            County = "County",
            PostCode = "Postcode",
            TownCity = "Town",
            GridReference = "TQ12345678",
            NationId = 1,
            Id = 1
        };

        var tasks = new RegistrationTaskStatus
        {
            Id = 1,
            ExternalId = externalId,
            TaskStatus = new LookupTaskStatus { Name = "SiteDetails", Id = 1 },
            RegistrationId = 1,
            TaskId = 1,
            TaskStatusId = 1,
            Task = new LookupRegulatorTask{ApplicationTypeId = 1, Id = 1, IsMaterialSpecific = false, Name = "SiteDetails"}
        };

        var registration = new Registration
        {
            Id = 1,
            ApplicationTypeId = 1,
            OrganisationId = organisationId,
            ExternalId = externalId,
            RegistrationStatusId = 1,
            BusinessAddress = address,
            ReprocessingSiteAddress = address,
            LegalDocumentAddress = address,
            ApplicantRegistrationTasksStatus = [tasks]
        };

        tasks.Registration = registration;

        _mockRegistrationRepository
            .Setup(r => r.GetByOrganisationAsync(1, organisationId))
            .ReturnsAsync(registration);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedDto);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnNull_NoRegistrationExists()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var query = new GetRegistrationByOrganisationQuery { ApplicationTypeId = 1, OrganisationId = organisationId };

        _mockRegistrationRepository
            .Setup(r => r.GetByOrganisationAsync(1, organisationId))
            .ReturnsAsync((Registration?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}