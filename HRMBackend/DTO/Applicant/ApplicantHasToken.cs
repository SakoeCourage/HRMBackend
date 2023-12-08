using System.ComponentModel.DataAnnotations;

namespace HRMBackend.DTO.Applicant
{
    public class ApplicantHasToken
    {

        [Key]
        public int id { get; set; }

        public int applicantID { get; set; }

        public string urlToken { get; set; }

        public virtual ApplicantDTO applicant { get;  }
    }
}
