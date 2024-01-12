using System.ComponentModel.DataAnnotations;

namespace HRMBackend.Model.Applicant
{
    public class ApplicantHasOTP
    {

        [Key]
        public int id { get; set; }
        public string contact { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; } = DateTime.Now;
        public string otp { get; set; } 
        public int applicantID { get; set; }
        public virtual Applicant applicant { get; set; }

       
    }
}
