
using Customer_API.Models;

namespace Customer_API.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <param name="context"></param>
    public class TransactionService(ApplicationDbContext context) : ITransactionService
    {
        private readonly ApplicationDbContext _context = context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
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
