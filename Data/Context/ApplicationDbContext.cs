using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Text;
using Domain.Enums;
using Domain.models;
using Microsoft.EntityFrameworkCore;
using shiftmaster.models;
using ShiftMaster.models;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

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
                .WithMany(r => r.Violations)
                .HasForeignKey(v => v.RosterID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SchedulingConstraintViolation>()
                .HasOne(v => v.Employee)
                .WithMany()
                .HasForeignKey(v => v.UserID)
                .OnDelete(DeleteBehavior.Restrict);


            // -- Prevent Cascade Delete on Shift Assignments --

            modelBuilder.Entity<ShiftAssignment>()
                .HasOne(sa => sa.Roster)
                .WithMany(r => r.ShiftAssignments)
                .HasForeignKey(sa => sa.RosterID)
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
                .WithMany(r => r.ShiftAssignments)
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


            modelBuilder.Entity<WorkLocation>().HasData(

                 new WorkLocation
                 {
                     LocationID = 1,
                     LocationName = "Chennai Plant",
                     City = "Chennai",
                     OperatingHours = "09:00-21:00",   // ✅ REQUIRED FIX
                     Status = ActiveStatus.Active
                 },
                 new WorkLocation
                 {
                     LocationID = 2,
                     LocationName = "Bangalore Hub",
                     City = "Bangalore",
                     OperatingHours = "08:00-20:00",   // ✅ REQUIRED FIX
                     Status = ActiveStatus.Active
                 }


               );


            modelBuilder.Entity<Department>().HasData(
                new Department
                {
                    departmentId = 1,
                    departmentName = "Production"
                },

                new Department
                {
                    departmentId = 2,
                    departmentName = "Maintenance"
                },
                new Department
                {
                    departmentId = 3,
                    departmentName = "Quality Control"
                }

            );

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    roleId = 1,
                    roleName = "Admin"
                },
                new Role
                {
                    roleId = 2,
                    roleName = "Employee"
                }
            );


            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserID = 1,
                    EmployeeID = "EMP001",
                    Name = "Admin User",
                    Email = "admin@shiftmaster.com",
                    PasswordHash = "AQAAAAEAACcQAAAAEExampleHashedPassword==",
                    Phone = "9876543210",
                    Status = UserStatus.Active,
                    LocationID = 1,
                    RoleID = 1, // ✅ Now valid
                    DepartmentID = 1
                }
            );



            modelBuilder.Entity<ShiftPattern>().HasData(
                new ShiftPattern
                {
                    PatternID = 1,
                    PatternName = "Morning Shift",
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    ShiftType = ShiftType.Morning,
                    Status = ActiveStatus.Active,
                    LocationID = 1,
                    MinStaffingLevel = 2
                },
                new ShiftPattern
                {
                    PatternID = 2,
                    PatternName = "Evening Shift",
                    StartTime = new TimeSpan(17, 0, 0),
                    EndTime = new TimeSpan(1, 0, 0),
                    ShiftType = ShiftType.Afternoon,
                    Status = ActiveStatus.Active,
                    LocationID = 1,
                    MinStaffingLevel = 2
                },
                new ShiftPattern
                {
                    PatternID = 3,
                    PatternName = "Night Shift",
                    StartTime = new TimeSpan(1, 0, 0),
                    EndTime = new TimeSpan(9, 0, 0),
                    ShiftType = ShiftType.Night,
                    Status = ActiveStatus.Active,
                    LocationID = 2,
                    MinStaffingLevel = 1
                }

            );


            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserID = 2,
                    EmployeeID = "EMP002",
                    Name = "Employee One",
                    Email = "emp1@shift.com",
                    PasswordHash = "xyz",
                    Phone = "9999999999",
                    Status = UserStatus.Active,
                    LocationID = 1,
                    RoleID = 2,
                    DepartmentID = 1
                },
                new User
                {
                    UserID = 3,
                    EmployeeID = "EMP003",
                    Name = "Employee Two",
                    Email = "emp2@shift.com",
                    PasswordHash = "xyz",
                    Phone = "8888888888",
                    Status = UserStatus.Active,
                    LocationID = 1,
                    RoleID = 2,
                    DepartmentID = 1
                }
            );

            modelBuilder.Entity<WeeklyRoster>().HasData(
                new WeeklyRoster
                {
                    RosterID = 1,
                    WeekStartDate = new DateTime(2026, 6, 16),
                    WeekEndDate = new DateTime(2026, 6, 22),
                    Status = RosterStatus.Draft
                }
            );

            modelBuilder.Entity<ShiftAssignment>().HasData(
            new ShiftAssignment
            {
                AssignmentID = 1,
                RosterID = 1,
                UserID = 2,
                AssignedDate = new DateTime(2026, 6, 18),
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(17, 0, 0),
                Role = "Operator",
                Status = ShiftAssignmentStatus.Assigned
            },
            new ShiftAssignment
            {
                AssignmentID = 2,
                RosterID = 1,
                UserID = 3,
                AssignedDate = new DateTime(2026, 6, 18),
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(17, 0, 0),
                Role = "Operator",
                Status = ShiftAssignmentStatus.Assigned
            }
);


            modelBuilder.Entity<EmployeeSkill>().HasData(
    new EmployeeSkill
    {
        EmpSkillID = 1,
        UserID = 2,
        SkillName = "Machine Operation",
        Status = ActiveStatus.Active
    }
);


            modelBuilder.Entity<SkillRequirement>().HasData(
    new SkillRequirement

    {
        SkillReqID = 1,
        DepartmentID = 1,   // ✅ FIXED (must exist in Departments table)
        LocationID = 1,
        MinCountPerShift = 1,
        SkillName = "Welding",
        Status = ActiveStatus.Active
    }

);
            modelBuilder.Entity<CoverAssignment>().HasData(
    new CoverAssignment
    {
        CoverID = 1,   // ✅ required for HasData

        CoverType = CoverType.Mandatory,   // ✅ enum (no string)
        OvertimeApplicable = true,

        Status = CoverStatus.Completed,  // ✅ enum (not string)

        OriginalAssignmentID = 9,  // ✅ must exist in ShiftAssignments
        CoveringUserID = 2,        // ✅ must exist in Users
        AssignedByID = 1           // ✅ must exist in Users
    }
);


            modelBuilder.Entity<AvailabilitySubmission>().HasData(
    new AvailabilitySubmission
    {

        AvailabilityID = 1,
        UserID = 2,
        WeekStartDate = new DateTime(2026, 6, 16),

        AvailableDays = "Mon,Tue,Wed,Thu,Fri",  // REQUIRED
        PreferredShiftType = ShiftType.Night.ToString(), // if required
        MaxHoursPerWeek = 40,
        SubmittedDate = new DateTime(2026, 6, 15),

        Status = AvailabilityStatus.Acknowledged

    }
);

            modelBuilder.Entity<LeaveBlock>().HasData(
    new LeaveBlock
    {
        LeaveBlockID = 1,
        UserID = 2,
        StartDate = new DateTime(2026, 6, 18),
        EndDate = new DateTime(2026, 6, 18),
        Status = LeaveStatus.Active
    }
);


            modelBuilder.Entity<SwapRequest>().HasData(
    new SwapRequest
    {


        SwapID = 1,
        RequesterUserID = 2,
        TargetUserID = 3,
        OriginalAssignmentID = 1,
        ProposedAssignmentID = 2,

        Reason = "Need to swap due to personal work",

        ApprovedByID = 1,  // ✅ recommended if required

        Status = ApprovalStatus.Approved


    }
);

            modelBuilder.Entity<SwapRequest>().HasData(
    new SwapRequest
    {
        SwapID = 2,
        RequesterUserID = 2,
        TargetUserID = 3,
        OriginalAssignmentID = 1,
        ProposedAssignmentID = 2,

       Reason = "Shift swapped and completed",

        ApprovedByID = 1,   // Admin user you already seeded

        Status = ApprovalStatus.Completed
    }
);


        }
    }
}