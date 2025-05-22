using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetRegistrationAccreditationReferenceByIdHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _rmRepositoryMock;
    private Mock<IMapper> _mapperMock;
    private GetRegistrationAccreditationReferenceByIdHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _rmRepositoryMock = new Mock<IRegistrationMaterialRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetRegistrationAccreditationReferenceByIdHandler(_rmRepositoryMock.Object, _mapperMock.Object);
    }

    [TestMethod]
    public async Task Handle_ExporterWithValidData_ReturnsDto()
    {
        var material = CreateMaterial(ApplicationOrganisationType.Exporter, 1, "PLS");

        _rmRepositoryMock.Setup(x => x.GetRegistrationMaterialById(Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"))).ReturnsAsync(material);

        var result = await _handler.Handle(new GetRegistrationAccreditationReferenceByIdQuery { Id = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9") }, default);

        result.Should().NotBeNull();
        result.ApplicationType.Should().Be("E");
        result.OrgCode.Should().Be("000456");
        result.MaterialCode.Should().Be("PLS");
        result.NationId.Should().Be(1);
    }

    [TestMethod]
    public async Task Handle_ReprocessorWithValidData_ReturnsDto()
    {
        var material = CreateMaterial(ApplicationOrganisationType.Reprocessor, 2, "MTL");

        _rmRepositoryMock.Setup(x => x.GetRegistrationMaterialById(Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"))).ReturnsAsync(material);

        var result = await _handler.Handle(new GetRegistrationAccreditationReferenceByIdQuery { Id = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9") }, default);

        result.Should().NotBeNull();
        result.ApplicationType.Should().Be("R");
        result.OrgCode.Should().Be("000456");
        result.MaterialCode.Should().Be("MTL");
        result.NationId.Should().Be(2);
    }

    [TestMethod]
    public async Task Handle_MaterialEntityIsNull_Throws()
    {
        _rmRepositoryMock.Setup(x => x.GetRegistrationMaterialById(Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"))).ReturnsAsync((RegistrationMaterial)null!);

        var act = async () => await _handler.Handle(new GetRegistrationAccreditationReferenceByIdQuery { Id = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9") }, default);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Material entity or its registration is null.");
    }

    [TestMethod]
    public async Task Handle_RegistrationIsNull_Throws()
    {
        var material = new RegistrationMaterial { Id = 1, Registration = null! };
        _rmRepositoryMock.Setup(x => x.GetRegistrationMaterialById(Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"))).ReturnsAsync(material);

        var act = async () => await _handler.Handle(new GetRegistrationAccreditationReferenceByIdQuery { Id = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9") }, default);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Material entity or its registration is null.");
    }

    [TestMethod]
    public async Task Handle_ExporterWithNullBusinessAddress_Throws()
    {
        var material = CreateMaterial(ApplicationOrganisationType.Exporter, null, "PLS", businessAddressNull: true);

        _rmRepositoryMock.Setup(x => x.GetRegistrationMaterialById(Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"))).ReturnsAsync(material);

        var act = async () => await _handler.Handle(new GetRegistrationAccreditationReferenceByIdQuery { Id = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9") }, default);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Business address NationId is null.");
    }

    [TestMethod]
    public async Task Handle_ExporterWithNullNationId_Throws()
    {
        var material = CreateMaterial(ApplicationOrganisationType.Exporter, null, "PLS");

        _rmRepositoryMock.Setup(x => x.GetRegistrationMaterialById(Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"))).ReturnsAsync(material);

        var act = async () => await _handler.Handle(new GetRegistrationAccreditationReferenceByIdQuery { Id = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9") }, default);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Business address NationId is null.");
    }

    [TestMethod]
    public async Task Handle_ReprocessorWithNullReprocessingSiteAddress_Throws()
    {
        var material = CreateMaterial(ApplicationOrganisationType.Reprocessor, null, "MTL", reprocessingAddressNull: true);

        _rmRepositoryMock.Setup(x => x.GetRegistrationMaterialById(Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"))).ReturnsAsync(material);

        var act = async () => await _handler.Handle(new GetRegistrationAccreditationReferenceByIdQuery { Id = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9") }, default);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Reprocessing site address NationId is null.");
    }

    [TestMethod]
    public async Task Handle_ReprocessorWithNullNationId_Throws()
    {
        var material = CreateMaterial(ApplicationOrganisationType.Reprocessor, null, "MTL");

        _rmRepositoryMock.Setup(x => x.GetRegistrationMaterialById(Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"))).ReturnsAsync(material);

        var act = async () => await _handler.Handle(new GetRegistrationAccreditationReferenceByIdQuery { Id = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9") }, default);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Reprocessing site address NationId is null.");
    }

    private RegistrationMaterial CreateMaterial(
        ApplicationOrganisationType appType,
        int? nationId,
        string materialCode,
        bool businessAddressNull = false,
        bool reprocessingAddressNull = false)
    {
        return new RegistrationMaterial
        {
            Id = 1,
            Material = new LookupMaterial { MaterialCode = materialCode },
            RegistrationId = 456,
            Registration = new Registration
            {
                Id = 456,
                ApplicationTypeId = (int)appType,
                BusinessAddress = businessAddressNull ? null : new Address { NationId = nationId },
                ReprocessingSiteAddress = reprocessingAddressNull ? null : new Address { NationId = nationId }
            }
        };
    }
}
