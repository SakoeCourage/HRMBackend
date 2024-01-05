using System.ComponentModel.DataAnnotations;

namespace HRMBackend.Model.Applicant
{
    public class ApplicantHasToken
    {

        [Key]
        public int id { get; set; }

        public int applicantID { get; set; }

        public DateTime createdAt { get; set; }

        public string urlToken { get; set; }

        public virtual Applicant applicant { get; }
    }
}
