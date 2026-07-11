using Microsoft.EntityFrameworkCore;
using ShiftMaster.CommsAuditService.Domain.Models;

namespace ShiftMaster.CommsAuditService.Infrastructure.Data
{
    public class CommsAuditDbContext : DbContext
    {
        public CommsAuditDbContext(DbContextOptions<CommsAuditDbContext> options) : base(options)
        {
        }

        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotificationID);
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.AuditID);
            });
        }
    }
}
