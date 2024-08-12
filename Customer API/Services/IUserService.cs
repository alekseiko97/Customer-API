using Customer_API.Models;

namespace Customer_API.Services
{
    public interface IUserService 
    {
        Task<User> GetUserInfoAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(string name, string surname);
    }
}
