namespace Customer_API.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public decimal Balance { get; set; }
        public List<Account> Accounts { get; set; } = new List<Account>(); 
    }
}
