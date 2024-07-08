using Microsoft.EntityFrameworkCore;
using SalesAnalytics.Core.Entities;
using SalesAnalytics.Core.Interfaces;

namespace SalesAnalytics.Infrastructure.Data
{
    public class SalesDbContext : DbContext, ISalesDbContext
    {
        public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<SaleRecord> SaleRecords { get; set; }
        public DbSet<ApiLog> ApiLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure your entities here

            //modelBuilder.Entity<ApiLog>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Timestamp).IsRequired();
            //    entity.Property(e => e.Method).HasMaxLength(10).IsRequired();
            //    entity.Property(e => e.Path).HasMaxLength(2048).IsRequired();
            //    entity.Property(e => e.Request).IsRequired();
            //    entity.Property(e => e.Response).IsRequired();
            //    entity.Property(e => e.ErrorMessage).HasMaxLength(2048);
            //    entity.Property(e => e.StatusCode).IsRequired();
            //});
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
