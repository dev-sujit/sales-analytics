using SalesAnalytics.Core.Entities;
using SalesAnalytics.Core.Interfaces;

namespace SalesAnalytics.Application.Services
{
    public class SaleRecordService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SaleRecordService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SaleRecord> GetSaleRecordByIdAsync(int id)
        {
            return await _unitOfWork.SaleRecordRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<SaleRecord>> GetAllSaleRecordsAsync(int page, int pageSize)
        {
            return await _unitOfWork.SaleRecordRepository.GetAllAsync(page, pageSize);
        }

        public async Task AddSaleRecordAsync(SaleRecord saleRecord)
        {
            await _unitOfWork.SaleRecordRepository.AddAsync(saleRecord);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateSaleRecordAsync(SaleRecord saleRecord)
        {
            await _unitOfWork.SaleRecordRepository.UpdateAsync(saleRecord);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteSaleRecordAsync(int id)
        {
            await _unitOfWork.SaleRecordRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<decimal> GetTotalSalesAsync(DateTime startDate, DateTime endDate)
        {
            // Example: Query and aggregate sales within the specified date range
            var sales = await _unitOfWork.SaleRecordRepository.GetSalesWithinDateRangeAsync(startDate, endDate);
            decimal totalSales = sales.Sum(s => s.Price); // Adjust based on your schema

            return totalSales;
        }

        public async Task<List<SalesTrendDTO>> GetSalesTrendsAsync(TrendInterval interval)
        {
            // Example: Logic to calculate sales trends based on the interval
            DateTime startDate, endDate;

            switch (interval)
            {
                case TrendInterval.Daily:
                    startDate = DateTime.Today.AddDays(-6); // Last 7 days including today
                    endDate = DateTime.Now;
                    break;
                case TrendInterval.Weekly:
                    startDate = DateTime.Today.AddDays(-27); // Last 28 days including today
                    endDate = DateTime.Now;
                    break;
                case TrendInterval.Monthly:
                    startDate = DateTime.Today.AddMonths(-5); // Last 6 months including this month
                    endDate = DateTime.Now;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(interval), interval, null);
            }

            // Example: Fetch sales trends from repository based on calculated date range
            var salesTrends = await _unitOfWork.SaleRecordRepository.GetSalesTrendsAsync(startDate, endDate);

            return salesTrends;
        }

        public async Task<List<SalesTrendDTO>> GetTopProductsAsync(DateTime startDate, DateTime endDate)
        {
            // Example: Query and aggregate sales within the specified date range
            var sales = await _unitOfWork.SaleRecordRepository.GetTopProductSalesAsync(startDate, endDate);
            //decimal totalSales = sales.Sum(s => s.Price); // Adjust based on your schema

            return sales;
        }
        
        public async Task<List<SaleRecord>> GetSalesByRegionAsync(string region)
        {
            // Example: Query and aggregate sales within the specified date range
            var sales = await _unitOfWork.SaleRecordRepository.GetSalesByRegionAsync(region);
            //decimal totalSales = sales.Sum(s => s.Price); // Adjust based on your schema

            return sales;
        }
    }
}
