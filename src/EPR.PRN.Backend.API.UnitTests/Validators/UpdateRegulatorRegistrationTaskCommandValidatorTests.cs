using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Validators.Regulator;
using FluentValidation.TestHelper;

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
            var model = new UpdateRegulatorRegistrationTaskCommand { TaskName = "Test Task", RegistrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d"), Status = (RegulatorTaskStatus)999, UserName = "UserName" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Status)
                .WithErrorMessage("Invalid Status value");
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Status_Is_Valid()
        {
            var model = new UpdateRegulatorRegistrationTaskCommand { TaskName = "Test Task", RegistrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d"), Status = RegulatorTaskStatus.Completed, UserName = "UserName" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Status);
        }

        [TestMethod]
        public void Should_Have_Error_When_Comment_Exceeds_MaxLength()
        {
            var model = new UpdateRegulatorRegistrationTaskCommand { TaskName = "Test Task", RegistrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d"), Status = RegulatorTaskStatus.Queried, Comments = new string('a', 501), UserName = "UserName" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Comments)
                .WithErrorMessage("Comment must not exceed 500 characters");
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Comment_Is_Within_MaxLength()
        {
            var model = new UpdateRegulatorRegistrationTaskCommand { TaskName = "Test Task", RegistrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d"), Status = RegulatorTaskStatus.Queried, Comments = new string('a', 500), UserName = "UserName" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Comments);
        }

        [TestMethod]
        public void Should_Have_Error_When_Comment_Is_Empty_And_Status_Is_Queried()
        {
            var model = new UpdateRegulatorRegistrationTaskCommand { TaskName = "Test Task", RegistrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d"), Status = RegulatorTaskStatus.Queried, Comments = string.Empty, UserName = "UserName" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Comments)
                .WithErrorMessage("Comment is required when status is Queried");
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Comment_Is_Not_Empty_And_Status_Is_Queried()
        {
            var model = new UpdateRegulatorRegistrationTaskCommand { TaskName = "Test Task", RegistrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d"), Status = RegulatorTaskStatus.Queried, Comments = "Valid comment", UserName = "UserName" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Comments);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Comment_Is_Empty_And_Status_Is_Not_Queried()
        {
            var model = new UpdateRegulatorRegistrationTaskCommand { TaskName = "Test Task", RegistrationId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d"), Status = RegulatorTaskStatus.Completed, Comments = string.Empty, UserName = "UserName" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Comments);
        }
    }
}
