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

        // Compliant: serialization constructor is protected and calls the base constructor
#pragma warning disable SYSLIB0051
        protected RegulatorInvalidOperationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Override for serialization support. Add custom fields if needed in the future.
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            // Example for custom fields (uncomment and adjust if you add any):
            // info.AddValue("MyCustomProperty", this.MyCustomProperty);
        }
#pragma warning restore SYSLIB0051
    }
}
