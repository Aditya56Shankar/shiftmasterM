using Microsoft.EntityFrameworkCore;
using ShiftMaster.AttendanceService.Domain.Models;

namespace ShiftMaster.AttendanceService.Infrastructure.Data
{
    public class AttendanceDbContext : DbContext
    {
        public AttendanceDbContext(DbContextOptions<AttendanceDbContext> options) : base(options)
        {
        }

        public DbSet<AttendanceRecord> AttendanceRecords { get; set; } = null!;
        public DbSet<TimesheetSummary> TimesheetSummaries { get; set; } = null!;
        public DbSet<OvertimeAuthorisation> OvertimeAuthorisations { get; set; } = null!;
        public DbSet<LabourReport> LabourReports { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AttendanceRecord>(entity =>
            {
                entity.HasKey(e => e.AttendanceID);
                entity.Property(e => e.ActualHoursWorked).HasPrecision(4, 2);
            });

            modelBuilder.Entity<TimesheetSummary>(entity =>
            {
                entity.HasKey(e => e.TimesheetID);
                entity.Property(e => e.RegularHours).HasPrecision(5, 2);
                entity.Property(e => e.OvertimeHours).HasPrecision(5, 2);
                entity.Property(e => e.PublicHolidayHours).HasPrecision(5, 2);
                entity.Property(e => e.TotalHours).HasPrecision(5, 2);
            });

            modelBuilder.Entity<OvertimeAuthorisation>(entity =>
            {
                entity.HasKey(e => e.OTID);
                entity.Property(e => e.PlannedOTHours).HasPrecision(5, 2);
                entity.Property(e => e.ActualOTHours).HasPrecision(5, 2);
            });

            modelBuilder.Entity<LabourReport>(entity =>
            {
                entity.HasKey(e => e.ReportID);
            });
        }
    }
}
