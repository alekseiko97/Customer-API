using Customer_API.Models;

namespace Customer_API.Services
{
    public class AccountService(ApplicationDbContext context): IAccountService
    {
        private readonly ApplicationDbContext _context = context;

        public Task AddTransactionAsync(int accountId, Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<Account> CreateAccountAsync(int customerId, decimal initialCredit)
        {
            throw new NotImplementedException();
        }

        public Task<Account> GetAccountAsync(int accountId)
        {
            throw new NotImplementedException();
        }
    }
}
