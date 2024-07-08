using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAnalytics.Core.Entities
{
    public class SalesTrendDTO
    {
        public DateTime Date { get; set; }
        public string Product { get; set; }
        public decimal SalesAmount { get; set; }
        public int NumberOfSales { get; set; }
    }
    public enum TrendInterval
    {
        Daily,
        Weekly,
        Monthly
    }
}
