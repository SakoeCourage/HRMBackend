using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HRMBackend.Model.SMS
{
    public class SMSCampaignHistory
    {
        [Key]
        public int id { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; } = DateTime.Now;
        [Required]
        public string campaignName { get; set; }
        [Required]
        public int smsTemplateId { get; set; }
        [Required]
        public int receipients { get; set; }
        public virtual SMSTemplate smsTemplate { get; set; }
       
        public virtual ICollection<SMSCampaignReceipient> smsReceipients { get; set; }

    }
}
