namespace EPR.PRN.Backend.Data.DataModels
{
    public class FileUploadStatus: BaseModel
    {
        public virtual ICollection<FileUpload> FileUploads { get; set; } = null!;
    }
}
