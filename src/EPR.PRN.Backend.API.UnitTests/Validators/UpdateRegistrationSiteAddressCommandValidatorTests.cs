using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Validators;
using EPR.PRN.Backend.Data.DTO;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation.TestHelper;

namespace EPR.PRN.Backend.API.UnitTests.Validators;


[TestClass]
public class UpdateRegistrationSiteAddressCommandValidatorTests
{
    private UpdateRegistrationSiteAddressCommandValidator _validator;

    [TestInitialize]
    public void Setup()
    {
        _validator = new UpdateRegistrationSiteAddressCommandValidator();
    }

    [TestMethod]
    public void Should_Have_Error_When_RegistrationId_Is_NotProvided()
    {
        var model = new UpdateRegistrationSiteAddressCommand
        {
            RegistrationId = 0,
            ReprocessingSiteAddress = new AddressDto()
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.RegistrationId);              ;
    }

    [TestMethod]
    public void Should_Have_Error_When_ReprocessingSiteAddress_Is_Null()
    {
        var model = new UpdateRegistrationSiteAddressCommand
        {
            RegistrationId = 1,
            ReprocessingSiteAddress = null
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.ReprocessingSiteAddress);
    }

    [TestMethod]
    public void Should_Have_Errors_When_ReprocessingSiteAddress_Id_Is_Zero_And_Fields_Missing()
    {
        var model = new UpdateRegistrationSiteAddressCommand
        {
            RegistrationId = 1,
            ReprocessingSiteAddress = new AddressDto
            {
                Id = 0,
                NationId = 0,
                GridReference = null,
                AddressLine1 = null,
                TownCity = null,
                PostCode = null
            }
        };

        var result = _validator.TestValidate(model);

        using (new AssertionScope())
        {
            result.ShouldHaveValidationErrorFor("ReprocessingSiteAddress.NationId");
            result.ShouldHaveValidationErrorFor("ReprocessingSiteAddress.GridReference");
            result.ShouldHaveValidationErrorFor("ReprocessingSiteAddress.AddressLine1");
            result.ShouldHaveValidationErrorFor("ReprocessingSiteAddress.TownCity");
            result.ShouldHaveValidationErrorFor("ReprocessingSiteAddress.PostCode");
        }
    }

    [TestMethod]
    public void Should_Not_Have_Errors_For_Valid_Command_When_Id_Is_Zero()
    {
        var model = new UpdateRegistrationSiteAddressCommand
        {
            RegistrationId = 1,
            ReprocessingSiteAddress = new AddressDto
            {
                Id = 0,
                NationId = 1,
                GridReference = "AB123",
                AddressLine1 = "123 Main St",
                TownCity = "Sampletown",
                PostCode = "AB12 3CD"
            }
        };

        var result = _validator.TestValidate(model);

        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Should_Not_Validate_Nested_Fields_When_ReprocessingSiteAddress_Id_Is_Not_Zero()
    {
        var model = new UpdateRegistrationSiteAddressCommand
        {
            RegistrationId = 1,
            ReprocessingSiteAddress = new AddressDto
            {
                Id = 1, // Not zero
                NationId = 0,
                GridReference = null,
                AddressLine1 = null,
                TownCity = null,
                PostCode = null
            }
        };

        var result = _validator.TestValidate(model);

        result.IsValid.Should().BeTrue();
    }
}
