namespace EPR.PRN.Backend.API.Common.Exceptions
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    [Serializable]
    public class RegulatorInvalidOperationException : InvalidOperationException
    {
        public RegulatorInvalidOperationException()
        {
        }

        public RegulatorInvalidOperationException(string? message) : base(message)
        {
        }

        public RegulatorInvalidOperationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}