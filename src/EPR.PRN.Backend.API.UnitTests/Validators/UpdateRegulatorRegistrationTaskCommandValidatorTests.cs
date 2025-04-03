using EPR.PRN.Backend.API.Common.Enums;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPR.PRN.Backend.API.UnitTests.Validators
{
    [TestClass]
    public class UpdateRegulatorRegistrationTaskCommandValidatorTests
    {
        private UpdateRegulatorRegistrationTaskCommandValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new UpdateRegulatorRegistrationTaskCommandValidator();
        }

        [TestMethod]
        public void Should_Have_Error_When_Status_Is_Invalid()
        {
            var model = new UpdateRegulatorRegistrationTaskCommand { Status = (StatusTypes)999 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Status)
                .WithErrorMessage("Invalid Status value");
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Status_Is_Valid()
        {
            var model = new UpdateRegulatorRegistrationTaskCommand { Status = StatusTypes.Complete };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Status);
        }

        [TestMethod]
        public void Should_Have_Error_When_Comment_Exceeds_MaxLength()
        {
            var model = new UpdateRegulatorRegistrationTaskCommand { Status = StatusTypes.Queried, Comment = new string('a', 501) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Comment)
                .WithErrorMessage("Comment must not exceed 500 characters");
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Comment_Is_Within_MaxLength()
        {
            var model = new UpdateRegulatorRegistrationTaskCommand { Status = StatusTypes.Queried, Comment = new string('a', 500) };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Comment);
        }

        [TestMethod]
        public void Should_Have_Error_When_Comment_Is_Empty_And_Status_Is_Queried()
        {
            var model = new UpdateRegulatorRegistrationTaskCommand { Status = StatusTypes.Queried, Comment = string.Empty };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Comment)
                .WithErrorMessage("Comment is required when status is Queried");
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Comment_Is_Not_Empty_And_Status_Is_Queried()
        {
            var model = new UpdateRegulatorRegistrationTaskCommand { Status = StatusTypes.Queried, Comment = "Valid comment" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Comment);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Comment_Is_Empty_And_Status_Is_Not_Queried()
        {
            var model = new UpdateRegulatorRegistrationTaskCommand { Status = StatusTypes.Complete, Comment = string.Empty };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Comment);
        }
    }
}
