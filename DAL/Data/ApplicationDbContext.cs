using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models;
using DAL.Data.Models.IdentityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class ApplicationDbContext :IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Advisor> Advisors { get; set; }
        public DbSet<AdviceRequest> AdviceRequests { get; set; }
        public DbSet<AdvisorAvailability> AdvisorAvailabilities { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<NewsItem> NewsItems { get; set; }
        public DbSet<ServiceOffering> ServiceOfferings { get; set; }
        public DbSet<VolunteerApplication> VolunteerApplications { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<HelpType> HelpTypes { get; set; }
        public DbSet<HelpRequest> HelpRequests { get; set; }
        public DbSet<ReconcileRequest> ReconcileRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Advisor)
                .WithOne(a => a.User)
                .HasForeignKey<Advisor>(a => a.UserId);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Admin)
                .WithOne(a => a.User)
                .HasForeignKey<Admin>(a => a.UserId);


            builder.Entity<Advisor>()
                .HasOne(a => a.Consultation)
                .WithMany(c => c.Advisors)
                .HasForeignKey(a => a.ConsultationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AdviceRequest>()
                .HasOne(ar => ar.Advisor)
                .WithMany(a => a.AdviceRequests)
                .HasForeignKey(ar => ar.AdvisorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AdviceRequest>()
                .HasOne(ar => ar.User)
                .WithMany(u => u.AdviceRequests)
                .HasForeignKey(ar => ar.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AdviceRequest>()
                .HasOne(ar => ar.Consultation)
                .WithMany(c => c.AdviceRequests)
                .HasForeignKey(ar => ar.ConsultationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Lecture>()
                .HasOne(l => l.Consultation)
                .WithMany(c => c.Lectures)
                .HasForeignKey(l => l.ConsultationId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Lecture>()
                .HasOne(l => l.CreatedByUser)
                .WithMany()
                .HasForeignKey(l => l.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AdvisorAvailability>()
                .HasOne(a => a.AdviceRequest)
                .WithOne()
                .HasForeignKey<AdvisorAvailability>(a => a.AdviceRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AdviceRequest>()
                .HasOne<AdvisorAvailability>()
                .WithMany()
                .HasForeignKey(a => a.AdvisorAvailabilityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
