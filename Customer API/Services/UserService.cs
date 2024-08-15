using Customer_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Customer_API.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public class UserService(ApplicationDbContext context): IUserService
    {
        private readonly ApplicationDbContext _context = context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<User> GetUserInfoAsync(int customerId)
        {
            return await _context.Users
                .Include(u => u.Accounts)
                .ThenInclude(a => a.Transactions)
                .FirstOrDefaultAsync(u => u.ID == customerId);
        }
    }
}
