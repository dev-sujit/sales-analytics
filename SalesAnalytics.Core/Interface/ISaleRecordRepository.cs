using SalesAnalytics.Core.Entities;

namespace SalesAnalytics.Core.Interfaces
{
    public interface ISaleRecordRepository
    {
        Task<SaleRecord> GetByIdAsync(int id);
        Task<IEnumerable<SaleRecord>> GetAllAsync(int page, int pageSize);
        Task AddAsync(SaleRecord saleRecord);
        Task UpdateAsync(SaleRecord saleRecord);
        Task DeleteAsync(int id);
        Task<List<SaleRecord>> GetSalesWithinDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<SalesTrendDTO>> GetSalesTrendsAsync(DateTime startDate, DateTime endDate);
        Task<List<SalesTrendDTO>> GetTopProductSalesAsync(DateTime startDate, DateTime endDate);
        Task<List<SaleRecord>> GetSalesByRegionAsync(string region);
    }
}
