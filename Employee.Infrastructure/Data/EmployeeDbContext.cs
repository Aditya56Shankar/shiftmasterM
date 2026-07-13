using Microsoft.EntityFrameworkCore;
using System.Linq;
using ShiftMaster.Employee.Domain.Models;

namespace ShiftMaster.Employee.Infrastructure.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {
        }

        public DbSet<LeaveBlock> LeaveBlocks { get; set; } = null!;
        public DbSet<EmployeeSkill> EmployeeSkills { get; set; } = null!;
        public DbSet<SkillRequirement> SkillRequirements { get; set; } = null!;
        public DbSet<AvailabilitySubmission> AvailabilitySubmissions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LeaveBlock>().Property(l => l.Reason).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<LeaveBlock>().Property(l => l.Status).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<EmployeeSkill>().Property(e => e.ProficiencyLevel).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<EmployeeSkill>().Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<SkillRequirement>().Property(s => s.Status).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<AvailabilitySubmission>().Property(a => a.Status).HasConversion<string>().HasMaxLength(20);

            // Disable Cascade Deletes
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
