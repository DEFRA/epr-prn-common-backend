using EPR.PRN.Backend.API.Common.Enums;

namespace EPR.PRN.Backend.API.Dto.Regulator;
public class NoteBase
{
    public Guid OrganisationId { get; init; }
    public List<QueryNoteDto> QueryNotes { get; set; } = [];
    public RegulatorTaskStatus TaskStatus { get; set; }

}

