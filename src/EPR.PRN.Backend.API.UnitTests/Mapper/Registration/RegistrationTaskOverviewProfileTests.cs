using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using FluentAssertions;

namespace EPR.PRN.Backend.API.UnitTests.Mapper.Registration;

[TestClass]
public class RegistrationTaskOverviewProfileTests : MappingProfileTestBase
{
    [TestMethod]
    public void Registration_To_RegistrationTaskOverviewDto_EnsureCorrectMapping()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var mapper = Mapper.CreateMapper();
        var source = new Data.DataModels.Registrations.Registration
        {
            Id = 1,
            ExternalId = externalId,
            OrganisationId = externalId,
            ApplicationTypeId = 1,
            Materials =
            [
                new()
                {
                    Id = 1,
                    Material = new LookupMaterial
                    {
                        Id = 1, MaterialCode = "STL", MaterialName = "Steel"
                    },
                    IsMaterialRegistered = true,
                    StatusUpdatedDate = new DateTime(2020, 01, 01, 12, 00, 00),
                    ApplicantTaskStatuses = new List<ApplicantRegistrationTaskStatus>
                    {
                        new ()
                        {
                            Id = 1, 
                            ExternalId = externalId,
                            RegistrationMaterialId = 1,
                            RegistrationId = 1,
                            TaskStatus = new LookupTaskStatus
                            {
                                Name = "NotStarted",
                                Id = 1
                            },
                            Task = new LookupApplicantRegistrationTask
                            {
                                Id = 1,
                                Name = ApplicantRegistrationTaskNames.WasteLicensesPermitsAndExemptions,
                                ApplicationTypeId = 1,
                                IsMaterialSpecific = false,
                                JourneyTypeId = 1
                            }
                        }
                    }
                }
            ],
            ApplicantRegistrationTasksStatus = new List<ApplicantRegistrationTaskStatus>
            {
                new ()
                {
                    Id = 1,
                    TaskId = 1,
                    RegistrationId = 1,
                    ExternalId = externalId,
                    TaskStatus = new LookupTaskStatus
                    {
                        Id = 1,
                        Name = "NotStarted"
                    },
                    Task = new LookupApplicantRegistrationTask
                    {
                        Id = 1,
                        Name = ApplicantRegistrationTaskNames.SiteAddressAndContactDetails,
                        ApplicationTypeId = 1,
                        IsMaterialSpecific = true,
                        JourneyTypeId = 1
                    }
                }
            }
        };

        var expected = new ApplicantRegistrationTasksOverviewDto
        {
            Id = externalId,
            Materials =
            [
                new()
                {
                    MaterialLookup = new MaterialLookupDto
                    {
                        Id = 1,
                        Name = "Steel"
                    },
                    IsMaterialRegistered = true,
                    Tasks = new List<ApplicantRegistrationTaskDto>
                    {
                        new ()
                        {
                            TaskName = ApplicantRegistrationTaskNames.WasteLicensesPermitsAndExemptions,
                            Id = externalId,
                            Status = "NotStarted"
                        }
                    }
                }
            ],
            OrganisationId = externalId,
            Tasks = new List<ApplicantRegistrationTaskDto>
            {
                new ()
                {
                    Status = "NotStarted",
                    TaskName = ApplicantRegistrationTaskNames.SiteAddressAndContactDetails,
                    Id = externalId
                }
            }

        };

        // Act
        var result = mapper.Map<ApplicantRegistrationTasksOverviewDto>(source);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}