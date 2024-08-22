namespace EPR.PRN.Backend.Obligation.Interfaces
{
    public interface IPomSubmissionData
    {
        Task<HttpResponseMessage> GetAggregatedPomData(string submissionIdString);
    }
}
