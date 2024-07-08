using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAnalytics.Core.Entities
{
    public class ApiLog
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }
    }

}
