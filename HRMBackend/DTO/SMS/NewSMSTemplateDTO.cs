using System.ComponentModel.DataAnnotations;

namespace HRMBackend.DTO.SMS
{
    public class NewSMSTemplateDTO
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string message { get; set; }
 
        [DataType(DataType.Text)]
        [MinLength(5, ErrorMessage = "The FieldName field must be at atleast {1} characters long.")]
        public string? description { get; set; }
    }
}
