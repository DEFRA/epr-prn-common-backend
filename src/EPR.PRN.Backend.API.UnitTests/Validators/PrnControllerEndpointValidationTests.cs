using EPR.PRN.Backend.API.Common.DTO;
using EPR.PRN.Backend.API.Validators;
using FluentAssertions;

namespace EPR.PRN.Backend.API.UnitTests.Validators
{
    [TestClass]
    public class PrnControllerEndpointValidationTests
    {
        
        [TestMethod]
        [DataRow("AccreditationNo", null)]
        [DataRow("AccreditationYear", null)]
        [DataRow("ObligationYear", null)]
        [DataRow("EvidenceMaterial", null)]
        [DataRow("EvidenceNo", null)]
        [DataRow("IssuedToEPRId", null)]
        [DataRow("EvidenceStatusCode", null)]
        [DataRow("EvidenceTonnes", null)]
        [DataRow("ExternalId", null)]
        [DataRow("IssuedByOrgName", null)]
        [DataRow("IssuedToOrgName", null)]
        [DataRow("ProducerAgency", null)]
        [DataRow("RecoveryProcessCode", null)]
        [DataRow("StatusDate", null)]
        public void Test_SavePrnDetailsRequestValidator_Returns_Correct_ValidationResultOnNullInput(string propertyName , object propertyValue)
        {
            var validator = new SavePrnDetailsRequestValidator();

            var dto = new SavePrnDetailsRequest()
            {
                AccreditationNo = "ABC",
                AccreditationYear = 2018,
                CancelledDate = DateTime.UtcNow.AddDays(-1),
                DecemberWaste = true,
                EvidenceMaterial = "Aluminium",
                EvidenceNo = Guid.NewGuid().ToString(),
                EvidenceStatusCode = Common.Enums.PrnStatus.AwaitingAcceptance,
                EvidenceTonnes = 5000,
                ExternalId = Guid.NewGuid(),
                IssueDate = DateTime.UtcNow.AddDays(-5),
                IssuedByNPWDCode = "NPWD367742",
                IssuedByOrgName = "ANB",
                IssuedToEPRId = Guid.NewGuid(),
                IssuedToNPWDCode = "NPWD557742",
                IssuedToOrgName = "ZNZ",
                IssuerNotes = "no notes",
                IssuerRef = "ANB-1123",
                MaterialOperationCode = "R-PLA",
                ObligationYear = 2025,
                PrnSignatory = "Pat Anderson",
                PrnSignatoryPosition = "Director",
                ProducerAgency = "TTL",
                RecoveryProcessCode = "N11",
                ReprocessorAgency = "BEX",
                StatusDate = DateTime.UtcNow,
            };

            // Get all property names from DTO class
            var props = typeof(SavePrnDetailsRequest)
                            .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                            .ToList();


            var matchingProp = props.FirstOrDefault(x => string.Equals(x.Name, propertyName, StringComparison.InvariantCulture));
            matchingProp.Should().NotBeNull();

            // Set the value of the property (overriding the default value set above) to the value passed in as the argument to this method
            matchingProp.SetValue(dto, propertyValue);

            // Act
            var result = validator.Validate(dto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Select(x => x.PropertyName).Should().Contain(propertyName);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void Test_SavePrnDetailsRequestValidator_Returns_Error_WhenCancellationDateNotCorrectlySet(bool isCancelledStatus)
        {
            var validator = new SavePrnDetailsRequestValidator();

            var dto = new SavePrnDetailsRequest()
            {
                AccreditationNo = "ABC",
                AccreditationYear = 2018,
                DecemberWaste = true,
                EvidenceMaterial = "Aluminium",
                EvidenceNo = Guid.NewGuid().ToString(),                
                EvidenceTonnes = 5000,
                ExternalId = Guid.NewGuid(),
                IssueDate = DateTime.UtcNow.AddDays(-5),
                IssuedByNPWDCode = "NPWD367742",
                IssuedByOrgName = "ANB",
                IssuedToEPRId = Guid.NewGuid(),
                IssuedToNPWDCode = "NPWD557742",
                IssuedToOrgName = "ZNZ",
                IssuerNotes = "no notes",
                IssuerRef = "ANB-1123",
                MaterialOperationCode = "R-PLA",
                ObligationYear = 2025,
                PrnSignatory = "Pat Anderson",
                PrnSignatoryPosition = "Director",
                ProducerAgency = "TTL",
                RecoveryProcessCode = "N11",
                ReprocessorAgency = "BEX",
                StatusDate = DateTime.UtcNow,
            };

            // Set Status based on test method parameter
            if (isCancelledStatus)
            {
                dto.EvidenceStatusCode = Common.Enums.PrnStatus.Cancelled;
                dto.CancelledDate = null;
            }
            else
            {
                dto.EvidenceStatusCode = Common.Enums.PrnStatus.AwaitingAcceptance;
                dto.CancelledDate = DateTime.UtcNow.AddDays(-1);
            }

            // Act
            var result = validator.Validate(dto);

            // Assert
            result.Should().NotBeNull();
            
            var errorPropertyNames = result.Errors.Select(x => x.PropertyName);
            var cancelledDatePropertyName = nameof(dto.CancelledDate);

            _ = isCancelledStatus 
                ? errorPropertyNames.Should().Contain(cancelledDatePropertyName)
                : errorPropertyNames.Should().NotContain(cancelledDatePropertyName);
        }

        [TestMethod]
        [DataRow("AccreditationNo", "ABC122378123123712381273123123123")]
        [DataRow("AccreditationYear", 25678)]
        [DataRow("ObligationYear", 25678)]
        [DataRow("EvidenceMaterial", "Material201223234234234234234")]
        [DataRow("EvidenceNo", "EV1231293812931231231231231")]
        [DataRow("IssuedByOrgName", "OrgName12313123123123123123123123123123213123123123123OrgName12313123123123123123123123123123213123123123123OrgName12313123123123123123123123123123213123123123123")]
        [DataRow("IssuedToOrgName", "OrgName12313123123123123123123123123123213123123123123OrgName12313123123123123123123123123123213123123123123OrgName12313123123123123123123123123123213123123123123")]
        [DataRow("ProducerAgency", "AgName12313123123123123123123123123123213123123123123OrgName12313123123123123123123123123123213123123123123OrgName12313123123123123123123123123123213123123123123")]
        [DataRow("RecoveryProcessCode", "Code123234342342342342342342342")]
        public void Test_SavePrnDetailsRequestValidator_Returns_Correct_ValidationResultOnInvalidInput(string propertyName, object propertyValue)
        {
            var validator = new SavePrnDetailsRequestValidator();

            var dto = new SavePrnDetailsRequest()
            {
                AccreditationNo = "ABC",
                AccreditationYear = 2018,
                CancelledDate = DateTime.UtcNow.AddDays(-1),
                DecemberWaste = true,
                EvidenceMaterial = "Aluminium",
                EvidenceNo = Guid.NewGuid().ToString(),
                EvidenceStatusCode = Common.Enums.PrnStatus.AwaitingAcceptance,
                EvidenceTonnes = 5000,
                ExternalId = Guid.NewGuid(),
                IssueDate = DateTime.UtcNow.AddDays(-5),
                IssuedByNPWDCode = "NPWD367742",
                IssuedByOrgName = "ANB",
                IssuedToEPRId = Guid.NewGuid(),
                IssuedToNPWDCode = "NPWD557742",
                IssuedToOrgName = "ZNZ",
                IssuerNotes = "no notes",
                IssuerRef = "ANB-1123",
                MaterialOperationCode = "R-PLA",
                ObligationYear = 2025,
                PrnSignatory = "Pat Anderson",
                PrnSignatoryPosition = "Director",
                ProducerAgency = "TTL",
                RecoveryProcessCode = "N11",
                ReprocessorAgency = "BEX",
                StatusDate = DateTime.UtcNow,
            };

            // Get all property names from DTO class
            var props = typeof(SavePrnDetailsRequest)
                            .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                            .ToList();


            var matchingProp = props.FirstOrDefault(x => string.Equals(x.Name, propertyName, StringComparison.InvariantCulture));
            matchingProp.Should().NotBeNull();

            // Set the value of the property (overriding the default value set above) to the value passed in as the argument to this method
            matchingProp.SetValue(dto, propertyValue);

            // Act
            var result = validator.Validate(dto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Select(x => x.PropertyName).Should().Contain(propertyName);
        }
    }
}
