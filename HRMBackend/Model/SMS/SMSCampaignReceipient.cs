using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HRMBackend.Model.SMS
{
    public static class SMSStatus
    {
        public static string pending = "pending";
        public static string successfull = "successfull";
        public static string failed = "failed";
    }
    public class SMSCampaignReceipient
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int campaignHistoryId { get; set; }
        [Required]
        public string message { get; set; }
        [Required]
        public string contact { get; set; }
        public string? firstName {get;set;}
        public string? lastName { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; } = DateTime.Now;
        public string? status { get; set; } = SMSStatus.pending;
        public virtual SMSCampaignHistory campaignHistory { get; set; }  

    }
}
