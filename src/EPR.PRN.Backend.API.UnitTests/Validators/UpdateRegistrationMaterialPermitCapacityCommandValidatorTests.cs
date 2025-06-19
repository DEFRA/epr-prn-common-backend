using AutoFixture;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace EPR.PRN.Backend.API.UnitTests.Validators;

[TestClass]
public class UpdateRegistrationMaterialPermitCapacityCommandValidatorTests
{
    private Fixture _fixture;
    private UpdateRegistrationMaterialPermitCapacityCommandValidator _validator;

    [TestInitialize]
    public void Setup()
    {
        _fixture = new Fixture();
        _validator = new UpdateRegistrationMaterialPermitCapacityCommandValidator();
    }

    [TestMethod]
    public void Should_Pass_When_Command_Is_Valid()
    {
        var command = _fixture.Build<UpdateRegistrationMaterialPermitCapacityCommand>()
            .With(x => x.PermitTypeId, (int)MaterialPermitType.InstallationPermit)
            .With(x => x.CapacityInTonnes, 5000)
            .Create();

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Should_Fail_When_PermitTypeId_Is_Empty()
    {
        var command = _fixture.Build<UpdateRegistrationMaterialPermitCapacityCommand>()
            .With(x => x.PermitTypeId, 0)
            .With(x => x.CapacityInTonnes, 5000)
            .Create();

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.PermitTypeId)
              .WithErrorMessage("PermitTypeId is required");
    }

    [TestMethod]
    public void Should_Fail_When_CapacityInTonnes_Is_Null()
    {
        var command = _fixture.Build<UpdateRegistrationMaterialPermitCapacityCommand>()
            .With(x => x.PermitTypeId, (int)MaterialPermitType.InstallationPermit)
            .With(x => x.CapacityInTonnes, (int?)null)
            .Create();

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.CapacityInTonnes)
              .WithErrorMessage("Weight must be a number greater than 0");
    }

    [TestMethod]
    public void Should_Fail_When_CapacityInTonnes_Is_Zero()
    {
        var command = _fixture.Build<UpdateRegistrationMaterialPermitCapacityCommand>()
            .With(x => x.PermitTypeId, (int)MaterialPermitType.InstallationPermit)
            .With(x => x.CapacityInTonnes, 0)
            .Create();

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.CapacityInTonnes);
              //.WithErrorMessage("Weight must be a number greater than 0");
    }

    [TestMethod]
    public void Should_Fail_When_CapacityInTonnes_Is_Greater_Than_10Million()
    {
        var command = _fixture.Build<UpdateRegistrationMaterialPermitCapacityCommand>()
            .With(x => x.PermitTypeId, (int)MaterialPermitType.InstallationPermit)
            .With(x => x.CapacityInTonnes, 10_000_001)
            .Create();

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.CapacityInTonnes)
              .WithErrorMessage("Weight must be a number less than 10,000,000");
    }

}
