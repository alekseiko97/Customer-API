
using Customer_API.Models;

namespace Customer_API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateTransactionAsync(Account account, decimal amount)
        {
            var transaction = new Transaction
            {
                Amount = amount,
                Timestamp = DateTime.UtcNow
            };

            account.Transactions.Add(transaction);

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
