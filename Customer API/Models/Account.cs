namespace Customer_API.Models
{
    public class Account
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions = new List<Transaction>();
    }
}
