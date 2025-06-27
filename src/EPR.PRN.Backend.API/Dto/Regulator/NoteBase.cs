using EPR.PRN.Backend.API.Common.Enums;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]
public class NoteBase
{
    public Guid OrganisationId { get; init; }
    public List<QueryNoteDto> QueryNotes { get; set; }
    public RegulatorTaskStatus TaskStatus { get; set; }
    
}

