using AutoFixture;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Repositories;
using EPR.PRN.Backend.Data;
using EPR.PRN.Backend.Data.DataModels;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Repositories;

[TestClass]
public class RepositoryTestsInMemory
{
    private Mock<ILogger<Repository>> _mockLogger;
    private Mock<IConfiguration> _configurationMock;

    private EprContext _context;
    private Repository _repository;
    private readonly Fixture _fixture = new();

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EprContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new EprContext(options);
        _mockLogger = new Mock<ILogger<Repository>>();
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c["LogPrefix"]).Returns("[EPR.PRN.Backend]");

        _repository = new Repository(_context, _mockLogger.Object, _configurationMock.Object);
        var orgId = Guid.NewGuid();
        var eprns = new List<Eprn>
        {
            CreateEprn(orgId, "searchTerm1", "Org1", "OrgName1", "Material1", "ProducerAgency1", "ReprocessorExporterAgency1", "IssuerReference1", "AccreditationNumber1", "ReprocessingSite1", 2023, 2023, "PackagingProducer1"),
            CreateEprn(orgId, "searchTerm2", "Org2", "OrgName2", "Material2", "ProducerAgency2", "ReprocessorExporterAgency2", "IssuerReference2", "AccreditationNumber2", "ReprocessingSite2", 2023, 2023, "PackagingProducer2")
        };
        _context.Prn.AddRange(eprns);
        _context.SaveChanges();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_WithSearchTerm_FiltersResults()
    {
        // Arrange
        var orgId = (await _context.Prn.FirstAsync(CancellationToken.None)).OrganisationId;
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 10,
            Search = "searchTerm"
        };
        // Act
        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);
        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(2, result.Items);
        Assert.IsTrue(result.Items.Any(i => i.PrnNumber.Contains("searchTerm")));
    }

    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_Returns_CorrectResults()
    {
        var orgId = (await _context.Prn.FirstAsync(CancellationToken.None)).OrganisationId;
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 10,
            Search = "searchTerm1"
        };
        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);
        Assert.HasCount(1, result.Items);
        Assert.AreEqual("searchTerm1", result.Items[0].PrnNumber);
    }

    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_Returns_Empty_When_NoMatch()
    {
        var orgId = (await _context.Prn.FirstAsync(CancellationToken.None)).OrganisationId;
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 10,
            Search = "nonexistent"
        };
        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);
        Assert.HasCount(0, result.Items);
    }

    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_Paginates_Correctly()
    {
        var orgId = (await _context.Prn.FirstAsync(CancellationToken.None)).OrganisationId;
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 1
        };
        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);
        Assert.HasCount(1, result.Items);
        Assert.AreEqual(2, result.TotalItems);
    }

    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_Filters_By_SearchTerm()
    {
        var orgId = (await _context.Prn.FirstAsync(CancellationToken.None)).OrganisationId;
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 10,
            Search = "Org2"
        };
        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);
        Assert.HasCount(1, result.Items);
        Assert.AreEqual("Org2", result.Items[0].IssuedByOrg);
    }

    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_ReturnsEmptyResponse_WhenNoResults()
    {
        // Arrange
        var orgId = Guid.NewGuid(); // Use a new GUID to ensure no matching records
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 10,
            Search = null,
            FilterBy = null,
            SortBy = null
        };
        // Act
        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);
        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(0, result.Items);
        Assert.AreEqual(0, result.TotalItems);
        Assert.AreEqual(request.Page, result.CurrentPage);
        Assert.AreEqual(request.PageSize, result.PageSize);
        Assert.AreEqual(request.Search, result.SearchTerm);
        Assert.AreEqual(request.FilterBy, result.FilterBy);
        Assert.AreEqual(request.SortBy, result.SortBy);
    }

    [TestMethod]
    [DataRow(PrnConstants.Filters.AcceptedAll, EprnStatus.ACCEPTED, PrnConstants.Materials.Wood)]
    [DataRow(PrnConstants.Filters.CancelledAll, EprnStatus.CANCELLED, PrnConstants.Materials.Wood)]
    [DataRow(PrnConstants.Filters.RejectedAll, EprnStatus.REJECTED, PrnConstants.Materials.Wood)]
    [DataRow(PrnConstants.Filters.AwaitingAll, EprnStatus.AWAITINGACCEPTANCE, PrnConstants.Materials.Wood)]
    [DataRow(PrnConstants.Filters.AwaitingAluminium, EprnStatus.AWAITINGACCEPTANCE, PrnConstants.Materials.Aluminium)]
    [DataRow(PrnConstants.Filters.AwaitingGlassOther, EprnStatus.AWAITINGACCEPTANCE, PrnConstants.Materials.GlassOther)]
    [DataRow(PrnConstants.Filters.AwaitingGlassMelt, EprnStatus.AWAITINGACCEPTANCE, PrnConstants.Materials.GlassMelt)]
    [DataRow(PrnConstants.Filters.AwaitngPaperFiber, EprnStatus.AWAITINGACCEPTANCE, PrnConstants.Materials.PaperFiber)]
    [DataRow(PrnConstants.Filters.AwaitngPlastic, EprnStatus.AWAITINGACCEPTANCE, PrnConstants.Materials.Plastic)]
    [DataRow(PrnConstants.Filters.AwaitngSteel, EprnStatus.AWAITINGACCEPTANCE, PrnConstants.Materials.Steel)]
    [DataRow(PrnConstants.Filters.AwaitngWood, EprnStatus.AWAITINGACCEPTANCE, PrnConstants.Materials.Wood)]
    public async Task GetSearchPrnsForOrganisation_Filter_Result_AsPerPassedFilterParameter(string filterBy, EprnStatus status, string material)
    {
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 10,
            Search = null,
            FilterBy = filterBy,
            SortBy = null
        };

        var orgId = Guid.NewGuid();

        var data = _fixture.Build<Eprn>()
                           .With(x => x.OrganisationId, orgId)
                           .Without(x => x.Id)
                           .With(x => x.MaterialName, material)
                           .With(x => x.PrnStatusId, 10)
                           .CreateMany().ToArray();

        //Assign passed status to first element and only this should present on result
        data[0].PrnStatusId = (int)status;

        _context.Prn.AddRange(data);
        await _context.SaveChangesAsync(CancellationToken.None);

        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);

        result.TotalItems.Should().Be(1);
        result.Items.Should().ContainSingle().Which.Should().Match<PrnDto>(x =>
        x.ExternalId == data[0].ExternalId
        && x.PrnNumber == data[0].PrnNumber);

    }

    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_Filter_Result_ReturnAll_IfFilterByISAnthingThanAboveListed()
    {
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 10,
            Search = null,
            FilterBy = "NO filter",
            SortBy = null
        };

        var orgId = Guid.NewGuid();

        var data = _fixture.Build<Eprn>()
                           .With(x => x.OrganisationId, orgId)
                           .Without(x => x.Id)
                           .CreateMany().ToArray();

        _context.Prn.AddRange(data);
        await _context.SaveChangesAsync(CancellationToken.None);

        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);

        result.TotalItems.Should().Be(data.Length);

    }

    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_Sort_Result_AsPerTonnage()
    {
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 10,
            Search = null,
            FilterBy = "NO filter",
            SortBy = PrnConstants.Sorts.TonnageAsc,
        };

        var orgId = Guid.NewGuid();

        var data = _fixture.Build<Eprn>()
                           .With(x => x.OrganisationId, orgId)
                           .Without(x => x.Id)
                           .With(x => x.TonnageValue, 100)
                           .CreateMany().ToArray();

        data[0].TonnageValue = 200;

        _context.Prn.AddRange(data);
        await _context.SaveChangesAsync(CancellationToken.None);

        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);

        result.TotalItems.Should().Be(3);
        result.Items[2].ExternalId.Should().Be(data[0].ExternalId);

        //Asserting desc
        request.SortBy = PrnConstants.Sorts.TonnageDesc;
        result = await _repository.GetSearchPrnsForOrganisation(orgId, request);

        result.TotalItems.Should().Be(3);
        result.Items[0].ExternalId.Should().Be(data[0].ExternalId);
    }

    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_Sort_Result_AsPerMaterial()
    {
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 10,
            Search = null,
            FilterBy = "NO filter",
            SortBy = PrnConstants.Sorts.MaterialAsc,
        };

        var orgId = Guid.NewGuid();

        var data = _fixture.Build<Eprn>()
                           .With(x => x.OrganisationId, orgId)
                           .Without(x => x.Id)
                           .With(x => x.MaterialName, "Plastic")
                           .CreateMany().ToArray();

        data[0].MaterialName = PrnConstants.Materials.Aluminium;

        _context.Prn.AddRange(data);
        await _context.SaveChangesAsync(CancellationToken.None);

        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);

        result.TotalItems.Should().Be(3);
        result.Items[0].ExternalId.Should().Be(data[0].ExternalId);

        //Asserting desc
        request.SortBy = PrnConstants.Sorts.MaterialDesc;
        result = await _repository.GetSearchPrnsForOrganisation(orgId, request);

        result.TotalItems.Should().Be(3);
        result.Items[2].ExternalId.Should().Be(data[0].ExternalId);
    }

    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_Sort_Result_AsIssueDate()
    {
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 10,
            Search = null,
            FilterBy = "NO filter",
            SortBy = PrnConstants.Sorts.IssueDateDesc,
        };

        var orgId = Guid.NewGuid();

        var data = _fixture.Build<Eprn>()
                           .With(x => x.OrganisationId, orgId)
                           .Without(x => x.Id)
                           .With(x => x.IssueDate, DateTime.UtcNow.AddHours(-3))
                           .CreateMany().ToArray();

        data[0].IssueDate = DateTime.UtcNow;

        _context.Prn.AddRange(data);
        await _context.SaveChangesAsync(CancellationToken.None);

        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);

        result.TotalItems.Should().Be(3);
        result.Items[0].ExternalId.Should().Be(data[0].ExternalId);

        //Asserting desc
        request.SortBy = PrnConstants.Sorts.IssueDateAsc;
        result = await _repository.GetSearchPrnsForOrganisation(orgId, request);

        result.TotalItems.Should().Be(3);
        result.Items[2].ExternalId.Should().Be(data[0].ExternalId);
    }

    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_Sort_Result_AsIssuedBy()
    {
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 10,
            Search = null,
            FilterBy = "NO filter",
            SortBy = PrnConstants.Sorts.IssuedByDesc,
        };

        var orgId = Guid.NewGuid();

        var data = _fixture.Build<Eprn>()
                           .With(x => x.OrganisationId, orgId)
                           .Without(x => x.Id)
                           .With(x => x.IssuedByOrg, "Alpa")
                           .CreateMany().ToArray();

        data[0].IssuedByOrg = "Beta";

        _context.Prn.AddRange(data);
        await _context.SaveChangesAsync(CancellationToken.None);

        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);

        result.TotalItems.Should().Be(3);
        result.Items[0].ExternalId.Should().Be(data[0].ExternalId);

        //Asserting desc
        request.SortBy = PrnConstants.Sorts.IssuedByAsc;
        result = await _repository.GetSearchPrnsForOrganisation(orgId, request);

        result.TotalItems.Should().Be(3);
        result.Items[2].ExternalId.Should().Be(data[0].ExternalId);
    }

    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_Sort_Result_AsDecemberWaste()
    {
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 10,
            Search = null,
            FilterBy = "NO filter",
            SortBy = PrnConstants.Sorts.DescemberWasteDesc,
        };

        var orgId = Guid.NewGuid();

        var data = _fixture.Build<Eprn>()
                           .With(x => x.OrganisationId, orgId)
                           .Without(x => x.Id)
                           .With(x => x.DecemberWaste, false)
                           .CreateMany().ToArray();

        data[2].DecemberWaste = true;

        _context.Prn.AddRange(data);
        await _context.SaveChangesAsync(CancellationToken.None);

        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);

        result.TotalItems.Should().Be(3);
        result.Items[0].ExternalId.Should().Be(data[2].ExternalId);
    }

    [TestMethod]
    public async Task GetSearchPrnsForOrganisation_Sort_Result_AsIssueDate_IfNoSortByPassed()
    {
        var request = new PaginatedRequestDto
        {
            Page = 1,
            PageSize = 10,
            Search = null,
            FilterBy = "NO filter",
            SortBy = null,
        };

        var orgId = Guid.NewGuid();

        var data = _fixture.Build<Eprn>()
                           .With(x => x.OrganisationId, orgId)
                           .Without(x => x.Id)
                           .With(x => x.IssueDate, DateTime.UtcNow.AddHours(-3))
                           .CreateMany().ToArray();

        data[2].IssueDate = DateTime.UtcNow;

        _context.Prn.AddRange(data);
        await _context.SaveChangesAsync(CancellationToken.None);

        var result = await _repository.GetSearchPrnsForOrganisation(orgId, request);

        result.TotalItems.Should().Be(3);
        result.Items[0].ExternalId.Should().Be(data[2].ExternalId);
    }

    private static Eprn CreateEprn(Guid orgId, string prnNumber, string issuedByOrg, string organisationName, string materialName, string producerAgency, string reprocessorExporterAgency, string issuerReference, string accreditationNumber, string reprocessingSite, int accreditationYear, int obligationYear, string packagingProducer)
    {
        return new Eprn
        {
            OrganisationId = orgId,
            PrnNumber = prnNumber,
            IssuedByOrg = issuedByOrg,
            ExternalId = Guid.NewGuid(),
            OrganisationName = organisationName,
            MaterialName = materialName,
            ProducerAgency = producerAgency,
            ReprocessorExporterAgency = reprocessorExporterAgency,
            IssuerReference = issuerReference,
            AccreditationNumber = accreditationNumber,
            ReprocessingSite = reprocessingSite,
            AccreditationYear = accreditationYear.ToString(),
            ObligationYear = obligationYear.ToString(),
            PackagingProducer = packagingProducer,
            IssueDate = DateTime.Now,
            CreatedOn = DateTime.Now
        };
    }

    

    [TestMethod]
    public async Task SavePrnDetails_SavesPrnAndHistory_Correctly()
    {
        var dto = new SavePrnDetailsRequest()
        {
            AccreditationNo = "ABC",
            AccreditationYear = 2018,
            CancelledDate = DateTime.UtcNow.AddDays(-1),
            DecemberWaste = true,
            EvidenceMaterial = "Aluminium",
            EvidenceNo = Guid.NewGuid().ToString(),
            EvidenceStatusCode = EprnStatus.AWAITINGACCEPTANCE,
            EvidenceTonnes = 5000,
            IssueDate = DateTime.UtcNow.AddDays(-5),
            IssuedByNPWDCode = "NPWD367742",
            IssuedByOrgName = "ANB",
            IssuedToEPRId = Guid.NewGuid(),
            IssuedToNPWDCode = "NPWD557742",
            IssuedToOrgName = "ZNZ",
            IssuerNotes = "no notes",
            IssuerRef = "ANB-1123",
            MaterialOperationCode = "R-PLA",
            ObligationYear = 2025,
            PrnSignatory = "Pat Anderson",
            PrnSignatoryPosition = "Director",
            ProducerAgency = "TTL",
            RecoveryProcessCode = "N11",
            ReprocessorAgency = "BEX",
            StatusDate = DateTime.UtcNow,
            SourceSystemId = "SSI1"
        };

        var entity = dto.ConvertToEprn();

        await _repository.SavePrnDetails(entity);

        var savedEnt = await _context.Prn.FirstOrDefaultAsync(x => x.PrnNumber == dto.EvidenceNo, CancellationToken.None);
        savedEnt.Should().NotBeNull();

        var savedHistory = await _context.PrnStatusHistory.FirstOrDefaultAsync(x => x.PrnIdFk == savedEnt.Id, CancellationToken.None);
        savedHistory.Should().NotBeNull();
    }

    [TestMethod]
    public async Task SavePrnDetails_UpdatePrn_Correctly()
    {
        var dto = new SavePrnDetailsRequest()
        {
            AccreditationNo = "ABC",
            AccreditationYear = 2018,
            CancelledDate = DateTime.UtcNow.AddDays(-1),
            DecemberWaste = true,
            EvidenceMaterial = "Aluminium",
            EvidenceNo = Guid.NewGuid().ToString(),
            EvidenceStatusCode = EprnStatus.AWAITINGACCEPTANCE,
            EvidenceTonnes = 5000,
            IssueDate = DateTime.UtcNow.AddDays(-5),
            IssuedByNPWDCode = "NPWD367742",
            IssuedByOrgName = "ANB",
            IssuedToEPRId = Guid.NewGuid(),
            IssuedToNPWDCode = "NPWD557742",
            IssuedToOrgName = "ZNZ",
            IssuerNotes = "no notes",
            IssuerRef = "ANB-1123",
            MaterialOperationCode = "R-PLA",
            ObligationYear = 2025,
            PrnSignatory = "Pat Anderson",
            PrnSignatoryPosition = "Director",
            ProducerAgency = "TTL",
            RecoveryProcessCode = "N11",
            ReprocessorAgency = "BEX",
            StatusDate = DateTime.UtcNow,
        };

        var entity = dto.ConvertToEprn();

        await _repository.SavePrnDetails(entity);

        var savedEnt = await _context.Prn.FirstOrDefaultAsync(x => x.PrnNumber == dto.EvidenceNo, CancellationToken.None);

        //updating 
        var updatingEntity = dto.ConvertToEprn();
        updatingEntity.MaterialName = "UpdatingMaterial";

        await _repository.SavePrnDetails(updatingEntity);

        var updatedEntity = await _context.Prn.FirstOrDefaultAsync(x => x.PrnNumber == dto.EvidenceNo, CancellationToken.None);

        savedEnt.ExternalId.Should().Be(updatedEntity.ExternalId);
        updatedEntity.MaterialName.Should().Be("UpdatingMaterial");
    }

    [TestMethod]
    public async Task SavePrnDetails_InsertsNewPrn_WithCorrectCreateAndUpdateDates()
    {
        var dto = new SavePrnDetailsRequest()
        {
            AccreditationNo = "ABC",
            AccreditationYear = 2018,
            CancelledDate = DateTime.UtcNow.AddDays(-1),
            DecemberWaste = true,
            EvidenceMaterial = "Aluminium",
            EvidenceNo = Guid.NewGuid().ToString(),
            EvidenceStatusCode = EprnStatus.AWAITINGACCEPTANCE,
            EvidenceTonnes = 5000,
            IssueDate = DateTime.UtcNow.AddDays(-5),
            IssuedByNPWDCode = "NPWD367742",
            IssuedByOrgName = "ANB",
            IssuedToEPRId = Guid.NewGuid(),
            IssuedToNPWDCode = "NPWD557742",
            IssuedToOrgName = "ZNZ",
            IssuerNotes = "no notes",
            IssuerRef = "ANB-1123",
            MaterialOperationCode = "R-PLA",
            ObligationYear = 2025,
            PrnSignatory = "Pat Anderson",
            PrnSignatoryPosition = "Director",
            ProducerAgency = "TTL",
            RecoveryProcessCode = "N11",
            ReprocessorAgency = "BEX",
            StatusDate = DateTime.UtcNow.AddDays(-5),
        };

        var entity = dto.ConvertToEprn();

        await _repository.SavePrnDetails(entity);

        var newPrn = await _context.Prn.SingleAsync(x => x.PrnNumber == dto.EvidenceNo, CancellationToken.None);
        newPrn.Should().NotBeNull();
        newPrn.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(5));
        newPrn.LastUpdatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(5));
    }

    [TestMethod]
    public async Task SavePrnDetails_UpdatesPrn_WithCorrectCreateAndUpdateDates()
    {
        var dto = new SavePrnDetailsRequest()
        {
            AccreditationNo = "ABC",
            AccreditationYear = 2018,
            CancelledDate = DateTime.UtcNow.AddDays(-1),
            DecemberWaste = true,
            EvidenceMaterial = "Aluminium",
            EvidenceNo = Guid.NewGuid().ToString(),
            EvidenceStatusCode = EprnStatus.AWAITINGACCEPTANCE,
            EvidenceTonnes = 5000,
            IssueDate = DateTime.UtcNow.AddDays(-5),
            IssuedByNPWDCode = "NPWD367742",
            IssuedByOrgName = "ANB",
            IssuedToEPRId = Guid.NewGuid(),
            IssuedToNPWDCode = "NPWD557742",
            IssuedToOrgName = "ZNZ",
            IssuerNotes = "no notes",
            IssuerRef = "ANB-1123",
            MaterialOperationCode = "R-PLA",
            ObligationYear = 2025,
            PrnSignatory = "Pat Anderson",
            PrnSignatoryPosition = "Director",
            ProducerAgency = "TTL",
            RecoveryProcessCode = "N11",
            ReprocessorAgency = "BEX",
            StatusDate = DateTime.UtcNow.AddDays(-5),
        };

        var entity = dto.ConvertToEprn();
        await _repository.SavePrnDetails(entity);

        var newPrn = await _context.Prn.SingleAsync(x => x.PrnNumber == dto.EvidenceNo, CancellationToken.None);
        DateTime createdDate = newPrn.CreatedOn;
        DateTime updatedDate = newPrn.LastUpdatedDate;
        await _repository.SavePrnDetails(newPrn);

        var updatedPrn = await _context.Prn.SingleAsync(x => x.PrnNumber == dto.EvidenceNo, CancellationToken.None);
        updatedPrn.CreatedOn.Should().Be(createdDate);
        updatedPrn.LastUpdatedDate.Should().BeAfter(updatedDate);
    }

    [TestMethod]
    [DataRow(EprnStatus.ACCEPTED)]
    [DataRow(EprnStatus.CANCELLED)]
    [DataRow(EprnStatus.REJECTED)]
    public async Task SavePrnDetails_WhenIncomingPrnIsAwaitingAcceptance_UpdatesPrn_WithOriginalStatus(EprnStatus eOldStatus)
    {
        // Arrange
        var dto = new SavePrnDetailsRequest()
        {
            AccreditationNo = "ABC",
            AccreditationYear = 2018,
            CancelledDate = DateTime.UtcNow.AddDays(-1),
            DecemberWaste = true,
            EvidenceMaterial = "Aluminium",
            EvidenceNo = Guid.NewGuid().ToString(),
            EvidenceStatusCode = eOldStatus,
            EvidenceTonnes = 5000,
            IssueDate = DateTime.UtcNow.AddDays(-5),
            IssuedByNPWDCode = "NPWD367742",
            IssuedByOrgName = "ANB",
            IssuedToEPRId = Guid.NewGuid(),
            IssuedToNPWDCode = "NPWD557742",
            IssuedToOrgName = "ZNZ",
            IssuerNotes = "no notes",
            IssuerRef = "ANB-1123",
            MaterialOperationCode = "R-PLA",
            ObligationYear = 2025,
            PrnSignatory = "Pat Anderson",
            PrnSignatoryPosition = "Director",
            ProducerAgency = "TTL",
            RecoveryProcessCode = "N11",
            ReprocessorAgency = "BEX",
            StatusDate = DateTime.UtcNow.AddDays(-5),
        };

        var entity = dto.ConvertToEprn();
        await _repository.SavePrnDetails(entity);
        await _context.SaveChangesAsync(CancellationToken.None);

        var awaitingAcceptancePrn = await _context.Prn.AsNoTracking().SingleAsync(x => x.PrnNumber == dto.EvidenceNo, CancellationToken.None);
        awaitingAcceptancePrn.PrnStatusId = (int)EprnStatus.AWAITINGACCEPTANCE;

        // Act
        await _repository.SavePrnDetails(awaitingAcceptancePrn);

        // Assert
        var updatedPrn = await _context.Prn.SingleAsync(x => x.PrnNumber == dto.EvidenceNo, CancellationToken.None);
        updatedPrn.PrnStatusId.Should().Be((int) eOldStatus);

        _mockLogger.Verify(logger => logger.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => $"{v}".ToString().StartsWith("Resetting status history on")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }

    [TestMethod]
    [DataRow(EprnStatus.ACCEPTED)]
    [DataRow(EprnStatus.CANCELLED)]
    [DataRow(EprnStatus.REJECTED)]
    [DataRow(EprnStatus.AWAITINGACCEPTANCE)]
    public async Task SavePrnDetails_WhenExistingPrnIsAwaitingAcceptance_UpdatesPrn_WithNewStatus(EprnStatus eNewStatus)
    {
        // Arrange
        var dto = new SavePrnDetailsRequest()
        {
            AccreditationNo = "ABC",
            AccreditationYear = 2018,
            CancelledDate = DateTime.UtcNow.AddDays(-1),
            DecemberWaste = true,
            EvidenceMaterial = "Aluminium",
            EvidenceNo = Guid.NewGuid().ToString(),
            EvidenceStatusCode = EprnStatus.AWAITINGACCEPTANCE,
            EvidenceTonnes = 5000,
            IssueDate = DateTime.UtcNow.AddDays(-5),
            IssuedByNPWDCode = "NPWD367742",
            IssuedByOrgName = "ANB",
            IssuedToEPRId = Guid.NewGuid(),
            IssuedToNPWDCode = "NPWD557742",
            IssuedToOrgName = "ZNZ",
            IssuerNotes = "no notes",
            IssuerRef = "ANB-1123",
            MaterialOperationCode = "R-PLA",
            ObligationYear = 2025,
            PrnSignatory = "Pat Anderson",
            PrnSignatoryPosition = "Director",
            ProducerAgency = "TTL",
            RecoveryProcessCode = "N11",
            ReprocessorAgency = "BEX",
            StatusDate = DateTime.UtcNow.AddDays(-5),
        };

        var entity =dto.ConvertToEprn();
        await _repository.SavePrnDetails(entity);
        await _context.SaveChangesAsync(CancellationToken.None);

        var newPrn = await _context.Prn.AsNoTracking().SingleAsync(x => x.PrnNumber == dto.EvidenceNo, CancellationToken.None);
        newPrn.PrnStatusId = (int) eNewStatus;

        // Act
        await _repository.SavePrnDetails(newPrn);

        // Assert
        var updatedPrn = await _context.Prn.SingleAsync(x => x.PrnNumber == dto.EvidenceNo, CancellationToken.None);
        updatedPrn.PrnStatusId.Should().Be((int)eNewStatus);

        _mockLogger.Verify(logger => logger.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => $"{v}".ToString().StartsWith("Resetting status history on")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Never);
    }
}
