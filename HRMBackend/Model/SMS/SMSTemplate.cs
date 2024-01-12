using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HRMBackend.Model.SMS
{
    public class SMSTemplate
    {
        [Key]
        [Required]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string message { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; } = DateTime.Now;
        [DataType(DataType.Text)]
        [MinLength(5, ErrorMessage = "The FieldName field must be at atleast {1} characters long.")]
        public string? description { get; set; }
        public Boolean readOnly { get; set; }
        [JsonIgnore]
        public virtual ICollection<SMSCampaignHistory> smsHistory { get; set; }
    }
}
