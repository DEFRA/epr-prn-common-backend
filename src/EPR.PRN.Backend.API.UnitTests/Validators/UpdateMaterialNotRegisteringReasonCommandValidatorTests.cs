using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Validators;
using FluentValidation.TestHelper;

namespace EPR.PRN.Backend.API.UnitTests.Validators
{
    [TestClass]
    public class UpdateMaterialNotRegisteringReasonCommandValidatorTests
    {
        private UpdateMaterialNotRegisteringReasonCommandValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new UpdateMaterialNotRegisteringReasonCommandValidator();
        }

        [TestMethod]
        public void ShouldNotHaveError_WhenTypeOfSuppliersIsSet()
        {
            var model = new UpdateMaterialNotRegisteringReasonCommand { MaterialNotRegisteringReason = "Out of stock" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]
        public void ShouldHaveError_WhenTypeOfSuppliersIsNotSet()
        {
            var model = new UpdateMaterialNotRegisteringReasonCommand { MaterialNotRegisteringReason = string.Empty };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MaterialNotRegisteringReason);
        }

        [TestMethod]
        public void ShouldNotHaveError_WhenTypeOfSuppliersIsExactly500Characters()
        {
            var value = new string('A', 500);
            var model = new UpdateMaterialNotRegisteringReasonCommand { MaterialNotRegisteringReason = value };

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.MaterialNotRegisteringReason);
        }

        [TestMethod]
        public void ShouldHaveError_WhenTypeOfSuppliersExceeds500Characters()
        {
            var value = new string('A', 501);
            var model = new UpdateMaterialNotRegisteringReasonCommand { MaterialNotRegisteringReason = value };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MaterialNotRegisteringReason)
                  .WithErrorMessage("Material not registering reason must not exceed 500 characters");
        }
    }
}