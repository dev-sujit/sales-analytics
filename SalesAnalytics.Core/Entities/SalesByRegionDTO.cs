using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAnalytics.Core.Entities
{
    public class SalesByRegionDTO
    {
        public string Region { get; set; }
        public decimal TotalSales { get; set; }
    }

}
