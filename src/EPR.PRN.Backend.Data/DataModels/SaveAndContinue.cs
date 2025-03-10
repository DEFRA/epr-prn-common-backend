using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class SaveAndContinue
    {
        [Key]
        public int Id { get; set; }
        
        public string? Area { get; set; }
        
        public string? Controller { get; set; }
        
        public string? Action { get; set; }
        
        public string? Parameters { get; set; }

        public int RegistrationId { get; set; }

        public virtual Registration Registration { get; set; }
    }
}
