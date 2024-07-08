namespace SalesAnalytics.Core.Entities
{
    public class SaleRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int NumberOfSales { get; set; }
        public string Region { get; set; }
    }
}
