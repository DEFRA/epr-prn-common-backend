namespace EPR.PRN.Backend.API.Helpers
{
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