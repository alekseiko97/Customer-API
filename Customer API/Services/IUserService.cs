using Customer_API.Models;

namespace Customer_API.Services
{
    public interface IUserService 
    {
        Task<User> GetUserInfoAsync(int customerId);
    }
}
