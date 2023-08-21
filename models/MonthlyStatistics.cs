namespace ecommerce.models
{
    public class MonthlyStats
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalOrderCount { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalBooksPurchased { get; set; }
    }
}