namespace EPR.PRN.Backend.API.Common.Exceptions
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

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

        protected RegulatorInvalidOperationException(SerializationInfo info, StreamingContext context)
        {
        }
    }
}