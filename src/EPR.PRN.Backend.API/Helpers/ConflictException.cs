using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace EPR.PRN.Backend.API.Helpers
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class ConflictException : Exception
    {
        public ConflictException() : base()
        {
        }

        public ConflictException(string message) : base(message)
        {
        }
        protected ConflictException(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
