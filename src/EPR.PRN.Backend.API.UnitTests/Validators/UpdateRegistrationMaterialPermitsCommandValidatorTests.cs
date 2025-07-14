using AutoFixture;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
namespace EPR.PRN.Backend.API.UnitTests.Validators;

[TestClass]
public class UpdateRegistrationMaterialPermitsCommandValidatorTests
{
    private UpdateRegistrationMaterialPermitsCommandValidator _validator;
    private Fixture _fixture;

    [TestInitialize]
    public void Setup()
    {
        _validator = new UpdateRegistrationMaterialPermitsCommandValidator();
        _fixture = new Fixture();
    }

    [TestMethod]
    public void Should_Have_Error_When_PermitTypeId_Is_Null()
    {
        // Arrange
        var command = _fixture.Build<UpdateRegistrationMaterialPermitsCommand>()
            .With(x => x.PermitTypeId, (int?)null)
            .Create();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PermitTypeId)
            .WithErrorMessage("PermitTypeId is required");
    }

    [TestMethod]
    [DataRow(MaterialPermitType.WasteExemption)]
    [DataRow(MaterialPermitType.PollutionPreventionAndControlPermit)]
    [DataRow(MaterialPermitType.WasteManagementLicence)]
    [DataRow(MaterialPermitType.InstallationPermit)]
    [DataRow(MaterialPermitType.EnvironmentalPermitOrWasteManagementLicence)]
    public void Should_Have_Error_When_PermitNumber_Is_Null_Or_Empty_AndNotWasteExemptions(int permitId)
    {
        var commandWithNull = _fixture.Build<UpdateRegistrationMaterialPermitsCommand>()
            .With(x => x.PermitTypeId, permitId)
            .With(x => x.PermitNumber, (string)null)
            .Create();

        var commandWithEmpty = _fixture.Build<UpdateRegistrationMaterialPermitsCommand>()
            .With(x => x.PermitTypeId, permitId)
            .With(x => x.PermitNumber, string.Empty)
            .Create();

        var resultNull = _validator.TestValidate(commandWithNull);
        var resultEmpty = _validator.TestValidate(commandWithEmpty);

        if (permitId is not (int)MaterialPermitType.WasteExemption)
        {
            resultNull.ShouldHaveValidationErrorFor(x => x.PermitNumber)
                .WithErrorMessage("PermitNumber is required");

            resultEmpty.ShouldHaveValidationErrorFor(x => x.PermitNumber)
                .WithErrorMessage("PermitNumber is required");
        }
        else
        {
            resultNull.ShouldNotHaveValidationErrorFor(x => x.PermitNumber);
            resultEmpty.ShouldNotHaveValidationErrorFor(x => x.PermitNumber);
        }
    }

    [TestMethod]
    public void Should_Not_Have_Error_When_Valid_Command()
    {
        var command = _fixture.Build<UpdateRegistrationMaterialPermitsCommand>()
            .With(x => x.PermitTypeId, 1)
            .With(x => x.PermitNumber, "ABC123")
            .Create();

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeTrue();
    }
}
