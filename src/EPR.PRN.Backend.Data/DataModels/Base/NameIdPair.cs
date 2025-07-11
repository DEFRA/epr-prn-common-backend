using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Base
{
    [ExcludeFromCodeCoverage]
    public abstract class NameIdPair
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
