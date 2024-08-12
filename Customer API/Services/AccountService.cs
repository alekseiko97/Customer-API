using Customer_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Customer_API.Services
{
    public class AccountService(ApplicationDbContext context) : IAccountService
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
            var user = await _context.Users
                .Include(u => u.Accounts)
                .SingleOrDefaultAsync(u => u.ID == customerId) ?? throw new ArgumentException("User not found", nameof(customerId));

            // create a new account
            var account = new Account
            {
                Balance = initialCredit,
            };

            // add the account to the user's collection of accounts
            user.Accounts.Add(account);

            // add the account to the database
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            // if initialCredit is not zero -> create a transaction
            if (initialCredit > 0)
            {
                var transaction = new Transaction
                {
                    Amount = initialCredit,
                    Timestamp = DateTime.UtcNow,
                };

                // add the transaction to the database
                _context.Transactions.Add(transaction);

                // link new transaction to the account
                account.Transactions.Add(transaction);

                // update the account's balance
                account.Balance += initialCredit;
                await _context.SaveChangesAsync();
            }

            return account;
        }

        public async Task<Account> GetAccountAsync(int accountId)
        {
            return await _context.Accounts.FindAsync(accountId);
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactions(int accountId)
        {
            // find the account by ID
            var account = await _context.Accounts
                .Include(a => a.Transactions) // ensure related transactions are included
                .SingleOrDefaultAsync(a => a.Id == accountId);

            // check if the account exists
            if (account == null)
            {
                return Enumerable.Empty<Transaction>(); // return an empty collection
            }

            // return the transactions for the account
            return account.Transactions;
        }

    }
}
