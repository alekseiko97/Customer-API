using Customer_API.Models;

namespace Customer_API.Services
{
    public interface ITransactionService
    {
        Task CreateTransactionAsync(Account account, decimal amount);
    }
}
