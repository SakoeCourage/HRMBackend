using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMBackend.DTO.Applicant
{
    public class ApplicantDTO
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

        [Required, EmailAddress]
        public string email { get; set; }

        [Required]
        public string contact { get; set; }

        public bool hasSubmittedApplication { get; set; } = false;

        public virtual ApplicantHasToken applicantHasToken { get; }

    }
}
