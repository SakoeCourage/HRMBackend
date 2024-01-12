using System.ComponentModel.DataAnnotations;

namespace HRMBackend.DTO.SMS
{
    public class NewFileTemplateSMSDTO
    {
        [Required]
        public string campaingName { get; set; }
        [Required]
        public int smsTemplateId { get; set; }
        [Required]
        public IFormFile templateFile { get; set; }
        public string? message { get; set; }
        public string? frequency { get; set; }
    }
}
