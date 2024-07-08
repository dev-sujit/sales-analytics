using Microsoft.EntityFrameworkCore;
using SalesAnalytics.Core.Entities;

namespace SalesAnalytics.Core.Interfaces
{
    public interface ISalesDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<SaleRecord> SaleRecords { get; set; }
        DbSet<ApiLog> ApiLogs { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
