using Customer_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Customer_API.Services
{
    public class AccountService(ApplicationDbContext context): IAccountService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddTransactionAsync(int accountId, Transaction transaction)
        {
            var account = await _context.Accounts.FindAsync(accountId) ?? throw new Exception($"Account not found for id: {accountId}");

            account.Balance += transaction.Amount;
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<Account> CreateAccountAsync(int customerId, decimal initialCredit)
        {
            var account = new Account { Balance = initialCredit};

            var user = _context.Users.FirstOrDefaultAsync(u => u.ID == customerId);
            
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            if (initialCredit > 0)
            {
                var transaction = new Transaction { Amount = initialCredit, Timestamp = DateTime.Now };
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();
            }

            return account;
        }

        public async Task<Account> GetAccountAsync(int accountId)
        {
            return await _context.Accounts.FindAsync(accountId);
        }
    }
}
