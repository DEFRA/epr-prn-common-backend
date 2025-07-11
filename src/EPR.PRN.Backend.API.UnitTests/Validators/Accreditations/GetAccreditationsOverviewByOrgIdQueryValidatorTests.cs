using EPR.PRN.Backend.API.Constants;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Validators.Accreditation;
using FluentValidation;
using FluentValidation.Results;

namespace EPR.PRN.Backend.API.UnitTests.Validators.Accreditations
{
    [TestClass]
    public class GetAccreditationsOverviewByOrgIdQueryValidatorTests
    {
        private GetAccreditationsOverviewByOrgIdQueryValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new GetAccreditationsOverviewByOrgIdQueryValidator();
        }

        [TestMethod]
        public void Should_Have_Validation_Error_When_OrganisationId_Is_Empty()
        {
            // Arrange
            var query = new GetAccreditationsOverviewByOrgIdQuery
            {
                OrganisationId = Guid.Empty
            };
            // Act
            ValidationResult result = _validator.Validate(query);
            // Assert
            Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "OrganisationId" && e.ErrorMessage == ValidationMessages.AccreditationOrganisationIdRequired));
        }

        [TestMethod]
        public void Should_Have_Validation_Error_When_OrganisationId_Is_Not_Provided()
        {
            // Arrange
            var query = new GetAccreditationsOverviewByOrgIdQuery();
            // Act
            ValidationResult result = _validator.Validate(query);
            // Assert
            Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "OrganisationId" && e.ErrorMessage == ValidationMessages.AccreditationOrganisationIdRequired));
        }

        [TestMethod]
        public void Should_Not_Have_Validation_Error_When_OrganisationId_Is_Valid()
        {
            // Arrange
            var query = new GetAccreditationsOverviewByOrgIdQuery
            {
                OrganisationId = Guid.NewGuid()
            };
            // Act
            ValidationResult result = _validator.Validate(query);
            // Assert
            Assert.IsFalse(result.Errors.Any(e => e.PropertyName == "OrganisationId"));
        }
    }
}
