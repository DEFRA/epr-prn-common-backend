using AutoFixture;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Dto;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.Constants;
using EPR.PRN.Backend.Obligation.Dto;
using EPR.PRN.Backend.Obligation.Helpers;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Models;
using EPR.PRN.Backend.Obligation.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.Obligation.UnitTests.Services;

[TestClass]
public class ObligationCalculatorServiceTests
{
	private Mock<IObligationCalculationRepository> _mockObligationCalculationRepository;
	private Mock<IRecyclingTargetDataService> _mockRecyclingTargetDataService;
	private Mock<IMaterialCalculationStrategyResolver> _mockStrategyResolver;
	private Mock<IPrnRepository> _mockPrnRepository;
	private Mock<IMaterialRepository> _mockMaterialRepository;
	private Mock<IObligationCalculationOrganisationSubmitterTypeRepository> _mockSubmitterTypeRepository;
	private Mock<IDateTimeProvider> _mockDateTimeProvider;
	private Mock<ILogger<ObligationCalculatorService>> _mockLogger;
	private ObligationCalculatorService _service;
	private Fixture _fixture;
	private readonly Guid submitterId = Guid.NewGuid(); // Either ComplianceSchemeId or DR OrganisationId
	private readonly int obligationCalculationYear = DateTime.UtcNow.Year;

	[TestInitialize]
	public void TestInitialize()
	{
		_fixture = new Fixture();
		_mockObligationCalculationRepository = new Mock<IObligationCalculationRepository>();
		_mockRecyclingTargetDataService = new Mock<IRecyclingTargetDataService>();
		_mockStrategyResolver = new Mock<IMaterialCalculationStrategyResolver>();
		_mockPrnRepository = new Mock<IPrnRepository>();
		_mockMaterialRepository = new Mock<IMaterialRepository>();
		_mockSubmitterTypeRepository = new Mock<IObligationCalculationOrganisationSubmitterTypeRepository>();
		_mockDateTimeProvider = new Mock<IDateTimeProvider>();
		_mockLogger = new Mock<ILogger<ObligationCalculatorService>>();
		_service = new ObligationCalculatorService(
			_mockObligationCalculationRepository.Object,
			_mockRecyclingTargetDataService.Object,
			_mockStrategyResolver.Object,
			_mockLogger.Object,
			_mockPrnRepository.Object,
			_mockMaterialRepository.Object,
			_mockSubmitterTypeRepository.Object,
			_mockDateTimeProvider.Object);
	}

	[TestMethod]
	public async Task GetObligationCalculation_ShouldReturnSuccess_WithExpectedData()
	{
		// Arrange
		var pcMaterialId = 5;
		var fcMaterialId = 8;
		var materials = GetMaterials();
		var responseMaterials = materials.Where(m => m.MaterialName != MaterialType.FibreComposite.ToString());
		var obligationCalculations = _fixture.CreateMany<ObligationCalculation>(8).ToList();
		for (int i = 0; i < obligationCalculations.Count; i++)
		{
			obligationCalculations[i].MaterialId = i + 1;
		}

		var prnList = _fixture.CreateMany<EprnResultsDto>(9).ToList();
		prnList[0].Eprn.MaterialName = PrnConstants.Materials.Plastic;
		prnList[1].Eprn.MaterialName = PrnConstants.Materials.PaperFiber;
		prnList[2].Eprn.MaterialName = PrnConstants.Materials.Steel;
		prnList[3].Eprn.MaterialName = PrnConstants.Materials.Wood;
		prnList[4].Eprn.MaterialName = PrnConstants.Materials.Aluminium;
		prnList[5].Eprn.MaterialName = PrnConstants.Materials.GlassOther;
		prnList[6].Eprn.MaterialName = PrnConstants.Materials.GlassMelt;
		prnList[7].Eprn.MaterialName = PrnConstants.Materials.PaperComposting;
		prnList[8].Eprn.MaterialName = PrnConstants.Materials.WoodComposting;

		prnList.ForEach(p => p.Eprn.ObligationYear = obligationCalculationYear.ToString());

		var prns = prnList.AsQueryable();
		_mockMaterialRepository.Setup(repo => repo.GetAllMaterials()).ReturnsAsync(materials);
		_mockObligationCalculationRepository.Setup(repo => repo.GetObligationCalculationBySubmitterIdAndYear(submitterId, obligationCalculationYear)).ReturnsAsync(obligationCalculations);
		_mockPrnRepository.Setup(repo => repo.GetAcceptedAndAwaitingPrnsByYear(submitterId, obligationCalculationYear)).Returns(prns);

		var acceptedTonnage = _fixture.CreateMany<EprnTonnageResultsDto>(7).ToList();
		var awaitingTonnage = _fixture.CreateMany<EprnTonnageResultsDto>(7).ToList();
		_mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync(GetRecyclingTargets());

		// Act
		var result = await _service.GetObligationCalculation(submitterId, obligationCalculationYear);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.ObligationModel.Should().NotBeNull();

		var obligationCalculationDict = obligationCalculations.ToDictionary(o => o.MaterialId);
		foreach (var material in responseMaterials)
		{
			var obligationData = result.ObligationModel.ObligationData.Find(d => d.MaterialName == material.MaterialName);
			obligationData.Should().NotBeNull();
			obligationData.MaterialName.Should().Be(material.MaterialName);

			var expectedObligationToMeet = CalculateExpectedObligation(obligationCalculationDict, material, pcMaterialId, fcMaterialId);
			var expectedTonnage = CalculateExpectedTonnage(obligationCalculationDict, material, pcMaterialId, fcMaterialId);

			obligationData.ObligationToMeet.Should().Be(expectedObligationToMeet);
			obligationData.Tonnage.Should().Be(expectedTonnage);
			obligationData.TonnageAccepted.Should().Be(acceptedTonnage.Find(t => t.MaterialName == material.MaterialName)?.TotalTonnage ?? 0);
			obligationData.TonnageAwaitingAcceptance.Should().Be(awaitingTonnage.Find(t => t.MaterialName == material.MaterialName)?.TotalTonnage ?? 0);
		}
	}

	private static int CalculateExpectedObligation(Dictionary<int, ObligationCalculation> obligationCalculationDict, Material material, int pcMaterialId, int fcMaterialId)
	{
		if (material.MaterialName == MaterialType.Paper.ToString())
		{
			var pcObligation = obligationCalculationDict[pcMaterialId].MaterialObligationValue;
			var fcObligation = obligationCalculationDict[fcMaterialId].MaterialObligationValue;
			return pcObligation + fcObligation;
		}
		return obligationCalculationDict[material.Id].MaterialObligationValue;
	}

	private static int CalculateExpectedTonnage(Dictionary<int, ObligationCalculation> obligationCalculationDict, Material material, int pcMaterialId, int fcMaterialId)
	{
		if (material.MaterialName == MaterialType.Paper.ToString())
		{
			var pcTonnage = obligationCalculationDict[pcMaterialId].Tonnage;
			var fcTonnage = obligationCalculationDict[fcMaterialId].Tonnage;
			return pcTonnage + fcTonnage;
		}
		return obligationCalculationDict[material.Id].Tonnage;
	}

	[TestMethod]
	public async Task GetObligationCalculation_ShouldReturnSuccess_WithExpectedStatus()
	{
		// Arrange
		var materials = GetMaterials();
		var obligationCalculations = _fixture.CreateMany<ObligationCalculation>(2).ToList();
		obligationCalculations[0].MaterialId = 1; // Plastic
		obligationCalculations[0].MaterialObligationValue = 2;
		obligationCalculations[1].MaterialId = 2; // Wood
		obligationCalculations[1].MaterialObligationValue = 1;

		var prnList = _fixture.CreateMany<EprnResultsDto>(2).ToList();
		prnList[0].Eprn.MaterialName = PrnConstants.Materials.Plastic;
		prnList[0].Eprn.PrnStatusId = 1;
		prnList[0].Status.StatusName = EprnStatus.ACCEPTED.ToString();
		prnList[0].Eprn.TonnageValue = 1;
		prnList[0].Eprn.ObligationYear = obligationCalculationYear.ToString();
		prnList[1].Eprn.MaterialName = PrnConstants.Materials.Wood;
		prnList[1].Eprn.PrnStatusId = 1;
		prnList[1].Status.StatusName = EprnStatus.ACCEPTED.ToString();
		prnList[1].Eprn.TonnageValue = 1;
		prnList[1].Eprn.ObligationYear = obligationCalculationYear.ToString();

		var prns = prnList.AsQueryable();
		_mockMaterialRepository.Setup(repo => repo.GetAllMaterials()).ReturnsAsync(materials);
		_mockObligationCalculationRepository.Setup(repo => repo.GetObligationCalculationBySubmitterIdAndYear(submitterId, obligationCalculationYear)).ReturnsAsync(obligationCalculations);
		_mockPrnRepository.Setup(repo => repo.GetAcceptedAndAwaitingPrnsByYear(submitterId, obligationCalculationYear)).Returns(prns);
		_mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync(GetRecyclingTargets());

		// Act
		var result = await _service.GetObligationCalculation(submitterId, obligationCalculationYear);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.ObligationModel.Should().NotBeNull();

		var obligationData = result.ObligationModel.ObligationData;
		obligationData.Should().NotBeNull();
		obligationData.Count.Should().Be(7);
		obligationData.Find(d => d.MaterialName == MaterialType.Plastic.ToString()).Status.Should().Be(ObligationConstants.Statuses.NotMet);
		obligationData.Find(d => d.MaterialName == MaterialType.Wood.ToString()).Status.Should().Be(ObligationConstants.Statuses.Met);
	}

	[TestMethod]
	[DataRow(141683, 149198, 0, ObligationConstants.Statuses.Met, 48339, 41384, 0, ObligationConstants.Statuses.Met)]
	[DataRow(10, 14, 0, ObligationConstants.Statuses.Met, 3, 0, 0, ObligationConstants.Statuses.Met)]
	[DataRow(3, 5, 0, ObligationConstants.Statuses.Met, 4, 0, 2, ObligationConstants.Statuses.NotMet)]
	[DataRow(8, 5, 3, ObligationConstants.Statuses.NotMet, 2, 5, 0, ObligationConstants.Statuses.Met)]
	[DataRow(10, 7, 3, ObligationConstants.Statuses.NotMet, 12, 4, 8, ObligationConstants.Statuses.NotMet)]
	public async Task GetObligationCalculation_ShouldReturnSuccess__WithAdjustedGlassObligations_UsingSurplusGlassRemeltPRNS
		(
			int remeltObligationToMeet,
			int remeltAcceptedPRNs,
			int remeltOutstanding,
			string remeltStatus,
			int remainingObligationToMeet,
			int remainingAcceptedPRNs,
			int remainingOutstanding,
			string remainingStatus
		)
	{
		// Arrange
		var materials = GetMaterials();
		var obligationCalculations = _fixture.CreateMany<ObligationCalculation>(2).ToList();
		obligationCalculations[0].MaterialId = 6; // Remaining Glass
		obligationCalculations[0].MaterialObligationValue = remainingObligationToMeet;
		obligationCalculations[1].MaterialId = 7; // Glass Re-melt
		obligationCalculations[1].MaterialObligationValue = remeltObligationToMeet;

		var prnList = _fixture.CreateMany<EprnResultsDto>(2).ToList();
		prnList[0].Eprn.MaterialName = PrnConstants.Materials.GlassOther;
		prnList[0].Eprn.PrnStatusId = 1;
		prnList[0].Status.StatusName = EprnStatus.ACCEPTED.ToString();
		prnList[0].Eprn.TonnageValue = remainingAcceptedPRNs;
		prnList[0].Eprn.ObligationYear = obligationCalculationYear.ToString();
		prnList[1].Eprn.MaterialName = PrnConstants.Materials.GlassMelt;
		prnList[1].Eprn.PrnStatusId = 1;
		prnList[1].Status.StatusName = EprnStatus.ACCEPTED.ToString();
		prnList[1].Eprn.TonnageValue = remeltAcceptedPRNs;
		prnList[1].Eprn.ObligationYear = obligationCalculationYear.ToString();

		var prns = prnList.AsQueryable();
		_mockMaterialRepository.Setup(repo => repo.GetAllMaterials()).ReturnsAsync(materials);
		_mockObligationCalculationRepository.Setup(repo => repo.GetObligationCalculationBySubmitterIdAndYear(submitterId, obligationCalculationYear)).ReturnsAsync(obligationCalculations);
		_mockPrnRepository.Setup(repo => repo.GetAcceptedAndAwaitingPrnsByYear(submitterId, obligationCalculationYear)).Returns(prns);
		_mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync(GetRecyclingTargets());

		// Act
		var result = await _service.GetObligationCalculation(submitterId, obligationCalculationYear);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.ObligationModel.Should().NotBeNull();

		var obligationData = result.ObligationModel.ObligationData;
		obligationData.Should().NotBeNull();
		obligationData.Count.Should().Be(7);
		obligationData.Find(d => d.MaterialName == MaterialType.GlassRemelt.ToString()).TonnageOutstanding.Should().Be(remeltOutstanding);
		obligationData.Find(d => d.MaterialName == MaterialType.GlassRemelt.ToString()).Status.Should().Be(remeltStatus);
		obligationData.Find(d => d.MaterialName == MaterialType.Glass.ToString()).TonnageOutstanding.Should().Be(remainingOutstanding);
		obligationData.Find(d => d.MaterialName == MaterialType.Glass.ToString()).Status.Should().Be(remainingStatus);
	}

	[TestMethod]
	public async Task GetObligationCalculation_ShouldReturnSuccess_WithTonnageOutstandingZero_WhenSrplusinAcceptedPRNs()
	{
		// Arrange
		var materials = GetMaterials();
		var obligationCalculations = _fixture.Build<ObligationCalculation>()
										.With(x => x.MaterialObligationValue, 1)
										.CreateMany(8)
										.ToList();

		obligationCalculations[0].MaterialId = 1;
		obligationCalculations[1].MaterialId = 2;
		obligationCalculations[2].MaterialId = 3;
		obligationCalculations[3].MaterialId = 4;
		obligationCalculations[4].MaterialId = 5;
		obligationCalculations[5].MaterialId = 6;
		obligationCalculations[6].MaterialId = 7;
		obligationCalculations[7].MaterialId = 8;

		var prnList = _fixture.CreateMany<EprnResultsDto>(8).ToList();
		
		prnList[0].Eprn.MaterialName = PrnConstants.Materials.PaperFiber;
		prnList[0].Eprn.PrnStatusId = 1;
		prnList[0].Status.StatusName = EprnStatus.ACCEPTED.ToString();
		prnList[0].Eprn.TonnageValue = 10;
		prnList[0].Eprn.ObligationYear = obligationCalculationYear.ToString();

		prnList[1].Eprn.MaterialName = PrnConstants.Materials.GlassOther;
		prnList[1].Eprn.PrnStatusId = 1;
		prnList[1].Status.StatusName = EprnStatus.ACCEPTED.ToString();
		prnList[1].Eprn.TonnageValue = 10;
		prnList[1].Eprn.ObligationYear = obligationCalculationYear.ToString();

		prnList[2].Eprn.MaterialName = PrnConstants.Materials.GlassMelt;
		prnList[2].Eprn.PrnStatusId = 1;
		prnList[2].Status.StatusName = EprnStatus.ACCEPTED.ToString();
		prnList[2].Eprn.TonnageValue = 10;
		prnList[2].Eprn.ObligationYear = obligationCalculationYear.ToString();

		prnList[3].Eprn.MaterialName = PrnConstants.Materials.Aluminium;
		prnList[3].Eprn.PrnStatusId = 1;
		prnList[3].Status.StatusName = EprnStatus.ACCEPTED.ToString();
		prnList[3].Eprn.TonnageValue = 10;
		prnList[3].Eprn.ObligationYear = obligationCalculationYear.ToString();

		prnList[4].Eprn.MaterialName = PrnConstants.Materials.Steel;
		prnList[4].Eprn.PrnStatusId = 1;
		prnList[4].Status.StatusName = EprnStatus.ACCEPTED.ToString();
		prnList[4].Eprn.TonnageValue = 10;
		prnList[4].Eprn.ObligationYear = obligationCalculationYear.ToString();

		prnList[5].Eprn.MaterialName = PrnConstants.Materials.Plastic;
		prnList[5].Eprn.PrnStatusId = 1;
		prnList[5].Status.StatusName = EprnStatus.ACCEPTED.ToString();
		prnList[5].Eprn.TonnageValue = 10;
		prnList[5].Eprn.ObligationYear = obligationCalculationYear.ToString();

		prnList[6].Eprn.MaterialName = PrnConstants.Materials.Wood;
		prnList[6].Eprn.PrnStatusId = 1;
		prnList[6].Status.StatusName = EprnStatus.ACCEPTED.ToString();
		prnList[6].Eprn.TonnageValue = 10;
		prnList[6].Eprn.ObligationYear = obligationCalculationYear.ToString();

		prnList[7].Eprn.MaterialName = PrnConstants.Materials.PaperComposting;
		prnList[7].Eprn.PrnStatusId = 1;
		prnList[7].Status.StatusName = EprnStatus.ACCEPTED.ToString();
		prnList[7].Eprn.TonnageValue = 10;
		prnList[7].Eprn.ObligationYear = obligationCalculationYear.ToString();

		var prns = prnList.AsQueryable();
		_mockMaterialRepository.Setup(repo => repo.GetAllMaterials()).ReturnsAsync(materials);
		_mockObligationCalculationRepository.Setup(repo => repo.GetObligationCalculationBySubmitterIdAndYear(submitterId, obligationCalculationYear)).ReturnsAsync(obligationCalculations);
		_mockPrnRepository.Setup(repo => repo.GetAcceptedAndAwaitingPrnsByYear(submitterId, obligationCalculationYear)).Returns(prns);
		_mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync(GetRecyclingTargets());

		// Act
		var result = await _service.GetObligationCalculation(submitterId, obligationCalculationYear);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.ObligationModel.Should().NotBeNull();

		var obligationData = result.ObligationModel.ObligationData;
		obligationData.Should().NotBeNull();
		obligationData.Count.Should().Be(7);
		obligationData.Find(d => d.MaterialName == MaterialType.Aluminium.ToString()).TonnageOutstanding.Should().Be(0);
		obligationData.Find(d => d.MaterialName == MaterialType.Paper.ToString()).TonnageOutstanding.Should().Be(0);
		obligationData.Find(d => d.MaterialName == MaterialType.Steel.ToString()).TonnageOutstanding.Should().Be(0);
		obligationData.Find(d => d.MaterialName == MaterialType.Wood.ToString()).TonnageOutstanding.Should().Be(0);
		obligationData.Find(d => d.MaterialName == MaterialType.Plastic.ToString()).TonnageOutstanding.Should().Be(0);
		obligationData.Find(d => d.MaterialName == MaterialType.GlassRemelt.ToString()).TonnageOutstanding.Should().Be(0);
		obligationData.Find(d => d.MaterialName == MaterialType.Glass.ToString()).TonnageOutstanding.Should().Be(0);
	}

	[TestMethod]
	public async Task GetObligationCalculation_ShouldReturnSuccess_WithNull_ForMaterialsNotSubmitted()
	{
		// Arrange
		var materials = GetMaterials();

		var obligationCalculations = _fixture.CreateMany<ObligationCalculation>(2).ToList();
		obligationCalculations[0].MaterialId = 1; //Plastic
		obligationCalculations[0].MaterialObligationValue = 100;
		obligationCalculations[1].MaterialId = 2; // Wood
		obligationCalculations[1].MaterialObligationValue = 200;

		var prnList = _fixture.CreateMany<EprnResultsDto>(2).ToList();

		var prns = prnList.AsQueryable();
		_mockMaterialRepository.Setup(repo => repo.GetAllMaterials()).ReturnsAsync(materials);
		_mockObligationCalculationRepository.Setup(repo => repo.GetObligationCalculationBySubmitterIdAndYear(submitterId, obligationCalculationYear)).ReturnsAsync(obligationCalculations);
		_mockPrnRepository.Setup(repo => repo.GetAcceptedAndAwaitingPrnsByYear(submitterId, obligationCalculationYear)).Returns(prns);
		_mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync(GetRecyclingTargets());

		// Act
		var result = await _service.GetObligationCalculation(submitterId, obligationCalculationYear);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.ObligationModel.Should().NotBeNull();

		var obligationData = result.ObligationModel.ObligationData;
		obligationData.Should().NotBeNull();
		obligationData.Count.Should().Be(7);

		obligationData.Find(d => d.MaterialName == MaterialType.Plastic.ToString()).ObligationToMeet.Should().NotBeNull();
		obligationData.Find(d => d.MaterialName == MaterialType.Wood.ToString()).ObligationToMeet.Should().NotBeNull();

		obligationData.Find(d => d.MaterialName == MaterialType.Glass.ToString()).ObligationToMeet.Should().BeNull();
		obligationData.Find(d => d.MaterialName == MaterialType.GlassRemelt.ToString()).ObligationToMeet.Should().BeNull();
		obligationData.Find(d => d.MaterialName == MaterialType.Paper.ToString()).ObligationToMeet.Should().BeNull();
		obligationData.Find(d => d.MaterialName == MaterialType.Steel.ToString()).ObligationToMeet.Should().BeNull();
		obligationData.Find(d => d.MaterialName == MaterialType.Aluminium.ToString()).ObligationToMeet.Should().BeNull();
	}

	[TestMethod]
	public async Task GetObligationCalculation_ShouldReturnResponse_WhenNoObligationExists()
	{
		// Arrange
		var materials = GetMaterials();
		var responseMaterials = materials.Where(m => m.MaterialName != MaterialType.FibreComposite.ToString());
		var obligationCalculations = new List<ObligationCalculation>();

		var prnList = _fixture.CreateMany<EprnResultsDto>().ToList();

		var prns = prnList.AsQueryable();
		_mockMaterialRepository.Setup(repo => repo.GetAllMaterials()).ReturnsAsync(materials);
		_mockObligationCalculationRepository.Setup(repo => repo.GetObligationCalculationBySubmitterIdAndYear(submitterId, obligationCalculationYear)).ReturnsAsync(obligationCalculations);
		_mockPrnRepository.Setup(repo => repo.GetAcceptedAndAwaitingPrnsByYear(submitterId, obligationCalculationYear)).Returns(prns);
		_mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync(GetRecyclingTargets());

		// Act
		var result = await _service.GetObligationCalculation(submitterId, obligationCalculationYear);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.ObligationModel.Should().NotBeNull();
		foreach (var material in responseMaterials)
		{
			var obligationData = result.ObligationModel.ObligationData.Find(d => d.MaterialName == material.MaterialName);
			obligationData.Should().NotBeNull();
			obligationData.MaterialName.Should().Be(material.MaterialName);
			obligationData.ObligationToMeet.Should().BeNull();
			obligationData.TonnageOutstanding.Should().BeNull();
			obligationData.Status.Should().Be(ObligationConstants.Statuses.NoDataYet);
		}
	}

	[TestMethod]
	public async Task GetObligationCalculation_ShouldReturnSuccess_WithNoData()
	{
		// Arrange
		var materials = new List<Material>();
		_mockMaterialRepository.Setup(repo => repo.GetAllMaterials()).ReturnsAsync(materials);

		// Act
		var result = await _service.GetObligationCalculation(submitterId, obligationCalculationYear);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Errors.Should().Contain($"No Materials found in PRN Backend Database");
	}

	[TestMethod]
	public async Task GetObligationCalculation_ShouldHandlePRNAwaitingAcceptanceCorrectly()
	{
		// Arrange		
		var materials = _fixture.CreateMany<Material>(5).ToList();
		var obligationCalculations = _fixture.CreateMany<ObligationCalculation>(6).ToList();
		var prnList = _fixture.CreateMany<EprnResultsDto>(5).ToList();
		prnList[0].Eprn.PrnStatusId = 4;
		prnList[0].Status.Id = 4;
		prnList[0].Status.StatusName = EprnStatus.AWAITINGACCEPTANCE.ToString();
		prnList[2].Eprn.PrnStatusId = 4;
		prnList[2].Status.Id = 4;
		prnList[2].Status.StatusName = EprnStatus.AWAITINGACCEPTANCE.ToString();
		prnList[4].Eprn.PrnStatusId = 4;
		prnList[4].Status.Id = 4;
		prnList[4].Status.StatusName = EprnStatus.AWAITINGACCEPTANCE.ToString();
		var prns = prnList.AsQueryable();
		_mockMaterialRepository.Setup(repo => repo.GetAllMaterials()).ReturnsAsync(materials);
		_mockObligationCalculationRepository.Setup(repo => repo.GetObligationCalculationBySubmitterIdAndYear(submitterId, obligationCalculationYear)).ReturnsAsync(obligationCalculations);
		_mockPrnRepository.Setup(repo => repo.GetAcceptedAndAwaitingPrnsByYear(submitterId, obligationCalculationYear)).Returns(prns);
		_mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync(GetRecyclingTargets());

		// Act
		var result = await _service.GetObligationCalculation(submitterId, obligationCalculationYear);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.ObligationModel.NumberOfPrnsAwaitingAcceptance.Should().Be(3);
	}

    [TestMethod]
    public async Task CalculatePomDataAsync_WhenSubmissionMaterialIsNullOrEmpty_ShouldLogErrorAndSkip()
    {
		// Arrange
        var organisationId = Guid.NewGuid();
        var submissions = new List<SubmissionCalculationRequest>
        {
            new()
			{
				OrganisationId = organisationId,
				PackagingMaterial = null,
				SubmitterId = submitterId,
				SubmitterType = ObligationCalculationOrganisationSubmitterTypeName.DirectRegistrant.ToString()
			}
        };
        _mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync([]);
		var loggedMessages = MockLogger();

		// Act
		var result = await _service.CalculateAsync(submitterId, submissions);

		// Assert
		result.Success.Should().BeFalse();
		loggedMessages.Should().Contain($"Material was null or empty for OrganisationId: {organisationId} and SubmitterId: {submitterId}.");
		loggedMessages.Should().Contain($"No calculations for SubmitterId: {submitterId}.");
	}

    [TestMethod]
    public async Task CalculatePomDataAsync_WhenMaterialIsInvalid_ShouldLogErrorAndSkip()
    {
		// Arrange
        var organisationId = Guid.NewGuid();
        var packagingMaterial = "InvalidMaterial";
        var submissions = new List<SubmissionCalculationRequest>
        {
            new()
			{
				OrganisationId = organisationId,
				PackagingMaterial = packagingMaterial,
				SubmitterId = submitterId,
				SubmitterType = ObligationCalculationOrganisationSubmitterTypeName.DirectRegistrant.ToString()
			}
        };
        _mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync([]);
        var loggedMessages = MockLogger();

		// Act
		var result = await _service.CalculateAsync(submitterId, submissions);

		// Assert
		result.Success.Should().BeFalse();
		loggedMessages.Should().Contain($"Material provided was not valid: {packagingMaterial} for OrganisationId: {organisationId} and SubmitterId: {submitterId}.");
		loggedMessages.Should().Contain($"No calculations for SubmitterId: {submitterId}.");
	}

	[TestMethod]
	public async Task CalculatePomDataAsync_WhenStrategyIsNull_ShouldLogErrorAndSkip()
	{
		// Arrange
		var organisationId = Guid.NewGuid();
		var packagingMaterial = "PL";
		var materials = GetMaterials();
		var submissions = new List<SubmissionCalculationRequest>
		{
			new()
			{
				OrganisationId = organisationId,
				PackagingMaterial = packagingMaterial,
				SubmitterId = organisationId,
				SubmitterType = ObligationCalculationOrganisationSubmitterTypeName.DirectRegistrant.ToString()
			}
		};
		_mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync([]);
		_mockMaterialRepository.Setup(repo => repo.GetAllMaterials()).ReturnsAsync(materials);
		_mockStrategyResolver.Setup(x => x.Resolve(MaterialType.Plastic)).Returns((IMaterialCalculationStrategy)null);
		var loggedMessages = MockLogger();

		// Act
		var result = await _service.CalculateAsync(organisationId, submissions);

		// Assert
		result.Success.Should().BeFalse();
		loggedMessages.Should().Contain($"Could not find handler for Material Type: {packagingMaterial} for OrganisationId: {organisationId} and SubmitterId: {organisationId}.");
		loggedMessages.Should().Contain($"No calculations for SubmitterId: {organisationId}.");
	}

    [TestMethod]
    public async Task CalculatePomDataAsync_WhenCalculationsAreEmpty_ShouldLogError()
    {
		// Arrange
        var packagingMaterial = "PL";
		var materials = GetMaterials();
		var submissions = new List<SubmissionCalculationRequest>
        {
            new() {
				OrganisationId = Guid.NewGuid(),
				PackagingMaterial = packagingMaterial,
				SubmitterId = submitterId,
				SubmitterType = ObligationCalculationOrganisationSubmitterTypeName.ComplianceScheme.ToString()
			}
        };

		_mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync([]);
		_mockMaterialRepository.Setup(repo => repo.GetAllMaterials()).ReturnsAsync(materials);
		var mockStrategy = new Mock<IMaterialCalculationStrategy>();
		mockStrategy.Setup(x => x.Calculate(It.IsAny<CalculationRequestDto>())).Returns([]);
		_mockStrategyResolver.Setup(x => x.Resolve(MaterialType.Plastic)).Returns(mockStrategy.Object);
		var loggedMessages = MockLogger();

		// Act
		var result = await _service.CalculateAsync(submitterId, submissions);

		// Assert
		result.Success.Should().BeFalse();
		loggedMessages.Should().Contain($"No calculations for SubmitterId: {submitterId}.");
	}

	[TestMethod]
	[DataRow("")]
	[DataRow(null)]
	[DataRow("Random")]
	public async Task CalculatePomDataAsync_WhenCalculationsAreEmpty_ShouldLogWarning(string submitterType)
	{
		// Arrange
		var packagingMaterial = "PL";
		var materials = GetMaterials();
		var submissions = new List<SubmissionCalculationRequest>
		{
			new() {
				OrganisationId = Guid.NewGuid(),
				PackagingMaterial = packagingMaterial,
				SubmitterId = submitterId,
				SubmitterType = submitterType
			}
		};

		_mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync([]);
		_mockMaterialRepository.Setup(repo => repo.GetAllMaterials()).ReturnsAsync(materials);
		var mockStrategy = new Mock<IMaterialCalculationStrategy>();
		mockStrategy.Setup(x => x.Calculate(It.IsAny<CalculationRequestDto>())).Returns([]);
		_mockStrategyResolver.Setup(x => x.Resolve(MaterialType.Plastic)).Returns(mockStrategy.Object);
		var loggedMessages = MockLogger();

		// Act
		var result = await _service.CalculateAsync(submitterId, submissions);

		// Assert
		result.Success.Should().BeFalse();
		loggedMessages.Should().Contain(msg => msg.Contains("SubmitterType provided is not valid"));
	}

	[TestMethod]
    public async Task CalculatePomDataAsync_WhenCalculationsAreSuccessful_ShouldReturnSuccess()
    {
		//Arrange
		var organisationId = Guid.NewGuid();
        var packagingMaterial = "PL";
		var materialWeight = 120;
		var recyclingTargets = GetRecyclingTargets();
		var materials = GetMaterials();
		var submitterTypeName = ObligationCalculationOrganisationSubmitterTypeName.ComplianceScheme;
		var submitterTypeId = (int)submitterTypeName;

		var submissions = new List<SubmissionCalculationRequest>
		{
			new() {
				OrganisationId = Guid.NewGuid(),
				SubmitterId = submitterId,
				SubmitterType = ObligationCalculationOrganisationSubmitterTypeName.ComplianceScheme.ToString(),
				PackagingMaterial = packagingMaterial,
				PackagingMaterialWeight = materialWeight,
				SubmissionPeriod = "2024"
			}
		};

		_mockRecyclingTargetDataService.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync(recyclingTargets);
		_mockMaterialRepository.Setup(repo => repo.GetAllMaterials()).ReturnsAsync(materials);
		_mockSubmitterTypeRepository.Setup(repo => repo.GetSubmitterTypeIdByTypeName(submitterTypeName)).ReturnsAsync(submitterTypeId);
		
		var mockStrategy = new Mock<IMaterialCalculationStrategy>();
		mockStrategy.Setup(x => x.Calculate(It.IsAny<CalculationRequestDto>())).Returns(
		[
			new ObligationCalculation()
		]);
		_mockStrategyResolver.Setup(x => x.Resolve(MaterialType.Plastic)).Returns(mockStrategy.Object);

		//Act
		var result = await _service.CalculateAsync(organisationId, submissions);

		//Aseert
		result.Success.Should().BeTrue();
		result.Calculations.Should().NotBeNullOrEmpty();

		_mockRecyclingTargetDataService.Verify(x => x.GetRecyclingTargetsAsync(), Times.Once);
		_mockSubmitterTypeRepository.Verify(x => x.GetSubmitterTypeIdByTypeName(submitterTypeName), Times.Once);
		_mockMaterialRepository.Verify(x => x.GetAllMaterials(), Times.Once);

		// Verify strategy resolver was called for each material type
		_mockStrategyResolver.Verify(x => x.Resolve(MaterialType.Plastic), Times.Once);

		// Verify that the individual strategies' Calculate methods were called
		mockStrategy.Verify(s => s.Calculate(It.IsAny<CalculationRequestDto>()), Times.Once);
	}

	[TestMethod]
	public async Task SoftDeleteAndAddObligationCalculationAsync_Should_CallRepository_WhenValidCalculationsAreProvided()
	{
		//Arrange
		var currentYear = DateTime.Now.Year;
		var calculations = new List<ObligationCalculation>
		{
			new()
			{
				OrganisationId = Guid.NewGuid(),
				SubmitterId = submitterId,
				SubmitterTypeId = (int)ObligationCalculationOrganisationSubmitterTypeName.ComplianceScheme,
				MaterialId = 1,
				Year = obligationCalculationYear,
				Tonnage = 100,
				MaterialObligationValue = 50
			}
		};
		_mockDateTimeProvider.Setup(m => m.CurrentYear).Returns(currentYear);

		//Act
		await _service.SoftDeleteAndAddObligationCalculationAsync(submitterId, calculations);

		//Assert
		_mockObligationCalculationRepository.Verify(x => x.SoftDeleteAndAddObligationCalculationBySubmitterIdAsync(submitterId, currentYear, calculations), Times.Once);
	}


	[TestMethod]
	public async Task SoftDeleteAndAddObligationCalculationAsync_EmptyCalculations_ShouldThrowArgumentException()
	{
		// Arrange
		var calculations = new List<ObligationCalculation>();

		// Act & Assert
		await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
			_service.SoftDeleteAndAddObligationCalculationAsync(Guid.NewGuid(), calculations));
	}

	[TestMethod]
	public async Task SoftDeleteAndAddObligationCalculationAsync_NullCalculations_ShouldThrowArgumentException()
	{
		// Arrange
		List<ObligationCalculation> calculations = null;

		// Act & Assert
		await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
			_service.SoftDeleteAndAddObligationCalculationAsync(Guid.NewGuid(), calculations));
	}

	private List<string> MockLogger()
	{
		var loggedMessages = new List<string>();
		_mockLogger.Setup(x => x.Log(
			LogLevel.Error,
			It.IsAny<EventId>(),
			It.Is<It.IsAnyType>((v, t) => true),
			It.IsAny<Exception>(),
			It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)))
		.Callback((LogLevel logLevel, EventId eventId, object state, Exception exception, Delegate formatter) =>
		{
			if (formatter != null)
			{
                string logMessage = formatter.DynamicInvoke(state, exception) as string;
                if (!string.IsNullOrEmpty(logMessage))
                {
					loggedMessages.Add(logMessage);
				}
			}
		});
		return loggedMessages;
	}

	public static Dictionary<int, Dictionary<MaterialType, double>> GetRecyclingTargets()
	{
		var targets = new List<RecyclingTarget>()
		{
            // Paper
            new() { Id = 1, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.75, Year = 2025 },
			new() { Id = 2, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.77, Year = 2026 },
			new() { Id = 3, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.79, Year = 2027 },
			new() { Id = 4, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.81, Year = 2028 },
			new() { Id = 5, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.83, Year = 2029 },
			new() { Id = 6, MaterialNameRT = MaterialType.Paper.ToString(), Target = 0.85, Year = 2030 },

            // Glass
            new() { Id = 7, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.74, Year = 2025 },
			new() { Id = 8, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.76, Year = 2026 },
			new() { Id = 9, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.78, Year = 2027 },
			new() { Id = 10, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.80, Year = 2028 },
			new() { Id = 11, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.82, Year = 2029 },
			new() { Id = 12, MaterialNameRT = MaterialType.Glass.ToString(), Target = 0.85, Year = 2030 },

            // Aluminium
            new() { Id = 13, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.61, Year = 2025 },
			new() { Id = 14, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.62, Year = 2026 },
			new() { Id = 15, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.63, Year = 2027 },
			new() { Id = 16, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.64, Year = 2028 },
			new() { Id = 17, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.65, Year = 2029 },
			new() { Id = 18, MaterialNameRT = MaterialType.Aluminium.ToString(), Target = 0.67, Year = 2030 },

            // Steel
            new() { Id = 19, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.8, Year = 2025 },
			new() { Id = 20, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.81, Year = 2026 },
			new() { Id = 21, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.82, Year = 2027 },
			new() { Id = 22, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.83, Year = 2028 },
			new() { Id = 23, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.84, Year = 2029 },
			new() { Id = 24, MaterialNameRT = MaterialType.Steel.ToString(), Target = 0.85, Year = 2030 },

            // Plastic
            new() { Id = 25, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.55, Year = 2025 },
			new() { Id = 26, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.57, Year = 2026 },
			new() { Id = 27, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.59, Year = 2027 },
			new() { Id = 28, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.61, Year = 2028 },
			new() { Id = 29, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.63, Year = 2029 },
			new() { Id = 30, MaterialNameRT = MaterialType.Plastic.ToString(), Target = 0.65, Year = 2030 },

            // Wood
            new() { Id = 31, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.45, Year = 2025 },
			new() { Id = 32, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.46, Year = 2026 },
			new() { Id = 33, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.47, Year = 2027 },
			new() { Id = 34, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.48, Year = 2028 },
			new() { Id = 35, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.49, Year = 2029 },
			new() { Id = 36, MaterialNameRT = MaterialType.Wood.ToString(), Target = 0.50, Year = 2030 },

            // Glass Remelt
            new() { Id = 37, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.75, Year = 2025 },
			new() { Id = 38, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.76, Year = 2026 },
			new() { Id = 39, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.77, Year = 2027 },
			new() { Id = 40, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.78, Year = 2028 },
			new() { Id = 41, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.79, Year = 2029 },
			new() { Id = 42, MaterialNameRT = MaterialType.GlassRemelt.ToString(), Target = 0.80, Year = 2030 },

            // Fibre Composite
			new() { Id = 43, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.55, Year = 2025 },
			new() { Id = 44, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.56, Year = 2026 },
			new() { Id = 45, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.77, Year = 2027 },
			new() { Id = 46, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.88, Year = 2028 },
			new() { Id = 47, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.79, Year = 2029 },
			new() { Id = 48, MaterialNameRT = MaterialType.FibreComposite.ToString(), Target = 0.80, Year = 2030 }
		};

		return targets
		.GroupBy(target => target.Year)
		.ToDictionary(
			group => group.Key,
			group => TransformTargets([.. group])
		);
	}

	private static Dictionary<MaterialType, double> TransformTargets(List<RecyclingTarget> recyclingTargets)
	{
		var dictionary = new Dictionary<MaterialType, double>();

		foreach (var target in recyclingTargets)
		{
			dictionary[EnumHelper.ConvertStringToEnum<MaterialType>(target.MaterialNameRT).Value] = target.Target;
		}

		return dictionary;
	}

	private static List<Material> GetMaterials()
	{
		return
		[
			new Material
			{
				Id = 1, MaterialCode = "PL",
				MaterialName = MaterialType.Plastic.ToString(),
				PrnMaterialMappings =
				[
					new PrnMaterialMapping()
					{
						Id = 1,
						PRNMaterialId = 1,
						NPWDMaterialName = PrnConstants.Materials.Plastic
					}
				]
			},
			new Material
			{
				Id = 2,
				MaterialCode = "WD",
				MaterialName = MaterialType.Wood.ToString(),
				PrnMaterialMappings =
				[
					new PrnMaterialMapping()
					{
						Id = 2,
						PRNMaterialId = 2,
						NPWDMaterialName = PrnConstants.Materials.Wood
					},
					new PrnMaterialMapping()
					{
						Id = 3,
						PRNMaterialId = 2,
						NPWDMaterialName = PrnConstants.Materials.WoodComposting
					}
				]
			},
			new Material
			{
				Id = 3,
				MaterialCode = "AL",
				MaterialName = MaterialType.Aluminium.ToString(),
				PrnMaterialMappings =
				[
					new PrnMaterialMapping()
					{
						Id = 4,
						PRNMaterialId = 3,
						NPWDMaterialName = PrnConstants.Materials.Aluminium
					}
				]
			},
			new Material
			{
				Id = 4,
				MaterialCode = "ST",
				MaterialName = MaterialType.Steel.ToString(),
				PrnMaterialMappings =
				[
					new PrnMaterialMapping()
					{
						Id = 5,
						PRNMaterialId = 4,
						NPWDMaterialName = PrnConstants.Materials.Steel
					}
				]
			},
			new Material
			{
				Id = 5,
				MaterialCode = "PC",
				MaterialName = MaterialType.Paper.ToString(),
				PrnMaterialMappings =
				[
					new PrnMaterialMapping()
					{
						Id = 6,
						PRNMaterialId = 5,
						NPWDMaterialName = PrnConstants.Materials.PaperFiber
					},
					new PrnMaterialMapping()
					{
						Id = 7,
						PRNMaterialId = 5,
						NPWDMaterialName = PrnConstants.Materials.PaperComposting
					}
				]
			},
			new Material
			{
				Id = 6,
				MaterialCode = "GL",
				MaterialName = MaterialType.Glass.ToString(),
				PrnMaterialMappings =
				[
					new PrnMaterialMapping()
					{
						Id = 8,
						PRNMaterialId = 6,
						NPWDMaterialName = PrnConstants.Materials.GlassOther
					}
				]
			},
			new Material
			{
				Id = 7,
				MaterialCode = "GR",
				MaterialName = MaterialType.GlassRemelt.ToString(),
				PrnMaterialMappings =
				[
					new PrnMaterialMapping()
					{
						Id = 9,
						PRNMaterialId = 7,
						NPWDMaterialName = PrnConstants.Materials.GlassMelt
					}
				]
			},
			new Material
			{
				Id = 8,
				MaterialCode = "FC",
				MaterialName = MaterialType.FibreComposite.ToString(),
			}
		];
	}
}