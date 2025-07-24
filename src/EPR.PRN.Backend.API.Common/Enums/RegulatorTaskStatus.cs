namespace EPR.PRN.Backend.API.Common.Enums;

/// <summary>
/// There is, as it stands as of July 2025, only one task status lookup in the database which is used for all journeys.
/// So this enum being named 'Regulator' is somewhat misleading but to not have to go and change over 100 references to this right now, adding this comment
/// for the next developer so that they are aware that this enum can be used for the applicant journey not just regulator.
/// </summary>
public enum RegulatorTaskStatus
{
    NotStarted = 1,
    Started = 2,
    CannotStartYet = 3,
    Queried = 4,
    Completed = 5
}
