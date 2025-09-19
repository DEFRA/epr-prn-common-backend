namespace EPR.PRN.Backend.API.Dto.Regulator;
public class QueryNoteDto
{
    public string Notes { get; set; } = string.Empty;
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
}

