using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using Domain.models;
using Microsoft.EntityFrameworkCore;
using shiftmaster.models;
using ShiftMaster.models;

namespace Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<WorkLocation> WorkLocations { get; set; }
        public DbSet<ShiftPattern> ShiftPatterns { get; set; }
        public DbSet<SkillRequirement> SkillRequirements { get; set; }
        public DbSet<AvailabilitySubmission> AvailabilitySubmissions { get; set; }
        public DbSet<LeaveBlock> LeaveBlocks { get; set; }
        public DbSet<EmployeeSkill> EmployeeSkills { get; set; }
        public DbSet<WeeklyRoster> WeeklyRosters { get; set; }
        public DbSet<ShiftAssignment> ShiftAssignments { get; set; }
        public DbSet<SchedulingConstraintViolation> SchedulingConstraintViolations { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<TimesheetSummary> TimesheetSummaries { get; set; }
        public DbSet<SwapRequest> SwapRequests { get; set; }
        public DbSet<CoverAssignment> CoverAssignments { get; set; }
        public DbSet<OvertimeAuthorisation> OvertimeAuthorisations { get; set; }
        public DbSet<LabourReport> LabourReports { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ENUM VALUE CONVERSIONS (Saving as Strings)
            modelBuilder.Entity<User>().Property(u => u.Status).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<WorkLocation>().Property(w => w.Type).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<WorkLocation>().Property(w => w.Status).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<ShiftPattern>().Property(s => s.ShiftType).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<ShiftPattern>().Property(s => s.Status).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<SkillRequirement>().Property(s => s.Status).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<AvailabilitySubmission>().Property(a => a.Status).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<LeaveBlock>().Property(l => l.Reason).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<LeaveBlock>().Property(l => l.Status).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<EmployeeSkill>().Property(e => e.ProficiencyLevel).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<EmployeeSkill>().Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<WeeklyRoster>().Property(w => w.Status).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<ShiftAssignment>().Property(s => s.Status).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<SchedulingConstraintViolation>().Property(s => s.ViolationType).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<SchedulingConstraintViolation>().Property(s => s.Severity).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<SchedulingConstraintViolation>().Property(s => s.Status).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<AttendanceRecord>().Property(a => a.Status).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<TimesheetSummary>().Property(t => t.Status).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<SwapRequest>().Property(s => s.Status).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<CoverAssignment>().Property(c => c.CoverType).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<CoverAssignment>().Property(c => c.Status).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<OvertimeAuthorisation>().Property(o => o.OTType).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<OvertimeAuthorisation>().Property(o => o.Status).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<LabourReport>().Property(l => l.Scope).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<Notification>().Property(n => n.Category).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<Notification>().Property(n => n.Status).HasConversion<string>().HasMaxLength(20);


            // RELATIONSHIP MAPPINGS BTW TABLES

            // 1-to-1 Relationship: ShiftAssignment to AttendanceRecord
            modelBuilder.Entity<ShiftAssignment>()
                .HasOne(sa => sa.Attendance)
                .WithOne(ar => ar.Assignment)
                .HasForeignKey<AttendanceRecord>(ar => ar.AssignmentID);

            // Circular Reference Fix: WorkLocation & Manager
            modelBuilder.Entity<WorkLocation>()
                .HasOne(wl => wl.Manager)
                .WithMany()
                .HasForeignKey(wl => wl.ManagerID)
                .OnDelete(DeleteBehavior.Restrict);

            // Circular Reference Fix: User & HomeLocation
            modelBuilder.Entity<User>()
                .HasOne(u => u.HomeLocation)
                .WithMany(wl => wl.Employees)
                .HasForeignKey(u => u.LocationID)
                .OnDelete(DeleteBehavior.Restrict);

            // Multi-User References in SwapRequest
            modelBuilder.Entity<SwapRequest>()
                .HasOne(s => s.Requester)
                .WithMany()
                .HasForeignKey(s => s.RequesterUserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SwapRequest>()
                .HasOne(s => s.Target)
                .WithMany()
                .HasForeignKey(s => s.TargetUserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SwapRequest>()
                .HasOne(s => s.ApprovedBy)
                .WithMany()
                .HasForeignKey(s => s.ApprovedByID)
                .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Delete on ShiftAssignment References in Swaps
            modelBuilder.Entity<SwapRequest>()
                .HasOne(s => s.OriginalAssignment)
                .WithMany()
                .HasForeignKey(s => s.OriginalAssignmentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SwapRequest>()
                .HasOne(s => s.ProposedAssignment)
                .WithMany()
                .HasForeignKey(s => s.ProposedAssignmentID)
                .OnDelete(DeleteBehavior.Restrict);

            //Multi-User References in CoverAssignment
            modelBuilder.Entity<CoverAssignment>()
                .HasOne(c => c.CoveringEmployee)
                .WithMany()
                .HasForeignKey(c => c.CoveringUserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CoverAssignment>()
                .HasOne(c => c.AssignedBy)
                .WithMany()
                .HasForeignKey(c => c.AssignedByID)
                .OnDelete(DeleteBehavior.Restrict);

            //Prevent Cascade Deletes on Approvers (Leave, OT, Timesheets)
            modelBuilder.Entity<LeaveBlock>()
                .HasOne(lb => lb.ApprovedBy)
                .WithMany()
                .HasForeignKey(lb => lb.ApprovedByID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OvertimeAuthorisation>()
                .HasOne(ot => ot.AuthorisedBy)
                .WithMany()
                .HasForeignKey(ot => ot.AuthorisedByID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TimesheetSummary>()
                .HasOne(ts => ts.ApprovedBy)
                .WithMany()
                .HasForeignKey(ts => ts.ApprovedByID)
                .OnDelete(DeleteBehavior.Restrict);

            // -- Prevent Cascade Delete on Roster Violations --
            modelBuilder.Entity<SchedulingConstraintViolation>()
                .HasOne(v => v.Roster)
                .WithMany()
                .HasForeignKey(v => v.RosterID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SchedulingConstraintViolation>()
                .HasOne(v => v.Employee)
                .WithMany()
                .HasForeignKey(v => v.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // -- Prevent Cascade Delete on Shift Assignments --
            modelBuilder.Entity<ShiftAssignment>()
                .HasOne(sa => sa.Employee)
                .WithMany()
                .HasForeignKey(sa => sa.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // 1. LeaveBlocks (Points to Employee AND Manager)
            modelBuilder.Entity<LeaveBlock>()
                .HasOne(lb => lb.Employee)
                .WithMany()
                .HasForeignKey(lb => lb.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. Overtime Authorisations (Points to Employee AND Manager)
            modelBuilder.Entity<OvertimeAuthorisation>()
                .HasOne(ot => ot.Employee)
                .WithMany()
                .HasForeignKey(ot => ot.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. Timesheet Summaries (Points to Employee AND Manager)
            modelBuilder.Entity<TimesheetSummary>()
                .HasOne(ts => ts.Employee)
                .WithMany()
                .HasForeignKey(ts => ts.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // -- Prevent Cascade Delete on ShiftAssignments -> WeeklyRosters --
            modelBuilder.Entity<ShiftAssignment>()
                .HasOne(sa => sa.Roster)
                .WithMany()
                .HasForeignKey(sa => sa.RosterID)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent Cascade Delete: Department & Users
            modelBuilder.Entity<User>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(u => u.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent Cascade Delete: Role & Users
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleID)
                .OnDelete(DeleteBehavior.Restrict);

            // Make EmployeeID globally unique across the system (Removed TenantId from this index)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.EmployeeID)
                .IsUnique();

            // ----------------------------------------------------------------------
            // GLOBAL RULE: Disable Cascade Deletes for the entire database!
            // (Safely moved out of the deleted helper method)
            // ----------------------------------------------------------------------
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