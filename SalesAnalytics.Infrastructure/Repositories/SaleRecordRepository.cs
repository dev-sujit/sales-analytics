using Microsoft.EntityFrameworkCore;
using SalesAnalytics.Core.Entities;
using SalesAnalytics.Core.Interfaces;
using SalesAnalytics.Infrastructure.Data;

namespace SalesAnalytics.Infrastructure.Repositories
{
    public class SaleRecordRepository : ISaleRecordRepository
    {
        private readonly SalesDbContext _context;

        public SaleRecordRepository(SalesDbContext context)
        {
            _context = context;
        }

        public async Task<SaleRecord> GetByIdAsync(int id)
        {
            return await _context.SaleRecords.FindAsync(id);
        }

        public async Task<IEnumerable<SaleRecord>> GetAllAsync(int page, int pageSize)
        {
            return await _context.SaleRecords
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task AddAsync(SaleRecord saleRecord)
        {
            await _context.SaleRecords.AddAsync(saleRecord);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SaleRecord saleRecord)
        {
            _context.SaleRecords.Update(saleRecord);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var saleRecord = await _context.SaleRecords.FindAsync(id);
            if (saleRecord != null)
            {
                _context.SaleRecords.Remove(saleRecord);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<SaleRecord>> GetSalesWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            // Example: Query sales records within the specified date range
            return await _context.SaleRecords
                .Where(s => s.Date >= startDate && s.Date <= endDate)
                .ToListAsync();
        }

        public async Task<List<SalesTrendDTO>> GetSalesTrendsAsync(DateTime startDate, DateTime endDate)
        {
            var salesTrends = await _context.SaleRecords
                .Where(s => s.Date >= startDate && s.Date <= endDate)
                .OrderByDescending(s => s.NumberOfSales >= 75)
                .ToListAsync();

            // Transform the anonymous type to SalesTrendDTO
            var salesTrendDTOs = salesTrends.Select(g => new SalesTrendDTO
            {
                Date = g.Date,
                Product = g.Product, // Include Product name
                SalesAmount = g.Price,
                NumberOfSales = g.NumberOfSales
            }).ToList();


            return salesTrendDTOs;
        }
        
        public async Task<List<SalesTrendDTO>> GetTopProductSalesAsync(DateTime startDate, DateTime endDate)
        {
            var salesTrends = await _context.SaleRecords
                .Where(s => s.Date >= startDate && s.Date <= endDate)
                .OrderByDescending(s => s.NumberOfSales)
                .Take(3)
                .ToListAsync();

            // Transform the anonymous type to SalesTrendDTO
            var salesTrendDTOs = salesTrends.Select(g => new SalesTrendDTO
            {
                Date = g.Date,
                Product = g.Product, // Include Product name
                SalesAmount = g.Price,
                NumberOfSales = g.NumberOfSales
            }).ToList();


            return salesTrendDTOs;
        }
        
        public async Task<List<SaleRecord>> GetSalesByRegionAsync(string region)
        {
            var salesTrends = await _context.SaleRecords
                .Where(s => s.Region == region)
                .ToListAsync();

            return salesTrends;
        }

    }
}
