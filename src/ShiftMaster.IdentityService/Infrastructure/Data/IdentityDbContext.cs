using Microsoft.EntityFrameworkCore;
using ShiftMaster.IdentityService.Domain.Models;

namespace ShiftMaster.IdentityService.Infrastructure.Data
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<WorkLocation> WorkLocations { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ENUM VALUE CONVERSIONS
            modelBuilder.Entity<User>().Property(u => u.Status).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<WorkLocation>().Property(w => w.Type).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<WorkLocation>().Property(w => w.Status).HasConversion<string>().HasMaxLength(20);

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

            // Make EmployeeID globally unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.EmployeeID)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .Property(r => r.roleName)
                .HasConversion<string>()
                .HasMaxLength(50);

            // Disable Cascade Deletes for this DbContext
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
