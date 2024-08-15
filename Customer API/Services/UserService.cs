using Customer_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Customer_API.Services
{
    public class UserService(ApplicationDbContext context): IUserService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<User> CreateUserAsync(string name, string surname)
        {
            User user = new()
            {
                Name = name,
                Surname = surname,
                Balance = 0 // initial balance is 0
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserInfoAsync(int customerId)
        {
            return await _context.Users.Include(u => u.Accounts).FirstOrDefaultAsync(u => u.ID == customerId);
        }
    }
}
