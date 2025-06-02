using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Controllers.Regulator;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;

using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

[TestClass]
public class RegulatorAccreditationControllerTests
{
    private Mock<IMediator> _mediatorMock;
    private Mock<IValidator<RegulatorAccreditationMarkAsDulyMadeCommand>> _validatorMock;
    private Mock<ILogger<RegulatorAccreditationController>> _loggerMock;
    private RegulatorAccreditationController _controller;

    [TestInitialize]
    public void TestInitialize()
    {
        _mediatorMock = new Mock<IMediator>();
        _validatorMock = new Mock<IValidator<RegulatorAccreditationMarkAsDulyMadeCommand>>();
        _loggerMock = new Mock<ILogger<RegulatorAccreditationController>>();
        _controller = new RegulatorAccreditationController(_mediatorMock.Object, _validatorMock.Object,  _loggerMock.Object);
    }

    [TestMethod]
    public async Task GetRegistrationAccreditationPaymentFeeDetailsById_ReturnsOk_WhenResultFound()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var dto = new AccreditationPaymentFeeDetailsDto
        {
            AccreditationId = accreditationId,
            OrganisationName = "Test Org",
            SiteAddress = "123 Test St, Testville, TS1 1AA",
            ApplicationReferenceNumber = "APP-001",
            MaterialName = "Plastic",
            SubmittedDate = DateTime.UtcNow,
            FeeAmount = 100.0m,
            ApplicationType = ApplicationOrganisationType.Reprocessor,
            Regulator = "EA"
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetRegistrationAccreditationPaymentFeesByIdQuery>(q => q.Id == accreditationId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        // Act
        var result = await _controller.GetRegistrationAccreditationPaymentFeeDetailsById(accreditationId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(dto);
    }


    [TestMethod]
    public async Task GetRegistrationAccreditationPaymentFeeDetailsById_ReturnsNotFound_WhenResultIsNull()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetRegistrationAccreditationPaymentFeesByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((AccreditationPaymentFeeDetailsDto)null!);

        // Act
        var result = await _controller.GetRegistrationAccreditationPaymentFeeDetailsById(accreditationId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
