using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace HRMBackend.Model.Applicant
{
    public class Applicant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; } = DateTime.Now;
        [Required]
        public string firsName { get; set; }
        [Required]
        public string lastName { get; set; }
        [EmailAddress]
        public string? email { get; set; }
        [Required]
        public string contact { get; set; }
        public bool? hasSubmittedApplication { get; set; } = false;
        [JsonIgnore]
        public virtual ApplicantHasOTP otp { get; set; }

    }
}
