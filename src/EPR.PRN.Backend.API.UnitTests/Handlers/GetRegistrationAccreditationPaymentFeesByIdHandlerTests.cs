using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.API.Profiles.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetRegistrationAccreditationPaymentFeesByIdHandlerTests
{
    private Mock<IRegulatorAccreditationRepository> _rmRepositoryMock;
    private IMapper _mapper;
    private GetRegulatorAccreditationPaymentFeesByIdHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _rmRepositoryMock = new Mock<IRegulatorAccreditationRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RegistrationMaterialProfile>();
        });
        _mapper = config.CreateMapper();

        _handler = new GetRegulatorAccreditationPaymentFeesByIdHandler(_rmRepositoryMock.Object, _mapper);
    }

    [TestMethod]
    public async Task Handle_ReturnsMappedDto_WhenAccreditationExists()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();

        var accreditation = new Accreditation
        {
            Id = 1,
            ExternalId = accreditationId,
            AccreditationYear = 2024,
            RegistrationMaterial = new RegistrationMaterial
            {
                Registration = new Registration { ExternalId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d") },
                Material = new LookupMaterial { MaterialName = "Plastic" }
            },
            AccreditationStatus = new LookupAccreditationStatus { Name = "Submitted" },
            CreatedOn = new DateTime(2024, 1, 1),
            ApplicationReferenceNumber = "REF-123",
        };

        var expectedDto = new AccreditationPaymentFeeDetailsDto
        {
            AccreditationId = accreditationId,
            OrganisationName = "Test Org",
            ApplicationReferenceNumber = "REF-123",
            MaterialName = "Plastic",
            SiteAddress = "123 Test St, Testville, TS1 1AA",
            SubmittedDate = accreditation.CreatedOn,
            ApplicationType = ApplicationOrganisationType.Reprocessor,
            Regulator = "EA",
            NationId = 3
        };

        var repoMock = new Mock<IRegulatorAccreditationRepository>();
        repoMock.Setup(r => r.GetAccreditationPaymentFeesById(accreditationId)).ReturnsAsync(accreditation);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<AccreditationPaymentFeeDetailsDto>(accreditation)).Returns(expectedDto);

        var handler = new GetRegulatorAccreditationPaymentFeesByIdHandler(repoMock.Object, mapperMock.Object);

        var query = new GetRegistrationAccreditationPaymentFeesByIdQuery { Id = accreditationId };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedDto);
    }

}
