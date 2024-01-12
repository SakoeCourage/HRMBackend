using System.ComponentModel.DataAnnotations;

namespace HRMBackend.DTO.Applicant
{
    public class ApplicantbulkuploadDTO
    {
        [Required]
        public string firsName { get; set; }

        [Required]
        public string lastName { get; set; }

        [Required, EmailAddress]
        public string email { get; set; }

        [Required]
        public string contact { get; set; }
    }
}
