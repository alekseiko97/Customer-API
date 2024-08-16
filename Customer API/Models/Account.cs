namespace Customer_API.Models
{
    public class Account
    {
        public int Id { get; set; }
        private decimal _balance;
        public decimal Balance
        {
            get => Transactions.Sum(x => x.Amount);
            private set => _balance = value; // Prevent external setting
        }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        // Method to add a transaction and update the balance
        public void AddTransaction(Transaction transaction)
        {
            ArgumentNullException.ThrowIfNull(transaction);

            Transactions.Add(transaction);
            _balance += transaction.Amount;
        }
    }
}
