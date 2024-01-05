using HRMBackend.Model.Applicant;
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
        public DbSet<Applicant> Applicant { get; set; }
        public DbSet<ApplicantHasToken> ApplicantHasToken { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Applicant>()
                .HasOne(e => e.applicantHasToken)
                .WithOne(e => e.applicant)
                .HasForeignKey<ApplicantHasToken>(e => e.applicantID);

        }



    }
}
