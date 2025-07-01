using EPR.PRN.Backend.API.Constants;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Validators.Registration;
using FluentValidation.Results;
namespace EPR.PRN.Backend.API.UnitTests.Validators;
[TestClass]
public class GetRegistrationsOverviewByOrgIdValidatorTests
{
    private GetRegistrationsOverviewByOrgIdValidator _validator;
    [TestInitialize]
    public void TestInitialize()
    {
        _validator = new GetRegistrationsOverviewByOrgIdValidator();
    }
    [TestMethod]
    public void Should_Have_Validation_Error_When_OrganisationId_Is_Empty()
    {
        // Arrange
        var query = new GetRegistrationsOverviewByOrgIdQuery
        {
            OrganisationId = Guid.Empty
        };
        // Act
        ValidationResult result = _validator.Validate(query);
        // Assert
        Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "OrganisationId" && e.ErrorMessage == ValidationMessages.RegistrationOrganisationIdRequired));
    }
    [TestMethod]
    public void Should_Have_Validation_Error_When_OrganisationId_Is_Not_Provided()
    {
        // Arrange
        var query = new GetRegistrationsOverviewByOrgIdQuery();
        // Act
        ValidationResult result = _validator.Validate(query);
        // Assert
        Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "OrganisationId" && e.ErrorMessage == ValidationMessages.RegistrationOrganisationIdRequired));
    }
    [TestMethod]
    public void Should_Not_Have_Validation_Error_When_OrganisationId_Is_Valid()
    {
        // Arrange
        var query = new GetRegistrationsOverviewByOrgIdQuery
        {
            OrganisationId = Guid.NewGuid()
        };
        // Act
        ValidationResult result = _validator.Validate(query);
        // Assert
        Assert.IsFalse(result.Errors.Any(e => e.PropertyName == "OrganisationId"));
    }
}