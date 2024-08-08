using Customer_API.Models;

namespace Customer_API.Services
{
    public interface IAccountService
    {
        Task<Account> CreateAccountAsync(int customerId, decimal initialCredit);
        Task<Account> GetAccountAsync(int accountId);
        Task AddTransactionAsync(int accountId, Transaction transaction);
    }
}
