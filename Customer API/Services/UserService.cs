using Customer_API.Models;

namespace Customer_API.Services
{
    public class UserService(ApplicationDbContext context): IUserService
    {
        private readonly ApplicationDbContext _context = context;
        
        public Task<User> GetUserInfoAsync(int customerId)
        {
            throw new NotImplementedException();
        }
    }
}
