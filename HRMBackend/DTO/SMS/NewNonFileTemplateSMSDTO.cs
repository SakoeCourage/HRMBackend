using System.ComponentModel.DataAnnotations;

namespace HRMBackend.DTO.SMS
{
    public class NewNonFileTemplateSMSDTO
    {
        [Required]
        public string campaingName { get; set; }
        [Required]
        public int smsTemplateId { get; set; }
        public string? message { get; set; }
        public int[]? staffIds { get; set; }
        public int? directorateId { get; set; }
        public int? departmentId { get; set; }
        public int? unitId { get; set; }
        public string? frequency { get; set; }
    }
}
