using HRMBackend.Model.Applicant;
using HRMBackend.Model.SMS;
using Microsoft.EntityFrameworkCore;

namespace HRMBackend.DB
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        /**
         * DB SETS 
         */
        public DbSet<Applicant> Applicant { get; set; }
        public DbSet<ApplicantHasOTP> ApplicantHasOTP { get; set; }
        public DbSet<SMSTemplate> SMSTemplate { get; set; }
        public DbSet<SMSCampaignHistory> SMSCampaignHistory { get; set; }
        public DbSet<SMSCampaignReceipient> SMSCampaignReceipient { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Applicant>()
                .HasOne(e => e.otp)
                .WithOne(e => e.applicant)
                .HasForeignKey<ApplicantHasOTP>(e => e.applicantID)
                .OnDelete(DeleteBehavior.Cascade);
                ;

            modelBuilder.Entity<SMSCampaignHistory>()
               .HasOne(e => e.smsTemplate)
               .WithMany(e => e.smsHistory)
               .HasForeignKey(e => e.smsTemplateId);

            modelBuilder.Entity<SMSCampaignReceipient>()
             .HasOne(e => e.campaignHistory)
             .WithMany(e => e.smsReceipients)
             .HasForeignKey(e => e.campaignHistoryId);

            modelBuilder.Entity<SMSCampaignReceipient>(entity =>
            {
                entity.Property(e => e.status)
                    .HasComment("Status => Pending or Successful or Failed");
            });
        }





    }
}
