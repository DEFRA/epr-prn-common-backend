namespace EPR.PRN.Backend.Data.DTO;

public class OverseasAddressContactDto
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public Guid CreatedBy { get; set; }
}
