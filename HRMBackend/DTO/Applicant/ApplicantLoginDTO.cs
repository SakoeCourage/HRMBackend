using System.ComponentModel.DataAnnotations;

namespace HRMBackend.DTO.Applicant
{
    public class ApplicantLoginDTO
    {
        [Required]
        public string contact { get; set; }
        
        [Required]
        public string otp { get; set; }
    }
}
