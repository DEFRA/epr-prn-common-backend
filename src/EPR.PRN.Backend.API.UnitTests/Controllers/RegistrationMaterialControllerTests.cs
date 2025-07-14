using AutoFixture;
using AutoMapper;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DTO;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

[TestClass]
public class RegistrationMaterialControllerTests
{
    private Mock<IMediator> _mediatorMock;
    private Mock<IValidationService> _validationServiceMock;
    private Mock<ILogger<RegistrationMaterialController>> _loggerMock;
    private Mock<IMapper> _mapperMock;
    private RegistrationMaterialController _controller;
    private static readonly Fixture _fixture = new();

    [TestInitialize]
    public void TestInitialize()
    {
        _mediatorMock = new Mock<IMediator>();
        _validationServiceMock = new Mock<IValidationService>();
        _loggerMock = new Mock<ILogger<RegistrationMaterialController>>();
        _mapperMock = new Mock<IMapper>();
        _controller = new RegistrationMaterialController(_mediatorMock.Object, _validationServiceMock.Object, _loggerMock.Object, _mapperMock.Object);
    }

    [TestMethod]
    public async Task CreateExemptionReferences_ValidCommand_ReturnsNoContent()
    {
        // Arrange  
        var command = new CreateExemptionReferencesCommand
        {           
            MaterialExemptionReferences = new List<MaterialExemptionReferenceDto>()
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateExemptionReferencesCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act  
        var result = await _controller.CreateExemptionReferences(Guid.NewGuid(), command);

        // Assert  
        result.Should().BeOfType<OkResult>();


        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.AtLeastOnce
        );
    }

    [TestMethod]
    public async Task CreateRegistrationMaterialExpectedCreatedResult()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var expectedResult = new CreatedResult(string.Empty, new CreateRegistrationMaterialDto
        {
            Id = externalId
        });
        var validator = new InlineValidator<RegistrationMaterialsOutcomeCommand>();
        validator.RuleFor(x => x.Status).Must(_ => false).WithMessage("Validation failed");

        var command = new CreateRegistrationMaterialCommand
        {
            RegistrationId = externalId,
            Material = "Steel"
        };

        // Expectations
        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreateRegistrationMaterialDto
            {
                Id = externalId
            });

        // Act
        var result = await _controller.CreateRegistrationMaterial(command);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task UpdateRegistrationMaterialPermits_ShouldReturnNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = _fixture.Build<UpdateRegistrationMaterialPermitsCommand>()
                              .Without(c => c.RegistrationMaterialId)
                              .Create();

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateRegistrationMaterialPermitsCommand>(), It.IsAny<CancellationToken>()))
        .Returns(Task.CompletedTask);

        _validationServiceMock.Setup(v => v.ValidateAsync(command, default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var result = await _controller.UpdateRegistrationMaterialPermits(id, command);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        command.RegistrationMaterialId.Should().Be(id);
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task UpdateRegistrationMaterialPermitCapacity_ShouldReturnNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = _fixture.Build<UpdateRegistrationMaterialPermitCapacityCommand>()
                              .Without(c => c.RegistrationMaterialId)
                              .Create();

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateRegistrationMaterialPermitCapacityCommand>(), It.IsAny<CancellationToken>()))
        .Returns(Task.CompletedTask);

        _validationServiceMock.Setup(v => v.ValidateAsync(command, default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var result = await _controller.UpdateRegistrationMaterialPermitCapacity(id, command);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        command.RegistrationMaterialId.Should().Be(id);
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task GetMaterialsPermitTypes_ShouldReturnOkWithResult()
    {
        // Arrange
        var expectedList = _fixture.Create<List<MaterialsPermitTypeDto>>();

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetMaterialsPermitTypesQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(expectedList);

        // Act
        var result = await _controller.GetMaterialsPermitTypes();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedList);
    }

    [TestMethod]
    public async Task DeleteRegistrationMaterial_EnsureCorrectResult()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var expectedResult = new OkResult();

        var command = new DeleteRegistrationMaterialCommand
        {
            RegistrationMaterialId = externalId
        };

        // Expectations
        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()));

        // Act
        var result = await _controller.DeleteRegistrationMaterial(externalId);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task UpdateMaximumWeight_EnsureCorrectResult()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var expectedResult = new OkResult();

        var command = new UpdateMaximumWeightCommand
        {
            RegistrationMaterialId = externalId,
            WeightInTonnes = 10,
            PeriodId = 1
        };

        // Expectations
        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()));

        // Act
        var result = await _controller.UpdateMaximumWeight(externalId, command);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task UpdateRegistrationMaterialTaskStatus_EnsureCorrectResult()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var expectedResult = new NoContentResult();

        var command = new UpdateRegistrationMaterialTaskStatusCommand
        {
            RegistrationMaterialId = externalId,
            Status = TaskStatuses.Completed,
            TaskName = ApplicantRegistrationTaskNames.SiteAddressAndContactDetails
        };

        // Expectations
        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()));

        // Act
        var result = await _controller.UpdateRegistrationMaterialTaskStatus(externalId, command);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

	[TestMethod]
	public async Task UpdateIsMaterialRegisteredAsync_ShouldReturnNoContent()
	{
		// Arrange
		var dtoList = _fixture.Create<List<UpdateIsMaterialRegisteredDto>>();

		_mediatorMock
			.Setup(m => m.Send(It.IsAny<UpdateIsMaterialRegisteredCommand>(), It.IsAny<CancellationToken>()))
			.Returns(Task.CompletedTask);

		// Act
		var result = await _controller.UpdateIsMaterialRegisteredAsync(dtoList);

		// Assert
		result.Should().BeOfType<NoContentResult>();
		_mediatorMock.Verify(m =>
			m.Send(It.Is<UpdateIsMaterialRegisteredCommand>(cmd =>
				cmd.UpdateIsMaterialRegisteredDto.SequenceEqual(dtoList)
			), It.IsAny<CancellationToken>()), Times.Once);
	}

    [TestMethod]
    public async Task UpsertRegistrationMaterialContactAsync_EnsureCorrectResult()
    {
        // Arrange  
        var registrationMaterialId = Guid.NewGuid();
        var registrationMaterialContact = _fixture.Create<RegistrationMaterialContactDto>();
        
        // Expectations  
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpsertRegistrationMaterialContactCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(registrationMaterialContact);

        // Act  
        var result = await _controller.UpsertRegistrationMaterialContactAsync(registrationMaterialId, registrationMaterialContact);

        // Assert  
        result.Should().BeOfType<OkObjectResult>();
        
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(registrationMaterialContact);
    }

    [TestMethod]
    public async Task UpsertRegistrationReprocessingDetailsAsync_EnsureCorrectResult()
    {
        // Arrange  
        var registrationMaterialId = Guid.NewGuid();
        var registrationReprocessingIO = _fixture.Create<RegistrationReprocessingIORequestDto>();
        var command = _fixture.Create<RegistrationReprocessingIOCommand>();

        // Expectations  
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<RegistrationReprocessingIOCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mapperMock
            .Setup(m => m.Map<RegistrationReprocessingIOCommand>(It.IsAny<RegistrationReprocessingIORequestDto>()))
            .Returns(command);

        // Act  
        var result = await _controller.UpsertRegistrationReprocessingDetailsAsync(registrationMaterialId, registrationReprocessingIO);

        // Assert  
        result.Should().BeOfType<OkResult>();
    }

    [TestMethod]
    public async Task SaveOverseasReprocessingSites_ShouldReturnNoContent_WhenValid()
    {
        // Arrange
        var registrationMaterialId = new Guid("3041bf68-6943-4fa0-8a02-7a8c587acf1d");
        var dto = new OverseasAddressSubmissionDto
        {            
            OverseasAddresses = new List<OverseasAddressDto>
            {
                new OverseasAddressDto
                {
                    ExternalId = Guid.NewGuid(),
                    AddressLine1 = "Test Line",
                    SiteCoordinates = "51.5074, -0.1278",
                    OrganisationName = "Test Organisation",
                    CityOrTown = "London",
                    CountryName = "United Kingdom",
                    CreatedBy = Guid.NewGuid(),
                    
                }
            }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateOverseasMaterialReprocessingSiteCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.SaveOverseasReprocessingSites(registrationMaterialId, dto);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _mediatorMock.Verify(m => m.Send(It.Is<CreateOverseasMaterialReprocessingSiteCommand>(cmd =>
            cmd.UpdateOverseasAddress.RegistrationMaterialId == registrationMaterialId &&
            cmd.UpdateOverseasAddress.OverseasAddresses == dto.OverseasAddresses
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task SaveOverseasReprocessingSites_ShouldThrowNullReferenceException_WhenSubmissionIsNull()
    {
        // Act
        Func<Task> act = async () => await _controller.SaveOverseasReprocessingSites(Guid.Empty ,null!);

        // Assert
        await act.Should().ThrowAsync<NullReferenceException>();
    }

    [TestMethod]
    public async Task SaveOverseasReprocessingSites_ShouldThrowException_WhenMediatorFails()
    {
        // Arrange
        var dto = _fixture.Create<OverseasAddressSubmissionDto>();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateOverseasMaterialReprocessingSiteCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Mediator failed"));

        // Act
        Func<Task> act = async () => await _controller.SaveOverseasReprocessingSites(Guid.NewGuid() ,dto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Mediator failed");
    }

    [TestMethod]
    public async Task UpdateRegistrationMaterialTaskStatus_ValidationFails()
    {
        // Arrange
        var externalId = Guid.NewGuid();

        var command = new UpdateRegistrationMaterialTaskStatusCommand
        {
            RegistrationMaterialId = externalId,
            Status = TaskStatuses.Completed,
            TaskName = ApplicantRegistrationTaskNames.SiteAddressAndContactDetails
        };

        // Expectations
        _validationServiceMock.Setup(o => o.ValidateAndThrowAsync<UpdateRegistrationTaskStatusCommandBase>(command, It.IsAny<CancellationToken>()))
            .Throws(new ValidationException(It.IsAny<string>()));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ValidationException>(async () => await _controller.UpdateRegistrationMaterialTaskStatus(externalId, command));
    }
}