using Microsoft.EntityFrameworkCore;
using ShiftMaster.SchedulingService.Models;
using ShiftMaster.SchedulingService.Enums;
using System.Linq;

namespace ShiftMaster.SchedulingService.Data
{
    public class SchedulingDbContext : DbContext
    {
        public SchedulingDbContext(DbContextOptions<SchedulingDbContext> options) : base(options)
        {
        }

        public DbSet<WeeklyRoster> WeeklyRosters { get; set; } = null!;
        public DbSet<ShiftAssignment> ShiftAssignments { get; set; } = null!;
        public DbSet<ShiftPattern> ShiftPatterns { get; set; } = null!;
        public DbSet<SchedulingConstraintViolation> SchedulingConstraintViolations { get; set; } = null!;
        public DbSet<SwapRequest> SwapRequests { get; set; } = null!;
        public DbSet<CoverAssignment> CoverAssignments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WeeklyRoster>().Property(w => w.Status).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<ShiftAssignment>().Property(s => s.Status).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<ShiftPattern>().Property(s => s.ShiftType).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<ShiftPattern>().Property(s => s.Status).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<SchedulingConstraintViolation>().Property(s => s.ViolationType).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<SchedulingConstraintViolation>().Property(s => s.Severity).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<SchedulingConstraintViolation>().Property(s => s.Status).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<SwapRequest>().Property(s => s.Status).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<CoverAssignment>().Property(c => c.CoverType).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<CoverAssignment>().Property(c => c.Status).HasConversion<string>().HasMaxLength(50);

            // Handle cascading paths cleanly
            modelBuilder.Entity<SwapRequest>()
                .HasOne(sr => sr.OriginalAssignment)
                .WithMany()
                .HasForeignKey(sr => sr.OriginalAssignmentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SwapRequest>()
                .HasOne(sr => sr.ProposedAssignment)
                .WithMany()
                .HasForeignKey(sr => sr.ProposedAssignmentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CoverAssignment>()
                .HasOne(ca => ca.OriginalAssignment)
                .WithMany()
                .HasForeignKey(ca => ca.OriginalAssignmentID)
                .OnDelete(DeleteBehavior.Restrict);

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
