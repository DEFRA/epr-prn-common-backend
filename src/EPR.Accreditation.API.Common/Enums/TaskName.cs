namespace EPR.Accreditation.API.Common.Enums
{
    // this is used to represent the particular tasks for the task progress page
    // each record for an accreditation in the TaskProgress table will have a Task name id
    // that this references
    public enum TaskName
    {
        Undefined,
        WasteLicencesAndPrns,
        UploadBusinessPlan,
        AboutMaterial,
        UploadSupportingDocuments
    }
}
