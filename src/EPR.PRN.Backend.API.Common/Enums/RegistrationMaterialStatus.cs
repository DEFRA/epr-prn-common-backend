using System.ComponentModel;

namespace EPR.PRN.Backend.API.Common.Enums;
public enum RegistrationMaterialStatus
{
    GRANTED = 1,
    REFUSED = 2
}
public enum ApplicationOrganisationType
{
    [Description("Reprocessor")]
    Reprocessor = 1,
    [Description("Exporter")]
    Exporter=2
}
public enum RegulatorTaskStatus
{
    [Description("Not Started")]
    NotStarted =1,
    [Description("Started")]
    Started =2,
    [Description("Completed")]
    Completed=3,
    [Description("Can Not Start Yet")]
    CannotStartYet=4,
    [Description("Queried")]
    Queried=5,
    [Description("Approved")]
    Approved=6
}