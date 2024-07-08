using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAnalytics.Core.Entities
{
    public class TopProductDTO
    {
        public int Rank { get; set; }
        public string ProductName { get; set; }
        public decimal SalesAmount { get; set; }
    }

}
