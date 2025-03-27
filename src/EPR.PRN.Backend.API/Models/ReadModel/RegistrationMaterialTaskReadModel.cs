#nullable disable
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Models.ReadModel;

[ExcludeFromCodeCoverage]
public class RegistrationMaterialTaskReadModel
{
    public Guid RegistrationId { get; set; }
    public List<Materials> Materials { get; set; }
}