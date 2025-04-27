using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;

using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EPR.PRN.Backend.API.Dto;

using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

[TestClass]
public class RegistrationMaterialControllerTests
{
    private Mock<IMediator> _mediatorMock;
    private Mock<IValidator<RegistrationMaterialsOutcomeCommand>> _validatorMock;
    private Mock<ILogger<RegistrationMaterialController>> _loggerMock;
    private RegistrationMaterialController _controller;

    [TestInitialize]
    public void TestInitialize()
    {
        _mediatorMock = new Mock<IMediator>();
        _validatorMock = new Mock<IValidator<RegistrationMaterialsOutcomeCommand>>();
        _loggerMock = new Mock<ILogger<RegistrationMaterialController>>();

        _controller = new RegistrationMaterialController(_mediatorMock.Object, _validatorMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task GetRegistrationOverviewDetailById_ReturnsOk_WithExpectedResult()
    {
        // Arrange
        int registrationId = 1;
        var expectedDto = new RegistrationOverviewDto
        {
            OrganisationName = "Test Organisation",
            Regulator = "Test Regulator"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetRegistrationOverviewDetailByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _controller.GetRegistrationOverviewDetailById(registrationId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(expectedDto);
    }

    [TestMethod]
    public async Task GetMaterialDetailById_ReturnsOk_WithExpectedResult()
    {
        // Arrange
        int materialId = 2;
        var expectedDto = new RegistrationMaterialDetailsDto();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetMaterialDetailByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _controller.GetMaterialDetailById(materialId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(expectedDto);
    }

    [TestMethod]
    public async Task UpdateRegistrationOutcome_ValidCommand_ReturnsNoContent()
    {
        // Arrange
        int materialId = 3;
        var command = new RegistrationMaterialsOutcomeCommand();

        _validatorMock
            .Setup(v => v.ValidateAsync(command, default))
            .ReturnsAsync(new ValidationResult());

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<RegistrationMaterialsOutcomeCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.UpdateRegistrationOutcome(materialId, command);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        command.Id.Should().Be(materialId);
    }

    [TestMethod]
    public async Task UpdateRegistrationOutcome_ShouldThrowValidationException_WhenValidationFails()
    {
        // Arrange
        var validator = new InlineValidator<RegistrationMaterialsOutcomeCommand>();
        validator.RuleFor(x => x.Status).Must(_ => false).WithMessage("Validation failed");

        var registrationId = 10;
        var command = new RegistrationMaterialsOutcomeCommand
        {
            Status = (RegistrationMaterialStatus)999
        };

        var controller = new RegistrationMaterialController(
            _mediatorMock.Object,
            validator,
            _loggerMock.Object
        );

        // Act & Assert
        await FluentActions.Invoking(() =>
            controller.UpdateRegistrationOutcome(registrationId, command)
        ).Should().ThrowAsync<ValidationException>();
    }

    [TestMethod]
    public async Task GetRegistrationOverviewDetailById_ReturnsOk_WithFullTasksAndMaterials()
    {
        // Arrange
        int registrationId = 2;

        var expectedDto = new RegistrationOverviewDto
        {
            Id = 2,
            OrganisationName = "1_Green Ltd",
            SiteAddress = "23, Ruby St, London, E12 3SE",
            OrganisationType = ApplicationOrganisationType.Reprocessor,
            Regulator = "EA",
            Tasks = new List<RegistrationTaskDto>
        {
            new RegistrationTaskDto
            {
                Id = 0,
                TaskName = "SiteAddressAndContactDetails",
                Status = "NotStarted",
                TaskData = new SiteAddressAndContactDetailsTaskDataDto
                {
                    NationId = 1,
                    SiteAddress = "23, Ruby St, London, E12 3SE",
                    GridReference = "SJ 854 662",
                    LegalCorrespondenceAddress = "23, Ruby St, London, E12 3SE"
                }
            },
            new RegistrationTaskDto
            {
                Id = 0,
                TaskName = "MaterialsAuthorisedOnSite",
                Status = "NotStarted",
                TaskData = new MaterialsAuthorisedOnSiteTaskDataDto
                {
                    RegistrationNumber = "DFG34573452",
                    MaterialsAuthorisation = new List<MaterialsAuthorisedOnSiteInfoDto>
                    {
                        new MaterialsAuthorisedOnSiteInfoDto
                        {
                            Material = "Plastic",
                            RegistrationStatus = "NotStarted",
                            Reason = "Lorem ipsum dolor sit amet, consectetur adipiscing1 elit. Fusce vulputate aliquet ornare. Vestibulum dolor nunc, tincidunt a diam nec, mattis venenatis sem2"
                        },
                        new MaterialsAuthorisedOnSiteInfoDto
                        {
                            Material = "Steel",
                            RegistrationStatus = "NotStarted",
                            Reason = "Lorem ipsum dolor sit amet, consectetur adipiscing2 elit. Fusce vulputate aliquet ornare. Vestibulum dolor nunc, tincidunt a diam nec, mattis venenatis sem2"
                        },
                        new MaterialsAuthorisedOnSiteInfoDto
                        {
                            Material = "Aluminium",
                            RegistrationStatus = "NotStarted",
                            Reason = "Lorem ipsum dolor sit amet, consectetur adipiscing3 elit. Fusce vulputate aliquet ornare. Vestibulum dolor nunc, tincidunt a diam nec, mattis venenatis sem2"
                        }
                    }
                }
            },
            new RegistrationTaskDto
            {
                Id = 0,
                TaskName = "RegistrationDulyMade",
                Status = "NotStarted",
                TaskData = null
            }
        },
            Materials = new List<RegistrationMaterialDto>
        {
            new RegistrationMaterialDto
            {
                Id = 4,
                RegistrationId = 2,
                MaterialName = "Plastic",
                Status = null,
                StatusUpdatedBy = null,
                StatusUpdatedDate = DateTime.MinValue,
                RegistrationReferenceNumber = "REF0002-01",
                Comments = "Test description for material 1 in registration 2",
                DeterminationDate = DateTime.Parse("2025-04-27T08:03:19.4429901Z"),
                Tasks = new List<RegistrationTaskDto>
                {
                    new RegistrationTaskDto { Id = 0, TaskName = "WasteLicensesPermitsAndExemptions", Status = "NotStarted" },
                    new RegistrationTaskDto { Id = 0, TaskName = "ReprocessingInputsAndOutputs", Status = "NotStarted" },
                    new RegistrationTaskDto { Id = 0, TaskName = "SamplingAndInspectionPlan", Status = "NotStarted" },
                    new RegistrationTaskDto { Id = 0, TaskName = "AssignOfficer", Status = "NotStarted" }
                }
            },
            new RegistrationMaterialDto
            {
                Id = 5,
                RegistrationId = 2,
                MaterialName = "Steel",
                Status = null,
                StatusUpdatedBy = null,
                StatusUpdatedDate = DateTime.MinValue,
                RegistrationReferenceNumber = "REF0002-02",
                Comments = "Test description for material 2 in registration 2",
                DeterminationDate = DateTime.Parse("2025-04-27T08:03:19.4429905Z"),
                Tasks = new List<RegistrationTaskDto>
                {
                    new RegistrationTaskDto { Id = 0, TaskName = "WasteLicensesPermitsAndExemptions", Status = "NotStarted" },
                    new RegistrationTaskDto { Id = 0, TaskName = "ReprocessingInputsAndOutputs", Status = "NotStarted" },
                    new RegistrationTaskDto { Id = 0, TaskName = "SamplingAndInspectionPlan", Status = "NotStarted" },
                    new RegistrationTaskDto { Id = 0, TaskName = "AssignOfficer", Status = "NotStarted" }
                }
            },
            new RegistrationMaterialDto
            {
                Id = 6,
                RegistrationId = 2,
                MaterialName = "Aluminium",
                Status = null,
                StatusUpdatedBy = null,
                StatusUpdatedDate = DateTime.MinValue,
                RegistrationReferenceNumber = "REF0002-03",
                Comments = "Test description for material 3 in registration 2",
                DeterminationDate = DateTime.Parse("2025-04-27T08:03:19.442991Z"),
                Tasks = new List<RegistrationTaskDto>
                {
                    new RegistrationTaskDto { Id = 0, TaskName = "WasteLicensesPermitsAndExemptions", Status = "NotStarted" },
                    new RegistrationTaskDto { Id = 0, TaskName = "ReprocessingInputsAndOutputs", Status = "NotStarted" },
                    new RegistrationTaskDto { Id = 0, TaskName = "SamplingAndInspectionPlan", Status = "NotStarted" },
                    new RegistrationTaskDto { Id = 0, TaskName = "AssignOfficer", Status = "NotStarted" }
                }
            }
        }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetRegistrationOverviewDetailByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _controller.GetRegistrationOverviewDetailById(registrationId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(expectedDto);
    }


}
