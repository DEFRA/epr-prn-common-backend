using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Constants;
using EPR.PRN.Backend.API.Validators;

namespace EPR.PRN.Backend.API.UnitTests.Validators;

[TestClass]
public class RegistrationOutcomeValidatorTests
{
    private RegistrationOutcomeValidator _validator;

    [TestInitialize]
    public void TestInitialize()
    {
        _validator = new RegistrationOutcomeValidator();
    }

    [TestMethod]
    public void Should_Have_Validation_Error_When_Status_Is_Invalid()
    {
        // Arrange  
        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"),
            Status = (RegistrationMaterialStatus)999,
            RegistrationReferenceNumber = "TestReference"
        };

        // Act  
        var result = _validator.Validate(command);

        // Assert  
        Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Status" && e.ErrorMessage == ValidationMessages.InvalidRegistrationOutcomeStatus));
    }

    [TestMethod]
    public void Should_Have_Validation_Error_When_Comments_Exceed_Max_Length()
    {
        // Arrange  
        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"),
            Status = RegistrationMaterialStatus.Refused,
            Comments = new string('A', 501),
            RegistrationReferenceNumber = "TestReference"
        };

        // Act  
        var result = _validator.Validate(command);

        // Assert  
        Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Comments" && e.ErrorMessage == ValidationMessages.RegistrationOutcomeCommentsMaxLength));
    }

    [TestMethod]
    public void Should_Have_Validation_Error_When_Comments_Are_Empty_And_Status_Is_Refused()
    {
        // Arrange  
        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"),
            Status = RegistrationMaterialStatus.Refused,
            Comments = string.Empty,
            RegistrationReferenceNumber = "TestReference"
        };

        // Act  
        var result = _validator.Validate(command);

        // Assert  
        Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Comments" && e.ErrorMessage == ValidationMessages.RegistrationOutcomeCommentsCommentsRequired));
    }

    [TestMethod]
    public void Should_Not_Have_Validation_Error_When_Comments_Are_Filled_And_Status_Is_Refused()
    {
        // Arrange  
        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"),
            Status = RegistrationMaterialStatus.Refused,
            Comments = "Some comments",
            RegistrationReferenceNumber = "TestReference"
        };

        // Act  
        var result = _validator.Validate(command);

        // Assert  
        Assert.IsFalse(result.Errors.Any(e => e.PropertyName == "Comments"));
    }

    [TestMethod]
    public void Should_Not_Have_Validation_Error_When_Command_Is_Valid()
    {
        // Arrange  
        var command = new RegistrationMaterialsOutcomeCommand
        {
            Id = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"),
            Status = RegistrationMaterialStatus.Granted,
            Comments = "Some comments",
            RegistrationReferenceNumber = "TestReference"
        };

        // Act  
        var result = _validator.Validate(command);

        // Assert  
        Assert.IsFalse(result.Errors.Any(e => e.PropertyName == "Id"));
        Assert.IsFalse(result.Errors.Any(e => e.PropertyName == "Status"));
        Assert.IsFalse(result.Errors.Any(e => e.PropertyName == "Comments"));
    }
}
