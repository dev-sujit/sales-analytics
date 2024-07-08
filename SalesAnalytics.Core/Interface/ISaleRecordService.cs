using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAnalytics.Core.Interface
{
    public interface ISaleRecordService
    {
        Task<decimal> GetTotalSalesAsync(DateTime startDate, DateTime endDate);
    }
}
