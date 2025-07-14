using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Validators;
using FluentValidation.TestHelper;

namespace EPR.PRN.Backend.API.UnitTests.Validators
{
    [TestClass]
    public class UpdateMaterialNotReprocessingReasonCommandValidatorTests
    {
        private UpdateMaterialNotReprocessingReasonCommandValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new UpdateMaterialNotReprocessingReasonCommandValidator();
        }

        [TestMethod]
        public void ShouldNotHaveError_WhenTypeOfSuppliersIsSet()
        {
            var model = new UpdateMaterialNotReprocessingReasonCommand { MaterialNotReprocessingReason = "Out of stock" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]
        public void ShouldHaveError_WhenTypeOfSuppliersIsNotSet()
        {
            var model = new UpdateMaterialNotReprocessingReasonCommand { MaterialNotReprocessingReason = string.Empty };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MaterialNotReprocessingReason);
        }

        [TestMethod]
        public void ShouldNotHaveError_WhenTypeOfSuppliersIsExactly500Characters()
        {
            var value = new string('A', 500);
            var model = new UpdateMaterialNotReprocessingReasonCommand { MaterialNotReprocessingReason = value };

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.MaterialNotReprocessingReason);
        }

        [TestMethod]
        public void ShouldHaveError_WhenTypeOfSuppliersExceeds500Characters()
        {
            var value = new string('A', 501);
            var model = new UpdateMaterialNotReprocessingReasonCommand { MaterialNotReprocessingReason = value };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MaterialNotReprocessingReason)
                  .WithErrorMessage("Material not reprocessing reason must not exceed 500 characters");
        }
    }
}