using HRMBackend.DTO.Applicant;
using Microsoft.EntityFrameworkCore;

namespace HRMBackend.DB
{
    public class Context:DbContext
    {
        public Context(DbContextOptions<Context> options):base(options)
        {
             
        }
        /**
         * DB SETS 
         */
        public DbSet<ApplicantDTO> Applicant { get; set; }
        public DbSet<ApplicantHasToken> ApplicantHasToken { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicantDTO>()
                .HasOne(e => e.applicantHasToken)
                .WithOne(e => e.applicant)
                .HasForeignKey<ApplicantHasToken>(e => e.applicantID);

        }



    }
}
