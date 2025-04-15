using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Validators;
using FluentValidation.TestHelper;

namespace EPR.PRN.Backend.API.UnitTests.Validators
{
    [TestClass]
    public class UpdateRegulatorApplicationTaskCommandValidatorTests
    {
        private UpdateRegulatorApplicationTaskCommandValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new UpdateRegulatorApplicationTaskCommandValidator();
        }

        [TestMethod]
        public void Should_Have_Error_When_Status_Is_Invalid()
        {
            var model = new UpdateRegulatorApplicationTaskCommand { TaskName = "Test Task", registrationMaterialId = 1, Status = (StatusTypes)999, UserName = "UserName" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Status)
                .WithErrorMessage("Invalid Status value");
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Status_Is_Valid()
        {
            var model = new UpdateRegulatorApplicationTaskCommand { TaskName = "Test Task", registrationMaterialId = 1, Status = StatusTypes.Completed, UserName = "UserName" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Status);
        }

        [TestMethod]
        public void Should_Have_Error_When_Comment_Exceeds_MaxLength()
        {
            var model = new UpdateRegulatorApplicationTaskCommand { TaskName = "Test Task", registrationMaterialId = 1, Status = StatusTypes.Queried, Comment = new string('a', 501), UserName = "UserName" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Comment)
                .WithErrorMessage("Comment must not exceed 500 characters");
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Comment_Is_Within_MaxLength()
        {
            var model = new UpdateRegulatorApplicationTaskCommand { TaskName = "Test Task", registrationMaterialId = 1, Status = StatusTypes.Queried, Comment = new string('a', 500), UserName = "UserName" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Comment);
        }

        [TestMethod]
        public void Should_Have_Error_When_Comment_Is_Empty_And_Status_Is_Queried()
        {
            var model = new UpdateRegulatorApplicationTaskCommand { TaskName = "Test Task", registrationMaterialId = 1, Status = StatusTypes.Queried, Comment = string.Empty, UserName = "UserName" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Comment)
                .WithErrorMessage("Comment is required when status is Queried");
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Comment_Is_Not_Empty_And_Status_Is_Queried()
        {
            var model = new UpdateRegulatorApplicationTaskCommand { TaskName = "Test Task", registrationMaterialId = 1, Status = StatusTypes.Queried, Comment = "Valid comment", UserName = "UserName" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Comment);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Comment_Is_Empty_And_Status_Is_Not_Queried()
        {
            var model = new UpdateRegulatorApplicationTaskCommand { TaskName = "Test Task", registrationMaterialId = 1, Status = StatusTypes.Completed, Comment = string.Empty, UserName = "UserName" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Comment);
        }
    }
}
