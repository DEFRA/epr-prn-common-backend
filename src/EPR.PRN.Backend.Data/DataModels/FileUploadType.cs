namespace EPR.PRN.Backend.Data.DataModels
{
    public class FileUploadType: BaseModel
    {
        public virtual ICollection<FileUpload> FileUploads { get; set; } = null!;
    }
}
