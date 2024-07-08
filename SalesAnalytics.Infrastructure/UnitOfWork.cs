using SalesAnalytics.Core.Interfaces;
using SalesAnalytics.Infrastructure.Data;
using System.Threading.Tasks;

namespace SalesAnalytics.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SalesDbContext _context;
        private readonly ISaleRecordRepository _saleRecordRepository;

        public UnitOfWork(SalesDbContext context, ISaleRecordRepository saleRecordRepository)
        {
            _context = context;
            _saleRecordRepository = saleRecordRepository;
        }

        public ISaleRecordRepository SaleRecordRepository => _saleRecordRepository;

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
