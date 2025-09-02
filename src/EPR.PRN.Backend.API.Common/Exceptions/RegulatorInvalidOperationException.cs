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

        public RegulatorInvalidOperationException(string? message)
            : base(message)
        {
        }

        public RegulatorInvalidOperationException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
#pragma warning disable SYSLIB0051
        protected RegulatorInvalidOperationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
#pragma warning restore SYSLIB0051
    }
}
