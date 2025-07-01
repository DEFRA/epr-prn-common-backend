using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Validators;
using FluentValidation.TestHelper;

namespace EPR.PRN.Backend.API.UnitTests.Validators
{
    [TestClass]
    public class UpsertRegistrationMaterialContactCommandValidatorTests
    {
        private UpsertRegistrationMaterialContactCommandValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new UpsertRegistrationMaterialContactCommandValidator();
        }

        [TestMethod]
        public void ShouldHaveError_WhenRegistrationMaterialIdIsNotSet()
        {
            var model = new UpsertRegistrationMaterialContactCommand { RegistrationMaterialId = Guid.Empty, UserId = Guid.NewGuid() };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.RegistrationMaterialId);
        }

        [TestMethod]
        public void ShouldHaveError_WhenUserIdIsNotSet()
        {
            var model = new UpsertRegistrationMaterialContactCommand { RegistrationMaterialId = Guid.NewGuid(), UserId = Guid.Empty };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [TestMethod]
        public void ShouldNotHaveError_WhenUserIdAndRegistrationMaterialIdAreSet()
        {
            var model = new UpsertRegistrationMaterialContactCommand { RegistrationMaterialId = Guid.NewGuid(), UserId = Guid.NewGuid() };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}