using Customer_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Customer_API.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="transactionService"></param>
    public class AccountService(ApplicationDbContext context, ITransactionService transactionService) : IAccountService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ITransactionService _transactionService = transactionService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="initialCredit"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Account> CreateAccountAsync(int customerId, decimal initialCredit)
        {
            // get user by customerId
            var user = await _context.Users
                .Include(u => u.Accounts)
                .SingleOrDefaultAsync(u => u.ID == customerId) ?? throw new ArgumentException("User not found", nameof(customerId));

            // create a new account
            var account = new Account { Balance = initialCredit };

            // add the account to the user's collection of accounts
            user.Accounts.Add(account);

            // add the account to the database
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            // if initialCredit is not zero -> create a transaction
            if (initialCredit != 0)
            {
                await _transactionService.CreateTransactionAsync(account, initialCredit);
            }

            return account;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<Account> GetAccountAsync(int accountId)
        {
            return await _context.Accounts.Include(a => a.Transactions).FirstOrDefaultAsync(a => a.Id == accountId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
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
