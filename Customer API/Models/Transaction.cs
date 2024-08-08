namespace Customer_API.Models
{
    public class Transaction
    {
        public int Id { get; set; } 
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
